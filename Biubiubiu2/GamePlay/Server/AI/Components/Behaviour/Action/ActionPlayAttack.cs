using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("开始攻击")]
    [TaskCategory("AI")]
    public class ActionPlayAttack : ActionComponent {
        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_PlayAttack {
            });
            return TaskStatus.Running;
        }
    }
}