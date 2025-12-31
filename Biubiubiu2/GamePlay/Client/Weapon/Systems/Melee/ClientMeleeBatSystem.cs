using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientMeleeBatSystem : C_Weapon_Base {
        protected override void OnStart() {
            base.OnStart();
            CreateEntity(string.Concat("Melee/Client/", weaponSign));
        }
    }
}