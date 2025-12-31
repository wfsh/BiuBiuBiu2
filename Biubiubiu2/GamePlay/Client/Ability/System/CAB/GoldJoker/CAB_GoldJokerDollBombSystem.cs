using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_GoldJokerDollBombSystem : C_Ability_Base {
        public Proto_AbilityAB_Auto.Rpc_GoldJokerDollBomb useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_GoldJokerDollBomb)InData;
            iEntity.SetPoint(useInData.startPoint);
            iEntity.SetRota(Quaternion.Euler(useInData.startRot));
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetEffectSign());
        }
        
        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>();
            AddComponent<ClientNetworkTransform>();
            AddComponent<ClientGoldJokerDollBomb>(new ClientGoldJokerDollBomb.InitData() {
                AttackRange = useInData.attackRange,
            });
            AddLifeTime();
        }
        
        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp),
            });
        }
    }
}
