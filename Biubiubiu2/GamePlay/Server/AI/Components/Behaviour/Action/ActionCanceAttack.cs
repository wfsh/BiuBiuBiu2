using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("取消攻击")]
    [TaskCategory("AI")]
    public class ActionCanceAttack : ActionComponent {
        public override TaskStatus OnUpdate() {
            iGPO?.Dispatcher(new SE_Behaviour.Event_AttackTargetGPO {
                TargetGPO = null
            });
            return TaskStatus.Running;
        }
    }
}