using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("巨像达达开始离场")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaStartLeave : ActionComponent {
        [SerializeField]
        private float timeToHide;

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_FightBoss.Event_StartToLeave() {
                TimeToHideEntity = timeToHide
            });
            return TaskStatus.Success;
        }
    }
}