using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerRocketBombSpawnerSystem : S_Ability_Base {
        public AbilityM_GoldJokerRocketBombSpawner useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerRocketBombSpawner)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            var lifeTime = 0f;
            foreach (var time in useMData.M_LifeTime) {
                lifeTime += time;
            }
            AddComponent<AbilityGoldJokerRocketBombSpawner>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = lifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
