using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGatlingGunFire : ClientAttackGunFire {
        private Color originalColor = Color.white; // 原始颜色
        private Color overheatedColor = Color.red; // 过热颜色
        private Transform gunMuzzle; // 枪口模型
        private Material targetMaterial; // 枪口材质
        private float rotationSpeed = 0f; // 当前旋转速度
        private float animSpeed = 500f; // 动画速度

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(UpdateFireVisuals);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            gunMuzzle = iEntity.GetBodyTran(GPOData.PartEnum.Head);
            var renderer = gunMuzzle.GetComponent<Renderer>();
            targetMaterial = renderer.material;
            originalColor = Color.white;
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(UpdateFireVisuals);
            gunMuzzle = null;
        }

        private void UpdateFireVisuals(float deltaTime) {
            if (gunMuzzle == null) return;
            // 控制旋转速度
            if (isDownFire && isReload == false && bulletCount > 0 && isCoolingDown == false) {
                rotationSpeed = Mathf.Lerp(rotationSpeed, animSpeed, deltaTime * 2f); // 加速到最高旋转速度
            } else {
                rotationSpeed = Mathf.Lerp(rotationSpeed, 0f, deltaTime * 2f); // 慢慢停止旋转
            }
            gunMuzzle.Rotate(Vector3.forward, rotationSpeed * deltaTime);
            if (targetMaterial != null) {
                // 动态修改 _BaseColor
                var t = 0f;
                if (isCoolingDown) {
                    t = Mathf.Clamp01(fireOverHotCDTimer / gunData.FireOverHotCDTime);
                } else {
                    t = Mathf.Clamp01(fireOverHotTimer / gunData.FireOverHotTime);
                }
                var newColor = Color.Lerp(originalColor, overheatedColor, t); // 从白色到红色渐变
                targetMaterial.SetColor("_BaseColor", newColor); // 设置材质颜色
            }
        }
    }
}