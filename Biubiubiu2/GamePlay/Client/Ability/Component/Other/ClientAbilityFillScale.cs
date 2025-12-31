using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityFillScale : ComponentBase {
        private Proto_Ability.Rpc_PlayFillScaleEffect abilityData;
        private float fillSpeed;
        private Vector3 scale;
        private Vector3 endScale;

        protected override void OnStart() {
            var abSystem = (CAB_PlayFillScaleEffectSystem)mySystem;
            abilityData = abSystem.useInData;
            if (abilityData.fillTime > 0) {
                fillSpeed = abilityData.endScale.z / abilityData.fillTime;
            }
            endScale = abilityData.endScale * 0.1f;
            scale = iEntity.GetLocalScale();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (fillSpeed > 0) {
                var fillLen = fillSpeed * delta;
                if (abilityData.isCircleFill) {
                    scale += Vector3.one * fillLen;
                } else {
                    scale.z += fillLen;
                }
                
                if (scale.z >= endScale.z) {
                    scale = endScale;
                }
            } else {
                scale = endScale;
            }
            
            iEntity.SetLocalScale(scale);
        }
    }
}
