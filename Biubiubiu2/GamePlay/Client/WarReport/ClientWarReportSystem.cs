using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWarReportSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ClientWarReport>();
            AddComponent<ClientWarReportPlay>();
        }
    }
}