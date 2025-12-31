using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("RexKing 攻击")]
    [TaskCategory("AI")]
    public class ActionRexKingAttack : ActionComponent {
        private IGPO target;
        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            target = null;
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }
        
        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_AttackTargetGPO {
                TargetGPO = target
            });
            return TaskStatus.Running;
        }
    }
}