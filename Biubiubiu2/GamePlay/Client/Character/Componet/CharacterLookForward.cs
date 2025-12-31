using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterLookForward : ComponentBase {
        private bool isUseGun = false;
        private bool isHoldOn = false;
        private bool isTakeOnMonster = false;
        private bool isLookForward = false;
        private CharacterData.FlyType flyType = CharacterData.FlyType.None;
        
        protected override void OnAwake() {
            this.mySystem.Register<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            this.mySystem.Register<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Register<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Register<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            this.mySystem.Unregister<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            this.mySystem.Unregister<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Unregister<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Unregister<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
        }
        
        public void SetUseWeaponSign(ISystemMsg body, CE_Weapon.UseWeapon entData) {
            if (entData.weapon != null && entData.weapon.GetWeaponType() == WeaponData.WeaponType.Gun) {
                isUseGun = true;
            } else {
                isUseGun = false;
            }
            CheckState();
        }

        public void SetHoldOnSign(ISystemMsg body, CE_Character.HoldOn entData) {
            isHoldOn = entData.HoldOnSign != "";
            CheckState();
        }

        public void SetTakeOnMonsterSign(ISystemMsg body, CE_Character.TakeOnMonster entData) {
            isTakeOnMonster = entData.HoldOnSign != "";
            CheckState();
        }
        
        public void OnFlyTypeCallBack(ISystemMsg body, CE_Character.FlyTypeChange entData) {
            this.flyType = (CharacterData.FlyType)entData.FlyType;
            CheckState();
        }

        private void CheckState() {
            if (isUseGun || isHoldOn || isTakeOnMonster || this.flyType != CharacterData.FlyType.None) {
                if (isLookForward == false) {
                    this.mySystem.Dispatcher(new CE_Character.IsLookForward {
                        IsTrue = true
                    });
                }
                isLookForward = true;
            } else {
                if (isLookForward) {
                    this.mySystem.Dispatcher(new CE_Character.IsLookForward {
                        IsTrue = false
                    });
                }
                isLookForward = false;
            }
        }
    }
}
