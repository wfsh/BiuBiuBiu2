using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("设置保持注视目标 GPO")]
    [TaskCategory("AI")]
    public class ActionSetKeepEyesOnGPO : ActionComponent {
        [SerializeField]
        private bool targetStatus;
        private IGPO targetGPO;

        public override void OnAwake() {
            base.OnAwake();
            iGPO?.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        protected override void OnClear() {
            iGPO?.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            this.targetGPO = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            if (targetGPO?.GetGpoID() != ent.TargetGPO?.GetGpoID()) {
                this.targetGPO = ent.TargetGPO;
            }
        }

        public override TaskStatus OnUpdate() {
            iGPO?.Dispatcher(new SE_Behaviour.Event_SetKeepEyesOnGPO() {
                IsEnabled = targetStatus && targetGPO != null,
                TargetGPO = targetGPO,
            });

            return TaskStatus.Running;
        }
    }
}