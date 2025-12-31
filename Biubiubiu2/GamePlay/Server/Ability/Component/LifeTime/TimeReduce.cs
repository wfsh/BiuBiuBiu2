using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class TimeReduce : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float LifeTime;
            public Action CallBack;
        }
        private float lifeTime = 0.0f;
        private float countLifeTime = 0.0f;
        private Action endTimeCallBack = null;
        protected override void OnAwake() {
            mySystem.Register<SE_Ability.SetLifeTime>(OnSetLifeTimeCallBack);
            mySystem.Register<SE_Ability.GetCountLifeTime>(OnGetCountLifeTimeCallBack);
            mySystem.Register<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
            if (initDataBase != null) {
                var initData = (InitData)initDataBase;
                SetLifeTime(initData.LifeTime, initData.CallBack);
            }
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.GetCountLifeTime>(OnGetCountLifeTimeCallBack);
            mySystem.Unregister<SE_Ability.SetLifeTime>(OnSetLifeTimeCallBack);
            mySystem.Unregister<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
            endTimeCallBack = null;
            RemoveUpdate(OnUpdate);
        }
        
        private void OnResetAbilityEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_ResetAbilityEffect ent) {
            countLifeTime = lifeTime;
        }
        
        private void OnSetLifeTimeCallBack(ISystemMsg body, SE_Ability.SetLifeTime ent) {
            SetLifeTime(ent.LifeTime, ent.OnLifeTimeEnd);
        }

        public void SetLifeTime(float time, Action callBack) {
            if (time <= 0) {
                var ability = (S_Ability_Base)mySystem;
                Debug.LogError("生命周期时间必须 > 0:" + (ability.AbilityTypeID) + " " + ability.RowId);
                return;
            }
            if (callBack != null) {
                this.endTimeCallBack = callBack;
            }
            lifeTime = time;
            countLifeTime = time;
        }
        
        private void OnGetCountLifeTimeCallBack(ISystemMsg body, SE_Ability.GetCountLifeTime ent) {
            ent.CallBack.Invoke(countLifeTime);
        }

        public void SetLifeTime(float time) {
            countLifeTime = time;
        }

        private void OnUpdate(float delta) {
            if (countLifeTime >= 0) {
                countLifeTime -= delta;
            } else {
                RemoveUpdate(OnUpdate);
                Dispatcher(new SE_Ability.Ability_LifeTimeEnd());
                endTimeCallBack.Invoke();
            }
        }
    }
}
