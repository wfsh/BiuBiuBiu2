using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIWeapon_Gun : ServerGPOWeapon_Gun {

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            // 关闭自动开火
            weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire {
                IsTrue = false,
            });
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire {
                IsTrue = ent.IsDead == false,
            });
        }
    }
}