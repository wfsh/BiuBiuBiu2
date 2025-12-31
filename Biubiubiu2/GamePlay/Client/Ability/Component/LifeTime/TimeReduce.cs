using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class TimeReduce : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float LifeTime;
            public Action CallBack;
        }
        private float lifeTime = 0.0f;
        private Action endTimeCallBack = null;
        protected override void OnAwake() {
            mySystem.Register<CE_Ability.SetLifeTime>(OnSetLifeTimeCallBack);
            var initData = (InitData)initDataBase;
            if (initData != null) {
                SetLifeTime(initData.LifeTime, initData.CallBack);
            }
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Ability.SetLifeTime>(OnSetLifeTimeCallBack);
            endTimeCallBack = null;
            RemoveUpdate(OnUpdate);
        }
        
        private void OnSetLifeTimeCallBack(ISystemMsg body, CE_Ability.SetLifeTime ent) {
            SetLifeTime(ent.LifeTime, ent.OnLifeTimeEnd);
        }

        public void SetLifeTime(float time, Action callBack) {
            if (time <= 0) {
                Debug.LogError("生命周期时间必须 > 0:");
                return;
            }
            if (callBack == null) {
                Debug.LogError("生命周期回调不能为 null");
                return;
            }
            this.endTimeCallBack = callBack;
            lifeTime = time;
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (lifeTime >= 0) {
                lifeTime -= delta;
            } else {
                RemoveUpdate(OnUpdate);
                endTimeCallBack.Invoke();
            }
        }
    }
}
