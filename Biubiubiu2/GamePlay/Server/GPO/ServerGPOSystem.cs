using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            GPOData.GPOIndex = 0;
            if (ModeData.IsSausageMode()) {
                AddComponent<ServerGoldDashGPOList>();
            } else {
                AddComponent<ServerGPOList>();
            }
        }
    }
}