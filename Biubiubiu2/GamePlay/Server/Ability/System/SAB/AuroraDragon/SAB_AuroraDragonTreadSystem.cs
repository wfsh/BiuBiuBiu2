using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonTreadSystem : S_Ability_Base {
        private AbilityM_AuroraDragonTread useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonTread)MData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }
        
        
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonTread>(new AbilityAuroraDragonTread.InitData() {
                Param = useMData
            });
            AddLifeCycle();
            AddHit();
        }
        
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useMData.M_ATK,
                WeaponItemId = 0,
            });
        }
    }
}