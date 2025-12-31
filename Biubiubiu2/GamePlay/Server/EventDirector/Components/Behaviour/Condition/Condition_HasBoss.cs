using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_HasBoss : EventDirectorCondition {
        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override bool CheckCondition() {
            var isTrue = false;
            List<IGPO> list = null;
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForGpoType {
                GpoType = GPOData.GPOType.AI,
                CallBack = l => list = l
            });
            if (list != null && list.Count > 0) {
                for (int i = 0; i < list.Count; i++) {
                    var gpo = list[i];
                    gpo.Dispatcher(new SE_AI.Event_GetIsBoss {
                        CallBack = b => {
                            isTrue = b;
                        },
                    });
                    if (isTrue) {
                        SetTriggerGPO(gpo);
                        break;
                    }
                }
            }
            if (compareType == EventDirectorData.CompareType.NotEqual) {
                return isTrue == false;
            }
            return isTrue;
        }
    }
}