using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkLogin : ComponentBase {
        private INetworkCharacter localNetwork;
        private bool isLogin = false;
        private bool isReLogining = false;
        private StageData.LoadEnum loadState = StageData.LoadEnum.None;
        private IProto_Doc localRoleInfo = null;
        private Dictionary<long, IProto_Doc> roleInfoProto = new Dictionary<long, IProto_Doc>();
        private List<INetworkCharacter> checkNetworks = new List<INetworkCharacter>();

        protected override void OnAwake() {
            MsgRegister.Register<M_Character.SetNetwork>(OnSetBehaviourCallBack);
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Register<M_Network.ClientConnect>(OnClientConnectCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Character.SetNetwork>(OnSetBehaviourCallBack);
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Unregister<M_Network.ClientConnect>(OnClientConnectCallBack);
        }
        
        private void OnUpdate(float deltaTime) {
            if (checkNetworks.Count <= 0) {
                return;
            }
            var len = checkNetworks.Count - 1;
            for (int i = len; i >= 0; i--) {
                var network = checkNetworks[i];
                if (network == null || network.IsDestroy()) {
                    checkNetworks.RemoveAt(i);
                    continue;
                }
                var netSync = (ICharacterSync)network.GetNetworkSync();
                if (netSync.SyncGpoId() != 0 && netSync.SyncPlayerId() != 0 && netSync.SyncTeamId() != 0) {
                    if (roleInfoProto.ContainsKey(netSync.SyncPlayerId()) == false) {
                        continue;
                    }
                    var protoData = roleInfoProto[netSync.SyncPlayerId()];
                    if (protoData != null) {
                        if (network.IsLocalPlayer() == false || ModeData.IsSausageMode()) {
                            checkNetworks.RemoveAt(i);
                            MsgRegister.Dispatcher(new CM_Character.AddCharacter {
                                iNetwork = network,
                                InDataDoc = protoData
                            });
                        } else {
                            if (localRoleInfo != null) {
                                checkNetworks.RemoveAt(i);
                                AddLocalCharacter(network, protoData);
                            }
                        }
                    }
                }
            }
        }
        
        public void OnSetBehaviourCallBack(M_Character.SetNetwork ent) {
            var behaviour = ent.iNetwork;
            checkNetworks.Add(behaviour);
            behaviour.AddProtoCallBack(Proto_Login.TargetRpc_CharacterInfo.ID, TargetRpcCharacterCallBack);
            if (behaviour.IsLocalPlayer()) {
                behaviour.AddProtoCallBack(Proto_Login.TargetRpc_Login_State.ID, TargetRpcLoginCallBack);
                behaviour.AddProtoCallBack(Proto_Login.TargetRpc_RoleInfo.ID, TargetRpcRoleInfoCallBack);
                PerfAnalyzerAgent.SetLog("开始上传角色信息:" + PlayerData.NickName + " " + PlayerData.PlayerId);
                localNetwork = behaviour;
                if (isLogin) {
                    ReLoginWar();
                } else {
                    LoginWar();
                }
            }
        }
        
        private void TargetRpcCharacterCallBack(INetwork network, IProto_Doc rpcData) {
            var protoData = (Proto_Login.TargetRpc_CharacterInfo)rpcData;
            roleInfoProto[protoData.playerId] = rpcData;
        }

        private void OnClientConnectCallBack (M_Network.ClientConnect ent) {
            if (isLogin) {
                ReLoginWar();
            }
        }

        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            loadState = ent.loadState;
            switch (ent.loadState) {
                case StageData.LoadEnum.LoginWar:
                    LoginWar();
                    break;
                case StageData.LoadEnum.SendLoginInfo:
                    SendLoginStageInfo();
                    break;
            }
        }

        private void LoginWar() {
            if (localNetwork == null || loadState != StageData.LoadEnum.LoginWar) {
                return;
            }
            PerfAnalyzerAgent.SetLog("Network 版本号:" + GameData.NetworkVersion);
            localNetwork.Cmd(new Proto_Login.Cmd_Login() {
                playerId = PlayerData.PlayerId,
                version = GameData.NetworkVersion,
                modeId = ModeData.ModeId,
                warId = WarData.WarId
            });
        }

        private void ReLoginWar() {
            if (localNetwork == null || localNetwork.IsOnline() == false) {
                return;
            }
            isReLogining = true;
            PerfAnalyzerAgent.SetTag("重连 Cmd_Login");
            Debug.Log("重连 Cmd_Login");
            localNetwork.Cmd(new Proto_Login.Cmd_Login() {
                playerId = PlayerData.PlayerId,
                version = GameData.NetworkVersion,
                modeId = ModeData.ModeId,
                warId = WarData.WarId
            });
        }

        private void TargetRpcLoginCallBack(INetwork behaviour, IProto_Doc doc) {
            var loginData = (Proto_Login.TargetRpc_Login_State)doc;
            var loginState = (ModeData.ModeLoginState)loginData.state;
            if (loginState == ModeData.ModeLoginState.Success) {
                PerfAnalyzerAgent.SetLog("Client 登录成功，开始上传基础信息:");
                if (isReLogining) {
                    SendReLoginStageInfo();
                    isReLogining = false;
                } else {
                    SendLoginStageInfo();
                    isLogin = true;
                }
                mySystem.Dispatcher(new CE_Network.LoginServerSuccess {
                    Network = behaviour 
                });
                MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                    loadState = StageData.LoadEnum.LoginWar,
                });
            } else {
                isLogin = false;
                Debug.Log($"登录战场失败，原因: {loginState}");
                MsgRegister.Dispatcher(new CM_UI.ShowDialog {
                    // [版号申请]
                    Message = $"登录战场失败。",
                    OnSure = () => {
                        Debug.Log("登录战场失败, 退出游戏");
                        MsgRegister.Dispatcher(new CM_Game.QuitGame());
                    },
                    OnCancel = null,
                });
                MsgRegister.Dispatcher(new CM_UI.EnterWarFailed() {
                    State = loginState,
                    WarId = WarData.WarId,
                });
            }
        }

        private void SendLoginStageInfo() {
            if (isLogin == false || loadState != StageData.LoadEnum.SendLoginInfo) {
                return;
            }
            PerfAnalyzerAgent.SetLog($"Client 登录 - 场景成功 PID:{PlayerData.PlayerId} NickName:{PlayerData.NickName}");
            localRoleInfo = null;
            localNetwork.Cmd(new Proto_Login.Cmd_Login_Stage {
                name = PlayerData.NickName, playerId = PlayerData.PlayerId,
            });
        }
        
        private void SendReLoginStageInfo() {
            if (isReLogining == false) {
                return;
            }
            PerfAnalyzerAgent.SetLog($"Client 重连 - 场景成功 PID:{PlayerData.PlayerId} NickName:{PlayerData.NickName}");
            localRoleInfo = null;
            localNetwork.Cmd(new Proto_Login.Cmd_Login_Stage {
                name = PlayerData.NickName, playerId = PlayerData.PlayerId,
            });
        }

        private void TargetRpcRoleInfoCallBack(INetwork behaviour, IProto_Doc doc) {
            localRoleInfo = doc;
        }

        private void AddLocalCharacter(INetwork behaviour, IProto_Doc localInData) {
            var rpcData = (Proto_Login.TargetRpc_RoleInfo)localRoleInfo;
            localRoleInfo = null;
            PerfAnalyzerAgent.SetLog($"Client 获取角色服务器信息 TeamId:{rpcData.teamId}  GpoID: {rpcData.gpoId}");
            MsgRegister.Dispatcher(new CM_Character.AddLocalCharacter {
                iNetwork = behaviour,
                RoleInfoDoc = rpcData,
                InDataDoc = localInData,
            });
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.SendLoginInfo,
            });
        }
    }
}