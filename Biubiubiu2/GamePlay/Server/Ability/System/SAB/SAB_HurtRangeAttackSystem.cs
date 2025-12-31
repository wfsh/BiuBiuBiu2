using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_HurtRangeAttackSystem : S_Ability_Base {
        private AbilityData.PlayAbility_HurtRangeAttack inData;
        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_HurtRangeAttack)MData;
            iEntity.SetPoint(inData.In_FirePoint);
            AddComponents();
        }

        protected override void OnStart() {
            if (inData.M_PlayEffectAbility > 0) {
                RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffect() {
                        configId = AbilityConfig.PlayEffect,
                        rowId = inData.M_PlayEffectAbility,
                        startPoint = inData.In_FirePoint,
                        lifeTime = (ushort)Mathf.CeilToInt(inData.M_LifeTime * 10f)
                    }
                );
            }
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddHit();
            AddSelect();
            AddComponent<ServerAbilityKnockbackGPO>();
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>( new ServerAbilityHurtGPO.InitData {
                Power = inData.M_Power,
                WeaponItemId = 0,
            });
        }
        
        private void AddSelect() {
            AddComponent<AttackRangeGPO>(new AttackRangeGPO.InitData{
                CheckPoint = inData.In_FirePoint,
                Range = inData.M_Range,
                IsSelfHurt = true,
                MaxHurtRatio = 1,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
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