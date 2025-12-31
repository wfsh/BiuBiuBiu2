﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOWeapon : ComponentBase {
        public interface IGPOWeapon {
            void SetWeaponSystem(IWeapon weaponSystem);
        }
        private IWeapon useWeapon;
        private IGPOWeapon characterWeapon;
        private Transform rHandTran;
        private int cacheUseWeaponId;

        protected override void OnAwake() {
            mySystem.Register<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            mySystem.Register<CE_Weapon.GetUseWeapon>(OnGetUseWeaponCallBack);
            mySystem.Register<CE_Character.UpdateWeaponList>(OnUpdateWeaponListCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            mySystem.Unregister<CE_Weapon.GetUseWeapon>(OnGetUseWeaponCallBack);
            mySystem.Unregister<CE_Character.UpdateWeaponList>(OnUpdateWeaponListCallBack);
            RemoveProtoCallBack(Proto_Weapon.TargetRpc_UseWeapon.ID, OnRpcUseWeaponCallBack);
            RemoveProtoCallBack(Proto_Weapon.Rpc_UseWeapon.ID, OnTargetRpcUseWeaponCallBack);
            RemoveWeapon();
            rHandTran = null;
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.TargetRpc_UseWeapon.ID, OnRpcUseWeaponCallBack);
            AddProtoCallBack(Proto_Weapon.Rpc_UseWeapon.ID, OnTargetRpcUseWeaponCallBack);
        }

        protected override void OnSetEntityObj(IEntity entity) {
            rHandTran = iEntity.GetBodyTran(GPOData.PartEnum.RightHand);
            SetParent();
        }
        
        private void OnRpcUseWeaponCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Weapon.TargetRpc_UseWeapon)proto;
            UseWeapon(data.weaponId);
        }

        private void OnTargetRpcUseWeaponCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Weapon.Rpc_UseWeapon)proto;
            UseWeapon(data.weaponId);
        }
        
        private void OnGetFireBoxCallBack(ISystemMsg body, CE_Weapon.GetFireBox data) {
            if (useWeapon == null) {
                return;
            }
            useWeapon.Dispatcher(new CE_Weapon.GetFireBox {
                CallBack = data.CallBack
            });
        }
        
        private void OnGetUseWeaponCallBack(ISystemMsg body, CE_Weapon.GetUseWeapon ent) {
            ent.CallBack(useWeapon);
        }

        private void OnUpdateWeaponListCallBack(ISystemMsg body, CE_Character.UpdateWeaponList ent) {
            if (cacheUseWeaponId == 0) {
                return;
            }
            var newWeapon = ent.Weapons[^1];
            if (newWeapon.GetWeaponId() == cacheUseWeaponId) {
                OnSetWeaponIdCallBack(true, newWeapon);
            }
        }

        protected void UseWeapon(int weaponId) {
            if (useWeapon != null && useWeapon.GetWeaponId() == weaponId) {
                return;
            }
            RemoveWeapon();
            if (weaponId != 0) {
                cacheUseWeaponId = weaponId;
                MsgRegister.Dispatcher(new CM_Weapon.UseWeapon {
                    CallBack = OnSetWeaponIdCallBack, WeaponId = weaponId
                });
            }
        }

        private void OnSetWeaponIdCallBack(bool existWeapon, IWeapon weapon) {
            if (!existWeapon) {
                return;
            }
            cacheUseWeaponId = 0;
            useWeapon = weapon;
            this.mySystem.Dispatcher(new CE_Weapon.UseWeapon {
                weapon = useWeapon
            });
            characterWeapon = GetWeaponComponent(weapon);
            characterWeapon.SetWeaponSystem(useWeapon);
            SetParent();
        }
        
        virtual protected IGPOWeapon GetWeaponComponent(IWeapon weapon) {
            IGPOWeapon gpoWeapon = null;
            switch (weapon.GetWeaponType()) {
                case WeaponData.WeaponType.Gun:
                    gpoWeapon = mySystem.AddComponentChild<ClientGPOWeapon_Gun>();
                    break;
                case WeaponData.WeaponType.Melee:
                    gpoWeapon = mySystem.AddComponentChild<ClientGPOWeapon_Melee>();
                    break;
            }
            return gpoWeapon;
        }

        private void SetParent() {
            if (rHandTran == null || useWeapon == null) {
                return;
            }
            useWeapon.SetParent(rHandTran);
        }

        private void RemoveWeapon() {
            if (IsUseWeapon() == false) {
                return;
            }
            this.mySystem.Dispatcher(new CE_Weapon.UseWeapon {
                weapon = null,
            });
            if (useWeapon != null) {
                this.mySystem.Dispatcher(new CE_Weapon.HasPackWeapon {
                    WeaponId = useWeapon.GetWeaponId(),
                    CallBack = HasPackWeapon,
                });
            }
            mySystem.RemoveComponent((ComponentBase)characterWeapon);
            characterWeapon = null;
            useWeapon = null;
        }

        private void HasPackWeapon(int weaponId, bool isTrue) {
            if (isTrue) {
                return;
            }
            useWeapon.SetParent(null);
            MsgRegister.Dispatcher(new CM_Weapon.RemoveWeapon {
                WeaponId = weaponId
            });
        }

        private int GetWeaponId() {
            return useWeapon == null ? 0 : useWeapon.GetWeaponId();
        }

        private string GetWeaponSign() {
            return useWeapon == null ? "" : useWeapon.GetWeaponSign();
        }

        private bool IsUseWeapon() {
            return useWeapon != null;
        }
    }
}
