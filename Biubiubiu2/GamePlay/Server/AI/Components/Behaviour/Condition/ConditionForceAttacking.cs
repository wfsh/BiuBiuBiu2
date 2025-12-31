using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否强制攻击中")]
    [TaskCategory("AI")]
    public class ConditionForceAttacking : ConditionComponent {
        public override TaskStatus OnUpdate() {
            if (iGPO.IsClear() || iEntity.IsClear()) {
                return TaskStatus.Failure;
            }

            var isAttacking = false;
            iGPO.Dispatcher(new SE_Behaviour.Event_GetForceAttacking() {
                CallBack = value => isAttacking = value
            });

            if (isAttacking) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}