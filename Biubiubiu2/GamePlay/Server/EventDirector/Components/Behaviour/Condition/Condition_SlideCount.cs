using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Condition_SlideCount : EventDirectorCondition {
        public struct InitData : IEventDirectorData {
            public float LimitCount;
            public void Serialize (string value) {
                LimitCount = float.Parse(value);
            }
        }
        private int slideCount = 0;
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
            gpo.Register<SE_GPO.Event_Slide>(OnGpoSlide);
        }

        protected override void OnRemoveGpo(IGPO gpo) {
            base.OnRemoveGpo(gpo);
            gpo.Unregister<SE_GPO.Event_Slide>(OnGpoSlide);
        }

        // GPO 监听
        private void OnGpoSlide(ISystemMsg body, SE_GPO.Event_Slide ent) {
            if (ent.IsSlide) {
                slideCount++;
                SetTriggerGPO(body.GetGPO());
            }
        }
        
        public override bool CheckCondition() {
            if (compareType == EventDirectorData.CompareType.Less) {
                return slideCount < useMData.LimitCount;
            }
            return slideCount >= useMData.LimitCount;
        }
    }
}