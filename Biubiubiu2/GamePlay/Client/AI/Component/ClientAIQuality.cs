using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIQuality : ComponentBase {
        private bool isBoss = false;
        private AIData.AIQuality aiQuality;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_AI.Event_GetIsBoss>(OnGetIsBoss);
            var mData = iGPO.GetMData();
            aiQuality = (AIData.AIQuality)mData.GetQuality();
            this.isBoss = aiQuality == AIData.AIQuality.Boss;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_AI.Event_GetIsBoss>(OnGetIsBoss);
        }
        
        private void OnGetIsBoss(ISystemMsg body, CE_AI.Event_GetIsBoss ent) {
            ent.CallBack(isBoss);
        }
    }
}