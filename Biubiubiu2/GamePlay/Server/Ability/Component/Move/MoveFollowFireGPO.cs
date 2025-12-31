using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class MoveFollowFireGPO : ServerNetworkComponentBase {
        private IGPO fireGPO = null;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
            var system = (S_Ability_Base)mySystem;
            fireGPO = system.FireGPO;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
            if (fireGPO == null || fireGPO.IsClear()) {
                return;
            }
            iEntity.SetPoint(fireGPO.GetPoint());
        }
    }
}