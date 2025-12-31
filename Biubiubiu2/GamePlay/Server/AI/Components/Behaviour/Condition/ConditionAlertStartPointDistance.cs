using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("判断与警戒触发时所处位置的距离")]
    [TaskCategory("AI")]
    public class ConditionAlertStartPointDistance : ConditionComponent {
        [SerializeField]
        private float checkDistance = 0.0f;
        private Vector3 startPoint;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override void OnStart() {
            base.OnStart();
        }

        public override TaskStatus OnUpdate() {
            if (iGPO == null) {
                return TaskStatus.Success;
            }

            iGPO.Dispatcher(new SE_AI.Event_GetAlertStartPoint() {
                CallBack = SetStartPoint
            });
            float sqrtDis = Vector3.SqrMagnitude(iGPO.GetPoint() - startPoint);
            return sqrtDis < Mathf.Pow(checkDistance, 2) ? TaskStatus.Success : TaskStatus.Failure;
        }

        private void SetStartPoint(Vector3 point) {
            startPoint = point;
        }
    }
}