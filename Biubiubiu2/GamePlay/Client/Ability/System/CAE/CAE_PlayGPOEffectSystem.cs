using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAE_PlayGPOEffectSystem : C_Ability_Base {
        private Proto_Ability.TargetRpc_PlayGPOEffect useInData;
        private bool isUse1P = false;
        private float scale = 1f;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_Ability.TargetRpc_PlayGPOEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityToPool(MData.GetEffectSign());
        }

        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>();
            AddComponent<ClientSetPointForTargetGPO>(new ClientSetPointForTargetGPO.InitData {
                TargetGPOId = useInData.gpoId,
            });
        }
    }
}