using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_SplitConeSystem : C_Ability_Base {
        private const float DURATION = 1;

        protected override void OnAwake() {
            var rpcData = (Proto_Ability.Rpc_SplitCone)InData;
            iEntity.SetPoint(rpcData.startPoint);
            iEntity.SetRota(rpcData.startRota);
            AddComponent<ClientAbilitySplitCone>(new ClientAbilitySplitCone.InitData {
                FireGpoId = FireGpoId,
            });
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = DURATION,
                CallBack = LifeTimeEnd
            });
        }

        protected override void OnStart() {
            var config = (AbilityData.PlayAbility_SplitCone)AbilityConfig.GetAbilityModData(AbilityConfig.BulletSplitCone);
            CreateEntityToPool(config.GetEffectSign());
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}