using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 本地操控直升机
    /// </summary>
    public class ClientAIHelicopterPropellerRotatio : ComponentBase {
        public Transform propellerTransformUp; // 螺旋桨 Transform
        public Transform propellerTransformLeft; // 螺旋桨 Transform
        public float propellerRotationSpeed = 1000f; // 螺旋桨旋转速度

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var list = iEntity.GetBodyTranList(GPOData.PartEnum.Object);
            for (int i = 0; i < list.Count; i++) {
                var tran = list[i];
                if (tran.name == "Helicopter_02") {
                    propellerTransformUp = tran;
                } else{
                    propellerTransformLeft = tran;
                }
            }
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            propellerTransformUp = null;
            propellerTransformLeft = null;
        }

        private void OnUpdate(float delta) {
            RotatePropeller(delta);
        }

        /// <summary>
        /// 让螺旋桨旋转
        /// </summary>
        private void RotatePropeller(float delta) {
            if (propellerTransformUp != null) {
                propellerTransformUp.Rotate(Vector3.up, propellerRotationSpeed * delta, Space.Self);
            }
            if (propellerTransformLeft != null) {
                propellerTransformLeft.Rotate(Vector3.left, propellerRotationSpeed * delta, Space.Self);
            }
        }
    }
}