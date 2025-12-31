using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class GoldDashMirrorConnectSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            if (NetworkData.IsStartServer) {
                AddComponent<ServerSpawnPrefabs>();
            }
        }
    }
}
