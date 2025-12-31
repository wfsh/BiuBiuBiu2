using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("刺激源连锁(怪物)")]
    [TaskCategory("AI")]
    public class ActionChainStimulus : ActionComponent {
        [SerializeField]
        private float chainRadius = 30.0f;
        private List<IGPO> gpoList;
        private IGPO stimulusGPO;
        private float hateValue = 0;

        public override void OnAwake() {
            base.OnAwake();
        }

        public override void OnStart() {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
        }

        protected override void OnClear() {
            gpoList = null;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            this.gpoList = list;
        }

        public override TaskStatus OnUpdate() {
            if (iEntity == null) {
                return TaskStatus.Running;
            }

            iGPO.Dispatcher(new SE_Behaviour.Event_GetLatestStimulusState() {
                CallBack = SetStimulusState
            });

            var point = iEntity.GetPoint();
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetGpoID() == iGPO.GetGpoID() || gpo.GetTeamID() != iGPO.GetTeamID()) {
                    continue;
                }
                var distance = Vector3.Distance(point, gpo.GetPoint());
                if (distance < chainRadius) {
                    gpo.Dispatcher(new SE_Behaviour.Event_ChainStimulus() {
                        CasterGPO = stimulusGPO,
                    });
                }
            }
            return TaskStatus.Success;
        }

        private void SetStimulusState(IGPO gpo, Vector3 point) {
            stimulusGPO = gpo;
        }
    }
}