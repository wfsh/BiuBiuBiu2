using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIManager : ManagerBase {
        private ServerAISystem aiSystem;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            aiSystem?.Clear();
            aiSystem = null;
            MsgRegister.Unregister<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        private void OnSetWorldNetworkCallBack(SM_Network.SetWorldNetwork ent) {
            aiSystem = AddSystem(delegate(ServerAISystem monsterSystem) {
                monsterSystem.SetNetwork(ent.network);
            });
        }
    }
}