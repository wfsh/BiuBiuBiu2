using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunFocusSystem : C_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponent<ClientGunVfx>();
            AddComponent<ClientGunAnim>();
            AddComponent<ClientGunAttribute>();
            AddComponent<ClientAttackGunFire>();
            // AddComponent<ClientGunFireEffect>();
            AddComponent<ClientGunMagazine>();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(string.Concat("Gun/", weaponSign));
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
        }
    }
}