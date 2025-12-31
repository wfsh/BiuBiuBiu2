using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_KillAI : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public int GpoMId;
            public float KillCount;
            public void Serialize (string value) {
                var arr = value.Split('&');
                GpoMId = int.Parse(arr[0]);
                KillCount = int.Parse(arr[1]);
            }
        }
        private int killCount = 0;
        private InitData useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }
        
        protected override void OnSetGpo(IGPO gpo) {
            base.OnSetGpo(gpo);
            gpo.Register<SE_GPO.Event_KillGPO>(OnKillGPOCallBack);
        }
        
        protected override void OnRemoveGpo(IGPO gpo) {
            base.OnRemoveGpo(gpo);
            gpo.Unregister<SE_GPO.Event_KillGPO>(OnKillGPOCallBack);
        }

        private void OnKillGPOCallBack(ISystemMsg body, SE_GPO.Event_KillGPO ent) {
            if (useMData.GpoMId == 0 || ent.DeadGPO.GetGpoMID() == useMData.GpoMId) {
                killCount++;
                SetTriggerGPO(body.GetGPO());
            }
        }

        public override bool CheckCondition() {
            if (compareType == EventDirectorData.CompareType.Less) {
                return killCount < useMData.KillCount;
            }
            return killCount >= useMData.KillCount;
        }
    }
}
