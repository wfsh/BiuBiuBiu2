using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerMeleeAttribute : ServerWeaponAttribute {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Weapon.Event_GetGunAttribute>(OnGetGunAttributeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_GetGunAttribute>(OnGetGunAttributeCallBack);
        }

        override protected int ATK() {
            if (weaponData == null) {
                return 0;
            }
            return Mathf.FloorToInt(weaponData.ATK * RandomAtk * 0.01f);
        }
        private void OnGetGunAttributeCallBack(ISystemMsg body, SE_Weapon.Event_GetGunAttribute ent) {
            ent.CallBack.Invoke(ATK(), 0, IntervalTime(), 0, 0, 0, 0);
        }
    }
}