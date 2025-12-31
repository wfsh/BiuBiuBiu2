using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityHurtGPOByValue : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int Power;
            public int WeaponItemId;
        }
        private int power = 0;
        private S_Ability_Base abSystem;
        private int weaponItemId;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            var initData = (InitData)initDataBase;
            SetPower(initData.Power, initData.WeaponItemId);
        }


        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
        }

        public void SetPower(int power, int weaponItemId) {
            this.power = power;
            this.weaponItemId = weaponItemId;
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            if (entData.hitGPO.IsClear()) {
                return;
            }

            var hurt = Hit(entData.hitGPO);
            if (hurt > 0) {
                PlayEffect(entData.hitGPO, entData.hitGPO.GetPoint(), hurt);
            }
        }

        private int Hit(IGPO hitGPO) {
            if (abSystem.FireGPO == null || abSystem.FireGPO.IsClear() || hitGPO.IsDead() || hitGPO.IsGodMode()) {
                return -1;
            }
            var distance = Vector3.Distance(abSystem.FireGPO.GetPoint(), hitGPO.GetPoint());
            abSystem.FireGPO.Dispatcher(new SE_Behaviour.Event_HateFindTarget {
                TargetGPO = hitGPO, Distance = distance,
            });
            hitGPO.Dispatcher(new SE_Behaviour.Event_HateLockTarget {
                TargetGPO = abSystem.FireGPO, Distance = distance,
            });
            hitGPO.Dispatcher(new SE_GPO.Event_GPOHurt {
                Hurt = power,
                AttackGPO = abSystem.FireGPO,
                AttackItemId = weaponItemId,
                DamageType = DamageType.Burn
            });
            return power;
        }


        private void PlayEffect(IGPO hitGpo, Vector3 hitPoint, int hurt) {
            if (hitGpo.IsClear()) {
                return;
            }Debug.Log("PlayEffect:" + AbilityM_PlayBloodSplatter.ID_BloodSplatter);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = abSystem.FireGPO,
                MData = AbilityM_PlayBloodSplatter.CreateForID(AbilityM_PlayBloodSplatter.ID_BloodSplatter),
                InData = new AbilityIn_PlayBloodSplatter {
                    In_HitGpoId = hitGpo.GetGpoID(),
                    In_HitPoint = hitPoint,
                    In_DiffPos = hitPoint - hitGpo.GetPoint(),
                    In_BloodValue = hurt,
                    In_HitItemId = weaponItemId
                }
            });
        }
    }
}