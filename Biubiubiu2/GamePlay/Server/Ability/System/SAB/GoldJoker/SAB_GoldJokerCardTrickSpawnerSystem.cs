using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerCardTrickSpawnerSystem : S_Ability_Base {
        public AbilityM_GoldJokerCardTrickSpawner useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerCardTrickSpawner)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void AddComponents() {
            var lifeTime = 0f;
            for (int i = 0; i < useMData.M_LifeTime.Length; i++) {
                lifeTime += useMData.M_LifeTime[i];
            }
            AddComponent<AbilityGoldJokerCardTrickSpawner>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = lifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
