using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGunSystem : S_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            // EnableComponentSample();
            AddComponents();
        }

        private void AddComponents() {
            if (ModeData.IsSausageMode()) {
                AddComponent<ServerSausageGunAttribute>();
            } else {
                AddComponent<ServerGunAttribute>();
            }
            AddComponent<ServerAttackGunFireBullet>();
            AddComponent<ServerAttackGunFire>();
            AddComponent<ServerGunMagazine>();
            AddComponent<ServerAttackAutoFire>(); // 自动射击
        }
    }
}