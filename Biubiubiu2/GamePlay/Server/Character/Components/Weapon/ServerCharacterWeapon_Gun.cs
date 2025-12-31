using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterWeapon_Gun : ServerGPOWeapon_Gun {
        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.Cmd_GunFire.ID, OnCmdGunFireCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_Reload.ID, OnCmdReloadCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_UseBullet.ID, OnCmdGunUseBulletCallBack);
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Weapon.Cmd_GunFire.ID, OnCmdGunFireCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_Reload.ID, OnCmdReloadCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_UseBullet.ID, OnCmdGunUseBulletCallBack);
        }
        
        private void OnCmdGunFireCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Weapon.Cmd_GunFire)protoDoc;
            this.mySystem.Dispatcher(new SE_GPO.Event_OnGunFire {
                TargetPoint = data.targetPoint, StartPoint = data.startPoint,
            });
        }
        
        private void OnCmdReloadCallBack(INetwork network, IProto_Doc protoDoc) {
            this.weapon.Dispatcher(new SE_Weapon.Event_OnReload {
            });
        }
          
        private void OnCmdGunUseBulletCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Weapon.Cmd_UseBullet)protoDoc;
            this.mySystem.Dispatcher(new SE_GPO.Event_OnUseBullet {
                BulletId = data.bulletId,
            });
        }
        
        override protected void OnUseBulletSuccess(int bulletId) {
            TargetRpc(networkBase, new Proto_Weapon.TargetRpc_UseBullet() {
                bulletId = bulletId,
            });
        }      
    }
}