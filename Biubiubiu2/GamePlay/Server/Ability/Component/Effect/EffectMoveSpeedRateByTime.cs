using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EffectMoveSpeedRateByTime : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float InitSpeedRate;
            public float TargetSpeedRate;
            public float ReachTargetSpeedRateTime;
        }
        private IGPOAbilityEffectData effectData = null;
        private float speedRate = 0;
        private float reachTargetSpeedRateTime = 0;
        private InitData initData;
        private float deltaSpeedRate = 0;
        private IGPO targetGPO;

        protected override void OnAwake() {
            mySystem.Register<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
            initData = (InitData)initDataBase;
            speedRate = initData.InitSpeedRate;
            deltaSpeedRate = initData.TargetSpeedRate - initData.InitSpeedRate;
            reachTargetSpeedRateTime = initData.ReachTargetSpeedRateTime;
        }

        protected override void OnStart() {
            base.OnStart(); 
            var ability = (S_Ability_Base)mySystem;
            targetGPO = ability.TargetGPO;
            targetGPO.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                Effect = AbilityEffectData.Effect.GpoMoveSpeedRate,
                Value = speedRate,
                CallBack = (data) => {
                    effectData = data;
                }
            });
            
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (reachTargetSpeedRateTime > -5) {
                if (reachTargetSpeedRateTime <= 0) {
                    reachTargetSpeedRateTime = -10;
                    speedRate = initData.TargetSpeedRate;
                } else {
                    reachTargetSpeedRateTime -= deltaTime;
                    speedRate = initData.InitSpeedRate + deltaSpeedRate * (initData.ReachTargetSpeedRateTime - reachTargetSpeedRateTime)/initData.ReachTargetSpeedRateTime;
                }
                effectData?.Remove();
                effectData = null;
                targetGPO.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                    Effect = AbilityEffectData.Effect.GpoMoveSpeedRate,
                    Value = speedRate,
                    CallBack = (data) => {
                        effectData = data;
                    }
                });
            }
        }

        protected override void OnClear() {
            base.OnClear();
            effectData?.Remove();
            effectData = null;
            mySystem.Unregister<SE_AbilityEffect.Event_ResetAbilityEffect>(OnResetAbilityEffectCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnResetAbilityEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_ResetAbilityEffect ent) {
        }
    }
}