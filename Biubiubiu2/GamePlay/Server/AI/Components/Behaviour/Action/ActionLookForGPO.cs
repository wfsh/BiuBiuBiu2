using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("看向目标 GPO 一次")]
    [TaskCategory("AI")]
    public class ActionLookForGPO : ActionComponent {
        private IGPO target;
        private Vector3 point = Vector3.zero;
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        override protected void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            this.target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            if (this.target != null && this.target.IsClear() == false) {
                var targetPoint = this.target.GetPoint();
                point.x = targetPoint.x;
                point.y = iEntity.GetPoint().y;
                point.z = targetPoint.z;
                iEntity.LookAT(this.point);
            }
            return TaskStatus.Running;
        }
    }
}