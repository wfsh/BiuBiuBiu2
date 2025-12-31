using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOWeapon_Gun : ComponentBase, ClientGPOWeapon.IGPOWeapon {
        protected IWeapon iWeapon;
        private bool isPlayAnim = false;
        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            CanceUseWeapon();
            RemoveProtoCallBack(Proto_Weapon.Rpc_GunFire.ID, OnRpcGunFireCallBack);
            RemoveProtoCallBack(Proto_Weapon.Rpc_Reload.ID, OnRpcReloadCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.Rpc_GunFire.ID, OnRpcGunFireCallBack);
            AddProtoCallBack(Proto_Weapon.Rpc_Reload.ID, OnRpcReloadCallBack);
        }

        private void OnRpcGunFireCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Weapon.Rpc_GunFire)rpcData;
            GunFire(data.isTrue);
        }

        private void OnRpcReloadCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Weapon.Rpc_Reload)rpcData;
            OnReload(data.isTrue);
        }

        protected void OnReload(bool isTrue) {
            var reloadTime = 0f;
            iWeapon.Dispatcher(new CE_Weapon.GetReloadTime {
                CallBack = time => {
                    reloadTime = time;
                }
            });
            mySystem.Dispatcher(new CE_Weapon.OnReload {
                IsReload = isTrue,
                ReloadTime = reloadTime,
            });
            iWeapon.Dispatcher(new CE_Weapon.OnReload {
                IsReload = isTrue,
                ReloadTime = reloadTime,
            });
        }

        public void SetWeaponSystem(IWeapon weapon) {
            this.iWeapon = weapon;
            PlayWeaponAnim();
        }

        public void CanceUseWeapon() {
            SetWeaponSystem(null);
        }

        protected void GunFire(bool isTrue) {
            if (isTrue) {
                mySystem.Dispatcher(new CE_Weapon.Fire {
                });
                this.iWeapon.Dispatcher(new CE_Weapon.Fire {
                });
            } else {
                mySystem.Dispatcher(new CE_Weapon.EndFire {
                });
                this.iWeapon.Dispatcher(new CE_Weapon.EndFire {
                });
            }
        }

        private void PlayWeaponAnim() {
            var isAnim = this.iWeapon != null;
            if (isPlayAnim != isAnim) {
                isPlayAnim = isAnim;
                if (isAnim == false) {
                    mySystem.Dispatcher(new CE_Character.StopAnimSign {
                        PlaySign = AnimConfig_HTR.Play_Fire,
                    });
                }
            }
        }
    }
}