using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterUseSight : ClientCharacterComponent {
        private bool isUseWeapon = false;
        private bool isHoldOn = false;
        private bool isTakeOnMonster = false;
        private bool isDrive = false;
        
        protected override void OnAwake() {
            this.mySystem.Register<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            this.mySystem.Register<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Register<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnPlayerDriveGPOCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            this.mySystem.Unregister<CE_Weapon.UseWeapon>(OnUseWeaponCallBack);
            this.mySystem.Unregister<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Unregister<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnPlayerDriveGPOCallBack);
        }

        public void SetHoldOnSign(ISystemMsg body, CE_Character.HoldOn entData) {
            isHoldOn = entData.HoldOnSign != "";
            SightState();
        }

        public void SetTakeOnMonsterSign(ISystemMsg body, CE_Character.TakeOnMonster entData) {
            isTakeOnMonster = entData.HoldOnSign != "";
            SightState();
        }

        private void OnUseWeaponCallBack(ISystemMsg body, CE_Weapon.UseWeapon entData) {
            isUseWeapon = entData.weapon != null;
            SightState();
        }

        private void OnPlayerDriveGPOCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO entData) {
            isDrive = entData.DriveGPO != null;
            SightState();
        }
        
        private void SightState() {
            if (isTakeOnMonster || isDrive) {
                MsgRegister.Dispatcher(new CM_Weapon.ShowGunSight {
                    Index = 2,
                });
            } else if (isUseWeapon == true){
                MsgRegister.Dispatcher(new CM_Weapon.ShowGunSight {
                    Index = 1,
                });
            } else {
                MsgRegister.Dispatcher(new CM_Weapon.HideGunSight());
            }
        }
    }
}
