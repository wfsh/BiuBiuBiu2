using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerRocketBombSystem : S_Ability_Base {
        public AbilityIn_GoldJokerRocketBomb useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_GoldJokerRocketBomb)InData;
            AddComponents();
        }
        
        protected override void AddComponents() {
            var param = useInData.In_Param;
            AddComponent<ServerAbilitySync>();
            AddComponent<AbilityGoldJokerRocketBomb>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = param.M_LifeTime[useInData.In_Index],
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = param.M_ATK[useInData.In_Index],
                WeaponItemId = 0,
            });
        }
    }
}
