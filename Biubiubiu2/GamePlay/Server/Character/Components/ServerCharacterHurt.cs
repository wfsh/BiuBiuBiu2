using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterHurt : ServerCharacterComponent {
        private ServerGPO fireGPO;
        private GPOData.AttributeData attributeData;
        private readonly Dictionary<int, long> gpoIdToFirstAttackTime = new Dictionary<int, long>();
        

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            mySystem.Register<SE_GPO.Event_UpdateAttribute>(OnUpdateAttributeCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_GetGPOAttacDuration>(OnGetGPOFirstAttackTime);
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new SE_GPO.Event_GetAttributeData {
                CallBack = GetAttributeCallBack
            });
        }
        
        public void GetAttributeCallBack(GPOData.AttributeData data) {
            attributeData = data;
        }

        protected override void OnClear() {
            base.OnClear();
            gpoIdToFirstAttackTime.Clear();
            mySystem.Unregister<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            mySystem.Unregister<SE_GPO.Event_UpdateAttribute>(OnUpdateAttributeCallBack);
            mySystem.Unregister<SE_GPO.Event_GetGPOAttacDuration>(OnGetGPOFirstAttackTime);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        private void OnUpdateAttributeCallBack(ISystemMsg body, SE_GPO.Event_UpdateAttribute ent) {
            attributeData = ent.Data;
        }

        private void OnGPOHurtCallBack(ISystemMsg body, SE_GPO.Event_GPOHurt ent) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            this.fireGPO = (ServerGPO)ent.AttackGPO;
            if (iGPO.IsGodMode() || iGPO.IsDead()) {
                return;
            }
            // 这边可以做伤害相关公式，比如 hurt - def  或者穿透啥的
            var downHp = Mathf.Max(1, ent.Hurt);
            this.fireGPO.Dispatcher(new SE_GPO.Event_SetHurtTOGpo {
                HurtValue = downHp, AttackGPO = this.fireGPO, HurtGPO = iGPO, DamageType = ent.DamageType, AttackItemId = ent.AttackItemId, IsHead = false
            });
            Dispatcher(new SE_GPO.Event_DownHP {
                DownHp = downHp, AttackGPO = fireGPO, AttackItemId = ent.AttackItemId, DownHpGPO = iGPO
            });
            gpoIdToFirstAttackTime.TryAdd(fireGPO.GpoID, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        private void OnGetGPOFirstAttackTime(ISystemMsg body, SE_GPO.Event_GetGPOAttacDuration ent) {
            var attackDuration = gpoIdToFirstAttackTime.TryGetValue(ent.AttackGPOId, out long firstAttackTime)
                ? (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - firstAttackTime) / 1000f
                : 0f;
            ent.CallBack.Invoke(attackDuration);
        }
        

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (!ent.IsDead) {
                return;
            }
            gpoIdToFirstAttackTime.Clear();
        }
    }
}