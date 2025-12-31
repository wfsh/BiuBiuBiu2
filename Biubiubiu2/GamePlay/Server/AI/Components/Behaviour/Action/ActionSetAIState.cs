using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("设置怪物状态")]
    [TaskCategory("AI")]
    public class ActionSetAIState : ActionComponent {
        [SerializeField]
        private AIBehaviourData.FightStateEnum targetState = AIBehaviourData.FightStateEnum.Idle;
        [SerializeField]
        private float warningMaxHate = 200;

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI.Event_ChangeAIState() {
                State = targetState,
                WarningMaxHate = warningMaxHate
            });

            return TaskStatus.Success;
        }
    }
}