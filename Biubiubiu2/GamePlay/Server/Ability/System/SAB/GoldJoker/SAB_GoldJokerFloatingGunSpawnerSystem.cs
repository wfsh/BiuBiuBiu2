using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerFloatingGunSpawnerSystem : S_Ability_Base {
        public AbilityM_GoldJokerFloatingGunSpawner useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerFloatingGunSpawner)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            AddComponent<AbilityGoldJokerFloatingGunSpawner>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
