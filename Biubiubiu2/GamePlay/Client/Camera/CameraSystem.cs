using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraSystem : SystemBase {
        protected override void OnAwake() {
            AddComponents();
        }

        protected void AddComponents() {
            AddComponent<CameraMove>();
            AddComponent<CameraFar>();
            AddComponent<CameraRota>();
            AddComponent<GetCameraCenterGPO>();
            AddComponent<CameraAutoLockTarget>();
            AddComponent<CameraCollisionHandler>();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityObj("Camera/BaseCamera", StageData.GameWorldLayerType.Character);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("镜头加载失败");
            }
        }
    }
}