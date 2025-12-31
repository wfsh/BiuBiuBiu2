using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIWeapon_Gun : ServerGPOWeapon_Gun {
        private bool isDead = false;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);;
            MsgRegister.Register<SM_ShortcutTool.Event_MonsterFire>(OnShortcutToolMonsterFireCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            EnabledAutoFire();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_ShortcutTool.Event_MonsterFire>(OnShortcutToolMonsterFireCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            isDead = ent.IsDead;
            EnabledAutoFire();
        }
        
        private void OnShortcutToolMonsterFireCallBack(SM_ShortcutTool.Event_MonsterFire ent) {
            EnabledAutoFire();
        }
        
        private void EnabledAutoFire() {
            var isTrue = !isDead && WarData.TestIsAIFire;
            weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire {
                IsTrue = isTrue,
            });
        }
    }
}