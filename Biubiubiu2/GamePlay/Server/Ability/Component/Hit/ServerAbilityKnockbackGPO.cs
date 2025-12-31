using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityKnockbackGPO : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float Duration;
            public float Force;
        }
        private float Duration = 0.5f;
        private float Force = 10f;

        protected override void OnAwake() {
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            var initData = (InitData)initDataBase;
            if (initData != null) {
                SetPower(initData.Duration, initData.Force);
            }
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            Hit((ServerGPO)entData.hitGPO);
        }

        public void SetPower(float duration, float force) {
            Duration = duration;
            Force = force;
        }

        private void Hit(ServerGPO hitGPO) {
            if (hitGPO.IsClear()) {
                return;
            }
            hitGPO.Dispatcher(new SE_GPO.Event_KnockbackGPO {
                Duration = Duration,
                Speed = Force,
                Dir = hitGPO.GetPoint() - iEntity.GetPoint(),
            });
        }
    }
}