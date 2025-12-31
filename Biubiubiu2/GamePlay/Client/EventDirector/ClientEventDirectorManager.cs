using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientEventDirectorManager : ManagerBase {
        private ClientEventDirectorSystem modeSystem;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            modeSystem = null;
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            if (modeSystem != null) {
                modeSystem.SetNetwork(ent.iNetwork);
            } else {
                modeSystem = AddSystem(delegate(ClientEventDirectorSystem monsterSystem) {
                    monsterSystem.SetNetwork(ent.iNetwork);
                });
            }
        }
    }
}