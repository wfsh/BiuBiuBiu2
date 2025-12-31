using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_SyncTransformEffect : C_Ability_Base {
        public Proto_Ability.Rpc_SyncTransformEffect useInData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_Ability.Rpc_SyncTransformEffect)InData;
            iEntity.SetPoint(useInData.startPoint);
            iEntity.SetRota(useInData.startRota);
            iEntity.SetLocalScale(useInData.startScale);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetEffectSign());
        }

        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>(); // NetworkBehaviour
            AddComponent<ClientNetworkTransform>();
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime,
            });
        }
    }
}