using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOWeapon_Gun : ServerNetworkComponentBase, ServerGPOWeapon.IGPOWeapon {
        protected S_Weapon_Base weapon;
        private Action<WeaponData.UseBulletData> fireCallBack;
        private string bulletSign;

        protected override void OnAwake() {
            base.OnAwake();
            this.mySystem.Register<SE_GPO.SetWeaponBullet>(OnSetWeaponBulletCallBack);
            this.mySystem.Register<SE_GPO.Event_OnUseBullet>(OnUseBulletCallBack);
            this.mySystem.Register<SE_GPO.Event_OnGunFire>(OnGunFireCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            if (weapon == null) {
                return;
            }
            weapon.Dispatcher(new SE_Weapon.Event_GetUseBulletData {
                CallBack = data => {
                    TargetRpc(networkBase, new Proto_Weapon.TargetRpc_UseBullet() {
                        bulletId = data.BulletId,
                    });
                },
            });
        }

        protected override void OnClear() {
            base.OnClear();
            this.weapon.Dispatcher(new SE_Weapon.Event_PutOn {
                IsTrue = false,
            });
            this.weapon.Unregister<SE_Weapon.Event_Fire>(OnPlayFireCallBack);
            this.weapon.Unregister<SE_Weapon.Event_FireEnd>(OnPlayFireEndCallBack);
            this.weapon.Unregister<SE_Weapon.Event_Reload>(OnReloadCallBack);
            this.weapon.Unregister<SE_Weapon.Event_UseBullet>(OnUseBulletSuccessCallBack);
            this.mySystem.Unregister<SE_GPO.Event_OnUseBullet>(OnUseBulletCallBack);
            this.mySystem.Unregister<SE_GPO.Event_OnGunFire>(OnGunFireCallBack);
            this.weapon = null;
            this.mySystem.Unregister<SE_GPO.SetWeaponBullet>(OnSetWeaponBulletCallBack);
        }

        public void SetWeaponSystem(IWeapon weapon, Action<WeaponData.UseBulletData> fireCallBack) {
            this.weapon = (S_Weapon_Base)weapon;
            this.weapon.Register<SE_Weapon.Event_Fire>(OnPlayFireCallBack);
            this.weapon.Register<SE_Weapon.Event_FireEnd>(OnPlayFireEndCallBack);
            this.weapon.Register<SE_Weapon.Event_UseBullet>(OnUseBulletSuccessCallBack);
            this.weapon.Register<SE_Weapon.Event_Reload>(OnReloadCallBack);
            this.weapon.Dispatcher(new SE_Weapon.Event_PutOn {
                IsTrue = true
            }, 0f);
            this.fireCallBack = fireCallBack;
        }

        private void OnSetWeaponBulletCallBack(ISystemMsg body, SE_GPO.SetWeaponBullet ent) {
            this.weapon.Dispatcher(new SE_Weapon.Event_SetALLBullet {
                BulletNum = ent.BulletCount, UseItemId = ent.UseItemId,
            });
        }

        private void OnGunFireCallBack(ISystemMsg body, SE_GPO.Event_OnGunFire ent) {
            var firDir = (ent.TargetPoint - ent.StartPoint).normalized;
            var startPoint = ent.StartPoint - firDir * 0.8f;
            this.weapon.Dispatcher(new SE_Weapon.Event_GunFire {
                TargetPoint = ent.TargetPoint, StartPoint = startPoint,
            });
        }

        private void OnUseBulletCallBack(ISystemMsg body, SE_GPO.Event_OnUseBullet ent) {
            this.weapon.Dispatcher(new SE_Weapon.Event_SetUseBullet {
                UseBullet = ent.BulletId,
            });
        }

        private void OnPlayFireCallBack(ISystemMsg body, SE_Weapon.Event_Fire ent) {
            this.fireCallBack.Invoke(ent.BulletData);
            Rpc(new Proto_Weapon.Rpc_GunFire {
                isTrue = true,
            });
        }
        private void OnPlayFireEndCallBack(ISystemMsg body, SE_Weapon.Event_FireEnd ent) {
            Rpc(new Proto_Weapon.Rpc_GunFire {
                isTrue = false,
            });
            iGPO.Dispatcher(new SE_GPO.Event_GunFireEnd());
        }
        private void OnReloadCallBack(ISystemMsg body, SE_Weapon.Event_Reload ent) {
            iGPO.Dispatcher(new SE_GPO.Event_GunReload ());
            Rpc(new Proto_Weapon.Rpc_Reload {
                isTrue = ent.isTrue,
            });
        }

        private void OnUseBulletSuccessCallBack(ISystemMsg body, SE_Weapon.Event_UseBullet ent) {
            OnUseBulletSuccess(ent.Data.BulletId);
        }

        virtual protected void OnUseBulletSuccess(int bulletId) {
        }
    }
}