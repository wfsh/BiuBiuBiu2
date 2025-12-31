using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOManager : ManagerBase {
        private ClientGPOSystem mSystem;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
            mSystem = AddSystem<ClientGPOSystem>(null);
        }

        protected override void OnClear() {
            base.OnClear();
            mSystem?.Clear();
            mSystem = null;
        }
    }
}