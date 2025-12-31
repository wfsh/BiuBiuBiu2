using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonFireEffect : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityIn_AuroraDragonFireEffect Param;
        }
        private SAB_AuroraDragonFireEffectSystem abSystem;
        private AbilityIn_AuroraDragonFireEffect config;
        private ServerGPO fireGPO;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonFireEffectSystem)mySystem;
            fireGPO = abSystem.FireGPO;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            fireGPO.Register<SE_AI_AuroraDragon.Event_AuroraDragonFireEffectScale>(OnEffectScaleCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            SetPoint();
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO.Unregister<SE_AI_AuroraDragon.Event_AuroraDragonFireEffectScale>(OnEffectScaleCallBack);
            fireGPO = null;
        }
        
        private void OnEffectScaleCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_AuroraDragonFireEffectScale ent) {
            abSystem.GetEntity().SetLocalScale(ent.Scale);
        }

        private void SetPoint() {
            if (config.In_AttackBoxTran != null) {
                iEntity.SetPoint(config.In_AttackBoxTran.position);
                if (!config.In_IsFollowHead) {
                    iEntity.SetLocalScale(config.In_StartScale);
                    iEntity.SetPoint(config.In_AttackBoxTran.position + config.In_StartPos);
                    iEntity.SetRota(Quaternion.Euler(fireGPO.GetRota().eulerAngles + config.In_StartRota.eulerAngles));
                }
            }
        }
    }
}