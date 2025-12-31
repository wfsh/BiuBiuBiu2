using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查和仇恨目标的距离")]
    [TaskCategory("AI")]
    public class ConditionHateGPODistance : ConditionComponent {
        [SerializeField]
        private float distance = 5f;
        private IGPO target;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        override protected void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            var taskStatus = TaskStatus.Failure;
            if (target == null || target.IsClear()|| iEntity.IsClear()) {
                return TaskStatus.Failure;
            }
            if (distance > 0f) {
                var myPoint = iEntity.GetPoint();
                myPoint.y = 0;
                var targetPoint = target.GetPoint();
                targetPoint.y = 0;
                var checkDistance = Vector3.Distance(myPoint, targetPoint);
                if (checkDistance < distance) {
                    return TaskStatus.Success;
                } else {
                    return TaskStatus.Failure;
                }
            }
            return TaskStatus.Success;
        }
    }
}