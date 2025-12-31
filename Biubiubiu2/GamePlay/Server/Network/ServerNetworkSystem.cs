using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddComponents() {
            AddComponent<ServerNetworkSerialize>();
            if (ModeData.IsSausageMode()) {
                AddComponent<ServerGoldDashBehaviour>();
            } else {
                AddComponent<ServerBehaviour>();
            }
            AddComponent<ServerWorldNetwork>();
            AddComponent<ServerWorldNetworkBehaviourList>();
        }
    }
}