using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("返回出生点途中")]
    [TaskCategory("AI")]
    public class ConditionIsComeBack : ConditionComponent {
        private bool isComeBack = false;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
        }

        public void OnComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            this.isComeBack = ent.IsTrue;
        }

        public override TaskStatus OnUpdate() {
            if (isComeBack) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}