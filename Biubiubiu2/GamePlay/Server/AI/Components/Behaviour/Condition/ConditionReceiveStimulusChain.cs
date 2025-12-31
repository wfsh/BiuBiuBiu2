using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否接收到刺激源连锁")]
    [TaskCategory("AI")]
    public class ConditionReceiveStimulusChain : ConditionComponent {
        private bool hasReceiveStimulusChain;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_ChainStimulus>(ChainStimulusCallBack);
            iGPO.Register<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Register<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_ChainStimulus>(ChainStimulusCallBack);
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Unregister<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        private void TriggerFightStatusCallBack(ISystemMsg body, SE_AI.Event_TriggerFightStatus ent) {
            hasReceiveStimulusChain = false;
        }

        private void ComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            hasReceiveStimulusChain = false;
        }

        private void ChainStimulusCallBack(ISystemMsg body, SE_Behaviour.Event_ChainStimulus ent) {
            hasReceiveStimulusChain = true;
        }

        public override TaskStatus OnUpdate() {
            bool status = hasReceiveStimulusChain;
            return status ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}