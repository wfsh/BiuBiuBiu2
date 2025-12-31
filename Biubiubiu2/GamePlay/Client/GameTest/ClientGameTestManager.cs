using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGameTestManager : ManagerBase {
        protected override void OnStart() {
            base.OnStart();
            AddSystem<ClientGameTestSystem>(null);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
        }
    }
}