namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerFireGunSystem : S_Weapon_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerGunAttribute>();
            AddComponent<ServerAttackFireGunFireBullet>();
            AddComponent<ServerAttackFireGunFire>();
            AddComponent<ServerGunMagazine>();
            AddComponent<ServerAttackAutoFire>(); // 自动射击
        }
    }
}