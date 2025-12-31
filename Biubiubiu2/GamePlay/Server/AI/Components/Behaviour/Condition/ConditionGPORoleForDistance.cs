using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("范围内是否有可攻击目标（怪物）")]
    [TaskCategory("AI")]
    public class ConditionGPORoleForDistance : ConditionComponent {
        [SerializeField]
        private float checkDistance = 0.0f;
        
        override public void OnAwake() {
            base.OnAwake();
        }

        override protected void OnClear() {
        }

        public override TaskStatus OnUpdate() {
            var taskStatus = TaskStatus.Failure;
            iGPO.Dispatcher(new SE_AI.Event_GetMinDistanceGPO {
                CallBack = distance => {
                    if (distance < checkDistance) {
                        taskStatus = TaskStatus.Success;
                    } else {
                        taskStatus = TaskStatus.Failure;
                    }
                } 
            });
            return taskStatus;
        }
    }
}