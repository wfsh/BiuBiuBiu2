using System;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIQuality : ComponentBase {
        private bool isBoss = false;
        private AIData.AIQuality aiQuality;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_AI.Event_GetIsBoss>(OnGetIsBoss);
            var mData = iGPO.GetMData();
            aiQuality = (AIData.AIQuality)mData.GetQuality();
            this.isBoss = aiQuality == AIData.AIQuality.Boss;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_AI.Event_GetIsBoss>(OnGetIsBoss);
        }
        
        private void OnGetIsBoss(ISystemMsg body, SE_AI.Event_GetIsBoss ent) {
            ent.CallBack(isBoss);
        }
    }
}