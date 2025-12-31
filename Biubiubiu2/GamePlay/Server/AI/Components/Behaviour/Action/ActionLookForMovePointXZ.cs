using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("看向移动点（水平）")]
    [TaskCategory("AI")]
    public class ActionLookForMovePointXZ : ActionComponent {
        private Vector3 point = Vector3.zero;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MovePoint>(OnSetMovePointCallBack);
        }

        override protected void OnClear() {
            iGPO?.Unregister<SE_Behaviour.Event_MovePoint>(OnSetMovePointCallBack);
        }

        private void OnSetMovePointCallBack(ISystemMsg body, SE_Behaviour.Event_MovePoint ent) {
            this.point = ent.movePoint;
        }

        public override TaskStatus OnUpdate() {
            if (iGPO == null) {
                return TaskStatus.Running;
            }
            if (this.point != Vector3.zero) {
                point.y = iEntity.GetPoint().y;
                iEntity.LookAT(this.point);
            }
            return TaskStatus.Running;
        }
    }
}