using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_GoldJokerFollowEffect : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_GoldJokerFollowEffect useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_GoldJokerFollowEffect)InData;
            iEntity.SetPoint(useInData.startPoint);
            iEntity.SetRota(Quaternion.Euler(useInData.startRot));
            iEntity.SetLocalScale(useInData.startScale);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetEffectSign());
        }
        
        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>();
            AddComponent<ClientNetworkTransform>();
            AddLifeTime();
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp),
            });
        }
    }
}
