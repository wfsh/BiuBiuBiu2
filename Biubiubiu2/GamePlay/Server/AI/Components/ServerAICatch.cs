using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAICatch : ServerNetworkComponentBase {
        private S_AI_Base aiSystem;
        protected override void OnAwake() {
            aiSystem = (S_AI_Base)mySystem;
            Register<SE_AI.Event_CatchAI>(OnCatchMonsterCallBack);
        }

        protected override void OnClear() {
            Unregister<SE_AI.Event_CatchAI>( OnCatchMonsterCallBack);
            base.OnClear();
        }

        public void OnCatchMonsterCallBack(ISystemMsg body, SE_AI.Event_CatchAI ent) {
            if (ent.CatchGPO == null) {
                Debug.LogError("捕捉不能为空");
                return;
            }
            aiSystem.AttributeData.nowHp = aiSystem.AttributeData.maxHp;
            // 可以补充抓宠概率值类的逻辑
            var serverGpo = (ServerGPO)ent.CatchGPO;
            serverGpo.Dispatcher(new SE_AI.Event_CatchAIState {
                CatchAIData = aiSystem.AttributeData,
                IsSuccess = true
            });
            Rpc(new Proto_AI.Rpc_CatchAI {
            });
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = iGPO.GetGpoID()
            });
        }
    }
}