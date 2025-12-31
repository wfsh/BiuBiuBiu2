using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class BulletSplitTimeEnd : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityIn_Bullet inData;
            public IGPO FireGPO;
        }
        private IGPO fireGPO;
        private AbilityIn_Bullet bullet;

        protected override void OnAwake() {
            Register<SE_Ability.Ability_LifeTimeEnd>(OnAbilityTimeEnd);
            var initData = (InitData)initDataBase;
            Init(initData.inData, initData.FireGPO);
        }

        protected override void OnClear() {
            Unregister<SE_Ability.Ability_LifeTimeEnd>(OnAbilityTimeEnd);
        }

        public void Init(AbilityIn_Bullet data, IGPO fireGPO) {
            bullet = data;
            this.fireGPO = fireGPO;
        }

        private void OnAbilityTimeEnd(ISystemMsg body, SE_Ability.Ability_LifeTimeEnd callback) {
            var diffusionReduction = 0f;
            fireGPO.Dispatcher(new SE_GPO.Event_GetRandomDiffusionReduction {
                CallBack = value => diffusionReduction = value
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_SplitCone {
                    ConfigId = AbilityConfig.BulletSplitCone,
                    In_StartPoint = iEntity.GetPoint(),
                    In_ForwardAngle = iEntity.GetEulerAngles(),
                    In_Speed = bullet.In_Speed,
                    In_WeaponItemId = bullet.In_WeaponItemId,
                    In_MoveDistance = WeaponData.Get(bullet.In_WeaponItemId).AttackRange,
                    In_DiffusionReductionAngle = diffusionReduction
                },
            });

        }
    }
}