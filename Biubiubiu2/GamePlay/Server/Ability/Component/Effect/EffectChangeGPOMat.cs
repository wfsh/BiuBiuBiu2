using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EffectChangeGPOMat : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public byte MatType;
        }
        private IGPOAbilityEffectData effectData = null;
        private byte matType = 0;

        protected override void OnAwake() {
            var initData = (InitData)initDataBase;
            SetMatType(initData.MatType);
        }

        protected override void OnStart() {
            base.OnStart(); 
            var ability = (S_Ability_Base)mySystem;
            var targetGPO = ability.TargetGPO;
            targetGPO.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                Effect = AbilityEffectData.Effect.ChangeMatType,
                Value = this.matType,
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

        public void SetMatType(byte matType) {
            this.matType = matType;
        }
    }
}