using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("扇形范围内是否有可攻击目标（怪物）")]
    [TaskCategory("Monster")]
    public class ConditionGPORoleForSectorArea : ConditionComponent {
        [UnityEngine.Tooltip("扫视角度")][SerializeField] private float sweepAngle;
        // 扫视距离用的怪物的视野距离 SightRadius

        public override TaskStatus OnUpdate() {
            if (iGPO == null) {
                return TaskStatus.Failure;
            }

            var taskStatus = TaskStatus.Failure;
            iGPO.Dispatcher(new SE_AI.Event_IsSectorAreaHasGPO {
                SweepAngle = sweepAngle,
                CallBack = success => {
                    taskStatus = success? TaskStatus.Success : TaskStatus.Failure;
                }
            });
            return taskStatus;
        }
    }
}