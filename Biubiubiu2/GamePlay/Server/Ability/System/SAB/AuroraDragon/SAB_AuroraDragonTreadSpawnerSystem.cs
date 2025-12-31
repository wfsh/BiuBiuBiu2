using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonTreadSpawnerSystem : S_Ability_Base {
        private AbilityM_AuroraDragonTreadSpawner useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonTreadSpawner)MData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonTreadSpawner>(new AbilityAuroraDragonTreadSpawner.InitData() {
                Param = useMData
            });
            AddLifeCycle();
        }

        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_TreadAttackTime * (float)useMData.M_TreadAttackNum + useMData.M_TreadFireAttacKTime
            });
        }
    }
}