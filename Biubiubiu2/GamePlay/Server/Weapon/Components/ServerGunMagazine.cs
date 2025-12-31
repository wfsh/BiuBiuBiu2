using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGunMagazine : ComponentBase {
        private WeaponData.GunData gunData;
        private List<WeaponData.UseBulletData> bulletList = new List<WeaponData.UseBulletData>();
        private WeaponData.UseBulletData useBulletData;
        private BulletData.Data bulletData;
        private float reloadTime = 0f;
        private float countReloadTime = 0f;
        private bool isPutOn = false;
        private bool isStartReload = false;
        private int magazineNum = 0;
        private int weaponId = 0;

        protected override void OnAwake() {
            var system = (S_Weapon_Base)mySystem;
            weaponId = system.weaponId;
            gunData = (WeaponData.GunData)system.GetData();
            mySystem.Register<SE_Weapon.Event_SetALLBullet>(OnSetBulletCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateItems>(OnUpdateItemsCallBack);
            mySystem.Register<SE_Weapon.Event_FireEnd>(OnPlayFireEndCallBack);
            mySystem.Register<SE_Weapon.Event_OnReload>(OnReloadCallBack);
            mySystem.Register<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
            mySystem.Register<SE_Weapon.Event_GetUseBulletData>(OnGetBulletDataCallBack);
            mySystem.Register<SE_Weapon.Event_SetUseBullet>(OnSetUseBulletCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateMagazineNum>(OnUpdateMagazineNumCallBack);
            mySystem.Register<SE_Weapon.Event_UpdateReloadTime>(OnUpdateReloadTimeCallBack);
            mySystem.Register<SE_Weapon.Event_ReloadAllBulletNoTime>(OnReloadAllBulletNoTimeCallBack);
        }
        
        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_SetALLBullet>(OnSetBulletCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateItems>(OnUpdateItemsCallBack);
            mySystem.Unregister<SE_Weapon.Event_FireEnd>(OnPlayFireEndCallBack);
            mySystem.Unregister<SE_Weapon.Event_OnReload>(OnReloadCallBack);
            mySystem.Unregister<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetUseBulletData>(OnGetBulletDataCallBack);
            mySystem.Unregister<SE_Weapon.Event_SetUseBullet>(OnSetUseBulletCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateMagazineNum>(OnUpdateMagazineNumCallBack);
            mySystem.Unregister<SE_Weapon.Event_UpdateReloadTime>(OnUpdateReloadTimeCallBack);
            mySystem.Unregister<SE_Weapon.Event_ReloadAllBulletNoTime>(OnReloadAllBulletNoTimeCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            FireReload();
            
        }
        
        private void OnUpdateMagazineNumCallBack(ISystemMsg body, SE_Weapon.Event_UpdateMagazineNum ent) {
            magazineNum = ent.MagazineNum;
            if (bulletList.Count > 0) {
                for (int i = 0; i < bulletList.Count; i++) {
                    var bulletData = bulletList[i];
                    var bullet = BulletData.GetBulletData(bulletData.BulletId);
                    bulletData.UseMagazineNum = (ushort)Mathf.CeilToInt((float)this.magazineNum / bullet.MagazineGridNum);;
                }
            } else {
                InitBulletList();
            }
        }
        
        private void OnUpdateReloadTimeCallBack(ISystemMsg body, SE_Weapon.Event_UpdateReloadTime ent) {
            reloadTime = ent.ReloadTime;
        }
        
        private void OnReloadAllBulletNoTimeCallBack(ISystemMsg body, SE_Weapon.Event_ReloadAllBulletNoTime ent) {
            ReloadBullet();
        }
          
        private void InitBulletList() {
            if (gunData.CanUseBullet.Length <= 0) {
                Debug.LogError($"{gunData.ItemId} CanUseBullet.Length <= 0");
                return;
            }
            for (int i = 0; i < gunData.CanUseBullet.Length; i++) {
                var canUseBulletData = gunData.CanUseBullet[i];
                var bulletData = BulletData.GetBulletData(canUseBulletData.BulletId);
                var isUnlimited = canUseBulletData.isUnlimited;
                var magazineNum = (ushort)Mathf.CeilToInt((float)this.magazineNum / bulletData.MagazineGridNum);
                var useBulletData = new WeaponData.UseBulletData {
                    BulletId = bulletData.BulletId,
                    UseItemId = bulletData.UsedItemID,
                    AbilityBulletID = bulletData.AbilityBulletID,
                    isUnlimited = isUnlimited,
                    UseMagazineNum = magazineNum,
                    UseBulletNum = magazineNum,
                    MaxBulletNum = isUnlimited ? -1 : 0,
                };
                bulletList.Add(useBulletData);
                if (i == 0 || isUnlimited) {
                    SetUseBullet(canUseBulletData.BulletId);
                }
            }
        }
        
        private void OnReloadCallBack(ISystemMsg body, SE_Weapon.Event_OnReload ent) {
            if (isStartReload || useBulletData.UseBulletNum >= useBulletData.UseMagazineNum) {
                return;
            }
            if (useBulletData.isUnlimited == false) {
                if (useBulletData.UseBulletNum >= useBulletData.MaxBulletNum) {
                    return;
                }
            }
            StartReload();
        }

        private void OnUpdateItemsCallBack(ISystemMsg body, SE_Weapon.Event_UpdateItems ent) {
            for (int j = 0; j < bulletList.Count; j++) {
                var bulletData = bulletList[j];
                if (bulletData.isUnlimited) {
                    continue;
                }
                bulletData.MaxBulletNum = 0;
                for (int i = 0; i < ent.ItemList.Count; i++) {
                    var itemData = ent.ItemList[i];
                    if (itemData.ItemId == bulletData.UseItemId) {
                        SetMaxBulletNum(bulletData, itemData.ItemNum);
                        return;
                    }
                }
                SetMaxBulletNum(bulletData, 0);
            }
        }

        private void OnPutOnCallBack(ISystemMsg body, SE_Weapon.Event_PutOn ent) {
            isPutOn = ent.IsTrue;
            if (ent.IsTrue) {
                mySystem.Dispatcher(new SE_Weapon.Event_GetALLBullet {
                    iWeapon = (IWeapon)mySystem
                });
                mySystem.Dispatcher(new SE_Weapon.Event_UseBullet {
                    Data = useBulletData
                });
                CheckReload();
                SendNowBulletNum();
            } else {
                CanceReload();
            }
        }

        private void OnSetBulletCallBack(ISystemMsg body, SE_Weapon.Event_SetALLBullet ent) {
            for (int i = 0; i < bulletList.Count; i++) {
                var bulletData = bulletList[i];
                if (bulletData.UseItemId == ent.UseItemId) {
                    SetMaxBulletNum(bulletData, ent.BulletNum);
                    break;
                }
            }
        }
        
        private void SetMaxBulletNum(WeaponData.UseBulletData bulletData, int bulletNum) {
            bulletData.MaxBulletNum = bulletNum;
            if (useBulletData == bulletData) {
                SendNowBulletNum();
            }
            CheckReload();
        }
        

        private void OnPlayFireEndCallBack(ISystemMsg body, SE_Weapon.Event_FireEnd ent) {
            if (useBulletData == null) {
                Debug.LogError("useBulletData is null");
                return;
            }
            SetBullet((ushort)(useBulletData.UseBulletNum - 1));
            CheckReload();
        }
        
        private void OnGetBulletDataCallBack(ISystemMsg body, SE_Weapon.Event_GetUseBulletData ent) {
            ent.CallBack(useBulletData);
        }
        
        private void OnSetUseBulletCallBack(ISystemMsg body, SE_Weapon.Event_SetUseBullet ent) {
            SetUseBullet(ent.UseBullet);
        }
        
        private void SetUseBullet(int bulletId) {
            var data = GetBullet(bulletId);
            if (data == null) {
                Debug.LogError("该枪械无法配备这个子弹:" + bulletId);
                return;
            }
            useBulletData = data;
            bulletData = BulletData.GetBulletData(data.BulletId);
            mySystem.Dispatcher(new SE_Weapon.Event_UseBullet {
                Data = useBulletData
            });
            SendNowBulletNum();
            CheckReload();
        }

        private WeaponData.UseBulletData GetBullet(int bulletId) {
            for (int i = 0; i < bulletList.Count; i++) {
                var useBulletData = bulletList[i];
                if (useBulletData.BulletId == bulletId) {
                    return useBulletData;
                }
            }
            return null;
        }
        
        
        private void FireReload() {
            if (isStartReload == false) {
                return;
            }
            if (countReloadTime > 0f) {
                countReloadTime -= Time.deltaTime;
                if (countReloadTime <= 0f) {
                    ReloadBullet();
                }
            } 
        }

        private void ReloadBullet() {
            ushort reloadBullet = 0;
            if (useBulletData.isUnlimited) {
                reloadBullet = useBulletData.UseMagazineNum;
                SetBullet(reloadBullet);
            } else {
                var maxReloadBullet = (ushort)Mathf.Min(useBulletData.UseMagazineNum, useBulletData.MaxBulletNum);
                reloadBullet = (ushort)(maxReloadBullet - useBulletData.UseBulletNum);
                if (reloadBullet <= 0) {
                    return;
                }
                mySystem.Dispatcher(new SE_Weapon.Event_OnDownBullet {
                    UseBullet = useBulletData.UseItemId,
                    BulletNum = reloadBullet,
                    CallBack = num => {
                        var nowBulletNum = useBulletData.UseBulletNum + num;
                        if (nowBulletNum > maxReloadBullet) {
                            nowBulletNum = maxReloadBullet;
                            Debug.Log("子弹数量超过最大值:" + maxReloadBullet + " 填充值:" + nowBulletNum + " 当前值:" + useBulletData.UseBulletNum + " 填充数量:" + num);
                        }
                        SetBullet((ushort)nowBulletNum);
                    }
                });
            }
            CanceReload();
        }

        private void SetBullet(ushort bullet) {
            useBulletData.UseBulletNum = bullet;
            SendNowBulletNum();
        }
        
        private void SendNowBulletNum() {
            mySystem.Dispatcher(new SE_Weapon.Event_NowBulletNum {
                BulletNum = useBulletData.UseBulletNum,
                UseBullet = useBulletData.BulletId,
                WeaponId = weaponId
            });
        }

        private void CheckReload() {
            if (isStartReload || isPutOn == false) {
                return;
            }
            if (useBulletData.isUnlimited == false && useBulletData.MaxBulletNum <= 0) {
                return;
            }
            if (useBulletData.UseBulletNum <= 0) {
                StartReload();
            }
        }

        private void StartReload() {
            if (reloadTime <= 0f) {
                ReloadBullet();
                return;
            }
            isStartReload = true;
            countReloadTime = reloadTime;
            mySystem.Dispatcher(new SE_Weapon.Event_Reload {
                isTrue = true,
            });
        }

        private void CanceReload() {
            if (isStartReload == false) {
                return;
            }
            isStartReload = false;
            mySystem.Dispatcher(new SE_Weapon.Event_Reload {
                isTrue = false,
            });
        }

        private void ChangeBullet() {
            var index = bulletList.IndexOf(useBulletData) - 1;
            for (int i = index; i >= 0; i--) {
                var bulletData = bulletList[i];
                if (bulletData.isUnlimited || bulletData.MaxBulletNum > 0) {
                    SetUseBullet(bulletData.BulletId);
                    return;
                }
            }
        }
    }
}