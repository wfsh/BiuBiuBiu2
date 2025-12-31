using System;
using System.Diagnostics;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Debug = UnityEngine.Debug;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {
    public class WebSocketClientNetworkChannel : INetworkChannel {
        protected int connectTimeout = 2000;
        protected int readTimeout = 0;
        protected int writeTimeout = 2000;

        private Action<NetworkStatus> onConnected;
        protected TaskWebSocket webSocket;
        
        protected string Url { get; private set; }
        
        protected ulong totalReceivedBytes;
        protected ulong totalSentBytes;

        protected int reconnectTimes = 0;

        protected INetworkReceivePacket ReceivePacket { get; private set; }
        protected INetworkSendPacket SendPacket { get; private set; }
        protected INetworkHeartPacket HeartPacket { get; private set; }
        protected INetworkSendCachePacket SendCachePacket { get; private set; }

        protected bool AutoReconnect { get; private set; } = false;
        protected int ReconnectTimesLimit { get; private set; } = 0;

        private Task<int> asyncReadTask;
        private Task asyncWriteTask;

        private double sendHeartPacketInterval;
        private double heartPacketTimeout;
        
        private Stopwatch sendHeartPacketWatch;
        private Stopwatch heartPacketTimeoutWatch;

        private NetworkStatus status = NetworkStatus.Disconnected;

        private string cookie;

        public NetworkStatus Status {
            get {
                return status;
            }
            protected set {
                status = value;
            }
        }
        
        public NetworkError NetworkError { get; private set; } = NetworkError.None;
        public bool Disposed { get; protected set; }

        internal WebSocketClientNetworkChannel() {
            totalReceivedBytes = 0;
            totalSentBytes = 0;
            Disposed = false;
        }

        public INetworkChannel SetConnectTimeout(int milliseconds) {
            this.connectTimeout = milliseconds;
            return this;
        }

        public INetworkChannel SetReadTimeout(int milliseconds) {
            this.readTimeout = milliseconds;
            return this;
        }

        public INetworkChannel SetWriteTimeout(int milliseconds) {
            this.writeTimeout = milliseconds;
            return this;
        }

        public INetworkChannel SetOnConnected(Action<NetworkStatus> onConnected) {
            this.onConnected += onConnected;
            return this;
        }

        public INetworkChannel SetNetworkReceivePacket(INetworkReceivePacket receivePacket) {
            this.ReceivePacket = receivePacket;
            return this;
        }

        public INetworkChannel SetNetworkSendPacket(INetworkSendPacket sendPacket) {
            this.SendPacket = sendPacket;
            return this;
        }

        public INetworkChannel SetKeepNetworkAlive(INetworkHeartPacket heartPacket, double sendHeartPacketInterval, double heartPacketTimeout) {
            if (sendHeartPacketInterval <= 0 || heartPacketTimeout <= 0) {
                throw new ArgumentException("心跳包间隔小于 0");
            }
            this.HeartPacket = heartPacket;
            this.sendHeartPacketInterval = sendHeartPacketInterval;
            this.heartPacketTimeout = heartPacketTimeout;
            sendHeartPacketWatch = new Stopwatch();
            heartPacketTimeoutWatch = new Stopwatch();
            return this;
        }

        public INetworkChannel SetAutoReconnect(int reconnectTimesLimit, INetworkSendCachePacket packet) {
            SendCachePacket = packet;
            AutoReconnect = true;
            ReconnectTimesLimit = reconnectTimesLimit;
            return this;
        }

        public virtual async Task<NetworkError> ConnectAsync(string url) {
            if (Status != NetworkStatus.Disconnected) {
                return NetworkError.InvalidOperation;
            }
            Status = NetworkStatus.Connecting;
            try {
                var error = await ConnectServerAsync(url);
                if (error != NetworkError.None) {
                    OnConnectFailed(error);
                    return error;
                }
                OnConnectSuccess();
                return NetworkError.None;
            } catch (Exception e) {
                OnConnectFailed(NetworkError.Exception);
                return NetworkError.Exception;
            }
        }

        protected virtual async Task<NetworkError> ConnectServerAsync(string url) {
            Url = url;
            webSocket = new TaskWebSocket();
            Debug.Log("ConnectServerAsync: " + url);
            var task = await webSocket.ConnectAsync(url, ConnectClose, connectTimeout);
            if (task == NetworkError.Timeout) {
                return NetworkError.Timeout;
            }
            return NetworkError.None;
        }

        private void ConnectClose() {
            ConnectFailed(NetworkError.Exception, NetworkStatus.Disconnected);
        }

        private void ProcessNetworkError() {
            if (AutoReconnect) {
                StartReconnect();
            } else {
                Close();
            }
        }

        protected virtual void StartReconnect() {
            if (Disposed) {
                return;
            }
            if (Status == NetworkStatus.Reconnecting) {
                return;
            }
            
            Status = NetworkStatus.Reconnecting;
            CloseNetworkStream();
            ReconnectAsync();
        }

        protected virtual async Task<NetworkError> ReconnectAsync() {
            if (Disposed) {
                return NetworkError.InvalidOperation;
            }
            reconnectTimes += 1;
            Debug.Log("[web]开始重连 ：" + reconnectTimes);
            var error = await ConnectServerAsync(Url);
            if (error != NetworkError.None) {
                OnReconnectFailed(error);
                return error;
            }
            OnReconnectSuccess();
            return NetworkError.None;
        }

        public virtual void SendAsync(INetworkPacket packet) {
            if (Disposed) {
                return;
            }
            SendPacket.Enqueue(packet);
        }

        public virtual void SendHeartPacket() {
            if (Disposed) {
                return;
            }
            SendPacket.Enqueue(HeartPacket.GetHeartPacket());
        }

        protected virtual void OnHeartPacketCallBack() {
            sendHeartPacketWatch?.Restart();
            heartPacketTimeoutWatch?.Stop();
        }

        protected virtual async void CloseNetworkStream() {
            asyncReadTask = null;
            asyncWriteTask = null;
            heartPacketTimeoutWatch?.Stop();
            sendHeartPacketWatch?.Stop();
            if (webSocket != null) {
                var client = webSocket;
                await client.CloseAsync();
            }
            webSocket = null;
        }

        public virtual void Close() {
            Debug.Log("【GameServer】关闭链接");
            Disposed = true;
            Status = NetworkStatus.Disconnected;
            this.onConnected = null;
            CloseNetworkStream();
        }

        public virtual void Update() {
            if (Disposed) {
                return;
            }
            switch (Status) {
                case NetworkStatus.Connected:
                    try {
                        ProcessReceivePacket();
                        ProcessSendPacket();
                        ProcessKeepAlive();
                    } catch (Exception e) {
                        Debug.LogError(e);
                        ProcessNetworkError();
                    }
                    break;
            }
        }

        private void ProcessReceivePacket() {
            asyncReadTask ??= webSocket.ReceiveAsync(ReceivePacket.Buffer, ReceivePacket.ReceivedLength, ReceivePacket.Length - ReceivePacket.ReceivedLength);
            // 如果异步读取任务未完成，则直接返回，等待下一次更新
            if (!asyncReadTask.IsCompleted) {
                return;
            }
            var readed = asyncReadTask.Result;
            // 如果读取到的数据为 0，或者任务失败（IsFaulted），则认为网络出错
            if (readed == 0 || asyncReadTask.IsFaulted) {
                ProcessNetworkError();
                return;
            }
            totalReceivedBytes += (ulong) readed;
            asyncReadTask?.Dispose();
            asyncReadTask = null;
            ReceivePacket.ProcessPacket(readed);
            while (ReceivePacket.NetworkPackets.Count > 0) {
                var packet = ReceivePacket.NetworkPackets.Dequeue();
                OnReceivedPacket(packet);
            }
        }

        private void ProcessSendPacket() {
            if (asyncWriteTask != null) {
                if (!asyncWriteTask.IsCompleted) {
                    return;
                }
                if (asyncWriteTask.IsFaulted) {
                    ProcessNetworkError();
                    return;
                }
            }

            asyncWriteTask?.Dispose();
            asyncWriteTask = null;

            SendPacket.ResetPacketBuffer();
            SendPacket.ProcessPacket();

            if (SendPacket.Position > 0) {
                totalSentBytes += (ulong)SendPacket.Position;
                SendCachePacket?.Write(SendPacket.Buffer, 0, SendPacket.Position);
                asyncWriteTask = webSocket.WriteAsync(SendPacket.Buffer, 0, SendPacket.Position);
            }
        }

        private void ProcessKeepAlive() {
            if (sendHeartPacketWatch == null || heartPacketTimeoutWatch == null) {
                return;
            }
            if (sendHeartPacketWatch.IsRunning && sendHeartPacketWatch.ElapsedMilliseconds >= sendHeartPacketInterval) {
                heartPacketTimeoutWatch.Restart();
                sendHeartPacketWatch.Stop();
                SendHeartPacket();
            }

            if (heartPacketTimeoutWatch.IsRunning && heartPacketTimeoutWatch.ElapsedMilliseconds >= heartPacketTimeout) {
                heartPacketTimeoutWatch.Stop();
                ProcessNetworkError();
            }
        }

        protected virtual void OnReceivedPacket(INetworkPacket packet) {
            
        }

        protected virtual void OnStartKeepAlive() {
            sendHeartPacketWatch?.Restart();
            heartPacketTimeoutWatch?.Stop();
        }

        protected virtual void OnConnectSuccess() {
            if (Disposed) {
                return;
            }
            Debug.Log("【GameServer】连接成功");
            Status = NetworkStatus.Connected;
            OnStartKeepAlive();
            onConnected?.Invoke(Status);
        }

        protected virtual void OnConnectFailed(NetworkError networkError) {
            if (Disposed) {
                return;
            }
            ConnectFailed(networkError, NetworkStatus.ConnectFailed);
        }
        
        protected virtual void OnReconnectSuccess() {
            if (Disposed) {
                return;
            }
            Debug.Log("【GameServer】重连成功");
            Status = NetworkStatus.Connected;
            onConnected?.Invoke(Status);
            OnStartKeepAlive();
        }

        protected virtual void OnReconnectFailed(NetworkError networkError) {
            if (Disposed) {
                return;
            }
            if (networkError != NetworkError.ReconnectDenied && reconnectTimes <= ReconnectTimesLimit) {
                ReconnectAsync();
            } else {
                ConnectFailed(networkError, NetworkStatus.ReconnectFailed);
            }
        }

        private void ConnectFailed(NetworkError networkError, NetworkStatus status) {
            if (Disposed) {
                return;
            }
            NetworkError = networkError;
            Status = status;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false) {
                return;
            }
#endif
            Debug.Log("【GameServer】连接失败:" + networkError + " Status:" + status);
            onConnected?.Invoke(Status);
        }
    }
}