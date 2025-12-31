using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("设置巨像达达是否可以使用技能")]
    [TaskCategory("AI_GiantDaDa")]
    public class ActionGiantDaDaSetCanUseAbility : ActionComponent {
        [SerializeField]
        private bool targetStatus;

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_GiantDaDa.Event_SetCanUseAbility() {
                CanUseAbility = targetStatus,
            });
            return TaskStatus.Success;
        }
    }
}