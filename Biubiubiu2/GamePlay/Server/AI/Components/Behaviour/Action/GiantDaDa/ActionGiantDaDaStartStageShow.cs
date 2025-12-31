using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("巨像达达开始阶段表演")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaStartStageShow : ActionComponent {
        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim() {
                Stage = SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim.StageEnum.Switch,
            });
            return TaskStatus.Success;
        }
    }
}