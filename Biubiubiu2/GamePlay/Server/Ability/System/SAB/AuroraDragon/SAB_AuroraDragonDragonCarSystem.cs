using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonDragonCarSystem : S_Ability_Base {
        private AbilityM_AuroraDragonDragonCar useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonDragonCar)MData;
            AddComponents();
            iEntity.SetRota(FireGPO.GetRota());
            iEntity.SetLocalScale(useMData.M_RushEffectScale);
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonDragonCar>(new AbilityAuroraDragonDragonCar.InitData() {
                Param = useMData
            });
            AddLifeCycle();
            AddHit();
        }

        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_ChargeTime + useMData.M_RushTime + useMData.M_WaitTime,
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