using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraManager : ManagerBase {
        protected override void OnStart() {
            base.OnStart();
            AddCameraSystem();
        }

        private void AddCameraSystem() {
            AddSystem<CameraSystem>(null);
        }
    }
}