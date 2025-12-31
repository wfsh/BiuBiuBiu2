using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackGunFireBullet : ComponentBase {
        private IGPO fireGPO;
        private WeaponData.UseBulletData bulletData;
        private WeaponData.GunData gunData;
        private int weaponItemId;
        private float fireDistance;
        private int hitCount;
        private int fireCount;

        protected override void OnAwake() {
            mySystem.Register<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
            mySystem.Register<SE_Weapon.Event_Fire>(OnFireCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
            mySystem.Register<SE_Weapon.Event_GetGunFireRecord>(OnGetGunFireRecordCallBack);
            var system = (S_Weapon_Base)mySystem;
            weaponItemId = system.GetWeaponItemId();
            gunData = (WeaponData.GunData)system.GetData();
        }

        protected override void OnStart() {
            var system = (S_Weapon_Base)mySystem;
            fireGPO = system.UseGPO();
        }

        protected override void OnClear() {
            base.OnClear();
            bulletData = null;
            fireCount = 0;
            hitCount = 0;
            mySystem.Unregister<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
            mySystem.Unregister<SE_Weapon.Event_Fire>(OnFireCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetGunFireRecord>(OnGetGunFireRecordCallBack);
        }

        private void OnFireCallBack(ISystemMsg body, SE_Weapon.Event_Fire ent) {
            bulletData = ent.BulletData;
        }

        private void OnUpdateFireDistanceCallBack(ISystemMsg body, SE_Weapon.Event_UpdateFireDistance ent) {
            fireDistance = ent.FireDistance;
        }

        private void OnGetGunFireRecordCallBack(ISystemMsg body, SE_Weapon.Event_GetGunFireRecord ent) {
            if (mySystem is ServerGunRPGSystem) {
                ent.CallBack(0, 0);
            } else {
                ent.CallBack(fireCount, hitCount);
            }
        }

        private void OnPlayFireAbilityBack(ISystemMsg body, SE_Weapon.Event_PlayFireAbility ent) {
            fireCount++;
            var bulletMoveDistance = WeaponData.GetWeaponBulletMoveDistance(fireDistance, gunData);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_Bullet.CreateForID(bulletData.AbilityBulletID),
                InData = new AbilityIn_Bullet {
                    In_StartPoint = ent.StartPoint,
                    In_TargetPoint = ent.TargetPoint,
                    In_Speed = gunData.BulletSpeed,
                    In_MoveDistance = bulletMoveDistance,
                    In_WeaponItemId = gunData.ItemId,
                    In_BulletAttnMap = gunData.BulletAttnMaps
                },
                OR_CallBack = OnAddSystemCallBack
            });
            // MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
            //     FireGPO = fireGPO,
            //     AbilityData = new AbilityData.PlayAbility_PenetratorGrenade {
            //         ModId = AbilityModData.PenetratorGrenadeBullet,
            //         In_StartPoint = ent.StartPoint,
            //         In_StartRota = GetStartRotation(ent.StartPoint, ent.TargetPoint)
            //     }
            // });
        }

        private void OnAddSystemCallBack(IAbilitySystem system) {
            if (mySystem is ServerGunRPGSystem) {
                return;
            }
            system.Register<SE_Ability.HitGPO>(OnAbilityHitGpo);
            return;
            void OnAbilityHitGpo(ISystemMsg body, SE_Ability.HitGPO hitGpo) {
                hitCount++;
                system.Unregister<SE_Ability.HitGPO>(OnAbilityHitGpo);
                
            }
        }

        // private Quaternion GetStartRotation(Vector3 startPoint, Vector3 targetPoint) {
        //     var dir = (targetPoint - startPoint).normalized;
        //     if (dir == Vector3.zero) {
        //         return Quaternion.identity;
        //     }
        //     var rot = Quaternion.LookRotation(dir);
        //     return rot;
        // }
    }
}