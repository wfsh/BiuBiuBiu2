using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_EffectFollowServerTransformSystem : C_Ability_Base {
        private Proto_Ability.Rpc_EffectFollowServerTransform abilityData;
        private AbilityData.PlayAbility_TrackingMissle modData;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_EffectFollowServerTransform)InData;
            modData = (AbilityData.PlayAbility_TrackingMissle)AbilityConfig.GetAbilityModData(abilityData.abilityModId);
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityToPool(modData.M_EffectSign);
        }
        

        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>(); // NetworkBehaviour
            AddComponent<ClientNetworkTransform>();
        }
    }
}