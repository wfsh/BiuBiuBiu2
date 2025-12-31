
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilitySystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }
        private void AddComponents() {
            AddComponent<ClientNetworkAbilitySync>();
        }
    }
}
