using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSceneSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ClientSceneElement>();
        }
    }
}