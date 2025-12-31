using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_ChangeMatSystem : S_Ability_Base {
        private AbilityM_ChangeMat useMData;
        private AbilityIn_ChangeMat useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_ChangeMat)MData;
            useInData = (AbilityIn_ChangeMat)InData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useInData.In_LifeTime,
                CallBack = LifeTimeEnd
            });
            AddComponent<EffectChangeGPOMat>(new EffectChangeGPOMat.InitData {
                MatType = useInData.In_MatType,
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}