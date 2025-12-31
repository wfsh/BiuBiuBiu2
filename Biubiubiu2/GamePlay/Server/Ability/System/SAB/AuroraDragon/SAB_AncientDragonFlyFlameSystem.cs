using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AncientDragonFlyFlameSystem : S_Ability_Base {
        private AbilityM_AncientDragonFlyFlame useMData;
        private Transform attackBoxTran;
        
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AncientDragonFlyFlame)MData;
      
            var entity = (AIEntity)FireGPO.GetEntity();
            attackBoxTran = entity.AttackTran;
            iEntity.SetPoint(attackBoxTran.position + useMData.M_EffectPos);
            iEntity.SetRota(attackBoxTran.rotation);
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAncientDragonFlyFlame>(new AbilityAncientDragonFlyFlame.InitData() {
                Param = useMData
            });
            AddLifeCycle();
            AddHit();
        }
        
        
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_FlyUpTime + useMData.M_FlyTime + useMData.M_FlyDownTime,
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