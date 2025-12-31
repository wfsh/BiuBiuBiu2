using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAttackMeleeBat : ComponentBase {
        private int batLoopAbilityId = 0;
        private int plungerAttackAbilityId = 0;
        private ServerGPO fireGPO;

        protected override void OnAwake() {
            mySystem.Register<SE_Weapon.Event_BatLoopAttack>(OnBatLoopAttackCallBack);
            mySystem.Register<SE_Weapon.Event_BatEndAttack>(OnBatEndAttackCallBack);
            mySystem.Register<SE_Weapon.Event_PlungerAttack>(OnPlungerAttackCallBack);
            mySystem.Register<SE_Weapon.Event_PlungerEnd>(OnPlungerEndCallBack);
            mySystem.Register<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
        }
        protected override void OnStart() {
            var system = (S_Weapon_Base)mySystem;
            fireGPO = (ServerGPO)system.UseGPO();
        }

        protected override void OnClear() {
            base.OnClear();
            fireGPO = null;
            CanceBatLoopAttack();
            CancePlungerLoopAttack();
            mySystem.Unregister<SE_Weapon.Event_BatLoopAttack>(OnBatLoopAttackCallBack);
            mySystem.Unregister<SE_Weapon.Event_BatEndAttack>(OnBatEndAttackCallBack);
            mySystem.Unregister<SE_Weapon.Event_PlungerAttack>(OnPlungerAttackCallBack);
            mySystem.Unregister<SE_Weapon.Event_PlungerEnd>(OnPlungerEndCallBack);
            mySystem.Unregister<SE_Weapon.Event_PutOn>(OnPutOnCallBack);
        }
        
        private void OnPutOnCallBack(ISystemMsg body, SE_Weapon.Event_PutOn ent) {
            if (ent.IsTrue) {
                return;
            }
            CanceBatLoopAttack();
            CancePlungerLoopAttack();
        }

        private void OnBatLoopAttackCallBack(ISystemMsg body, SE_Weapon.Event_BatLoopAttack ent) {
            CanceBatLoopAttack();
            if (ent.IsAttack) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    CallBack = GetBatLoopAbility,
                    FireGPO = fireGPO,
                    AbilityMData = new AbilityData.PlayAbility_BatLoopAttack() {
                        ConfigId = AbilityConfig.BatLoopAttack,
                    }
                });
            }
        }

        private void CanceBatLoopAttack() {
            if (batLoopAbilityId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = batLoopAbilityId
            });
            batLoopAbilityId = 0;
        }

        private void GetBatLoopAbility(IAbilitySystem system) {
            batLoopAbilityId = system.GetAbilityId();
        }

        private void OnBatEndAttackCallBack(ISystemMsg body, SE_Weapon.Event_BatEndAttack ent) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                CallBack = null,
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_BatEndAttack() {
                    ConfigId = AbilityConfig.BatEndAttack, In_StartPoint = ent.StartPoint, In_StartRota = ent.StartRota,
                }
            });
        }

        private void OnPlungerAttackCallBack(ISystemMsg body, SE_Weapon.Event_PlungerAttack ent) {
            CancePlungerLoopAttack();
            if (ent.StartRota != Quaternion.identity) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    CallBack = GetPlungerLoopAbility,
                    FireGPO = fireGPO,
                    AbilityMData = new AbilityData.PlayAbility_PlungerAttackLoop() {
                        ConfigId = AbilityConfig.PlungerAttackLoop, In_StartRota = ent.StartRota,
                    }
                });
            }
        }

        private void GetPlungerLoopAbility(IAbilitySystem system) {
            plungerAttackAbilityId = system.GetAbilityId();
        }

        private void CancePlungerLoopAttack() {
            if (plungerAttackAbilityId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = plungerAttackAbilityId
            });
            plungerAttackAbilityId = 0;
        }

        private void OnPlungerEndCallBack(ISystemMsg body, SE_Weapon.Event_PlungerEnd ent) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                CallBack = GetPlungerLoopAbility,
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_PlungerEndAttack() {
                    ConfigId = AbilityConfig.PlungerEndAttack,
                    In_StartRota = ent.StartRota,
                    In_StartPoint = ent.StartPoint
                }
            });
        }
    }
}