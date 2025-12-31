using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIHurt : ServerNetworkComponentBase {
        private ServerGPO fireGPO;
        private GPOData.AttributeData attributeData;
        private readonly Dictionary<int, long> gpoIdToFirstAttackTime = new Dictionary<int, long>();

        protected override void OnAwake() {
            Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            Register<SE_GPO.Event_GetGPOAttacDuration>(OnGetGPOFirstAttackTime);
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
            // 这边可以做伤害相关公式，比如 hurt - def  或者穿透啥的
            int hurt = ent.Hurt;
            if (ent.IsHead) {
                hurt = Mathf.CeilToInt(hurt * ent.HeadAddPowerRatio);
            }
            var downHp = Mathf.Max(1, hurt);
            this.fireGPO.Dispatcher(new SE_GPO.Event_SetHurtTOGpo {
                HurtValue = downHp,
                AttackGPO = this.fireGPO,
                HurtGPO = iGPO,
                AttackItemId = ent.AttackItemId,
                DamageType = ent.DamageType,
            });
            Dispatcher(new SE_GPO.Event_DownHP {
                DownHp = downHp, AttackGPO = fireGPO, AttackItemId = ent.AttackItemId, DownHpGPO = iGPO, IsHead = ent.IsHead
            });
            gpoIdToFirstAttackTime.TryAdd(fireGPO.GpoID, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        private void OnDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            gpoIdToFirstAttackTime.Clear();
        }

        private void OnGetGPOFirstAttackTime(ISystemMsg body, SE_GPO.Event_GetGPOAttacDuration ent) {
            var attackDuration = gpoIdToFirstAttackTime.TryGetValue(ent.AttackGPOId, out long firstAttackTime)
                ? (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - firstAttackTime) / 1000f
                : 0f;
            ent.CallBack.Invoke(attackDuration);
        }
    }
}