using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查前方范围内是否有障碍物（头）")]
    [TaskCategory("AI")]
    public class ConditionIsObstacleInSightRange : ConditionComponent {
        [SerializeField]
        private float checkDistance;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override TaskStatus OnUpdate() {
            bool isObstacle = false;
            iGPO.Dispatcher(new SE_Behaviour.Event_IsObstacleInSightRange() {
                checkDistance = checkDistance,
                Callback = result => {
                    isObstacle = result;
                }
            });

            return isObstacle ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}