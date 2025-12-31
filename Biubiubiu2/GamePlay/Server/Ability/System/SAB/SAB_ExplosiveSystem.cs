using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_ExplosiveSystem : S_Ability_Base {
        private Action onClearCallback;
        private AbilityIn_Explosive useInData;
        private AbilityM_Explosive useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_Explosive)InData;
            useMData = (AbilityM_Explosive)MData;
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
            Register<SE_Ability.Ability_SetClearCallback>(OnSetClearCallback);
        }

        protected override void OnClear() {
            Unregister<SE_Ability.Ability_SetClearCallback>(OnSetClearCallback);
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddHit();
            AddSelect();
            if (useMData.M_IsStrikeFly) {
                AddComponent<ServerAbilityStrikeFlyGPO>(new ServerAbilityStrikeFlyGPO.InitData {  //之前使用的默认值
                    Duration = 2,
                    Force = 5
                });
            }
        }

        protected override void OnStart() {
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffect() {
                    configId = AbilityConfig.PlayEffect,
                    rowId = useMData.M_PlayEffectAbility,
                    startPoint = useInData.In_StartPoint,
                    lifeTime = (ushort)Mathf.CeilToInt(useMData.M_LifeTime * 10f),
                    scale = (byte)Mathf.CeilToInt((useInData.In_Range / 5f) * 10f)
                }
            );
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPODownHp>(new ServerAbilityHurtGPODownHp.InitData {
                HitEffectAbilityConfigId = AbilityM_PlayBloodSplatter.ID_BloodSplatter,
                DamageType = DamageType.Explosive,
                HurtHp = useInData.In_Hurt,
                HurtItemId = useInData.In_WeaponId,
            });
        }

        private void AddSelect() {
            AddComponent<AttackRangeGPO>(new AttackRangeGPO.InitData() {
                CheckPoint = useInData.In_StartPoint,
                Range = useInData.In_Range,
                IsSelfHurt = useMData.M_IsHurtSelf,
                MaxHurtRatio = useMData.M_MaxDistanceHurtRatio,
                IsRayRange = useMData.M_IsRayRange
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
            onClearCallback?.Invoke();
        }

        private void OnSetClearCallback(ISystemMsg body, SE_Ability.Ability_SetClearCallback ent) {
            onClearCallback = ent.Callback;
        }
    }
}