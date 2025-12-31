using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;


namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWorldNetwork : ComponentBase {
        private GameObject networkGameObj = null;

        protected override void OnAwake() {
            MsgRegister.Register<M_Network.ServerStart>(OnServerConnectSuccess);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.ServerStart>(OnServerConnectSuccess);
        }

        private void OnServerConnectSuccess(M_Network.ServerStart ent) {
            MsgRegister.Dispatcher(new M_Network.Spawn {
                CallBack = OnAddNetworkCallBack,
                Sign = NetworkData.Spawn_WorldNetwork
            });
        }

        private void OnAddNetworkCallBack(GameObject gameObj) {
            networkGameObj = gameObj;
            var networkBase = networkGameObj.GetComponent<INetwork>();
            var networkSync = networkGameObj.GetComponent<IWorldSync>();
            MsgRegister.Dispatcher(new SM_Network.SetWorldNetwork {
                network = networkBase,
                worldSync = networkSync
            });
        }
    }
}