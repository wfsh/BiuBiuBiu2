using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherWeapon : ClientGPOWeapon {
        override protected IGPOWeapon GetWeaponComponent(IWeapon weapon) {
            IGPOWeapon gpoWeapon = null;
            switch (weapon.GetWeaponType()) {
                case WeaponData.WeaponType.Gun:
                    gpoWeapon = mySystem.AddComponentChild<ClientGPOWeapon_Gun>();
                    break;
                case WeaponData.WeaponType.Melee:
                    gpoWeapon = mySystem.AddComponentChild<CharacterOtherWeapon_Melee>();
                    break;
            }
            return gpoWeapon;
        }
    }
}