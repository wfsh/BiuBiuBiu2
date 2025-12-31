using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunMagazine : ComponentBase {
        private WeaponData.GunData gunData;
        private List<WeaponData.UseBulletData> bulletList = new List<WeaponData.UseBulletData>();
        private WeaponData.UseBulletData useBulletData;
        private int magazineNum;
        private bool isReload = false;
        private float reloadTime = 0f;

        protected override void OnAwake() {
            var system = (C_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)system.GetData();
            mySystem.Register<CE_Weapon.SetBullet>(OnSetBulletCallBack);
            mySystem.Register<CE_Weapon.GetAllBulletList>(OnGetAllBulletDataCallBack);
            mySystem.Register<CE_Weapon.GetUseBulletData>(OnGetUseBulletDataCallBack);
            mySystem.Register<CE_Weapon.SetUseBullet>(OnSetUseBulletCallBack);
            mySystem.Register<CE_Weapon.UpdatePickItem>(OnUpdatePickItemCallBack);
            mySystem.Register<CE_Weapon.CanReload>(OnCanReloadCallBack);
            mySystem.Register<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            mySystem.Register<CE_Weapon.OnReload>(OnReloadCallBack);
        }
        
        protected override void OnStart() {
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Weapon.SetBullet>(OnSetBulletCallBack);
            mySystem.Unregister<CE_Weapon.GetAllBulletList>(OnGetAllBulletDataCallBack);
            mySystem.Unregister<CE_Weapon.GetUseBulletData>(OnGetUseBulletDataCallBack);
            mySystem.Unregister<CE_Weapon.SetUseBullet>(OnSetUseBulletCallBack);
            mySystem.Unregister<CE_Weapon.UpdatePickItem>(OnUpdatePickItemCallBack);
            mySystem.Unregister<CE_Weapon.CanReload>(OnCanReloadCallBack);
            mySystem.Unregister<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            mySystem.Unregister<CE_Weapon.OnReload>(OnReloadCallBack);
            bulletList.Clear();
        }
        
        private void OnSetWeaponAttributeCallBack(ISystemMsg body, CE_Weapon.SetWeaponAttribute ent) {
            magazineNum = ent.MagazineNum;
            reloadTime = ent.ReloadTime;
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

        private void OnReloadCallBack(ISystemMsg body, CE_Weapon.OnReload ent) {
            isReload = ent.IsReload;
            var useTime = 0f;
            if (isReload == true) {
                useTime = reloadTime;
            } else {
                useTime = 0;
            }
            mySystem.Dispatcher(new CE_Weapon.OutReloadTime {
                ReloadTime = useTime
            });
        }

        private void InitBulletList() {
            for (int i = 0; i < gunData.CanUseBullet.Length; i++) {
                var canUseBulletData = gunData.CanUseBullet[i];
                var bulletData = BulletData.GetBulletData(canUseBulletData.BulletId);
                var isUnlimited = canUseBulletData.isUnlimited;
                bulletList.Add(new WeaponData.UseBulletData {
                    BulletId = canUseBulletData.BulletId,
                    UseItemId = bulletData.UsedItemID,
                    isUnlimited = isUnlimited,
                    UseMagazineNum = (ushort)Mathf.CeilToInt((float)magazineNum / bulletData.MagazineGridNum),
                    UseBulletNum = 0,
                    MaxBulletNum = 0,
                });
                if (i == 0 || isUnlimited) {
                    SetUseBullet(canUseBulletData.BulletId);
                }
            }
            mySystem.Dispatcher(new CE_Weapon.AllBulletList {
                BulletList = bulletList
            });
        }

        private void OnUpdatePickItemCallBack(ISystemMsg body, CE_Weapon.UpdatePickItem ent) {
            var list = ent.PickItemList;
            for (int j = 0; j < bulletList.Count; j++) {
                var bulletData = bulletList[j];
                if (bulletData.isUnlimited) {
                    continue;
                }
                for (int i = 0; i < list.Count; i++) {
                    var itemData = list[i];
                    if (itemData.ItemId == bulletData.UseItemId) {
                        bulletData.MaxBulletNum = itemData.ItemNum;
                        mySystem.Dispatcher(new CE_Weapon.UpdateBulletNum {
                            UseBulletData = bulletData
                        });
                        return;
                    }
                }
                bulletData.MaxBulletNum = 0;
                mySystem.Dispatcher(new CE_Weapon.UpdateBulletNum {
                    UseBulletData = bulletData
                });
            }
        }

        private void OnSetBulletCallBack(ISystemMsg body, CE_Weapon.SetBullet ent) {
            useBulletData.UseBulletNum = (ushort)ent.BulletNum;
            mySystem.Dispatcher(new CE_Weapon.UpdateBulletNum {
                UseBulletData = useBulletData,
            });
        }
        private void OnCanReloadCallBack(ISystemMsg body, CE_Weapon.CanReload ent) {
            if (useBulletData.UseBulletNum >= useBulletData.UseMagazineNum || 
                (useBulletData.isUnlimited == false && useBulletData.UseBulletNum >= useBulletData.MaxBulletNum)) {
                ent.CallBack?.Invoke(false);
            } else {
                ent.CallBack?.Invoke(true);
            }
        }

        private void OnGetAllBulletDataCallBack(ISystemMsg body, CE_Weapon.GetAllBulletList ent) {
            ent.CallBack?.Invoke(bulletList);
        }

        private void OnGetUseBulletDataCallBack(ISystemMsg body, CE_Weapon.GetUseBulletData ent) {
            ent.CallBack?.Invoke(useBulletData);
        }
            
        private void OnSetUseBulletCallBack(ISystemMsg body, CE_Weapon.SetUseBullet ent) {
            SetUseBullet(ent.UseBullet);
        }

        private void SetUseBullet(int bulletId) {
            var data = GetBullet(bulletId);
            if (data == null) {
                return;
            }
            useBulletData = data;
            mySystem.Dispatcher(new CE_Weapon.OnUseBulletData {
                UseBulletData = useBulletData
            });
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
    }
}