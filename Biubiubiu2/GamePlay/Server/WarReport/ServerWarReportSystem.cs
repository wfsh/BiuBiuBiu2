using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWarReportSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerWarReport>();
            AddComponent<ServerWarReportSave>();
        }
    }
}