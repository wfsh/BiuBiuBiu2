using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGameTestSystem : SystemBase {
        protected override void OnAwake() {
            AddComponent<GameTestControl>();
        }
    }
}