using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerItemManager : ManagerBase {
        private ServerItemSystem itemsSystem;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            itemsSystem?.Clear();
            itemsSystem = null;
            MsgRegister.Unregister<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
        }

        private void OnSetWorldNetworkCallBack(SM_Network.SetWorldNetwork ent) {
            itemsSystem = AddSystem(delegate(ServerItemSystem monsterSystem) {
                monsterSystem.SetNetwork(ent.network);
            });
        }
    }
}