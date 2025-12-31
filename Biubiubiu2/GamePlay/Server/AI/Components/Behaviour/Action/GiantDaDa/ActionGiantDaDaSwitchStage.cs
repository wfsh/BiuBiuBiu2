using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("巨像达达切换阶段")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaSwitchStage : ActionComponent {
        [SerializeField]
        private int nextStage;

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_GiantDaDa.Event_SwitchStage() {
                NextStage = nextStage - 1,
            });
            return TaskStatus.Success;
        }
    }
}