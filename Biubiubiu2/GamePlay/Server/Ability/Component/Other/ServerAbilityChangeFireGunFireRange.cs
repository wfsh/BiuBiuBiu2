using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityChangeFireGunFireRange : ServerNetworkComponentBase {
        private SAB_FireGunFireSystem mSystem;
        private bool mIsResetFireRange = false;
        
        protected override void OnAwake() {
            base.OnAwake();
            mSystem = (SAB_FireGunFireSystem)mySystem;
            mySystem.Register<SE_Ability.Ability_FireGunFireRangeChange>(OnFireChangeCallBack);
            mySystem.Register<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new SE_Ability.Ability_SetRangeChangeCallBack() {
                Callback = ChangeRangeCallBack
            });
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_Ability.Ability_FireGunFireRangeChange>(OnFireChangeCallBack);
            mySystem.Unregister<SE_Weapon.Event_PlayFireAbility>(OnPlayFireAbilityBack);
        }

        
        private void OnPlayFireAbilityBack(ISystemMsg body, SE_Weapon.Event_PlayFireAbility ent) {
            // Debug.LogError("OnPlayFireAbilityBack  FireGunFireS");
            if (mSystem.FireGPO.IsDead()) {
                MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                    AbilityId = mSystem.AbilityId
                });
                return;
            }

            iEntity.SetPoint(ent.StartPoint);
            iEntity.SetRota(
                Quaternion.LookRotation((ent.TargetPoint - ent.StartPoint).normalized));
            mySystem.Dispatcher(new SE_Ability.Ability_RaycastStartCheck());
        }
        
        private void ChangeRangeCallBack(float r) {
            if (Mathf.Abs(mSystem.abilityData.M_Range - r) > 0.5f) {
                //下发火焰range变更
                // Debug.LogError("ChangeRange:"+r);
                mIsResetFireRange = false;
                Dispatcher(new SE_Ability.Ability_FireGunFireRangeChange {
                    range = r,
                    initRange = mSystem.abilityData.M_InitRange
                });
            }
            else {
                if (mIsResetFireRange == false) {
                    mIsResetFireRange = true;
                    Dispatcher(new SE_Ability.Ability_FireGunFireRangeChange {
                        range = mSystem.abilityData.M_Range,
                        initRange = mSystem.abilityData.M_InitRange
                    });
                }
            }
        }
        
        private void OnFireChangeCallBack(ISystemMsg body, SE_Ability.Ability_FireGunFireRangeChange ent) {
            //range变更下发到客户端
            // Debug.LogError("Rpc_ChangeFireGunFireRange");
            Rpc(new Proto_Ability.Rpc_ChangeFireGunFireRange() {
                range = ent.range,
                initRange = ent.initRange
            });
        }
    }
}