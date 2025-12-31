using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("朝最新刺激源移动")]
    [TaskCategory("AI")]
    public class ActionMoveForStimulusPoint : ActionComponent {
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 latestStimulusPoint;

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_GetLatestStimulusState() {
                CallBack = SetLatestStimulusPoint
            });

            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = latestStimulusPoint,
                MoveType = this.moveType
            });
            return TaskStatus.Running;
        }

        private void SetLatestStimulusPoint(IGPO gpo, Vector3 point) {
            latestStimulusPoint = point;
        }
    }
}