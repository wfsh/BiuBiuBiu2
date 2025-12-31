using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("巨像达达离开完成")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaLeaveComplete : ActionComponent {
        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_FightBoss.Event_LeaveComplete() {
                isShowTime = true,
                languageSign = "PickBossItem_CountDown"
            });
            return TaskStatus.Success;
        }
    }
}