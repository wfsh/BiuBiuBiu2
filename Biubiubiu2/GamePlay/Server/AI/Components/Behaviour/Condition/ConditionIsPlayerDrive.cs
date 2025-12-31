using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否开始驾驶")]
    [TaskCategory("AI")]
    public class ConditionIsPlayerDrive : ConditionComponent {
        private bool isPlayerDrive = false;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_GPO.Event_PlayerDrive>(OnDriveIngCallBack);
        }

        override public void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_GPO.Event_PlayerDrive>(OnDriveIngCallBack);
        }

        public void OnDriveIngCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isPlayerDrive = ent.IsDrive;
        }

        public override TaskStatus OnUpdate() {
            if (isPlayerDrive) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}