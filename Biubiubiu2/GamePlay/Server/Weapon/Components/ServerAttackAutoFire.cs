using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackAutoFire : ComponentBase {
        private List<IGPO> gpoList;
        private WeaponData.GunData gunData;
        private WeaponData.UseBulletData bulletData;
        private IGPO useGPO;
        private IGPO lockGPO;
        private float maxRange = 0f; // 扩散最大值
        private ushort bulletCount = 0;
        private float reloadTime = 0f;
        private float fireIntervalTime = 0f;
        private float intervalTime = -1f;
        private float checkLockGPO = 1.0f;
        private bool isReload = false;
        private bool isFire = false;
        private bool isDownFire = false;
        private float maxDistance = 50f;
        private float maxAngle = 30f;
        private bool isStartAutoFire = false;
        private RaycastHit[] raycastHit;
        private bool isPutOn = false;
        private float startFireWaitTimer = 1f; // 开火等待时间

        protected override void OnAwake() {
            var system = (S_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)system.GetData();
            maxAngle = gunData.AutoFireAngle;
            mySystem.Register<SE_Weapon.Event_UseBullet>(OnUseBulletCallBack);
            mySystem.Register<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            mySystem.Register<SE_Weapon.Event_Reload>(OnReloadCallBack);
            mySystem.Register<SE_Weapon.Event_EnabledAutoFire>(OnEnabledAutoFireCallBack);
            mySystem.Register<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTimeCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateFireAngle>(OnUpdateFireAngleCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
        }

        protected override void OnStart() {
            InitAutoFire();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_UseBullet>(OnUseBulletCallBack);
            mySystem.Unregister<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            mySystem.Unregister<SE_Weapon.Event_Reload>(OnReloadCallBack);
            mySystem.Unregister<SE_Weapon.Event_EnabledAutoFire>(OnEnabledAutoFireCallBack);
            mySystem.Unregister<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTimeCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateFireAngle>(OnUpdateFireAngleCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateFireDistance>(OnUpdateFireDistanceCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnEnabledAutoFireCallBack(ISystemMsg body, SE_Weapon.Event_EnabledAutoFire ent) {
            isStartAutoFire = ent.IsTrue;
            if (ent.IsTrue) {
                startFireWaitTimer = ent.StartFireWaitTime;
            }
        }

        private void OnUpdateFireAngleCallBack(ISystemMsg body, SE_Weapon.Event_UpdateFireAngle ent) {
            maxAngle = ent.FireAngle;
        }

        private void OnUpdateFireDistanceCallBack(ISystemMsg body, SE_Weapon.Event_UpdateFireDistance ent) {
            maxDistance = ent.FireDistance;
        }

        private void OnUpdateIntervalTimeCallBack(ISystemMsg body, SE_Weapon.Event_UpdateIntervalTime ent) {
            intervalTime = ent.IntervalTime;
        }

        private void InitAutoFire() {
            if (useGPO != null) {
                return;
            }
            useGPO = ((S_Weapon_Base)mySystem).UseGPO();
            raycastHit = new RaycastHit[10];
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
            AddUpdate(OnUpdate);
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            gpoList = list;
        }

        private void OnUpdate(float deltaTime) {
            if (isPutOn == false || intervalTime <= -1 || useGPO.IsClear() || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                OnFire(false);
                return;
            }
            if (startFireWaitTimer > 0f) {
                startFireWaitTimer -= Time.deltaTime;
                return;
            }
            if (isStartAutoFire) {
                CheckLockGPO();
                if (lockGPO != null && lockGPO.IsClear()) {
                    lockGPO = null;
                }
                if (lockGPO != null  &&  bulletCount > 0f) {
                    OnFire(true);
                } else {
                    if (isDownFire) {
                        OnFire(false);
                    }
                }
            } else {
                OnFire(false);
            }
            FireInterval();
        }

        private void CheckLockGPO() {
            if (checkLockGPO > 0f) {
                checkLockGPO -= Time.deltaTime;
                return;
            }
            checkLockGPO = 0.1f;
            lockGPO = FindNearestGPOInFront();
        }


        private void FireInterval() {
            if (fireIntervalTime > 0f) {
                fireIntervalTime -= Time.deltaTime;
            } else {
                if (isFire == false || isReload) {
                    return;
                }
                if (isDownFire) {
                    if (bulletCount > 0) {
                        fireIntervalTime = intervalTime;
                        Fire();
                    }
                } else {
                    CanceFire();
                }
            }
        }

        private void OnFire(bool isDown) {
            if (isDown) {
                isDownFire = true;
                isFire = true;
            } else {
                isDownFire = false;
            }
        }

        private void Fire() {
            var hitHeadRatio = Random.Range(0f, 1f);
            Transform hitTransform = null;
            if (hitHeadRatio <= 0.2f) {
                hitTransform = lockGPO.GetBodyTran(GPOData.PartEnum.Head);
            } else {
                hitTransform = lockGPO.GetBodyTran(GPOData.PartEnum.Body);
            }
            var hitPoint = lockGPO.GetPoint();
            if (hitTransform != null) {
                hitPoint = hitTransform.position;
            }
            mySystem.Dispatcher(new SE_Weapon.Event_GunFire {
                StartPoint = iEntity.GetPoint(), TargetPoint = hitPoint,
            });
        }

        private void CanceFire() {
            isFire = false;
            fireIntervalTime = 0f;
        }

        /// <summary>
        /// 查找最近的目标对象 GPO
        /// </summary>
        private IGPO FindNearestGPOInFront() {
            IGPO nearestGpo = null;
            var nearestValue = Mathf.Infinity;
            foreach (var gpo in gpoList) {
                if (gpo.IsClear() || useGPO.GetTeamID() == gpo.GetTeamID()) {
                    continue;
                }
                var directionToPet = gpo.GetPoint() - useGPO.GetPoint();
                var distanceToPet = directionToPet.magnitude;
                if (distanceToPet <= maxDistance) {
                    // 计算当前对象正前方与宠物方向的夹角
                    var angle = Vector3.Angle(useGPO.GetForward(), directionToPet);
                    // 判断宠物是否在前方 XX 度范围内
                    if (angle <= maxAngle) {
                        // 90度的范围，单边 XX 度
                        var checkValue = angle + distanceToPet;
                        if (checkValue < nearestValue) {
                            var isObstacle = CheckObstacle(useGPO, gpo);
                            if (isObstacle == false) {
                                nearestValue = checkValue;
                                nearestGpo = gpo;
                            }
                        }
                    }
                }
            }
            return nearestGpo;
        }

        // 检查和 GPO 之间是否有障碍物
        private bool CheckObstacle(IGPO targetGpo, IGPO useGpo) {
            var useTran = CheckGPOTran(useGpo);
            var targetTran = CheckGPOTran(targetGpo);
            if (useTran == null || targetTran == null) {
                return false;
            }
            var startPoint = useTran.position;
            var endPoint = targetTran.position;
            var forwart = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forwart, raycastHit, distance,~(LayerData.ClientLayerMask | LayerData.AirWallLayerMask));
            var isHit = false;
            if (count > 0) {
                isHit = CheckObstacleRaycastHit(count);
            }
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, endPoint, isHit ? Color.red : Color.green);
#endif
            return isHit;
        }

        public bool CheckObstacleRaycastHit(int count) {
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }
                return true;
            }
            return false;
        }

        private Transform CheckGPOTran(IGPO igpo) {
            var tran = igpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = igpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            return tran;
        }

        private void OnUseBulletCallBack(ISystemMsg body, SE_Weapon.Event_UseBullet ent) {
            bulletData = ent.Data;
        }

        private void OnNowBulletNumCallBack(ISystemMsg body, SE_Weapon.Event_NowBulletNum ent) {
            bulletCount = ent.BulletNum;
        }

        private void OnReloadCallBack(ISystemMsg body, SE_Weapon.Event_Reload ent) {
            isReload = ent.isTrue;
        }

        private void OnPutOnCallBack(ISystemMsg body, SE_Weapon.Event_PutOn ent) {
            isPutOn = ent.IsTrue;
        }
    }
}