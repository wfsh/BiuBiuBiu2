using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlayRaySystem : C_Ability_Base {
        public Proto_AbilityAB_Auto.Rpc_PlayRay useInData;
        private AbilityM_PlayRay useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayRay)InData;
            useMData = (AbilityM_PlayRay)MData;
            iEntity.SetPoint(useInData.startPoint);
            iEntity.SetRota(Quaternion.LookRotation(useInData.direction));
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetEffectSign());
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientAbilityNetworkBehaviour>(); // NetworkBehaviour
            AddComponent<ClientNetworkTransform>();
            if (!useMData.M_IsFixScale) {
                AddComponent<ClientAbilityPlayRayEffect>();
                iEntity.SetLocalScale(Vector3.zero);
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp),
            });
        }
    }
}
