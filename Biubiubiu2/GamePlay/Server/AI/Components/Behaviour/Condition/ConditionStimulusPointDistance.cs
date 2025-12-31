using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("判断和刺激点的距离")]
    [TaskCategory("AI")]
    public class ConditionStimulusPointDistance : ConditionComponent {
        [SerializeField]
        private float checkDistance = 0.0f;
        private Vector3 stimulusPoint;

        public override TaskStatus OnUpdate() {
            if (iGPO == null) {
                return TaskStatus.Success;
            }

            iGPO.Dispatcher(new SE_Behaviour.Event_GetLatestStimulusState() {
                CallBack = SetStimulusPoint
            });
            float sqrtDis = Vector3.SqrMagnitude(iGPO.GetPoint() - stimulusPoint);
            return sqrtDis < Mathf.Pow(checkDistance, 2) ? TaskStatus.Success : TaskStatus.Failure;
        }

        private void SetStimulusPoint(IGPO gpo, Vector3 point) {
            stimulusPoint = point;
        }
    }
}