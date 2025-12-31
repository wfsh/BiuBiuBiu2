using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_HasItem : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public int ItemId;
            public void Serialize (string value) {
                ItemId = int.Parse(value);
            }
        }
        private InitData useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        public override bool CheckCondition() {
            var isTrue = false;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                gpo.Dispatcher(new SE_EventDirector.HasItem {
                    ItemId = useMData.ItemId,
                    CallBack = b => {
                        isTrue = b;
                    },
                });
                if (isTrue) {
                    SetTriggerGPO(gpo);
                    break;
                }
            }
            if (compareType == EventDirectorData.CompareType.NotEqual) {
                return isTrue == false;
            }
            return isTrue;
        }
    }
}