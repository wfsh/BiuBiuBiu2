using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using kcp2k;
using Mirror;
using Mirror.SimpleWeb;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MirrorConnect : ComponentBase {
        private MirrorEntity entity;
        private MirrorManager mirrorManager;
        private SimpleWebTransport webTransport;
        private KcpTransport kcpTransport;
        private bool isTaskLoadStart = false;
        private bool isShowWarEnd = false;
        private bool isEnableSSL = false;
        private string certUrl = "";

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Register<M_Game.ShowWarEnd>(OnShowWarEndCallBack);
            MsgRegister.Register<M_Game.LoginGameScene>(OnStageLoadEndCallBack);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            entity = (MirrorEntity)iEntity;
            mirrorManager = entity.manager;
            mirrorManager.OnServerConnectEvent += OnServerConnect;
            mirrorManager.OnServerDisconnectEvent += OnServerDisconnect;
            mirrorManager.OnClientConnectEvent += OnClientConnect;
            mirrorManager.OnClientDisconnectEvent += OnClientDisconnect;
            Connect();
        }

        protected override void OnClear() {
            base.OnClear();
            DisConnect();
            MsgRegister.Unregister<M_Game.ShowWarEnd>(OnShowWarEndCallBack);
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Unregister<M_Game.LoginGameScene>(OnStageLoadEndCallBack);
            entity = null;
        }

        private void OnShowWarEndCallBack(M_Game.ShowWarEnd ent) {
            isShowWarEnd = true;
        }

        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            switch (ent.loadState) {
                case StageData.LoadEnum.Connect:
                    isTaskLoadStart = true;
                    Connect();
                    break;
            }
        }

        private void OnStageLoadEndCallBack(M_Game.LoginGameScene ent) {
            PrintLog($"IP：{NetworkData.Config.IP}   Port: {NetworkData.Config.Port}");
            PrintLog($"IsKcp：{NetworkData.Config.IsKCP}");
            if (NetworkData.IsStartClient) {
                PrintLog($"NickName：{PlayerData.NickName}");
            }
        }

        private void PrintLog(string log) {
            Debug.Log(log);
            PerfAnalyzerAgent.SetLog(log);
        }

        public void Connect() {
            if (isTaskLoadStart == false || mirrorManager == null) {
                return;
            }
            if (NetworkData.IsStartClient && NetworkData.IsStartServer) {
                StartHost();
            } else {
                StartServer();
            }
        }

        public void DisConnect() {
            if (mirrorManager == null) {
                return;
            }
            PerfAnalyzerAgent.SetLog($"DisConnect Mirror");
            if (NetworkData.IsStartClient && NetworkData.IsStartServer) {
                mirrorManager.StopHost();
            } else {
                mirrorManager.StopServer();
            }
            mirrorManager.OnServerConnectEvent -= OnServerConnect;
            mirrorManager.OnServerDisconnectEvent -= OnServerDisconnect;
            mirrorManager.OnClientConnectEvent -= OnClientConnect;
            mirrorManager.OnClientDisconnectEvent -= OnClientDisconnect;
            mirrorManager = null;
        }

        public void StartServer() {
            Debug.Log("StartServer ISKcp:" + NetworkData.Config.IsKCP);
            PerfAnalyzerAgent.SetTag("StartServer");
            LoadConfig();
            SetPort();
            mirrorManager.StartServer();
            StartServerSuccess();
        }

        public void StartHost() {
            Debug.Log("StartHost" + NetworkData.Config.IsKCP);
            PerfAnalyzerAgent.SetTag("StartHost");
            SetPort();
            mirrorManager.StartHost();
            StartServerSuccess();
        }

        private void StartServerSuccess() {
            if (NetworkServer.active) {
                MsgRegister.Dispatcher(new M_Network.SetSpawnPrefabs {
                    SpawnPrefabs = mirrorManager.spawnPrefabs
                });
                MsgRegister.Dispatcher(new M_Network.ServerStart());
            }
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.Connect,
            });
        }

        private void SetPort() {
            if (mirrorManager.transport as SimpleWebTransport) {
                webTransport = mirrorManager.transport as SimpleWebTransport;
                webTransport.port = NetworkData.Config.Port;
                webTransport.sslEnabled = isEnableSSL;
                webTransport.sslCertJson = certUrl;
                Debug.Log("Transport: SimpleWeb  Port:" + NetworkData.Config.Port + " -- " + webTransport.sslEnabled);
            } else {
                kcpTransport = mirrorManager.transport as KcpTransport;
                kcpTransport.Port = NetworkData.Config.Port;
                if (NetworkData.Config.IsSetKcpConfig) {
                    kcpTransport.Interval = NetworkData.Config.Interval;
                    kcpTransport.FastResend = NetworkData.Config.FastResend;
                    kcpTransport.SendWindowSize = NetworkData.Config.SendWindowSize;
                    kcpTransport.ReceiveWindowSize = NetworkData.Config.ReceiveWindowSize;
                    // kcpTransport.RecvBufferSize = NetworkData.Config.RecvBufferSize;
                    // kcpTransport.SendBufferSize = NetworkData.Config.SendBufferSize;
                    kcpTransport.MaxRetransmit = NetworkData.Config.MaxRetransmit;
                }
                Debug.Log("Transport: ThreadedKcpTransport  Port:" + NetworkData.Config.Port);
            }
        }

        public void LoadConfig() {
#if UNITY_EDITOR
            isEnableSSL = true;
            certUrl = Application.streamingAssetsPath + "/cert.json";
#else
            var configFile = Application.dataPath + "/config.json";
            if (!File.Exists(configFile)) {
                Debug.LogError("config.json not found");
                return;
            }
            var conf = (Hashtable)MiniJSON.jsonDecode(File.ReadAllText(configFile));
            isEnableSSL = Convert.ToBoolean(conf["SSL"].ToString());
            certUrl = Application.dataPath + "/cert.json";
#endif
        }

        private void OnServerConnect(NetworkConnectionToClient conn) {
            Debug.Log("服务器连接成功" + conn.connectionId + " Time :" + Time.realtimeSinceStartup);
        }

        private void OnServerDisconnect(NetworkConnectionToClient conn) {
            Debug.Log("服务器断开连接:" + conn.connectionId + " Time :" + Time.realtimeSinceStartup);
            MsgRegister.Dispatcher(new M_Network.ServerDisconnect {
                ConnId = conn.connectionId
            });
        }

        private void OnClientConnect() {
            Debug.Log(sign);
            PerfAnalyzerAgent.SetTag(sign);
            MsgRegister.Dispatcher(new M_Network.ClientConnect());
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.Connect,
            });
        }

        private void OnClientDisconnect() {
            if (isShowWarEnd) {
                return;
            }
            Debug.Log("客户端断开连接");
            PerfAnalyzerAgent.SetTag("客户端断开连接");
            MsgRegister.Dispatcher(new M_Network.ClientDisconnect {
            });
        }
    }
}