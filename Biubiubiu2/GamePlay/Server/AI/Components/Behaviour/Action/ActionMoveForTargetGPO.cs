using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("朝目标对象移动")]
    [TaskCategory("AI")]
    public class ActionMoveForTargetGPO : ActionComponent {
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
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
            if (this.target == null) {
                return TaskStatus.Running;
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = this.target.GetPoint(),
                MoveType = this.moveType
            });
            return TaskStatus.Running;
        }
    }
}