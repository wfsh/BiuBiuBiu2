using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerShortcutToolSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerCreateWebSocket>();
            AddComponent<ServerShortcutTool>();
        }
    }
}