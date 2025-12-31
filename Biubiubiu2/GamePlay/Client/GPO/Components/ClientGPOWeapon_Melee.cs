using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOWeapon_Melee : ComponentBase, ClientGPOWeapon.IGPOWeapon {
        private IWeapon iWeapon;

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            CanceUseWeapon();
        }

        public void SetWeaponSystem(IWeapon weapon) {
            this.iWeapon = weapon;
        }

        public void CanceUseWeapon() {
            PlayAttackAnim(0);
            iWeapon = null;
        }

        public void PlayAttackAnim(int attackAnimId) {
            mySystem.Dispatcher(new CE_Character.PlayAttackAnim {
                PlayAnimId = attackAnimId,
            });
        }
    }
}