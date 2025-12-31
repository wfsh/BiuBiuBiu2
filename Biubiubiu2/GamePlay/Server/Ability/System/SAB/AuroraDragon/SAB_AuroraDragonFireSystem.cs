using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonFireSystem : S_Ability_Base {
        private AbilityM_AuroraDragonFire useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonFire)MData;
            AddComponents();
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            Debug.Log("SAB_AuroraDragonFireSystem Rota:" + useMData.M_EffectRota +"  Point:" + useMData.M_EffectPos);
            AddComponent<AbilityAuroraDragonFire>(new AbilityAuroraDragonFire.InitData() {
                Param = useMData,
            });
            AddLifeCycle();
            AddHit();
        }
        
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_ChargeTime + useMData.M_AttackTime + useMData.M_WaitTime
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