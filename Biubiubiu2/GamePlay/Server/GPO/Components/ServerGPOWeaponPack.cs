using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOWeaponPack : ServerNetworkComponentBase {
        private IWeapon useWeapon;
        private const int MaxPackGrid = 2; // 武器背包最大格子数量
        private bool canInItemPack = true;
        private bool isDrive = false;
        protected List<IWeapon> weaponList = new List<IWeapon>(MaxPackGrid);

        private List<SE_Mode.PlayModeCharacterWeapon> weaponDataList = new List<SE_Mode.PlayModeCharacterWeapon>();

        protected override void OnAwake() {
            MsgRegister.Register<SM_ShortcutTool.Event_EquipWeaponChange>(OnShortcutToolEquipChangeCallBack);
            mySystem.Register<SE_GPO.Event_CanInWeaponPack>(OnCanInWeaponPackCallBack);
            mySystem.Register<SE_GPO.AddWeaponPack>(OnAddWeaponPackCallBack);
            mySystem.Register<SE_GPO.Event_RemoveAllWeaponPack>(OnRemoveAllWeaponPackCallBack);
            mySystem.Register<SE_GPO.Event_RemoveWeapon>(OnRemoveWeaponCallBack);
            mySystem.Register<SE_GPO.Event_GetPackWeaponList>(OnGetPackWeaponListCallBack);
            mySystem.Register<SE_GPO.Event_OnEquipPackWeapon>(OnEquipWeaponCallBack);
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Register<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Register<SE_GPO.Event_OnTakeBackWeapon>(OnTakeBackWeaponListCallBack);
            mySystem.Register<SE_Item.Event_UpdateItems>(OnUpdateItemsCallBack);
            mySystem.Register<SE_Item.Event_AddWeaponForMode>(OnAddWeaponForModeCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveAllWeapon();
            weaponDataList.Clear();
            MsgRegister.Unregister<SM_ShortcutTool.Event_EquipWeaponChange>(OnShortcutToolEquipChangeCallBack);
            mySystem.Unregister<SE_GPO.Event_CanInWeaponPack>(OnCanInWeaponPackCallBack);
            mySystem.Unregister<SE_GPO.AddWeaponPack>(OnAddWeaponPackCallBack);
            mySystem.Unregister<SE_GPO.Event_RemoveAllWeaponPack>(OnRemoveAllWeaponPackCallBack);
            mySystem.Unregister<SE_GPO.Event_RemoveWeapon>(OnRemoveWeaponCallBack);
            mySystem.Unregister<SE_GPO.Event_GetPackWeaponList>(OnGetPackWeaponListCallBack);
            mySystem.Unregister<SE_GPO.Event_OnEquipPackWeapon>(OnEquipWeaponCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnPlayerDriveCallBack);
            mySystem.Unregister<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Unregister<SE_GPO.Event_OnTakeBackWeapon>(OnTakeBackWeaponListCallBack);
            mySystem.Unregister<SE_Item.Event_UpdateItems>(OnUpdateItemsCallBack);
            mySystem.Unregister<SE_Item.Event_AddWeaponForMode>(OnAddWeaponForModeCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        protected override void Sync(List<INetworkCharacter> network) {
            RPCALLWeapon(network);
        }
        private void RPCALLWeapon(List<INetworkCharacter> networks) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                weapon.Dispatcher(new SE_Weapon.Event_GetGunAttribute {
                    CallBack = (atkValue, magazineValue, intervalValue, fireRangeValue, reloadValue,
                        fireDistanceValue, fireOverHotTime) => {
                        TargetRpcList(networks, new Proto_Weapon.TargetRpc_AddWeapon {
                            itemId = weapon.GetWeaponItemId(),
                            weaponId = weapon.GetWeaponId(),
                            skinItemId = weapon.GetWeaponSkinItemId(),
                            ATK = atkValue,
                            magazineNum = magazineValue,
                            intervalTime = intervalValue,
                            reloadTime = reloadValue,
                            FireDistance = fireDistanceValue,
                            fireOverHotTime = fireOverHotTime,
                        });
                    }
                });
                weapon.Dispatcher(new SE_Weapon.Event_GetUseBulletData {
                    CallBack = (bulletData) => {
                        TargetRpcList(networks, new Proto_Weapon.TargetRpc_WeaponBullet() {
                            weaponId = weapon.GetWeaponId(),
                            bulletCount = bulletData.UseBulletNum,
                        });
                    }
                });
            }
        }

        private void OnAddWeaponForModeCallBack(ISystemMsg body, SE_Item.Event_AddWeaponForMode ent) {
            AddWeaponPickItem(ent.WeaponData);
        }

        private void OnShortcutToolEquipChangeCallBack(SM_ShortcutTool.Event_EquipWeaponChange ent) {
            if (ent.GpoId >= 0) {
                if (ent.GpoId != GpoID) {
                    return;
                }
            } else {
                if (ent.GpoId == -2 && iGPO.GetGPOType() != GPOData.GPOType.Role) {
                    return;
                }
            }
            RemoveWeaponForIndex(ent.Index);
            if (weaponList.Count >= MaxPackGrid) {
                return;
            }
            var data = new SE_Mode.PlayModeCharacterWeapon();
            data.WeaponItemId = ent.WeaponItemId;
            data.Index = ent.Index;
            data.IsSuperWeapon = false;
            AddWeaponPickItem(data);
            iGPO.Dispatcher(new SE_Item.Event_AddPickItem {
                ItemId = ItemSet.Id_GunBullet, ItemNum = 999, IsProtect = false, IsQuickUse = true,
            });
        }

        private void AddWeaponPickItem(SE_Mode.PlayModeCharacterWeapon data) {
            weaponDataList.Add(data);
            mySystem.Dispatcher(new SE_Item.Event_AddPickItem {
                ItemId = data.WeaponItemId, SkinItemId = data.WeaponSkinItemId,  ItemNum = 1, IsProtect = false, IsQuickUse = true,
            });
        }

        private void OnAddWeaponPackCallBack(ISystemMsg body, SE_GPO.AddWeaponPack ent) {
            AddWeapon(ent.AddWeaponId, ent.AddWeaponSkinId);
        }

        private void OnRemoveAllWeaponPackCallBack(ISystemMsg body, SE_GPO.Event_RemoveAllWeaponPack ent) {
            RemoveAllWeapon();
        }

        private void OnRemoveWeaponCallBack(ISystemMsg body, SE_GPO.Event_RemoveWeapon ent) {
            RemoveWeaponForId(ent.WeaponId);
        }

        private void OnGetPackWeaponListCallBack(ISystemMsg body, SE_GPO.Event_GetPackWeaponList ent) {
            ent.CallBack(weaponList);
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                return;
            }
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = (S_Weapon_Base)weaponList[i];
                weapon.Dispatcher(new SE_Weapon.Event_ReloadAllBulletNoTime {
                });
            }
        }

        private void OnUpdateItemsCallBack(ISystemMsg body, SE_Item.Event_UpdateItems ent) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = (S_Weapon_Base)weaponList[i];
                weapon.Dispatcher(new SE_Weapon.Event_UpdateItems {
                    ItemList = ent.ItemList
                });
            }
        }

        /// <summary>
        /// 判断武器背包是否还有位置
        /// </summary>
        /// <param name="ent"></param>
        private void OnCanInWeaponPackCallBack(ISystemMsg body, SE_GPO.Event_CanInWeaponPack ent) {
            ent.CallBack(weaponList.Count < MaxPackGrid);
        }

        private void AddWeapon(int itemId, int skinItemId) {
            if (weaponList.Count >= MaxPackGrid) {
                Debug.LogError("超出武器背包可容纳的上限");
                return;
            }
            MsgRegister.Dispatcher(new SM_Weapon.Event_AddWeapon {
                WeaponItemId = itemId, WeaponSkinItemId = skinItemId, UseGPO = iGPO, CallBack = AddWeaponCallBack,
            });
        }

        private void AddWeaponCallBack(IWeapon weapon) {
            if (HasWeapon(weapon.GetWeaponId())) {
                return;
            }
            weaponList.Add(weapon);
            AddWeaponInPackSuccess(weapon);
            weapon.Register<SE_Weapon.Event_GetALLBullet>(OnGetALLBulletCallBack);
            weapon.Register<SE_Weapon.Event_OnDownBullet>(OnDownBulletCallBack);
            weapon.Register<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
        }

        private void AddWeaponInPackSuccess(IWeapon weapon) {
            mySystem.Dispatcher(new SE_GPO.AddWeaponInPackSuccess {
                Weapon = weapon,
            });
            SetWeaponRandomAttributeData(weapon);
            RpcAddWeapon(weapon);
            if (useWeapon == null && isDrive == false) {
                // 默认装备一把武器
                EquipPackWeapon(weapon.GetWeaponId());
            }
        }

        private void OnDownBulletCallBack(ISystemMsg body, SE_Weapon.Event_OnDownBullet ent) {
            mySystem.Dispatcher(new SE_Item.Event_DownBullet {
                UseBullet = ent.UseBullet,
                BulletNum = ent.BulletNum,
                CallBack = num => {
                    ent.CallBack?.Invoke(num);
                }
            });
        }

        private void RpcAddWeapon(IWeapon weapon) {
            weapon.Dispatcher(new SE_Weapon.Event_GetGunAttribute {
                CallBack = (atkValue, magazineValue, intervalValue, fireRangeValue, reloadValue,
                    fireDistanceValue, fireOverHotTime) => {
                    Rpc(new Proto_Weapon.Rpc_AddWeapon {
                        itemId = weapon.GetWeaponItemId(),
                        skinItemId = weapon.GetWeaponSkinItemId(),
                        weaponId = weapon.GetWeaponId(),
                        ATK = atkValue,
                        magazineNum = magazineValue,
                        intervalTime = intervalValue,
                        reloadTime = reloadValue,
                        FireDistance = fireDistanceValue,
                        fireOverHotTime = fireOverHotTime,
                    });
                }
            });
        }

        private void SetWeaponRandomAttributeData(IWeapon weapon) {
            var weaponItemId = weapon.GetWeaponItemId();
            if (weaponDataList.Count <= 0) {
                return;
            }
            for (int i = 0; i < weaponDataList.Count; i++) {
                var weaponDataItem = weaponDataList[i];
                if (weaponDataItem.WeaponItemId == weaponItemId) {
                    weapon.Dispatcher(new SE_Weapon.Event_SetRandomAttributeData {
                        Data = weaponDataItem,
                    });
                    weaponDataItem.iWeapon = weapon;
                    weaponDataList.RemoveAt(i);
                    return;
                }
            }
        }
        
        private void OnGetALLBulletCallBack(ISystemMsg body, SE_Weapon.Event_GetALLBullet ent) {
            mySystem.Dispatcher(new SE_Item.Event_GetALLItemForType {
                ItemType = ItemTypeSet.Id_Bullet,
                CallBack = (itemList) => {
                    if (itemList == null) {
                        return;
                    }
                    for (int i = 0; i < itemList.Count; i++) {
                        var itemData = itemList[i];
                        var modData = ItemData.GetData(itemData.ItemId);
                        ent.iWeapon.Dispatcher(new SE_Weapon.Event_SetALLBullet {
                            BulletNum = itemData.ItemNum, UseItemId = modData.Id,
                        });
                    }
                }
            });
        }

        public void OnPlayerDriveCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isDrive = ent.IsDrive;
        }

        public void OnEquipWeaponCallBack(ISystemMsg body, SE_GPO.Event_OnEquipPackWeapon ent) {
            var weaponId = ent.WeaponId;
            if (HasWeapon(weaponId) == false || weaponId <= 0) {
                if (weaponList.Count > 0) {
                    EquipPackWeapon(weaponList[0].GetWeaponId());
                }
            } else {
                EquipPackWeapon(weaponId);
            }
        }

        protected void EquipPackWeapon(int weaponId) {
            if (weaponId == 0) {
                this.mySystem.Dispatcher(new SE_GPO.SetUseWeapon {
                    UseGPOId = GpoID, Weapon = null, PutAwakeWeaponCallBack = null, FireCallBack = null,
                });
            } else {
                if (HasWeapon(weaponId) == false) {
                    Debug.LogError("没有这把武器:" + weaponId);
                    return;
                }
                var useWeapon = GetWeapon(weaponId);
                this.mySystem.Dispatcher(new SE_GPO.SetUseWeapon {
                    UseGPOId = GpoID, Weapon = useWeapon, PutAwakeWeaponCallBack = null, FireCallBack = OnGunFireCallBack,
                });
            }
        }

        private void OnUseWeaponCallBack(ISystemMsg body, SE_GPO.UseWeapon ent) {
            useWeapon = ent.Weapon;
        }

        private bool HasWeapon(int weaponId) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                if (weapon.GetWeaponId() == weaponId) {
                    return true;
                }
            }
            return false;
        }

        private IWeapon GetWeapon(int weaponId) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                if (weapon.GetWeaponId() == weaponId) {
                    return weapon;
                }
            }
            return null;
        }

        private void OnTakeBackWeaponListCallBack(ISystemMsg body, SE_GPO.Event_OnTakeBackWeapon ent) {
            var weaponId = ent.WeaponId;
            TakeBackWeapon(weaponId);
        }

        protected void TakeBackWeapon(int weaponId) {
            var weapon = GetWeapon(weaponId);
            if (weapon == null) {
                Debug.LogError("没有可以卸下的武器 ：" + weaponId);
                return;
            }
            mySystem.Dispatcher(new SE_Item.Event_CanInItemPack {
                CallBack = CanInItemPack,
            });
            if (canInItemPack == false) {
                return;
            }
            RemoveWeaponForId(weaponId);
            //将武器重新转换成物品
            MsgRegister.Dispatcher(new SM_Item.Event_AddOwnItem {
                ItemSign = weapon.GetWeaponSign(), ItemNum = 1, OwnGPO = iGPO,
            });
        }

        private void CanInItemPack(bool isTrue) {
            canInItemPack = isTrue;
        }

        private void RemoveWeaponForId(int weaponId) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                if (weapon.GetWeaponId() == weaponId) {
                    RemoveWeapon(weapon);
                    weaponList.RemoveAt(i);
                    return;
                }
            }
        }

        private void RemoveWeaponForIndex(int index) {
            var len = index - 1;
            if (len < 0 || len >= weaponList.Count) {
                return;
            }
            var weapon = weaponList[len];
            RemoveWeapon(weapon);
            weaponList.RemoveAt(len);
        }

        private void RemoveAllWeapon() {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                RemoveWeapon(weapon);
            }
            weaponList.Clear();
        }

        private void RemoveWeapon(IWeapon weapon) {
            if (iGPO == null) {
                return;
            }
            weapon.Unregister<SE_Weapon.Event_GetALLBullet>(OnGetALLBulletCallBack);
            weapon.Unregister<SE_Weapon.Event_OnDownBullet>(OnDownBulletCallBack);
            weapon.Unregister<SE_Weapon.Event_NowBulletNum>(OnNowBulletNumCallBack);
            // 如果卸下的武器在使用，一起卸下
            iGPO.Dispatcher(new SE_GPO.SetCanceUseWeapon {
                WeaponId = weapon.GetWeaponId(),
            });
            Rpc(new Proto_Weapon.Rpc_TakeBackWeapon {
                weaponId = weapon.GetWeaponId()
            });
            // 删除武器
            MsgRegister.Dispatcher(new SM_Weapon.Event_RemoveWeapon {
                WeaponId = weapon.GetWeaponId(),
            });
        }

        private void OnNowBulletNumCallBack(ISystemMsg body, SE_Weapon.Event_NowBulletNum ent) {
            OnNowBulletNum(ent.WeaponId, ent.BulletNum);
        }

        virtual protected void OnNowBulletNum(int weaponItemId, ushort bulletNum) {
        }

        private void OnGunFireCallBack(WeaponData.UseBulletData bulletData) {
        }
    }
}