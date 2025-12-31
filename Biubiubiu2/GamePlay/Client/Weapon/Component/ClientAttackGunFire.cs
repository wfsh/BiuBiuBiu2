using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAttackGunFire : ComponentBase {
        protected bool isDownFire = false;
        protected bool isReload = false;
        protected bool isFire = false;
        protected float startFireWaitTimer = 0f; // 开火等待时间
        protected float countFireOverHotTimer = 0f; // 持续射击时间
        protected float fireOverHotTimer = 0f; // 持续射击时间
        protected float fireOverHotCDTimer = 0f; // 冷却时间
        protected bool isCoolingDown = false; // 是否处于冷却
        protected C_Weapon_Base weaponBase;
        protected int bulletCount;
        protected WeaponData.GunData gunData;
        
        private float countTime = 0f;
        private Transform fireBox;
        private float fireIntervalTime = -1f;
        private int teamId = 0;

        protected override void OnAwake() {
            this.mySystem.Register<CE_Weapon.StartFire>(OnStartFireCallBack);
            this.mySystem.Register<CE_Weapon.SetBullet>(OnSetBulletCallBack);
            this.mySystem.Register<CE_Weapon.OnReload>(OnReloadCallBack);
            this.mySystem.Register<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            this.mySystem.Register<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            weaponBase = (C_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)weaponBase.ModeData;
            startFireWaitTimer = gunData.StartFireWaitTime;
            fireOverHotTimer = gunData.FireOverHotTime;
            teamId = weaponBase.useGPO.GetTeamID();
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            mySystem.Dispatcher(new CE_Weapon.GetFireOverHotTimer() {
                CallBack = value => {
                    fireOverHotTimer = value;
                } 
            });
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            GetFireBox();
        }

        private void GetFireBox() {
            var entity = (GunEntity)iEntity;
            fireBox = entity.FireBox;
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            this.mySystem.Unregister<CE_Weapon.OnReload>(OnReloadCallBack);
            this.mySystem.Unregister<CE_Weapon.StartFire>(OnStartFireCallBack);
            this.mySystem.Unregister<CE_Weapon.SetBullet>(OnSetBulletCallBack);
            this.mySystem.Unregister<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            this.mySystem.Unregister<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            this.gunData = null;
            this.weaponBase = null;
            this.fireBox = null;
        }

        
        private void OnSetWeaponAttributeCallBack(ISystemMsg body, CE_Weapon.SetWeaponAttribute ent) {
            fireIntervalTime = ent.IntervalTime;
        }
        private void OnGetFireBoxCallBack(ISystemMsg body, CE_Weapon.GetFireBox ent) {
            ent.CallBack(fireBox);
        }

        private void OnReloadCallBack(ISystemMsg body, CE_Weapon.OnReload ent) {
            isReload = ent.IsReload;
        }

        public void OnSetBulletCallBack(ISystemMsg body, CE_Weapon.SetBullet ent) {
            SetBulletNum(ent.BulletNum);
        }

        private void SetBulletNum(int value) {
            bulletCount = value;
        }

        private void OnUpdate(float deltaTime) {
            if (this.fireBox == null) {
                return;
            }
            UpdateFireOverHotCD();
            CountFireOverHotTimer();
            CountStartFireWaitTimer();
            if (isDownFire && isReload == false && startFireWaitTimer <= 0f && isCoolingDown == false && bulletCount > 0 && ModeData.PlayGameState == ModeData.GameStateEnum.RoundStart) {
                isFire = true;
            } else {
                CanceFire();
            }
            UpdateFire(deltaTime);
        }

        public void OnStartFireCallBack(ISystemMsg body, CE_Weapon.StartFire ent) {
            if (bulletCount <= 0) {
#if UNITY_EDITOR
    Debug.Log("没子弹，不能射击");
#endif
            }
            if (ent.IsTrue) {
                isDownFire = true;
            } else {
                isDownFire = false;
            }
        }

        private void UpdateFire(float delta) {
            if (countTime > 0f) {
                countTime -= delta;
            } else {
                if (isFire) {
                    MsgRegister.Dispatcher(new CM_Camera.GetCameraCenterObjPoint {
                        CallBack = GetCameraTargetPoint, FarDistance = Mathf.Min(gunData.FireDistance, 100), IgnoreTeamId = teamId
                    });
                    countTime = fireIntervalTime;
                }
            }
        }
        private void CountStartFireWaitTimer() {
            if (gunData.StartFireWaitTime <= 0) {
                return;
            }
            if (isCoolingDown == false && isDownFire && isReload == false) {
                if (startFireWaitTimer > 0f) {
                    startFireWaitTimer -= Time.deltaTime;
                }
            } else {
                if (startFireWaitTimer < gunData.StartFireWaitTime) {
                    startFireWaitTimer += Time.deltaTime;
                }
            }
        }
        
        private void CountFireOverHotTimer() {
            if (fireOverHotTimer <= 0) {
                return;
            }
            if (isFire) {
                if (countFireOverHotTimer >= fireOverHotTimer) {
                    StartCoolingDown();
                } else {
                    countFireOverHotTimer += Time.deltaTime;
                }
            } else {
                if (countFireOverHotTimer > 0f) {
                    countFireOverHotTimer -= Time.deltaTime;
                }
            }
            if (isCoolingDown == false) {
                mySystem.Dispatcher(new CE_Weapon.OutFireOverHotTimer {
                    NowOverHotTimer = countFireOverHotTimer, MaxOverHotTime = fireOverHotTimer, IsCooling = false
                });
            }
        }

        private void StartCoolingDown() {
            isCoolingDown = true;
            countFireOverHotTimer = 0f;
            fireOverHotCDTimer = gunData.FireOverHotCDTime;
        }

        private void UpdateFireOverHotCD() {
            if (isCoolingDown == false) {
                return;
            }
            fireOverHotCDTimer -= Time.deltaTime;
            if (fireOverHotCDTimer <= 0f) {
                isCoolingDown = false;
            }
            mySystem.Dispatcher(new CE_Weapon.OutFireOverHotTimer {
                NowOverHotTimer = fireOverHotCDTimer, MaxOverHotTime = gunData.FireOverHotCDTime, IsCooling = true
            });
        }

        private void GetCameraTargetPoint(Vector3 targetPoint, bool isHit) {
            if (this.fireBox == null) {
                return;
            }
            mySystem.Dispatcher(new CE_Weapon.Fire {
                StartPoint = this.fireBox.position, TargetPoint = targetPoint,
            });
        }

        private void CanceFire() {
            if (isFire == false) {
                return;
            }
            mySystem.Dispatcher(new CE_Weapon.EndFire());
            countTime = 0f;
            isFire = false;
        }
    }
}