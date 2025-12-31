using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCreateWebSocket : ComponentBase {
        public const string PERF_DOG_URL = "wss://test-tools-biubiubiu2.xmfunny.com/1.0/mirror/serve";
        private WebSocketClient webSocketClient;
        private Stack<string> messageList = new Stack<string>(10);
        private bool isStartWebSocket = false;
        private List<string> outMessageList = new List<string>(10);

        protected override void OnAwake() {
            mySystem.Register<SE_ShortcutTool.CreateWebSocket>(OnCreateWebSocketCallBack);
            mySystem.Register<SE_ShortcutTool.CloseWebSocket>(OnCloseWebSocketCallBack);
            mySystem.Register<SE_ShortcutTool.OutMessage>(OnOutMessageCallBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_ShortcutTool.CreateWebSocket>(OnCreateWebSocketCallBack);
            mySystem.Unregister<SE_ShortcutTool.CloseWebSocket>(OnCloseWebSocketCallBack);
            mySystem.Unregister<SE_ShortcutTool.OutMessage>(OnOutMessageCallBack);
            RemoveUpdate(OnUpdate);
            CloseWebSocket();
        }

        private void OnCreateWebSocketCallBack(ISystemMsg body, SE_ShortcutTool.CreateWebSocket msg) {
            if (isStartWebSocket == false) {
                CreateWebSocketClient(msg.OpenMessage);
            } else {
                Debug.LogError($"ShortcutTool WebSocket 已经启动. OpenMessage:{msg.OpenMessage}");
            }
        }

        private void OnCloseWebSocketCallBack(ISystemMsg body, SE_ShortcutTool.CloseWebSocket msg) {
            CloseWebSocket();
        }

        private void OnOutMessageCallBack(ISystemMsg body, SE_ShortcutTool.OutMessage msg) {
            if (webSocketClient != null) {
                if (isStartWebSocket == false || outMessageList.Count > 0) {
                    outMessageList.Add(msg.Message);
                    return;
                }
                webSocketClient.Send(msg.Message);
            }
        }
        
        private void SendSaveOutMessage() {
            if (outMessageList.Count > 0) {
                foreach (var message in outMessageList) {
                    webSocketClient.Send(message);
                }
                outMessageList.Clear();
            }
        }

        private void CreateWebSocketClient(string openMessage) {
            if (webSocketClient == null) {
                webSocketClient = new WebSocketClient();
                webSocketClient.OnOpen += WSocketClient_OnOpen;
                webSocketClient.OnMessage += WSocketClient_OnMessage;
                webSocketClient.OnClose += WSocketClient_OnClose;
                webSocketClient.OnError += WSocketClient_OnError;
                webSocketClient.Connect(PERF_DOG_URL, openMessage);
            }
        }

        private void OnUpdate(float deltaTime) {
            if (isStartWebSocket == true) {
                SendSaveOutMessage();
            }
            RenderMessageLoop();
        }

        private void CloseWebSocket() {
            isStartWebSocket = false;
            if (webSocketClient != null) {
                webSocketClient.Close();
                webSocketClient = null;
            }
        }

        private void WSocketClient_OnOpen() {
            Debug.Log("[ServerCreateWebSocket] ws open");
        }

        private void WSocketClient_OnMessage(object sender, string data) {
            messageList.Push(data);
        }

        private void RenderMessageLoop() {
            while (messageList.Count > 0) {
                var msg = messageList.Pop();
                try {
                    switch (msg) {
                        case "connect_success":
                            isStartWebSocket = true;
                            break;
                        case "close":
                            CloseWebSocket();
                            break;
                        default:
                            mySystem.Dispatcher(new SE_ShortcutTool.InMessage {
                                Message = msg
                            });
                            break;
                    }
                } catch (Exception ex) {
                    Debug.LogError("[ServerCreateWebSocket] Error PerfMsg:" + ex);
                }
            }
        }

        private void WSocketClient_OnError(object sender, Exception ex) {
            Debug.LogError(ex.Message);
            WSocketClient_OnMessage(null, "close");
        }

        private void WSocketClient_OnClose() {
            Debug.Log("[ServerCreateWebSocket] ws close");
        }
    }
}