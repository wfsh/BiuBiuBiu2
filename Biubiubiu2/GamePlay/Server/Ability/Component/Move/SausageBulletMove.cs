using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class SausageBulletMove : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityData.PlayAbility_SausageBullet.BulletSpeedAndGravity[] speedAndGravity;
            public Vector3 startPoint;
            public float speedRatio;
        }
        private AbilityData.PlayAbility_SausageBullet.BulletSpeedAndGravity[] speedAndGravity;
        private int speedAnmGravityIndex;
        private float currentBulletDownSpeed;
        private Vector3 movePos;
        private float flyTime;
        private float speedRatio;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.speedAndGravity, initData.startPoint, initData.speedRatio);
        }

        protected override void OnStart() {
            base.OnStart();
            UpdateRegister.AddFixedUpdate(OnFixedUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            UpdateRegister.RemoveFixedUpdate(OnFixedUpdate);
        }

        public void SetData(AbilityData.PlayAbility_SausageBullet.BulletSpeedAndGravity[] list, Vector3 startPoint, float speedRatio) {
            this.speedAndGravity = list;
            this.movePos = startPoint;
            this.speedRatio = speedRatio;
        }

        private void OnFixedUpdate(float delta) {
            var len = speedAndGravity.Length - 1;
            var data = speedAndGravity[speedAnmGravityIndex];
            var fixedDeltaTime = Time.fixedDeltaTime;
            flyTime += fixedDeltaTime;
            currentBulletDownSpeed += data.gravity * fixedDeltaTime;
            movePos += (iEntity.GetForward() * data.bulletSpeed * speedRatio) * fixedDeltaTime;
            movePos += (-Vector3.up * currentBulletDownSpeed) * fixedDeltaTime;
            if (flyTime > data.flyTime * speedRatio && speedAnmGravityIndex < len) {
                speedAnmGravityIndex++;
            }
            iEntity.SetPoint(movePos);
        }
    }
}