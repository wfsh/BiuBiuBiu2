using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientItemManager : ManagerBase {
        private ClientItemSystem itemsSystem;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            if (itemsSystem != null) {
                itemsSystem.Clear();
                itemsSystem = null;
            }
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            itemsSystem = AddSystem(delegate(ClientItemSystem monsterSystem) {
                monsterSystem.SetNetwork(ent.iNetwork);
            });
        }
    }
}