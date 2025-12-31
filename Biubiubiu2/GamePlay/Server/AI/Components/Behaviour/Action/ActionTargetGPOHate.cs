using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("仇恨值计算(怪物)")]
    [TaskCategory("AI")]
    public class ActionTargetGPOHate : ActionComponent {
        [SerializeField]
        private float checkDistance = 30.0f;
        private IGPO targetGPO = null;
        private List<IGPO> GPOList;

        override public void OnAwake() {
            base.OnAwake();
        }

        public override void OnStart() {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
        }

        override protected void OnClear() {
            GPOList = null;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            this.GPOList = list;
        }

        public override TaskStatus OnUpdate() {
            if (iEntity == null) {
                return TaskStatus.Running;
            }
            var point = iEntity.GetPoint();
            for (int i = 0; i < GPOList.Count; i++) {
                var gpo = GPOList[i];
                if (gpo.IsClear() || gpo.IsGodMode() || gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }
                var distance = Vector3.Distance(point, gpo.GetPoint());
                if (distance < checkDistance) {
                    // 临时规则
                    iGPO.Dispatcher(new SE_Behaviour.Event_FillHateToValue() {
                        CasterGPO = gpo,
                        Value = 100 * (checkDistance - distance) / checkDistance
                    });
                    iGPO.Dispatcher(new SE_Behaviour.Event_HateFindTarget {
                        TargetGPO = gpo, Distance = distance,
                    });
                    gpo.Dispatcher(new SE_Behaviour.Event_HateLockTarget {
                        TargetGPO = iGPO, Distance = distance,
                    });
                }
            }
            return TaskStatus.Running;
        }
    }
}