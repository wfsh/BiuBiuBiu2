using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_FireCount : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public float LimitCount;
            public void Serialize (string value) {
                LimitCount = float.Parse(value);
            }
        }
        private int fireCount = 0;
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
            gpo.Register<SE_GPO.Event_GunFireEnd>(OnGpoGunFireEnd);
        }
        
        protected override void OnRemoveGpo(IGPO gpo) {
            base.OnRemoveGpo(gpo);
            gpo.Unregister<SE_GPO.Event_GunFireEnd>(OnGpoGunFireEnd);
        }

        private void OnGpoGunFireEnd(ISystemMsg body, SE_GPO.Event_GunFireEnd ent) {
            fireCount++;
            SetTriggerGPO(body.GetGPO());
        }

        public override bool CheckCondition() {
            if (compareType == EventDirectorData.CompareType.Less) {
                return fireCount < useMData.LimitCount;
            }
            return fireCount >= useMData.LimitCount;
        }
    }
}