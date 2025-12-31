using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BatLoopAttack : S_Ability_Base {
        private AbilityData.PlayAbility_BatLoopAttack inData;
        protected override void OnStart() {
            base.OnStart();
            inData = (AbilityData.PlayAbility_BatLoopAttack)MData;
            CreateEntity(inData.M_EffectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddComponent<AbilityBatLoopAttack>();
            AddComponent<ServerAbilityKnockbackGPO>();
            AddComponent<ServerAbilityHurtGPO>( new ServerAbilityHurtGPO.InitData {
                Power = inData.M_Power,
                WeaponItemId = 0,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
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