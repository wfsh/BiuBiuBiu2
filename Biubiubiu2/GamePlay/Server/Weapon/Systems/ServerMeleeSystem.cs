using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerMeleeSystem : S_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerMeleeAttribute>();
            AddComponent<ServerMeleeAttackLifeCycle>();
        }

        protected override void OnStart() {
            base.OnStart();
            if (weaponItemId == ItemSet.Id_ShieldBeastcamp) {
                CreateEntity(string.Concat("Melee/Server/", weaponSign));
            }
        }
    }
}