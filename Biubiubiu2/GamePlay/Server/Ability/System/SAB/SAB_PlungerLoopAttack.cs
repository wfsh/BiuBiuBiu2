using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlungerLoopAttack : S_Ability_Base {
        private AbilityData.PlayAbility_PlungerAttackLoop inData;
        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_PlungerAttackLoop)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(inData.M_EffectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddComponent<AbilityPlungerLoopAttack>();
            AddComponent<ServerAbilityKnockbackGPO>();
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = inData.M_Power,
                WeaponItemId = 0,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = inData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}