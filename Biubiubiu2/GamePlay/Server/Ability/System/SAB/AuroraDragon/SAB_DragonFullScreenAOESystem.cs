using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_DragonFullScreenAOESystem : S_Ability_Base {
        public AbilityData.PlayAbility_DragonFullScreenAOE InData;
        private AbilityM_DragonFullScreenAOESpawner Param;

        protected override void OnAwake() {
            base.OnAwake();
            InData = (AbilityData.PlayAbility_DragonFullScreenAOE)MData;
            Param = InData.In_Param;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            Param = null;
        }   

        private void AddComponents() {
            base.AddComponents();
            AddAttack();
            AddLifeCycle();
            AddHit();
        }

        private void AddAttack() {
            AddComponent<AbilityDragonFullScreenAOE>(new AbilityDragonFullScreenAOE.InitData {
                Param = Param,
                SpawnPointIndex = InData.In_SpawnPointIndex,
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = Param.M_ATK,
                WeaponItemId = 0,
            });
        }

        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = Param.M_LifeTime
            });
        }
    }
}
