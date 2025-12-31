using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("触发警戒状态")]
    [TaskCategory("AI")]
    public class ActionTriggerAlertStatus : ActionComponent {
        [SerializeField]
        private bool isEnabled;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI.Event_TriggerAlertStatus() {
                isEnabled = isEnabled
            });
            return TaskStatus.Success;
        }
    }
}