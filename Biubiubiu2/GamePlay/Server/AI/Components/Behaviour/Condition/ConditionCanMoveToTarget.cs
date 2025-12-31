using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否能移动到目标点")]
    [TaskCategory("AI")]
    public class ConditionCanMoveToTarget : ConditionComponent {
        private IGPO target;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            if (target == null) {
                return TaskStatus.Failure;
            }

            bool success = false;
            iGPO.Dispatcher(new SE_Behaviour.Event_CanMoveToTarget() {
                CallBack = value => success = value
            });
            if (success) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
