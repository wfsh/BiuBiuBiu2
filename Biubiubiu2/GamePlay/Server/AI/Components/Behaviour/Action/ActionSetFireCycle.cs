using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("设置开启开火循环")]
    [TaskCategory("AI")]
    public class ActionSetFireCycle : ActionComponent {
        [SerializeField]
        private bool targetStatus;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_AI.Event_SetFireCycle() {
                isEnabled = targetStatus
            });

            return TaskStatus.Success;
        }
    }
}