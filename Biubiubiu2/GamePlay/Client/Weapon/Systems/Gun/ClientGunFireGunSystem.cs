using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunFireGunSystem : C_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ClientGunVfx>();
            // AddComponent<ClientGunAnim>();
            AddComponent<ClientGunAttribute>();
            AddComponent<ClientAttackGunFire>();
            // AddComponent<ClientGunFireEffect>();
            AddComponent<ClientGunMagazine>();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(string.Concat("Gun/", weaponSkinSign));
        }

        protected override void OnClear() {
            base.OnClear();
        }
    }
}