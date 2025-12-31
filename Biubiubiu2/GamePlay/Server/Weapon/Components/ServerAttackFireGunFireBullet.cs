using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackFireGunFireBullet : ComponentBase {
        private IGPO fireGPO;
        private WeaponData.UseBulletData bulletData;
        private WeaponData.GunData gunData;
        private int weaponItemId;
        private float fireDistance;
        private int hitCount;
        private int fireCount;
        private bool isAbilityInit;
        private float holdFireTime = 0.5f;
        private IAbilitySystem mAbilitySystem;

        protected override void OnAwake() {
            mySystem.Register<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
            mySystem.Register<SE_Weapon.Event_Fire>(OnFireCallBack);
            mySystem.Register<SE_Weapon.Event_GetGunFireRecord>(OnGetGunFireRecordCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
            var system = (S_Weapon_Base)mySystem;
            weaponItemId = system.GetWeaponItemId();
            fireGPO = system.UseGPO();
            fireGPO.Register<SE_GPO.Event_SetIsDead>(OnGPODeadCallBack);
            gunData = (WeaponData.GunData)system.GetData();
            AddUpdate(OnUpdate);
        }


        protected override void OnClear() {
            base.OnClear();
            FireEnd();
            bulletData = null;
            fireCount = 0;
            hitCount = 0;
            mySystem.Unregister<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
            mySystem.Unregister<SE_Weapon.Event_Fire>(OnFireCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetGunFireRecord>(OnGetGunFireRecordCallBack);
            fireGPO.Unregister<SE_GPO.Event_SetIsDead>(OnGPODeadCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnGPODeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                // Debug.LogError("FireGPO dead");
                FireEnd();
            }
        }
        private void OnUpdateFireDistanceCallBack(ISystemMsg body, SE_Weapon.Event_UpdateFireDistance ent) {
            fireDistance = ent.FireDistance;
        }

        private void OnUpdate(float deltaTime) {
            if (holdFireTime <= 0) {
                holdFireTime = 1;
                FireEnd();
                return;
            }

            holdFireTime -= deltaTime;
        }

        private void FireEnd() {
            isAbilityInit = false;
            LifeTimeEnd();
            mAbilitySystem = null;
        }


        private void LifeTimeEnd() {
            if (mAbilitySystem == null) {
                return;
            }

            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = mAbilitySystem.GetAbilityId()
            });
        }

        private void OnAddAbilityCallBack(IAbilitySystem ibs) {
            mAbilitySystem = ibs;
        }

        private void OnFireCallBack(ISystemMsg body, SE_Weapon.Event_Fire ent) {
            bulletData = ent.BulletData;
            if (isAbilityInit == false) {
                isAbilityInit = true;
                //实例化火焰ability
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    CallBack = OnAddAbilityCallBack,
                    FireGPO = fireGPO,
                    AbilityMData = new AbilityData.PlayAbility_FireGunFire() {
                        ConfigId = AbilityConfig.ChangeFireGunFireState,
                        M_Range = fireDistance,
                        M_InitRange = gunData.FireDistance,
                        In_WeaponItemId = gunData.ItemId,
                        In_BulletAttnMap = gunData.BulletAttnMaps,
                    },
                });
            }

            holdFireTime = 0.5f;
        }


        private void OnGetGunFireRecordCallBack(ISystemMsg body, SE_Weapon.Event_GetGunFireRecord ent) {
            if (mySystem is ServerGunRPGSystem) {
                ent.CallBack(0, 0);
            }
            else {
                ent.CallBack(fireCount, hitCount);
            }
        }

        private void OnPlayFireAbilityBack(ISystemMsg body, SE_Weapon.Event_PlayFireAbility ent) {
            fireCount++;
            mAbilitySystem.Dispatcher(ent);
        }
    }
}