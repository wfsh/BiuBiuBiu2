using System;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityLifeCycle : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float Duration;
            public Action EndCallBack;
        }
        private float lifeTime = 0.0f;
        private Action endTimeCallBack = null;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetLifeCycle(initData.Duration, initData.EndCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            endTimeCallBack = null;
            RemoveUpdate(OnUpdate);
        }

        public void SetLifeCycle(float time, Action callBack = null) {
            lifeTime = time;
            endTimeCallBack = callBack;
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (lifeTime >= 0) {
                lifeTime -= delta;
            } else {
                RemoveUpdate(OnUpdate);
                LifeTimeEnd();
            }
        }

        private void LifeTimeEnd() {
            var abSystem = (C_Ability_Base)mySystem;
            endTimeCallBack?.Invoke();
            abSystem.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = abSystem.GetAbilityId()
            });
        }
    }
}