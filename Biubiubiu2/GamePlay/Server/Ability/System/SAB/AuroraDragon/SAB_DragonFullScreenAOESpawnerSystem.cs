using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_DragonFullScreenAOESpawnerSystem : S_Ability_Base {
        private AbilityM_DragonFullScreenAOESpawner useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_DragonFullScreenAOESpawner)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }
        

        private void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityDragonFullScreenAOESpawner>(new AbilityDragonFullScreenAOESpawner.InitData() {
                Param = useMData
            });
            AddLifeCycle();
        }
        
        private void AddLifeCycle() {
            var lifeTime = 0f;
            for (int i = 0; i < useMData.M_SpawnerCount; i++) {
                lifeTime += useMData.M_LifeTime;
            }
            lifeTime += Mathf.Max(0, (useMData.M_SpawnerCount - 1) * useMData.M_NextTime);
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = lifeTime
            });
        }
    }
}
