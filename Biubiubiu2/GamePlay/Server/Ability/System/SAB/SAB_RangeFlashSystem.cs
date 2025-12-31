using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_RangeFlashSystem : S_Ability_Base {
        private AbilityData.PlayAbility_RangeFlash inData;

        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_RangeFlash)MData;
            iEntity.SetPoint(inData.In_StartPoint);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddHit();
            AddComponent<ServerAbilityRangeFlashAttack>(new ServerAbilityRangeFlashAttack.InitData {
                IsSelfHurt = false,
                IngoreGPOId = inData.In_IngoreGPOId,
                CheckPoint = inData.In_StartPoint,
                Range = inData.In_Range,
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPODownHp>(new ServerAbilityHurtGPODownHp.InitData {
                HurtHp = inData.M_Power,
                HurtItemId = inData.In_WeaponId,
                HitEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_FlashgunHit,
                DamageType = DamageType.Flash
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                    LifeTime = inData.M_LifeTime,
                    CallBack = LifeTimeEnd
                });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}