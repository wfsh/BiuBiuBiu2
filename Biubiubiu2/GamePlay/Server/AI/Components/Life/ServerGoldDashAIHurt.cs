using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIHurt : ServerNetworkComponentBase {
        private ServerGPO fireGPO;
        private GPOData.AttributeData attributeData;

        protected override void OnAwake() {
            Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            attributeData = ((S_AI_Base)mySystem).AttributeData;
        }

        protected override void OnClear() {
            Unregister<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            attributeData = null;
            fireGPO = null;
        }

        private void OnGPOHurtCallBack(ISystemMsg body, SE_GPO.Event_GPOHurt ent) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            if (iGPO.IsGodMode()) {
                return;
            }
            this.fireGPO = (ServerGPO)ent.AttackGPO;
            if (CheckHurtInRange(this.fireGPO) == false) {
                this.fireGPO.Dispatcher(new SE_AI_FightBoss.Event_HurtOutOfFightRange());
                return;
            }
            // 这边可以做伤害相关公式，比如 hurt - def  或者穿透啥的
            int hurt = ent.Hurt;
            if (ent.IsHead) {
                hurt = Mathf.CeilToInt(hurt * ent.HeadAddPowerRatio);
            }
            var downHp = Mathf.Max(1, hurt);
            Dispatcher(new SE_GPO.Event_DownHP {
                DownHp = downHp,
                DownHpGPO = iGPO,
                AttackGPO = fireGPO,
                AttackItemId = ent.AttackItemId,
                IsHead = ent.IsHead,
            });
            this.fireGPO?.Dispatcher(new SE_GPO.Event_SetHurtTOGpo {
                HurtValue = downHp,
                AttackGPO = this.fireGPO,
                IsHead = ent.IsHead,
                HurtGPO = iGPO
            });
        }

        private bool CheckHurtInRange(IGPO gpo) {
            var isInRange = true;
            Dispatcher(new SE_AI_FightBoss.Event_CheckGPOInFightRange {
                GPO = gpo,
                CallBack = (inRange) => {
                    isInRange = inRange;
                }
            });
            return isInRange;
        }

        private void OnDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
        }
    }
}