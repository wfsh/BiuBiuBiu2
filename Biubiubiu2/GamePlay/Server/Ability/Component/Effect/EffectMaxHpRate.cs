using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EffectMaxHpRate : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float Value;
        }
        private IGPOAbilityEffectData effectData = null;
        private float value = 0.0f;
        private InitData initData;
        private IGPO targetGPO;

        protected override void OnAwake() {
            initData = (InitData)initDataBase;
            var ability = (S_Ability_Base)mySystem;
            targetGPO = ability.TargetGPO;
            SetValue(initData.Value);
        }

        protected override void OnStart() {
            base.OnStart();
            targetGPO.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                Effect = AbilityEffectData.Effect.GpoMaxHpRate,
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
        }

        public void SetValue(float rate) {
            value = rate;
        }
        
    }
}