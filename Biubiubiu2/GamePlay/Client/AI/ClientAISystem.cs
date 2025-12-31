using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAISystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }
        
        private void AddComponents() {
            AddComponent<ClientAIWorld>();
        }
    }
}