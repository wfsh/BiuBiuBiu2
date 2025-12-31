using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWeaponManager : ManagerBase {
        private Dictionary<int, C_Weapon_Base> weaponSystemData;

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Register<CM_Weapon.AddWeapon>(OnAddWeaponCallBack);
            MsgRegister.Register<CM_Weapon.RemoveWeapon>(OnRemoveWeaponCallBack);
            MsgRegister.Register<CM_Weapon.UseWeapon>(OnUseWeaponCallBack);
            weaponSystemData = new Dictionary<int, C_Weapon_Base>();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_Weapon.AddWeapon>(OnAddWeaponCallBack);
            MsgRegister.Unregister<CM_Weapon.RemoveWeapon>(OnRemoveWeaponCallBack);
            MsgRegister.Unregister<CM_Weapon.UseWeapon>(OnUseWeaponCallBack);
            weaponSystemData?.Clear();
            weaponSystemData = null;
        }

        private void OnAddWeaponCallBack(CM_Weapon.AddWeapon ent) {
            C_Weapon_Base weapon;
            if (weaponSystemData.TryGetValue(ent.WeaponId, out weapon) == false) {
                weapon = GetSystemForWeaponItemId(ent.WeaponItemId, ent.UseGPO, delegate(C_Weapon_Base weapon) {
                    weapon.SetWeaponData(ent.WeaponId, ent.WeaponItemId, ent.WeaponSkinItemId);
                    weapon.SetUseGPO(ent.UseGPO);
                    weaponSystemData.Add(ent.WeaponId, weapon);
                });
            }
            ent.CallBack?.Invoke(weapon);
        }

        private void OnUseWeaponCallBack(CM_Weapon.UseWeapon ent) {
            var existWeapon = weaponSystemData.TryGetValue(ent.WeaponId, out var weapon);
            ent.CallBack?.Invoke(existWeapon, weapon);
        }

        private void OnRemoveWeaponCallBack(CM_Weapon.RemoveWeapon ent) {
            C_Weapon_Base system;
            if (weaponSystemData.TryGetValue(ent.WeaponId, out system) == false) {
                return;
            }
            system.Clear();
            weaponSystemData.Remove(ent.WeaponId);
        }

        public C_Weapon_Base GetSystemForWeaponItemId(int itemId, IGPO useGPO, Action<C_Weapon_Base> callBack) {
            C_Weapon_Base system = null;
            switch (itemId) {
                case ItemSet.Id_Gatling:
                    system = AddSystem<ClientGunGatlingSystem>(callBack);
                    break;
                case ItemSet.Id_ParticleCannon:
                    system = AddSystem<ClientGunParticleCannonSystem>(callBack);
                    break;
                case ItemSet.Id_FireGun:
                    system = AddSystem<ClientGunFireGunSystem>(callBack);
                    break;
            }
            if (system == null) {
                var data = WeaponData.Get(itemId);
                switch (data.WeaponType) {
                    case WeaponData.WeaponType.Gun:
                        system = AddSystem<ClientGunFocusSystem>(callBack);
                        break;
                    case WeaponData.WeaponType.Melee:
                        system = AddSystem<ClientMeleeBatSystem>(callBack);
                        break;
                    default:
                        Debug.LogError("添加武器失败 WeaponType:" + data.WeaponType);
                        break;
                }
            }
            return system;
        }
    }
}