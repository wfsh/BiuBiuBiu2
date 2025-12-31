using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterWeaponPack : ServerGPOWeaponPack {
        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Weapon.Cmd_UseWeapon.ID, OnSetWeaponCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_TakeBackWeapon.ID, OnTakeBackWeaponCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.Cmd_UseWeapon.ID, OnSetWeaponCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_TakeBackWeapon.ID, OnTakeBackWeaponCallBack);
        }

        private void OnTakeBackWeaponCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_TakeBackWeapon)cmdData;
            TakeBackWeapon(data.weaponId);
        }

        public void OnSetWeaponCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_UseWeapon)cmdData;
            EquipPackWeapon(data.weaponId);
        }  
        
        override protected void OnNowBulletNum(int weaponId, ushort bulletNum) {
            TargetRpc(networkBase, new Proto_Weapon.TargetRpc_WeaponBullet() {
                weaponId = weaponId,
                bulletCount = bulletNum,
            });
        }
    }
}