using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOWeapon : ServerNetworkComponentBase {
        public interface IGPOWeapon {
            void SetWeaponSystem(IWeapon weapon, Action<WeaponData.UseBulletData> fireCallBack);
        }
        private Action<IWeapon> putAwakeWeaponCallBack;
        private Action<WeaponData.UseBulletData> attackCallBack;
        private IGPOWeapon weaponComponent;
        protected S_Weapon_Base useWeapon = null;
        private Transform rHandTran;

        protected override void OnAwake() {
            base.OnAwake();
            this.mySystem.Register<SE_GPO.SetUseWeapon>(OnSetUseWeaponCallBack);
            this.mySystem.Register<SE_GPO.GetUseWeapon>(OnGetUseWeaponCallBack);
            this.mySystem.Register<SE_GPO.SetCanceUseWeapon>(OnSetCanceUseWeaponCallBack);
        }
        
        protected override void OnSetEntityObj(IEntity entity) {
            rHandTran = iEntity.GetBodyTran(GPOData.PartEnum.RightHand);
            SetParent();
        }

        protected override void Sync(List<INetworkCharacter> networks) {
            // 重连需要
            Rpc(new Proto_Weapon.Rpc_UseWeapon {
                weaponId = useWeapon?.GetWeaponId() ?? 0,
            });
        }

        protected override void OnClear() {
            base.OnClear();
            this.mySystem.Unregister<SE_GPO.SetUseWeapon>(OnSetUseWeaponCallBack);
            this.mySystem.Unregister<SE_GPO.SetCanceUseWeapon>(OnSetCanceUseWeaponCallBack);
            this.mySystem.Unregister<SE_GPO.GetUseWeapon>(OnGetUseWeaponCallBack);
            this.putAwakeWeaponCallBack = null;
            this.attackCallBack = null;
            rHandTran = null;
        }
        private void OnSetCanceUseWeaponCallBack(ISystemMsg body, SE_GPO.SetCanceUseWeapon entData) {
            if (entData.WeaponId > 0) {
                if (useWeapon != null && useWeapon.GetWeaponId() != entData.WeaponId) {
                    return;
                }
            } 
            CanceWeapon();
        }
        private void OnGetUseWeaponCallBack(ISystemMsg body, SE_GPO.GetUseWeapon ent) {
            ent.CallBack.Invoke(useWeapon);
        }

        private void OnSetUseWeaponCallBack(ISystemMsg body, SE_GPO.SetUseWeapon entData) {
            if (this.putAwakeWeaponCallBack != null) {
                this.putAwakeWeaponCallBack.Invoke(useWeapon);
            }
            this.putAwakeWeaponCallBack = entData.PutAwakeWeaponCallBack;
            this.attackCallBack = entData.FireCallBack;
            if (entData.Weapon != null) {
                UseWeapon(entData.Weapon);
            } else {
                CanceWeapon();
            }
        }
        
        private void UseWeapon(IWeapon weapon) {
            SetUseWeapon((S_Weapon_Base)weapon);
            AddComponent(useWeapon);
            SetParent();
        }

        private void SetParent() {
            if (rHandTran == null || useWeapon == null) {
                return;
            }
            useWeapon.SetParent(rHandTran);
        }

        public void AddComponent(IWeapon weapon) {
            mySystem.RemoveComponent((ComponentBase)weaponComponent);
            weaponComponent = GetWeaponComponent(weapon);
            weaponComponent.SetWeaponSystem(useWeapon, this.attackCallBack);
        }
        
        virtual protected IGPOWeapon GetWeaponComponent(IWeapon weapon) {
            IGPOWeapon gpoWeapon = null;
            switch (weapon.GetWeaponType()) {
                case WeaponData.WeaponType.Gun:
                    gpoWeapon = mySystem.AddComponentChild<ServerGPOWeapon_Gun>();
                    break;
                case WeaponData.WeaponType.Melee:
                    gpoWeapon = mySystem.AddComponentChild<ServerGPOWeapon_Melee>();
                    break;
            }
            return gpoWeapon;
        }

        private void CanceWeapon() {
            if (useWeapon == null) {
                return;
            }
            mySystem.RemoveComponent((ComponentBase)weaponComponent);
            weaponComponent = null;
            SetUseWeapon(null);
        }

        private void SetUseWeapon(IWeapon useWeapon) {
            mySystem.Dispatcher(new SE_GPO.UseWeapon {
                Weapon = useWeapon
            });
            this.useWeapon = (S_Weapon_Base)useWeapon;
            if (useWeapon == null) {
                Rpc(new Proto_Weapon.Rpc_UseWeapon {
                    weaponId = 0
                });
            } else {
                Rpc(new Proto_Weapon.Rpc_UseWeapon {
                    weaponId = useWeapon.GetWeaponId()
                });
            }
        }
    }
}