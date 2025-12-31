using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityStrikeFlyGPO : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float Duration = 2f;
            public float Force = 5f;
        }
        private float Duration = 2f;
        private float Force = 5f;

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

        public void SetPower(float duration, float force) {
            Duration = duration;
            Force = force;
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            Hit((ServerGPO)entData.hitGPO);
        }

        private void Hit(ServerGPO hitGPO) {
            if (hitGPO.IsClear()) {
                return;
            }
            hitGPO.Dispatcher(new SE_GPO.Event_StrikeFlyGPO {
                Duration = Duration, 
                Force = Force, 
                Dir = hitGPO.GetPoint() - iEntity.GetPoint(),
            });
        }
    }
}