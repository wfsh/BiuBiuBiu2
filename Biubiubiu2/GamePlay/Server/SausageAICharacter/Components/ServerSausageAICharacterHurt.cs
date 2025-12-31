using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageAICharacterHurt : ServerNetworkComponentBase {
        private ServerGPO fireGPO;
        private int aiId;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            var characterSystem = (S_SausageAI_Base)mySystem;
            aiId = characterSystem.AIId;
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
        }
        
        private void OnGPOHurtCallBack(ISystemMsg body, SE_GPO.Event_GPOHurt ent) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            this.fireGPO = (ServerGPO)ent.AttackGPO;
            if (iGPO.IsGodMode() || iGPO.IsDead()) {
                return;
            }
            var downHp = Mathf.Max(1, ent.Hurt);
            this.fireGPO.Dispatcher(new SE_GPO.Event_SetHurtTOGpo {
                HurtValue = downHp,
                AttackGPO = this.fireGPO,
                IsHead = ent.IsHead,
                HurtGPO = iGPO
            });
            Dispatcher(new SE_GPO.Event_DownHP {
                DownHp = 0,
                DownHpGPO = iGPO,
                AttackGPO = fireGPO,
                AttackItemId = ent.AttackItemId,
            });
            var attackerType = ent.AttackGPO.GetGPOType();
            if (attackerType == GPOData.GPOType.AI || attackerType == GPOData.GPOType.MasterAI) {
                var quality = (AIData.AIQuality)ent.AttackGPO.GetMData().GetQuality();
                MsgRegister.Dispatcher(new SM_Sausage.AttackSausageAICharacterDownHp {
                    AiId = aiId,
                    MonsterId = ent.AttackGPO.GetGpoID(),
                    AIQuality = quality,
                    DownHp = downHp,
                    AttackSourcePos = ent.AttackGPO.GetPoint(),
                    AttackType = ent.AttackItemId > 0 ?
                        SM_Sausage.AttackSausageAICharacterDownHp.AttackSourceType.Gun :
                        SM_Sausage.AttackSausageAICharacterDownHp.AttackSourceType.Ability,
                });
            }
        }
    }
}