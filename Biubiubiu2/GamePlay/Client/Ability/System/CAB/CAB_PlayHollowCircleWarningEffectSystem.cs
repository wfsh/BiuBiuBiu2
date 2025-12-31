using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlayHollowCircleWarningEffectSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayHollowCircleWarningEffect useInData;
        private AbilityM_PlayHollowCircleWarningEffect useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayHollowCircleWarningEffect)InData;
            useMData = (AbilityM_PlayHollowCircleWarningEffect)MData;
            iEntity.SetPoint(useInData.startPoint);
            iEntity.SetLocalScale(Vector3.one * useInData.maxDistance);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetEffectSign());
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }

        private void AddComponents() {
            AddComponent<ClientAbilityLifeCycle>( new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime,
                EndCallBack = null
            });
            AddComponent<ClientGoldJokerPlayHollowCircleWarning>(new ClientGoldJokerPlayHollowCircleWarning.InitData {
                AttackOffset = useInData.attackOffset / 100f
            });
        }
    }
}
