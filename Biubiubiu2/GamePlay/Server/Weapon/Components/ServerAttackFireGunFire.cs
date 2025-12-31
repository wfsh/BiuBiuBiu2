using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackFireGunFire : ComponentBase {
        private ushort bullet = 0;
        private float reloadTime = 0f;
        private float fireIntervalTime = 0f;
        private float intervalTime = -1f;
        private WeaponData.GunData gunData;
        private WeaponData.UseBulletData bulletData;
        private bool isReload = false;
        private bool isFire = false;

        protected override void OnAwake() {
            var system = (S_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)system.GetData();
            mySystem.Register<SE_Weapon.Event_GunFire>(OnGunFireCallBack);
            mySystem.Register<SE_Weapon.Event_UseBullet>(OnUseBulletCallBack);
            mySystem.Register<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            mySystem.Register<SE_Weapon.Event_Reload>(OnReloadCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTimeCallBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_GunFire>(OnGunFireCallBack);
            mySystem.Unregister<SE_Weapon.Event_UseBullet>(OnUseBulletCallBack);
            mySystem.Unregister<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            mySystem.Unregister<SE_Weapon.Event_Reload>(OnReloadCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTimeCallBack);
            RemoveUpdate(OnUpdate);
        }


        private void OnUpdate(float deltaTime) {
            if (intervalTime <= -1) {
                return;
            }

            FireInterval();
        }

        private void FireInterval() {
            if (fireIntervalTime > 0f) {
                fireIntervalTime -= Time.deltaTime;
            }

            if (fireIntervalTime <= 0) {
                fireIntervalTime = 0f;
                if (isFire) {
                    isFire = false;
                    mySystem.Dispatcher(new SE_Weapon.Event_FireEnd {
                    });
                }
            }
        }

        private void OnUseBulletCallBack(ISystemMsg body, SE_Weapon.Event_UseBullet ent) {
            bulletData = ent.Data;
        }

        private void OnNowBulletNumCallBack(ISystemMsg body, SE_Weapon.Event_NowBulletNum ent) {
            bullet = ent.BulletNum;
        }

        private void OnReloadCallBack(ISystemMsg body, SE_Weapon.Event_Reload ent) {
            isReload = ent.isTrue;
        }

        private void OnUpdateIntervalTimeCallBack(ISystemMsg body, SE_Weapon.Event_UpdateIntervalTime ent) {
            intervalTime = ent.IntervalTime;
        }

        private void OnGunFireCallBack(ISystemMsg body, SE_Weapon.Event_GunFire ent) {
            if (isReload || bullet <= 0 || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }

            if (fireIntervalTime <= 0f) {
                Fire(ent.StartPoint, ent.TargetPoint);
            }
        }

        private void Fire(Vector3 startPoint, Vector3 targetPoint) {
            isFire = true;
            fireIntervalTime = intervalTime; // 比客户端计算的延迟相对减少。用来做一定程度的网络延迟兼容
            mySystem.Dispatcher(new SE_Weapon.Event_Fire {
                BulletData = bulletData,
            });
            for (int i = 0; i < gunData.FireBulletNum; i++) {
                var direction = (targetPoint - startPoint).normalized;
                if (direction == Vector3.zero) {
                    continue;
                }

                mySystem.Dispatcher(new SE_Weapon.Event_PlayFireAbility {
                    StartPoint = startPoint, TargetPoint = targetPoint,
                });
            }
        }
    }
}