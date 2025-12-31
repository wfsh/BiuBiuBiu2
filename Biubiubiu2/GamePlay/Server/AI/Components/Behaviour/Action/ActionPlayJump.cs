using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("跳跃")]
    [TaskCategory("AI")]
    public class ActionPlayJump : ActionComponent {
        override public void OnAwake() {
            base.OnAwake();
        }

        override protected void OnClear() {
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_OnJump());
            return TaskStatus.Running;
        }
    }
}