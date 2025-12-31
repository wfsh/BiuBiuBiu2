using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackGunFire : ComponentBase {
        private ushort bullet = 0;
        private float reloadTime = 0f;
        private float fireIntervalTime = 0f;
        private float intervalTime = -1f;
        private WeaponData.GunData gunData;
        private WeaponData.UseBulletData bulletData;
        private bool isReload = false;
        private float maxRange = 0f; // 扩散最大值
        private bool isFire = false;

        protected override void OnAwake() {
            var system = (S_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)system.GetData();
            mySystem.Register<SE_Weapon.Event_GunFire>(OnGunFireCallBack);
            mySystem.Register<SE_Weapon.Event_UseBullet>(OnUseBulletCallBack);
            mySystem.Register<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateFireRange>(OnUpdateFireRangeCallBack);
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
            mySystem.Unregister<SE_Weapon.Event_UpdateFireRange>(OnUpdateFireRangeCallBack);
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
            } else {
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

        private void OnUpdateFireRangeCallBack(ISystemMsg body, SE_Weapon.Event_UpdateFireRange ent) {
            maxRange = ent.FireRange * 0.1f;
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

        /// <summary>
        /// 圆形扩散计算
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private List<Vector3> GetRandomPoint(Vector3 firePoint, Vector3 endPoint, int spreadCount) {
            var spreadPoints = new List<Vector3>(spreadCount);
            for (int i = 0; i < spreadCount; i++) {
                if (maxRange <= 0f) {
                    spreadPoints.Add(endPoint);
                } else {
                    var distance = Vector3.Distance(firePoint, endPoint);
                    var ratio = Mathf.Clamp(distance / 100f, 0.01f, 1f);
                    var randomDistance = Random.Range(0f, maxRange * ratio);
                    var angle = Random.Range(0f, 360f);
                    var x = randomDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
                    var y = randomDistance * Mathf.Sin(angle * Mathf.Deg2Rad);
                    spreadPoints.Add(endPoint + new Vector3(x, y, x));
                }
            }
            return spreadPoints;
        }

        /// <summary>
        /// 计算横向等间距扩散点
        /// </summary>
        /// <param name="firePoint">起点（枪口位置）</param>
        /// <param name="endPoint">射击目标点</param>
        /// <param name="spreadCount">扩散数量</param>
        /// <param name="spreadWidth">扩散总宽度</param>
        /// <returns></returns>
        private List<Vector3> GetSpreadPoints(Vector3 firePoint, Vector3 endPoint, int spreadCount) {
            var spreadPoints = new List<Vector3>();
            if (spreadCount <= 1 || maxRange <= 0f) {
                spreadPoints.Add(endPoint);
                return spreadPoints;
            }
            // 计算射击方向
            var shootDirection = (endPoint - firePoint).normalized;
            // 计算横向偏移基准轴（以射击方向为基准，获取其垂直方向）
            var right = Vector3.Cross(shootDirection, Vector3.up).normalized;
            // 确保偏移轴不为零向量（可能与 up 轴共线）
            if (right == Vector3.zero) {
                right = Vector3.right;
            }
            // 计算起始点，让子弹散布点沿 `right` 轴等间距排列
            var startOffset = -maxRange * 0.5f;
            var step = maxRange / (spreadCount - 1);

            for (int i = 0; i < spreadCount; i++) {
                var spreadOffset = right * (startOffset + step * i);
                spreadPoints.Add(endPoint + spreadOffset);
            }
            return spreadPoints;
        }

        private void Fire(Vector3 startPoint, Vector3 targetPoint) {
            isFire = true;
            fireIntervalTime = intervalTime * 0.8f; // 比客户端计算的延迟相对减少。用来做一定程度的网络延迟兼容
            mySystem.Dispatcher(new SE_Weapon.Event_Fire {
                BulletData = bulletData,
            });
            List<Vector3> spreadPoints;
            if (gunData.SightStyle == WeaponData.SightStyle_Row) {
                spreadPoints = GetSpreadPoints(startPoint, targetPoint, gunData.FireBulletNum);
            } else {
                spreadPoints = GetRandomPoint(startPoint, targetPoint, gunData.FireBulletNum);
            }
            for (int i = 0; i < gunData.FireBulletNum; i++) {
                var randomPoint = spreadPoints[i];
                var direction = (randomPoint - startPoint).normalized;
                if (direction == Vector3.zero) {
                    continue;
                }
                mySystem.Dispatcher(new SE_Weapon.Event_PlayFireAbility {
                    StartPoint = startPoint, TargetPoint = randomPoint,
                });
            }
        }
    }
}