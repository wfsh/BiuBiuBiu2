using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOWeaponPack : ComponentBase {
        protected List<IWeapon> weaponPack = new List<IWeapon>();
        private List<ItemData.PickItemData> pickItemDatas = new List<ItemData.PickItemData>();
        private IWeapon useWeapon;
        private Transform GunPackTran;
        private Transform MeleePackTran;
        private bool isEntityLoadEnd = false;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Register<CE_Weapon.HasPackWeapon>(OnHasPackWeaponCallBack);
            mySystem.Register<CE_Character.GetWeaponList>(OnGetWeaponListCallBack);
            mySystem.Register<CE_Character.Event_UpDateItem>(OnUpDateItemCallBack);
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            isEntityLoadEnd = true;
            GunPackTran = iEntity.GetBodyTran(GPOData.PartEnum.GunPack1);
            MeleePackTran = iEntity.GetBodyTran(GPOData.PartEnum.MeleePack);
            for (int i = 0; i < weaponPack.Count; i++) {
                SetWeaponParent(weaponPack[i]);
            }
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.TargetRpc_AddWeapon.ID, OnTargetRpcAddWeaponCallBack);
            AddProtoCallBack(Proto_Weapon.Rpc_AddWeapon.ID, OnRpcAddWeaponCallBack);
            AddProtoCallBack(Proto_Weapon.Rpc_TakeBackWeapon.ID, OnRpcTakeBackWeaponCallBack);
            AddProtoCallBack(Proto_Weapon.TargetRpc_WeaponBullet.ID, OnUseWeaponBulletCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveAllPackWeapon();
            useWeapon = null;
            mySystem.Unregister<CE_Character.GetWeaponList>(OnGetWeaponListCallBack);
            mySystem.Unregister<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Unregister<CE_Weapon.HasPackWeapon>(OnHasPackWeaponCallBack);
            mySystem.Unregister<CE_Character.Event_UpDateItem>(OnUpDateItemCallBack);
            RemoveProtoCallBack(Proto_Weapon.TargetRpc_AddWeapon.ID, OnTargetRpcAddWeaponCallBack);
            RemoveProtoCallBack(Proto_Weapon.Rpc_AddWeapon.ID, OnRpcAddWeaponCallBack);
            RemoveProtoCallBack(Proto_Weapon.Rpc_TakeBackWeapon.ID, OnRpcTakeBackWeaponCallBack);
            RemoveProtoCallBack(Proto_Weapon.TargetRpc_WeaponBullet.ID, OnUseWeaponBulletCallBack);
        }

        private void OnGetWeaponListCallBack(ISystemMsg body, CE_Character.GetWeaponList ent) {
            ent.CallBack(weaponPack);
        }

        private void OnUseWeaponCallBack(ISystemMsg body, CE_Weapon.UseWeapon ent) {
            var putAwayWeapon = useWeapon;
            useWeapon = ent.weapon;
            if (putAwayWeapon != null && putAwayWeapon != useWeapon && HasWeapon(putAwayWeapon.GetWeaponId())) {
                SetWeaponParent(putAwayWeapon);
            }
        }
        
        private void OnUseWeaponBulletCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Weapon.TargetRpc_WeaponBullet)rpcData;
            for (int i = 0; i < weaponPack.Count; i++) {
                var weapon = weaponPack[i];
                if (weapon.GetWeaponId() == data.weaponId) {
                    weapon.Dispatcher(new CE_Weapon.SetBullet {
                        BulletNum = data.bulletCount
                    });
                }
            }
        }

        protected void AddPackWeapon(int itemId, int skinItemId, int weaponId, int ATK, int magazineNum, float intervalTime, float reloadTime, float fireDistance, float fireOverHotTime) {
            if (HasWeapon(weaponId)) {
                return;
            }
            MsgRegister.Dispatcher(new CM_Weapon.AddWeapon {
                CallBack = weapon => {
                    OnAddPackWeaponCallBack(weapon);
                    weapon.Dispatcher(new CE_Weapon.SetWeaponAttribute() {
                        ATK = ATK,
                        MagazineNum = magazineNum,
                        IntervalTime = intervalTime,
                        ReloadTime = reloadTime,
                        FireDistance = fireDistance,
                        FireOverHotTime = fireOverHotTime,
                    });
                }, WeaponItemId = itemId, WeaponSkinItemId = skinItemId, WeaponId = weaponId, UseGPO = iGPO
            });
        }

        protected void RemovePackWeapon(int weaponId) {
            for (int i = 0; i < weaponPack.Count; i++) {
                var weapon = weaponPack[i];
                if (weapon.GetWeaponId() == weaponId) {
                    weaponPack.RemoveAt(i);
                    MsgRegister.Dispatcher(new CM_Weapon.RemoveWeapon {
                        WeaponId = weaponId,
                    });
                }
            }
        }

        private void RemoveAllPackWeapon() {
            for (int i = 0; i < weaponPack.Count; i++) {
                var weapon = weaponPack[i];
                weapon.SetParent(null);
                MsgRegister.Dispatcher(new CM_Weapon.RemoveWeapon {
                    WeaponId = weapon.GetWeaponId(),
                });
            }
            weaponPack.Clear();
        }

        protected bool HasWeapon(int weaponId) {
            for (int i = 0; i < weaponPack.Count; i++) {
                var weapon = weaponPack[i];
                if (weapon.GetWeaponId() == weaponId) {
                    return true;
                }
            }
            return false;
        }

        private void OnHasPackWeaponCallBack(ISystemMsg body, CE_Weapon.HasPackWeapon ent) {
            var isTrue = HasWeapon(ent.WeaponId);
            ent.CallBack(ent.WeaponId, isTrue);
        }

        private void OnAddPackWeaponCallBack(IWeapon weapon) {
            SetWeaponParent(weapon);
            weaponPack.Add(weapon);
            UpdateItem(weapon);
            mySystem.Dispatcher(new CE_Character.UpdateWeaponList {
                Weapons = weaponPack,
            });
            OnAddPackWeapon(weapon);
        }
        
        virtual protected void OnAddPackWeapon(IWeapon weapon) {
        }

        private void SetWeaponParent(IWeapon weapon) {
            if (isEntityLoadEnd == false) {
                return;
            }
            if (useWeapon != null && weapon.GetWeaponId() == useWeapon.GetWeaponId()) {
                return;
            }
            var parent = GunPackTran;
            if (weapon.GetWeaponType() == WeaponData.WeaponType.Melee) {
                parent = MeleePackTran;
            }
            weapon.SetParent(parent);
        }

        private void OnRpcAddWeaponCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Weapon.Rpc_AddWeapon)proto;
            AddPackWeapon(data.itemId, data.skinItemId, data.weaponId, data.ATK, data.magazineNum, data.intervalTime, data.reloadTime, data.FireDistance, data.fireOverHotTime);
        }

        private void OnTargetRpcAddWeaponCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Weapon.TargetRpc_AddWeapon)proto;
            AddPackWeapon(data.itemId, data.skinItemId, data.weaponId, data.ATK, data.magazineNum, data.intervalTime, data.reloadTime, data.FireDistance, data.fireOverHotTime);
        }

        private void OnRpcTakeBackWeaponCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Weapon.Rpc_TakeBackWeapon)proto;
            RemovePackWeapon(data.weaponId);
        }
        private void OnUpDateItemCallBack(ISystemMsg body, CE_Character.Event_UpDateItem ent) {
            pickItemDatas = ent.PickItemList;
            for (int i = 0; i < weaponPack.Count; i++) {
                var weapon = weaponPack[i];
                UpdateItem(weapon);
            }
        }

        private void UpdateItem(IWeapon weapon) {
            weapon.Dispatcher(new CE_Weapon.UpdatePickItem {
                PickItemList = pickItemDatas,
            });
            OnUpDateItem();
        }

        virtual protected void OnUpDateItem() {
            
        }
    }
}