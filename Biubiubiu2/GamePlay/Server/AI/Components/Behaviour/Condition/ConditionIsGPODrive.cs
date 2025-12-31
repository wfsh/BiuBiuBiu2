using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否被驾驶")]
    [TaskCategory("AI")]
    public class ConditionIsGPODrive : ConditionComponent {
        private GPOData.GPOType driveGpoType = GPOData.GPOType.NULL;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_AI.Event_DriveState>(OnDriveIngCallBack);
        }

        override public void OnStart() {
            base.OnStart();
            iGPO?.Dispatcher(new SE_AI.Event_GetDriveState {
                CallBack = gpoType => {
                    driveGpoType = gpoType;
                }
            });
        }

        protected override void OnClear() {
            iGPO?.Unregister<SE_AI.Event_DriveState>(OnDriveIngCallBack);
        }

        public void OnDriveIngCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveGpoType = ent.DriveGpoType;
        }

        public override TaskStatus OnUpdate() {
            if (driveGpoType == GPOData.GPOType.Role) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}