using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("攻击目标")]
    [TaskCategory("AI")]
    public class ActionAttackForGPO : ActionComponent {
        [SerializeField]
        private float fireLimitTime = 0.0f;

        private IGPO target;
        private Vector3 point;
        private float fireTime = 0.0f;

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
            if (Time.realtimeSinceStartup - fireTime < this.fireLimitTime) {
                return TaskStatus.Running;
            }
            fireTime = Time.realtimeSinceStartup;
            iGPO.Dispatcher(new SE_Behaviour.Event_AttackTargetGPO {
                TargetGPO = target
            });
            return TaskStatus.Running;
        }
    }
}