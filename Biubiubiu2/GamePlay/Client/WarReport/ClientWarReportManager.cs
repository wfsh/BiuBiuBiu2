using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWarReportManager : ManagerBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddSystem<ClientWarReportSystem>(null);
        }

        protected override void OnClear() {
            base.OnClear();
        }
    }
}