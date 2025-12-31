using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOManager : ManagerBase {
        private ServerGPOSystem mSystem;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
            mSystem = AddSystem<ServerGPOSystem>(null);
        }

        protected override void OnClear() {
            base.OnClear();
            mSystem.Clear();
            mSystem = null;
        }
    }
}