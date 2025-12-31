using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否朝目标点移动中")]
    [TaskCategory("AI")]
    public class ConditionIsMovePoint : ConditionComponent {
        private bool isMovePoint = false;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
        }

        public void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            this.isMovePoint = ent.IsTrue;
        }

        public override TaskStatus OnUpdate() {
            if (isMovePoint) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}