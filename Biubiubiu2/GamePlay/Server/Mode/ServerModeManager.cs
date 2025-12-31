using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeManager : ManagerBase {
        private ServerModeSystem modeSystem;

        protected override void OnAwake() {
            base.OnAwake();
            ModeData.SetIsIntoMode(true);
            MsgRegister.Register<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            modeSystem.Clear();
            modeSystem = null;
            MsgRegister.Unregister<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        private void OnSetWorldNetworkCallBack(SM_Network.SetWorldNetwork ent) {
            modeSystem = AddSystem(delegate(ServerModeSystem monsterSystem) {
                monsterSystem.SetNetwork(ent.network);
            });
        }
    }
}