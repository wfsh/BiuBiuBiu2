using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityDownHpByTime : ComponentBase {
        private float burnTime = 0.1f;
        private AbilityM_HurtGPOByTime useMData;
        private SAE_HurtGPOByTimeSystem mSystem;

        protected override void OnAwake() {
            base.OnAwake();
            mSystem = (SAE_HurtGPOByTimeSystem)mySystem;
            useMData = (AbilityM_HurtGPOByTime)mSystem.MData;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            burnTime -= deltaTime;
            if (mSystem.TargetGPO.IsGodMode()) {
                LifeTimeEnd();
                return;
            }
            if (burnTime <= 0) {

                burnTime = useMData.M_DeltaTime;
                //拿到对应的GPO去发扣血
                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = mSystem.TargetGPO,
                    HurtRatio = 1f,
                });
            }
        }
        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = mSystem.AbilityId
            });
        }
    }
}