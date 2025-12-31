using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class NetworkConnectManager : ManagerBase {
        private SystemBase mySystem;
        protected override void OnAwake() {
            base.OnAwake();
            if (ModeData.IsSausageMode()) {
                mySystem = AddSystem<GoldDashMirrorConnectSystem>(null);
            } else {
                mySystem = AddSystem<MirrorConnectSystem>(null);
            }
        }

        protected override void OnClear() {
            base.OnClear();
            if (mySystem != null) {
                mySystem.Clear();
                mySystem = null;
            }
        }
    }
}