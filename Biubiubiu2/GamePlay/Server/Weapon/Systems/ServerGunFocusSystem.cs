using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGunFocusSystem : S_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            // EnableComponentSample();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerGunAttribute>();
            AddComponent<ServerAttackGunFireBullet>();
            AddComponent<ServerAttackGunFire>();
            AddComponent<ServerGunMagazine>();
            AddComponent<ServerAttackAutoFire>(); // 自动射击
        }
    }
}