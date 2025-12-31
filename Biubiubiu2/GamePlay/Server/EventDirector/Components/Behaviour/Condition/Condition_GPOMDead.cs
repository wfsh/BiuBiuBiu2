using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_GPOMDead : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public int GpoMId;
            public int DeadCount;
            public void Serialize (string value) {
                var arr = value.Split('&');
                GpoMId = int.Parse(arr[0]);
                DeadCount = int.Parse(arr[1]);
            }
        }
        private int deadCount = 0;
        private InitData useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnSetGpo(IGPO gpo) {
            base.OnSetGpo(gpo);
            gpo.Register<SE_GPO.Event_SetIsDead>(OnSetIsDead);
        }

        protected override void OnRemoveGpo(IGPO gpo) {
            base.OnRemoveGpo(gpo);
            gpo.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDead);
        }

        private void OnSetIsDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.DeadGpo.GetGpoMID() == useMData.GpoMId) {
                deadCount++;
                SetTriggerGPO(body.GetGPO());
            }
        }

        public override bool CheckCondition() {
            if (compareType == EventDirectorData.CompareType.Less) {
                return deadCount < useMData.DeadCount;
            }
            return deadCount >= useMData.DeadCount;
        }
    }
}