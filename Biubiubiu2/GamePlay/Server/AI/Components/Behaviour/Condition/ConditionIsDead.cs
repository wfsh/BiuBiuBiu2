using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查自己是否已经死亡")]
    [TaskCategory("AI")]
    public class ConditionIsDead : ConditionComponent {
        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override TaskStatus OnUpdate() {
            bool isDead = false;
            iGPO.Dispatcher(new SE_GPO.Event_GetIsDead() {
                CallBack = result => {
                    isDead = result;
                }
            });

            return isDead ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}