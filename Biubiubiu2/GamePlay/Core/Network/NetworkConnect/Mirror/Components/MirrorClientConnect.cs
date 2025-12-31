using System;
using kcp2k;
using Mirror.SimpleWeb;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.PerfAnalyzer;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MirrorClientConnect : ComponentBase {
        public enum ConnectStateEnum {
            None,
            ConnectIng,
            Connect,
            DisConnect,
            ReConnectIng,
        }
        private MirrorEntity entity;
        private MirrorManager mirrorManager;
        private float checkTimeOut = -1.0f;
        private float connectTime = -1.0f;
        private bool isShowWarEnd = false;
        private bool isQuitToLobby = false;
        private int reConnectCount = 0;
        private bool isStartConnectTask = false;
        private ConnectStateEnum connectState = ConnectStateEnum.None;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Register<M_Game.ShowWarEnd>(OnShowWarEndCallBack);
            MsgRegister.Register<M_Game.LoginGameScene>(OnStageLoadEndCallBack);
        }
        
        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            AddUpdate(OnUpdate);
            entity = (MirrorEntity)iEntity;
            mirrorManager = entity.manager;
            mirrorManager.OnClientConnectEvent += OnClientConnect;
            mirrorManager.OnClientDisconnectEvent += OnClientDisconnect;
            Connect();
        }

        protected override void OnClear() {
            base.OnClear();
            DisConnect();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<M_Game.ShowWarEnd>(OnShowWarEndCallBack);
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Unregister<M_Game.LoginGameScene>(OnStageLoadEndCallBack);
            entity = null;
        }

        private void OnShowWarEndCallBack(M_Game.ShowWarEnd ent) {
            isShowWarEnd = true;
        }

        private void OnUpdate(float deltaTime) {
            if (connectState != ConnectStateEnum.ConnectIng) {
                return;
            }
            if (checkTimeOut >= 0f) {
                checkTimeOut += deltaTime;
                if (checkTimeOut >= 5f) {
                    PerfAnalyzerAgent.SetLog($"[Client] 连接超时 5S");
                    Debug.Log($"[Mirror] 连接超时 5S");
                    checkTimeOut = -1.0f;
                    ReConnect();
                }
            }
        }
        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            switch (ent.loadState) {
                case StageData.LoadEnum.Connect:
                    isStartConnectTask = true;
                    Connect();
                    break;
                case StageData.LoadEnum.Room:
                    isQuitToLobby = true;
                    break;
            }
        }

        private void OnStageLoadEndCallBack(M_Game.LoginGameScene ent) {
            PrintLog($"IP：{NetworkData.Config.IP}   Port: {NetworkData.Config.Port}");
            PrintLog($"IsKcp：{NetworkData.Config.IsKCP}");
            PrintLog($"Tick：{mirrorManager.sendRate}");
            if (NetworkData.IsStartClient) {
                PrintLog($"NickName：{PlayerData.NickName}");
                PrintLog($"[Client] 连接耗时:{connectTime}");
            }
        }

        private void PrintLog(string log) {
            Debug.Log(log);
            PerfAnalyzerAgent.SetLog(log);
        }

        public void Connect() {
            if (isStartConnectTask == false || mirrorManager == null || connectState == ConnectStateEnum.ConnectIng || isShowWarEnd) {
                return;
            }
            StartClient(NetworkData.Config.IsKCP, NetworkData.Config.ProxyAddr, NetworkData.Config.IP, NetworkData.Config.Port);
        }

        public void DisConnect() {
            PerfAnalyzerAgent.SetLog($"DisConnect Mirror");
            if (mirrorManager) {
                mirrorManager.StopClient();
                mirrorManager.OnClientConnectEvent -= OnClientConnect;
                mirrorManager.OnClientDisconnectEvent -= OnClientDisconnect;
                mirrorManager = null;
            }
        }

        public void StartClient(bool isKCP, string proxyAddr, string host, ushort port) {
            connectState = ConnectStateEnum.ConnectIng;
            var sign = "StartClient:" + host + " Port:" + port;
            Debug.Log(sign);
            PerfAnalyzerAgent.SetTag(sign);
            SetPort();
            if (connectTime < 0f) {
                connectTime = Time.realtimeSinceStartup;
            }
            checkTimeOut = 0f;
            mirrorManager.networkAddress = host;
            if (!isKCP && !String.IsNullOrEmpty(proxyAddr)) {
                var uri = new Uri($"{proxyAddr}/{host}:{port}");
                Debug.Log("StartClient uri:" + uri);
                mirrorManager.StartClient(uri);
            } else {
                Debug.Log("StartClient");
                mirrorManager.StartClient();
            }
        }

        private void SetPort() {
            if (mirrorManager.transport as SimpleWebTransport) {
                var transport = mirrorManager.transport as SimpleWebTransport;
                transport.port = NetworkData.Config.Port;
                Debug.Log("Transport: SimpleWeb  Port:" + NetworkData.Config.Port + " -- " + transport.sslEnabled);
            // } else if (mirrorManager.transport as ThreadedKcpTransport) {
            //     var transport = mirrorManager.transport as ThreadedKcpTransport;
            //     transport.port = NetworkData.Config.Port;
            //     if (NetworkData.Config.IsSetKcpConfig) {
            //         transport.Interval = NetworkData.Config.Interval;
            //         transport.FastResend = NetworkData.Config.FastResend;
            //         transport.SendWindowSize = NetworkData.Config.SendWindowSize;
            //         transport.ReceiveWindowSize = NetworkData.Config.ReceiveWindowSize;
            //         transport.RecvBufferSize = NetworkData.Config.RecvBufferSize;
            //         transport.SendBufferSize = NetworkData.Config.SendBufferSize;
            //         transport.MaxRetransmit = NetworkData.Config.MaxRetransmit;
            //     }
            //     Debug.Log("Transport: ThreadedKcpTransport  Port:" + NetworkData.Config.Port);
            } else {
                var transport = mirrorManager.transport as KcpTransport;
                transport.Port = NetworkData.Config.Port;
                if (NetworkData.Config.IsSetKcpConfig) {
                    transport.Interval = NetworkData.Config.Interval;
                    transport.FastResend = NetworkData.Config.FastResend;
                    transport.SendWindowSize = NetworkData.Config.SendWindowSize;
                    transport.ReceiveWindowSize = NetworkData.Config.ReceiveWindowSize;
                    // transport.RecvBufferSize = NetworkData.Config.RecvBufferSize;
                    // transport.SendBufferSize = NetworkData.Config.SendBufferSize;
                    transport.MaxRetransmit = NetworkData.Config.MaxRetransmit;
                }
                Debug.Log("Transport: KcpTransport  Port:" + NetworkData.Config.Port);
            }
        }
        
        
        
        private void OnClientConnect() {
            if (isShowWarEnd) {
                DisConnect();
                return;
            }
            var sign = "客户端连接成功 耗时：" + GetConnectTime();
            checkTimeOut = -1f;
            connectState = ConnectStateEnum.Connect;
            reConnectCount = 0;
            Debug.Log(sign);
            PerfAnalyzerAgent.SetTag(sign);
            MsgRegister.Dispatcher(new M_Network.ClientConnect());
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.Connect,
            });
        }
        
        private float GetConnectTime() {
            if (connectTime < 0f) {
                return 0f;
            }
            return Time.realtimeSinceStartup - connectTime;
        }

        private void OnClientDisconnect() {
            if (connectState == ConnectStateEnum.ReConnectIng) {
                return;
            }
            var isConnectIng = connectState == ConnectStateEnum.ConnectIng;
            connectState = ConnectStateEnum.DisConnect;
            if (isShowWarEnd || isQuitToLobby || mirrorManager == null) {
                return;
            }
            PerfAnalyzerAgent.SetTag("客户端断开连接");
            if (isConnectIng) {
                Debug.Log("[Mirror] 客户端断开连接");
                ReConnect();
            } else {
                Debug.Log("[Mirror] 客户端断开连接 返回大厅");
                TipQuitToLobby();
            }
        }
        
        

        private void ReConnect() {
            connectState = ConnectStateEnum.ReConnectIng;
            if (reConnectCount > 3) {
                Debug.LogError($"[Mirror]重连失败，返回大厅");
                TipQuitToLobby();
            } else {
                Debug.Log("[Mirror]开始重连:" + reConnectCount);
                reConnectCount++;
                mirrorManager.StopClient();
                UpdateRegister.AddInvoke(Connect, 1f);
            }
        }

        private void TipQuitToLobby() {
            MsgRegister.Dispatcher(new CM_UI.ShowDialog {
                Message = "网络断开连接，返回大厅",
                OnSure = () => {
                    MsgRegister.Dispatcher(new M_Network.ClientDisconnect());
                },
                OnCancel = null,
            });
        }
    }
}