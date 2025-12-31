using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EffectHurtValueRate : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float Value;
        }
        private IGPOAbilityEffectData effectData = null;
        private float value = 0.0f;
        private InitData initData;

        protected override void OnAwake() {
            mySystem.Register<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
            initData = (InitData)initDataBase;
            SetValue(initData.Value);
        }

        protected override void OnStart() {
            base.OnStart(); 
            var ability = (S_Ability_Base)mySystem;
            var targetGPO = ability.TargetGPO;
            targetGPO.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                Effect = AbilityEffectData.Effect.GpoHurtValueRate,
                Value = value,
                CallBack = (data) => {
                    effectData = data;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            effectData?.Remove();
            effectData = null;
            mySystem.Unregister<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
        }

        private void OnResetAbilityEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_ResetAbilityEffect ent) {
        }

        public void SetValue(float rate) {
            value = rate;
        }
    }
}