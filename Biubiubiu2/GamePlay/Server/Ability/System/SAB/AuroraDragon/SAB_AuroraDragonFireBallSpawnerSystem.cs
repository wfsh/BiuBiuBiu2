using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonFireBallSpawnerSystem : S_Ability_Base {
        private AbilityM_AuroraDragonFireBallSpawner useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonFireBallSpawner)MData;
            AddComponents();
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            AddAttack();
            AddLifeCycle();
        }
        
        private void AddAttack() {
            AddComponent<AbilityAuroraDragonFireBallSpawner>(new AbilityAuroraDragonFireBallSpawner.InitData() {
                Param = useMData
            });
        }

        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_FireBallStartAnimTime + useMData.M_FireBallAnimTime * (float)useMData.M_FireBallAttackNum +
                           useMData.M_FireBallEndAnimTime
            });
        }
    }
}