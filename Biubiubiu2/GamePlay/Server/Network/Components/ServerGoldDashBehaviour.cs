using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashBehaviour : ComponentBase {
        private List<INetworkCharacter> networks;

        protected override void OnAwake() {
            networks = new List<INetworkCharacter>();
            MsgRegister.Register<M_Character.SetNetwork>(OnSetNetworkCallBack);
            MsgRegister.Register<M_Network.ServerDisconnect>(OnServerDisconnectCallBack);
            MsgRegister.Register<M_Network.GetAllNetwork>(OnGetAllNetworkCallBack);
            MsgRegister.Register<M_Network.GetAllNetworkForPoint>(OnGetAllNetworkForPointCallBack);
            MsgRegister.Register<SM_Character.CmdLoginStage>(OnCmdLoginStageCallBack);
        }
        
        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Character.SetNetwork>(OnSetNetworkCallBack);
            MsgRegister.Unregister<M_Network.ServerDisconnect>(OnServerDisconnectCallBack);
            MsgRegister.Unregister<M_Network.GetAllNetwork>(OnGetAllNetworkCallBack);
            MsgRegister.Unregister<M_Network.GetAllNetworkForPoint>(OnGetAllNetworkForPointCallBack);
            MsgRegister.Unregister<SM_Character.CmdLoginStage>(OnCmdLoginStageCallBack);
            networks.Clear();
        }

        public void OnSetNetworkCallBack(M_Character.SetNetwork ent) {
            Debug.Log("注册服务器 Cmd_Login:" + ent.GetID());
            var network = ent.iNetwork;
            // network.AddProtoCallBack(Proto_Login.Cmd_Login.ID, CmdLoginCallBack);
            // network.AddProtoCallBack(Proto_Login.Cmd_Login_Stage.ID, CmdLoginStageCallBack);
            if (WarReportData.IsStartSausageWarReport) {
                MsgRegister.Dispatcher(new SM_Character.AddCharacterGoldDash {
                    INetwork = network,
                    Name = "",
                    PlayerId = Random.Range(100000, 999999),
                    TeamId = "WarReport",
                });
                networks.Add(network);
            }
        }

        public void CmdLoginCallBack(INetwork network, IProto_Doc cmdData) {
            // var loginData = (Proto_Login.Cmd_Login)cmdData;
            // MsgRegister.Dispatcher(new SM_Mode.CheckCharacterLogin {
            //     PlayerId = loginData.playerId,
            //     Mode = loginData.mode,
            //     NetworkVersion = loginData.version,
            //     CallBack = state => {
            //         LoginMode(state, loginData, network);
            //     }
            // });
        }
        
        public void LoginMode(ModeData.ModeLoginState state, Proto_Login.Cmd_Login loginData, INetwork network) {
            Debug.Log("CmdLoginCallBack:" + loginData.playerId);
            if (state != ModeData.ModeLoginState.Success) {
                Debug.LogError($"{loginData.playerId} 登录失败 {state}");
            }
            network.TargetRpc(network, new Proto_Login.TargetRpc_Login_State {
                state = (int)state
            });
        }
        
        private void OnCmdLoginStageCallBack (SM_Character.CmdLoginStage ent) {
            Debug.Log("OnCmdLoginStageCallBack:" + ent.INetwork);
            MsgRegister.Dispatcher(new SM_Character.AddCharacterGoldDash {
                INetwork = ent.INetwork,
                Name = ent.Name,
                PlayerId = ent.PlayerId,
                TeamId = ent.TeamId
            });
            networks.Add(ent.INetwork);
        }

        public void CmdLoginStageCallBack(INetwork network, IProto_Doc cmdData) {
            var loginData = (Proto_Login.Cmd_Login_Stage)cmdData;
            Debug.Log("CmdLoginStageCallBack:" + loginData.name);
            // MsgRegister.Dispatcher(new SM_Character.AddCharacter {
            //     ICmdData = loginData,
            //     INetwork = network
            // });
            networks.Add((INetworkCharacter)network);
        }
        public void OnServerDisconnectCallBack(M_Network.ServerDisconnect ent) {
            for (int i = 0; i < networks.Count; i++) {
                var network = networks[i];
                if (network.IsDestroy() || network.ConnId() == ent.ConnId) {
                    networks.RemoveAt(i);
                    MsgRegister.Dispatcher(new M_Network.DisconnectNetwork {
                        INetwork = network
                    });
                }
            }
        }

        public void OnGetAllNetworkCallBack(M_Network.GetAllNetwork ent) {
            ent.CallBack.Invoke(networks);
        }
        public void OnGetAllNetworkForPointCallBack(M_Network.GetAllNetworkForPoint ent) {
            if (ent.Distance <= -1) {
                ent.CallBack.Invoke(networks);
                return;
            }
            var result = new List<INetworkCharacter>();
            for (int i = 0; i < networks.Count; i++) {
                var network = networks[i];
                var distance = Vector3.Distance(network.GetPoint(), ent.Point);
                if (distance > ent.Distance) {
                    continue;
                }
                result.Add(network);
            }
            ent.CallBack.Invoke(result);
        }
    }
}