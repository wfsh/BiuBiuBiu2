using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonDelayBlastSystem : S_Ability_Base {
        public AbilityData.PlayAbility_AuroraDragonDelayBlast InData;
        private AbilityM_DragonDelayBlastSpawner Param;
        
        protected override void OnAwake() {
            base.OnAwake();
            InData = (AbilityData.PlayAbility_AuroraDragonDelayBlast)MData;
            Param = InData.In_Param;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }
        
        private void AddComponents() {
            base.AddComponents();
            AddAttack();
            AddLifeCycle();
            AddHit();
        }
        
        private void AddAttack() {
            AddComponent<AbilityAuroraDragonDelayBlast>(new AbilityAuroraDragonDelayBlast.InitData {
                Param = Param,
            });
        }

        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = Param.M_CreateTime + Param.M_BoomTime + 1f
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = Param.M_ATK,
                WeaponItemId = 0,
            });
        }
    }
}