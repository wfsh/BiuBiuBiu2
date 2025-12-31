using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {
    public class TaskWebSocket {
#if UNITY_EDITOR
        static TaskWebSocket() {
            // 监听运行模式的变化
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            // 检查当前状态
            if (state == PlayModeStateChange.ExitingPlayMode) {
                instance?.Close();
                instance = null;
            }
        }
#endif
        private static TaskWebSocket instance;
        private readonly Queue<byte[]> messageQueue = new Queue<byte[]>();
        private readonly object queueLock = new object();
        private TaskCompletionSource<bool> messageAvailableTcs = new TaskCompletionSource<bool>();
        private TaskCompletionSource<NetworkError> connectTcs;
        private TaskCompletionSource<bool> closeTcs;
        private UnityWebSocket.WebSocket webSocket = null;
        private Action onConnectClose;

        public async Task<NetworkError> ConnectAsync(string url, Action onConnectClose, int timeoutMs = 5000) {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false) {
                return NetworkError.None;
            }
#endif
            instance = this;
            CreateWebSocket(url);
            this.onConnectClose = onConnectClose;
            connectTcs = new TaskCompletionSource<NetworkError>();
            webSocket.ConnectAsync();
            if (await Task.WhenAny(connectTcs.Task, Task.Delay(timeoutMs)) != connectTcs.Task) {
                await CloseAsync();
                return NetworkError.Timeout;
            }
            return await connectTcs.Task;
        }

        private void CreateWebSocket(string url) {
            if (webSocket != null) {
                CloseAsync();
            }
            Debug.Log("创建 WebSocket 连接 " + url);
            PerfAnalyzerAgent.SetTag($"创建 WebSocket 连接 {url}");
            webSocket = new UnityWebSocket.WebSocket(url);
            webSocket.OnOpen += (sender, args) => {
                Debug.Log("WebSocket 连接 Success:" + url);
                PerfAnalyzerAgent.SetTag($"WebSocket 连接成功 {url}");
                connectTcs?.TrySetResult(NetworkError.None);
            };
            webSocket.OnError += async (sender, args) => {
                Debug.LogError($"WebSocket 连接 OnError URL:{url} Message:{args.Message}");
                PerfAnalyzerAgent.SetTag($"WebSocket 连接错误");
                connectTcs?.TrySetException(new Exception(args.Message));
                await CloseAsync();
            };
            webSocket.OnClose += (sender, args) => {
                Debug.Log($"WebSocket 连接 OnClose URL:{url} Code:{args.Code} Reason:{args.Reason}");
                PerfAnalyzerAgent.SetTag($"WebSocket 连接 OnClose");
                closeTcs?.TrySetResult(true);
                this.onConnectClose?.Invoke();
            };
            webSocket.OnMessage += (sender, args) => {
                lock (queueLock) {
                    messageQueue.Enqueue(args.RawData);
                    messageAvailableTcs.TrySetResult(true);
                }
            };
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count) {
            var segment = new ArraySegment<byte>(buffer, offset, count);
            webSocket.SendAsync(segment.ToArray());
            await Task.CompletedTask;
        }

        public async Task<int> ReceiveAsync(byte[] buffer, int offset, int count) {
            await WaitForMessageAsync();
            // 尝试从消息队列中取出数据
            lock (queueLock) {
                if (messageQueue.Count == 0) {
                    throw new InvalidOperationException("No data available in the message queue.");
                }
                var data = messageQueue.Dequeue();
                // 如果队列为空，重置等待任务
                if (messageQueue.Count == 0) {
                    messageAvailableTcs = new TaskCompletionSource<bool>();
                }
                var length = Math.Min(count, data.Length);
                Array.Copy(data, 0, buffer, offset, length);
                return length;
            }
        }

        private async Task WaitForMessageAsync() {
            while (true) {
                TaskCompletionSource<bool> tcs;
                lock (queueLock) {
                    if (messageQueue.Count > 0) {
                        return; // 队列中已有消息，直接返回
                    }
                    tcs = messageAvailableTcs;
                }
                await tcs.Task; // 等待消息到达
            }
        }

        public async Task CloseAsync() {
            if (webSocket == null || webSocket.ReadyState != UnityWebSocket.WebSocketState.Open) {
                return; // 防止重复关闭
            }
            closeTcs = new TaskCompletionSource<bool>();
            Close();
            Debug.Log("WebSocket CloseAsync 连接关闭");
            await closeTcs.Task;
        }

        public void Close() {
            if (webSocket == null) {
                return;
            }
            webSocket.CloseAsync();
            webSocket = null;
#if UNITY_EDITOR
            Debug.Log("WebSocket 连接关闭");
#endif
        }
    }
}