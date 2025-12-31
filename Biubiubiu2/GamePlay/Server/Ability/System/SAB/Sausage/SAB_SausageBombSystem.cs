using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_SausageBombSystem : S_Ability_Base {
        private AbilityIn_SausageBomb useInData;
        private AbilityM_SausageBomb useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_SausageBomb)InData;
            useMData = (AbilityM_SausageBomb)MData;
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddHit();
            AddSelect();
            if (useMData.M_IsStrikeFly) {
                AddComponent<ServerAbilityStrikeFlyGPO>();
            }
        }
        private void AddHit() {
            if (useInData.In_Hurt != 0) {
                AddComponent<ServerAbilityHurtGPODownHp>(new ServerAbilityHurtGPODownHp.InitData {
                    HurtHp = useInData.In_Hurt,
                    HurtItemId = 0,
                });
            }
            AddComponent<ServerAbilityBombEffect>(new ServerAbilityBombEffect.InitData {
                MData = useInData.MData,
                InData = useInData.InData,
            });
        }

        private void AddSelect() {
            AddComponent<AttackRangeGPO>(new AttackRangeGPO.InitData {
                CheckPoint = useInData.In_StartPoint,
                Range = useInData.In_Range,
                IsSelfHurt = false,
                MaxHurtRatio = 0.4f,
                IsRayRange = true,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = 0.5f,
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