using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerUMP9System : S_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerAttackGunFireBullet>();
            AddComponent<ServerAttackGunFire>();
            AddComponent<ServerGunMagazine>();
            AddComponent<ServerAttackAutoFire>(); // 自动射击
        }
    }
}