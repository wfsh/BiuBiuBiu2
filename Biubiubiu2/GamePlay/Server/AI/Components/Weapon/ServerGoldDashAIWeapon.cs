using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIWeapon : ServerGPOWeapon {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
        }

        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            Dispatcher(new SE_GPO.Event_GetPackWeaponList() {
                CallBack = list => {
                    for (int i = 0, count = list.Count; i < count; i++) {
                        var weapon = list[i];
                        weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire {
                            IsTrue = ent.isEnabled,
                        });
                    }
                }
            });
        }
        override protected IGPOWeapon GetWeaponComponent(IWeapon weapon) {
            IGPOWeapon gpoWeapon = null;
            switch (weapon.GetWeaponType()) {
                case WeaponData.WeaponType.Gun:
                    gpoWeapon = mySystem.AddComponentChild<ServerGoldDashAIWeapon_Gun>();
                    break;
                case WeaponData.WeaponType.Melee:
                    gpoWeapon = mySystem.AddComponentChild<ServerGPOWeapon_Melee>();
                    break;
            }
            return gpoWeapon;
        }
    }
}