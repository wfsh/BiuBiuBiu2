using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_DragonDelayBlastSpawnerSystem : S_Ability_Base {
        private AbilityM_DragonDelayBlastSpawner useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_DragonDelayBlastSpawner)MData;
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
            AddComponent<AbilityDragonDelayBlastSpawner>(new AbilityDragonDelayBlastSpawner.InitData() {
                Param = useMData
            });
            AddLifeCycle();
        }
        
 
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_CreateTime +  useMData.M_BoomTime + useMData.M_AttackNum * useMData.M_CreateInterval + 1f
            });
        }
    }
}
