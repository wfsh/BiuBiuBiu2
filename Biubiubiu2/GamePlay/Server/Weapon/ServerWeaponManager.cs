using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWeaponManager : ManagerBase {
        private List<S_Weapon_Base> weaponList = new List<S_Weapon_Base>();
        private int weaponIndex = 0;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Weapon.Event_AddWeapon>(OnAddWeaponCallBack);
            MsgRegister.Register<SM_Weapon.Event_RemoveWeapon>(OnRemoveWeaponCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Weapon.Event_AddWeapon>(OnAddWeaponCallBack);
            MsgRegister.Unregister<SM_Weapon.Event_RemoveWeapon>(OnRemoveWeaponCallBack);
        }

        private void OnAddWeaponCallBack(SM_Weapon.Event_AddWeapon ent) {
            var weaponId = ent.WeaponItemId;
            var weapon = AddWeapon(weaponId, delegate(S_Weapon_Base weapon) {
                weaponIndex++;
                weapon.SetWeaponData(weaponIndex, weaponId, ent.WeaponSkinItemId);
                weapon.SetUseGPO(ent.UseGPO);
                weaponList.Add(weapon);
            });
            ent.CallBack(weapon);
        }

        private S_Weapon_Base AddWeapon(int itemId, Action<S_Weapon_Base> callBack) {
            S_Weapon_Base weapon = null;
            switch (itemId) {
                case ItemSet.Id_FireGun:
                    weapon = AddSystem<ServerFireGunSystem>(callBack);
                    break;
            }
            if (weapon == null) {
                var data = WeaponData.Get(itemId);
                switch (data.WeaponType) {
                    case WeaponData.WeaponType.Gun:
                        weapon = AddSystem<ServerGunSystem>(callBack);
                        break;
                    case WeaponData.WeaponType.Melee:
                        weapon = AddSystem<ServerMeleeSystem>(callBack);
                        break;
                    default:
                        Debug.LogError("添加武器失败 WeaponType:" + data.WeaponType);
                        break;
                }
            }
            return weapon;
        }

        private void OnRemoveWeaponCallBack(SM_Weapon.Event_RemoveWeapon ent) {
            for (int i = 0; i < weaponList.Count; i++) {
                var weapon = weaponList[i];
                if (weapon.GetWeaponId() == ent.WeaponId) {
                    weapon.Clear();
                    weaponList.RemoveAt(i);
                    return;
                }
            }
        }
    }
}