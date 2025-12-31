
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ClientNetworkSerialize>();
            AddComponent<ClientNetworkLogin>();
            // AddComponent<ClientNetworkSync>();
            AddComponent<ClientWorldNetworkBehaviourList>();
            // AddComponent<ClientNetworkPhysicsPing>();
        }
    }
}
