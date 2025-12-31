using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityLifeCycle_GpoDead : ServerAbilityLifeCycle {
        private IGPO fireGpo;
        protected override void OnAwake() {
            base.OnAwake();
            var system = (S_Ability_Base)mySystem;
            fireGpo = system.FireGPO;
            fireGpo.Register<SE_GPO.Event_SetIsDead>(OnFireGPODeadCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            fireGpo?.Unregister<SE_GPO.Event_SetIsDead>(OnFireGPODeadCallBack);
            fireGpo = null;
        }
        
        private void OnFireGPODeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                ChangeLifeCycle(0);
            }
        }
    }
}