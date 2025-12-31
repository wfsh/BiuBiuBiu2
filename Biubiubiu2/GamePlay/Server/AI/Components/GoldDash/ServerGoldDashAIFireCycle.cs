using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIFireCycle : ComponentBase {
        private S_AI_Base aiBase;
        private IWeapon weaponUsing;
        private bool isCurFiring;
        private bool attackStartCountDown;
        private float attackCountDown;
        private float attackIntervalCountDown;
        private bool isCycleEnabled;

        protected override void OnAwake() {
            base.OnAwake();
            Register<SE_AI.Event_SetFireCycle>(OnSetFireCycleCallBack);
            Register<SE_GPO.UseWeapon>(OnSetUseWeapon);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            aiBase = (S_AI_Base)mySystem;
            isCycleEnabled = true;
            isCurFiring = false;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            Unregister<SE_AI.Event_SetFireCycle>(OnSetFireCycleCallBack);
            Unregister<SE_GPO.UseWeapon>(OnSetUseWeapon);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            if (weaponUsing != null) {
                weaponUsing.Unregister<SE_Weapon.Event_Fire>(OnFireCallBack);
            }
            RemoveUpdate(OnUpdate);
        }

        private void OnSetUseWeapon(ISystemMsg body, SE_GPO.UseWeapon ent) {
            if (weaponUsing != null) {
                weaponUsing.Unregister<SE_Weapon.Event_Fire>(OnFireCallBack);
            }

            weaponUsing = ent.Weapon;
            weaponUsing.Register<SE_Weapon.Event_Fire>(OnFireCallBack);
        }

        private void OnFireCallBack(ISystemMsg body, SE_Weapon.Event_Fire ent) {
            if (!attackStartCountDown && attackIntervalCountDown <= 0) {
                attackStartCountDown = true;
                Dispatcher(new SE_AI.Event_GetArmedCustomData() {
                    CallBack = ResetAttackCountDown
                });
            }
        }

        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            isCycleEnabled = ent.isEnabled;
        }

        private void ResetAttackCountDown(bool isCfgInit, MonsterArmedCustom cfg) {
            if (isCfgInit) {
                attackCountDown = cfg.AttackDuration;
            }
        }

        private void OnSetFireCycleCallBack(ISystemMsg body, SE_AI.Event_SetFireCycle ent) {
            isCurFiring = ent.isEnabled;
            if (!isCurFiring) {
                SetAutoFire(false);
                attackCountDown = 0;
                attackIntervalCountDown = 0;
            }
        }

        private void OnUpdate(float deltaTime) {
            if (!isCycleEnabled) {
                return;
            }
            if (!isCurFiring) {
                return;
            }

            if (!attackStartCountDown && attackIntervalCountDown >= 0) {
                attackIntervalCountDown -= deltaTime;

                if (attackIntervalCountDown < 0) {
                    SetAutoFire(true);
                }
            }

            if (attackStartCountDown && attackCountDown >= 0) {
                attackCountDown -= deltaTime;

                if (attackCountDown < 0) {
                    attackStartCountDown = false;
                    Dispatcher(new SE_AI.Event_GetArmedCustomData() {
                        CallBack = ResetAttackIntervalCountDown
                    });

                    SetAutoFire(false);
                }
            }
        }

        private void ResetAttackIntervalCountDown(bool isCfgInit, MonsterArmedCustom cfg) {
            if (isCfgInit) {
                attackIntervalCountDown = cfg.AttackInterval;
            }
        }

        private void SetAutoFire(bool isEnabled) {
            Dispatcher(new SE_GPO.Event_GetPackWeaponList() {
                CallBack = list => {
                    for (int i = 0, count = list.Count; i < count; i++) {
                        var weapon = list[i];
                        weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire {
                            IsTrue = isEnabled,
                        });
                    }
                }
            });
        }
    }
}