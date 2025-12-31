using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;


namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_RangeFlashSystem : C_Ability_Base {
        private Proto_Ability.Rpc_RangeFlash abilityData;
        private IAbilityMData _modMData;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_RangeFlash)InData;
            _modMData = AbilityConfig.GetAbilityModData(abilityData.abilityModId);
            AddComponents();
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientChangeFlashRange>(new ClientChangeFlashRange.InitData {
                Length = abilityData.GPOId.Length - 1,
                AbilityData = abilityData,
                ModMData = _modMData,
            });
            if (string.IsNullOrEmpty(_modMData.GetEffectSign())) {
                LifeTimeEnd();
            }
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = abilityData.lifeTime * 0.1f,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}