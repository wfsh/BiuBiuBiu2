using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("朝向目标")]
    [TaskCategory("AI")]
    public class ActionLookTarget : ActionComponent {
        private IGPO target;
        private Vector3 point = Vector3.zero;
        private float delayTime = 0.0f;


        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        protected override void OnClear() {
            if (iGPO == null) {
                return;
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_LookTarget() {
                isLookTarget = false,
                lookTarget = null
            });
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
            iGPO.Dispatcher(new SE_Behaviour.Event_LookTarget() {
                isLookTarget = true,
                lookTarget = target
            });
            return TaskStatus.Running;
        }

        public override void OnConditionalAbort() {
            iGPO.Dispatcher(new SE_Behaviour.Event_LookTarget() {
                isLookTarget = false,
                lookTarget = null
            });
        }
    }
}