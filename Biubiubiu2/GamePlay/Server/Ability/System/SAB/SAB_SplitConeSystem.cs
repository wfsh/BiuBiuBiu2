using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_SplitConeSystem : S_Ability_Base {
        private const float DURATION = 1;
        private AbilityData.PlayAbility_SplitCone inData;

        protected override void OnAwake() {
            inData = (AbilityData.PlayAbility_SplitCone)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = DURATION,
                CallBack = LifeTimeEnd
            });
            AddComponent<SplitConeAttack>(new SplitConeAttack.InitData {
                AbilityData = inData,
                FireGPO = FireGPO,
            });
        }

        protected override void OnStart() {
            RPCAbility(new Proto_Ability.Rpc_SplitCone {
                startPoint  = inData.In_StartPoint,
                startRota = Quaternion.Euler(inData.In_ForwardAngle)
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}