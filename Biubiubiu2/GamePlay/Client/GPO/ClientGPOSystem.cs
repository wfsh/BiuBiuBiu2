using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            if (ModeData.IsSausageMode()) {
                AddComponent<ClientGoldDashGPOList>();
            } else {
                AddComponent<ClientGPOList>();
            }
        }
    }
}