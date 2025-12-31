using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("在可视范围内扫视")]
    [TaskCategory("Monster")]
    public class ActionSweep : ActionComponent {
        [SerializeField] private float sweepAngle;  // 扫视角度
        [SerializeField] private float duration;    // 执行扫视一圈的总时间

        private Quaternion startRotate; // 起始朝向
        private bool sweepDirection;    // 扫视方向，true为右，false为左
        private float currentAngle;     // 当前相对于起始朝向的角度
        private float sweepSpeed;       // 扫视旋转速度（度/秒）
        private bool isSweeping;        // 是否正在扫视

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_ActionSweepEnd>(OnActionSweepEndCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_ActionSweepEnd>(OnActionSweepEndCallBack);
        }

        public override void OnStart() {
            isSweeping = true;
            iGPO.Dispatcher(new SE_Behaviour.Event_ActionSweepChange() {
                IsSweep = true,
                SweepAngle = sweepAngle / 2,
                Duration = duration,
            });
        }

        public override void OnEnd() {
            if (iGPO == null) {
                return;
            }

            isSweeping = false;
            iGPO.Dispatcher(new SE_Behaviour.Event_ActionSweepChange() {
                IsSweep = false,
            });
        }

        private void OnActionSweepEndCallBack(ISystemMsg body, SE_Behaviour.Event_ActionSweepEnd obj) {
            isSweeping = false;
        }

        public override TaskStatus OnUpdate() {
            if (!isSweeping) {
                return TaskStatus.Failure;
            }

            return TaskStatus.Running;
        }

        public override void OnConditionalAbort() {
            if (iGPO == null) {
                return;
            }

            isSweeping = false;
            iGPO.Dispatcher(new SE_Behaviour.Event_ActionSweepChange() {
                IsSweep = false,
            });
        }
    }
}