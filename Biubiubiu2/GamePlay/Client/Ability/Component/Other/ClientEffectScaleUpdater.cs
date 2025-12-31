using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientEffectScaleUpdater : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Vector3 ScaleChangeSpeed;
        }
        private Vector3 scaleChangeSpeed;

        protected override void OnAwake() {
            var initData = (InitData)initDataBase;
            SetScaleChangeSpeed(initData.ScaleChangeSpeed);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (iEntity == null) {
                return;
            }

            Vector3 curScale = iEntity.GetLocalScale();
            iEntity.SetLocalScale(curScale + scaleChangeSpeed * delta);
        }

        public void SetScaleChangeSpeed(Vector3 changeSpeed) {
            scaleChangeSpeed = changeSpeed;
        }
    }
}