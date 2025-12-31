using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_DragonFireTornadoSystem : S_Ability_Base {
        private AbilityM_DragonFireTornado InData;

        protected override void OnAwake() {
            base.OnAwake();
            InData = (AbilityM_DragonFireTornado)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }
        

        private void AddComponents() {
            AddAttack();
            AddLifeCycle();
            AddHit();
        }

        private void AddAttack() {
            AddComponent<ServerAbilitySync>();
            AddComponent<AbilityDragonFireTornado>(new AbilityDragonFireTornado.InitData() {
                Param = InData
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = InData.M_ATK,
                WeaponItemId = 0,
            });
        }

        private void AddLifeCycle() {
            // 总时间 = ((前摇时间  + 攻击间隔 + 特效持续时间) * 生成轮数)))
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = ((InData.M_WarmTime + InData.M_AttackEffecPlayTime) + InData.M_LifeTime * InData.M_SpawnCout)
            });
        }
    }
}
