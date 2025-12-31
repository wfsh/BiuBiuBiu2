using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("巨像达达进入 Idle 状态")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaEnterIdle : ActionComponent {
        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_GiantDaDa.Event_GiantDaDaPlayIdleAnim());
            return TaskStatus.Success;
        }
    }
}