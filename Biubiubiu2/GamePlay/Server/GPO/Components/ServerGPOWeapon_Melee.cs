using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOWeapon_Melee : ServerNetworkComponentBase, ServerGPOWeapon.IGPOWeapon {
        protected S_Weapon_Base weapon;
        public void SetWeaponSystem(IWeapon weapon, Action<WeaponData.UseBulletData> fireCallBack) {
            this.weapon = (S_Weapon_Base)weapon;
        }
    }
}