using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查巨像达达技能使用状态")]
    [TaskCategory("AI")]
    public class ConditionIsGiantDaDaInAbilityUseStatus : ConditionComponent {
        [SerializeField]
        public SE_AI_GiantDaDa.Event_GetCurAbilityUseStatus.AbilityUseStatus targetStatus;
        private SE_AI_GiantDaDa.Event_GetCurAbilityUseStatus.AbilityUseStatus curStatus;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
        }

        public override void OnStart() {
        }


        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI_GiantDaDa.Event_GetCurAbilityUseStatus() {
                CallBack = SetCurStatus
            });

            return curStatus == targetStatus ? TaskStatus.Success : TaskStatus.Failure;
        }
        private void SetCurStatus(SE_AI_GiantDaDa.Event_GetCurAbilityUseStatus.AbilityUseStatus status) {
            curStatus = status;
        }
    }
}