using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonTreadFireSystem : S_Ability_Base {
        private AbilityM_AuroraDragonTreadFire Config;
        private Transform attackBoxTran;
        
        protected override void OnAwake() {
            base.OnAwake();
            Config = (AbilityM_AuroraDragonTreadFire)MData;
            AddComponents();
        }
        
        protected override void OnStart() {
            base.OnStart();
            var entity = FireGPO?.GetEntity();
            if (entity != null && entity is AIEntity monsterEntity) {
                attackBoxTran = monsterEntity.AttackTran;
                iEntity.SetPoint(attackBoxTran.position);
                iEntity.SetRota(attackBoxTran.rotation);
            }
            iEntity.SetLocalScale(Vector3.one);
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonTreadFire>(new AbilityAuroraDragonTreadFire.InitData() {
                Param = Config
            });
            AddLifeCycle();
            AddHit();
        }
        
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = Config.M_LifeTime
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = Config.M_ATK,
                WeaponItemId = 0,
            });
        }
    }
}