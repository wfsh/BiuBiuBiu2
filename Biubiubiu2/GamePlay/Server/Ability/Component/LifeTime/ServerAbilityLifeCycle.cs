using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityLifeCycle : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float LifeTime;
            public Action EndTimeCallBack;
        }
        public float LifeTime { get; private set; }
        private Action endTimeCallBack = null;

        protected override void OnAwake() {
            Register<SE_Ability.SetLifeCycleDuration>(OnSetLifeCycleDuration);
            Register<SE_Ability.GetLifeTime>(OnGetLifeTime);
            var initData = (InitData)initDataBase;
            SetLifeCycle(initData.LifeTime, initData.EndTimeCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            endTimeCallBack = null;
            Unregister<SE_Ability.SetLifeCycleDuration>(OnSetLifeCycleDuration);
            Unregister<SE_Ability.GetLifeTime>(OnGetLifeTime);
            RemoveUpdate(OnUpdate);
        }

        public void SetLifeCycle(float time, Action callBack = null) {
            if (time <= 0) {
                Debug.LogError("生命周期时间必须 > 0:");
                return;
            }
            ChangeLifeCycle(time);
            endTimeCallBack = callBack;
            AddUpdate(OnUpdate);
        }
        
        private void OnGetLifeTime(ISystemMsg body, SE_Ability.GetLifeTime ent) {
            ent.CallBack.Invoke(LifeTime);
        }

        public void ChangeLifeCycle(float time) {
            LifeTime = time;
        }

        private void OnUpdate(float delta) {
            if (LifeTime >= 0) {
                LifeTime -= delta;
            } else {
                RemoveUpdate(OnUpdate);
                LifeTimeEnd();
            }
        }

        private void LifeTimeEnd() {
            var abSystem = (IAbilitySystem)mySystem;
            endTimeCallBack?.Invoke();
            MsgRegister.Dispatcher(new SM_Ability.BeforeRemoveAbility() {
                abSystem = abSystem,
            });
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = abSystem.GetAbilityId(),
            });
        }

        private void OnSetLifeCycleDuration(ISystemMsg body, SE_Ability.SetLifeCycleDuration ent) {
            LifeTime = ent.LifeTime;
        }
    }
}