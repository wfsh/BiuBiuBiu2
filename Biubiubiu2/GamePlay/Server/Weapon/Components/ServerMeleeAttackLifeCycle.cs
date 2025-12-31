using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerMeleeAttackLifeCycle : ComponentBase {
        private ServerGPO fireGPO;
        private bool isStartAutoFire;
        private float fireIntervalTime = 0f;
        private float intervalTime = -1f;
        private WeaponData.Data meleeData;
        private int attackAnimIndex;
        protected override void OnAwake() {
            base.OnAwake();
            var system = (S_Weapon_Base)mySystem;
            fireGPO = (ServerGPO)system.UseGPO();
            mySystem.Register<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTime);
            fireGPO.Register<SE_AI.Event_SetFireCycle>(OnSetFireCycle);
            meleeData = system.GetData();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_UpdateIntervalTime>(OnUpdateIntervalTime);
            fireGPO.Unregister<SE_AI.Event_SetFireCycle>(OnSetFireCycle);
            fireGPO = null;
            RemoveUpdate(OnUpdate);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }
        

        private void OnUpdate(float deltaTime) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart || !isStartAutoFire || iEntity == null) {
                return;
            }
            if (fireIntervalTime > 0f) {
                fireIntervalTime -= Time.deltaTime;
                return;
            }
            fireIntervalTime = intervalTime;
            OnAttack();
        }

        private void OnUpdateIntervalTime(ISystemMsg arg1, SE_Weapon.Event_UpdateIntervalTime ent) {
            intervalTime = ent.IntervalTime;
        }

        private void OnAttack() {
            fireGPO.Dispatcher(new SE_GPO.Event_PlayAttack());
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_BoxRectAttack.CreateForKey(meleeData.ItemId),
                InData = new AbilityIn_BoxRectAttack {
                    In_Atk = meleeData.ATK,
                    In_AttackRange = 1.5f,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_StartRotation = fireGPO.GetRota(),
                }
            });
        }

        // 开关
        private void OnSetFireCycle(ISystemMsg arg1, SE_AI.Event_SetFireCycle ent) {
            isStartAutoFire = ent.isEnabled;
        }
    }
}