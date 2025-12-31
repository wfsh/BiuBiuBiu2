using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否捕捉到枪声")]
    [TaskCategory("AI")]
    public class ConditionCaughtFireEvent : ConditionComponent {
        private bool hasCaughtFireEvent;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_AI.Event_CaughtFireEvent>(CaughtFireCallBack);
            iGPO.Register<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Register<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_AI.Event_CaughtFireEvent>(CaughtFireCallBack);
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Unregister<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        private void TriggerFightStatusCallBack(ISystemMsg body, SE_AI.Event_TriggerFightStatus ent) {
            hasCaughtFireEvent = false;
        }

        private void ComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            hasCaughtFireEvent = false;
        }

        private void CaughtFireCallBack(ISystemMsg body, SE_AI.Event_CaughtFireEvent ent) {
            hasCaughtFireEvent = true;
        }

        public override TaskStatus OnUpdate() {
            bool status = hasCaughtFireEvent;
            return status ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}