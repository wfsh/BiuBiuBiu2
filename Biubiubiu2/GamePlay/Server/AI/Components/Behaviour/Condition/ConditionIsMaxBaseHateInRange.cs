using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查最高仇恨值的基础部分（左闭右开）")]
    [TaskCategory("AI")]
    public class ConditionIsMaxBaseHateInRange : ConditionComponent {
        [SerializeField]
        private float minValue;
        [SerializeField]
        private float maxValue;
        private float hateValue = 0;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_GetMaxHateTargetData() {
                CallBack = SetHateValueCache
            });

            if (hateValue >= minValue && hateValue < maxValue) {
                return TaskStatus.Success;
            } else {
                return TaskStatus.Failure;
            }
        }

        private void SetHateValueCache(IGPO gpo, float value) {
            hateValue = value;
        }
    }
}