using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否被驾驶呼唤")]
    [TaskCategory("AI")]
    public class ConditionIsDriveCall : ConditionComponent {
        private bool isDriveCallIng = false;

        override public void OnAwake() {
            base.OnAwake();
            // iGPO.Register<SE_Monster.Event_OnDriver>(OnDriverCallCallBack);
        }

        override public void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            // iGPO.Unregister<SE_Monster.Event_OnDriver>(OnDriverCallCallBack);
        }

        // public void OnDriverCallCallBack(ISystemMsg body, SE_Monster.Event_OnDriver ent) {
        //     isDriveCallIng = ent.IsDriver;
        // }

        public override TaskStatus OnUpdate() {
            if (isDriveCallIng) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}