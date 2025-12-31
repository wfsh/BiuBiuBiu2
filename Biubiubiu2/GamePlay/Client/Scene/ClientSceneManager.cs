using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSceneManager : ManagerBase {
        private ClientSceneSystem sceneSystem;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            if (sceneSystem == null) {
                sceneSystem = AddSystem(delegate(ClientSceneSystem monsterSystem) {
                    monsterSystem.SetNetwork(ent.iNetwork);
                });
            } else {
                sceneSystem.SetNetwork(ent.iNetwork);
            }
        }
    }
}