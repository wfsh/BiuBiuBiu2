using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BellowAttackSystem : S_Ability_Base {
        private AbilityM_BellowAttack useMData;
        private AbilityIn_BellowAttack useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_BellowAttack)MData;
            useInData = (AbilityIn_BellowAttack)InData;
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddAttack();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffect() {
                configId = AbilityConfig.PlayEffect,
                rowId = useMData.M_PlayEffectAbility,
                startPoint = useInData.In_StartPoint,
                lifeTime = (ushort)Mathf.CeilToInt(useMData.M_LifeTime * 10f)
            });
        }

        private void AddAttack() {
            AddComponent<ServerAbilityHurtGPO>( new ServerAbilityHurtGPO.InitData {
                Power = useMData.M_Power,
                WeaponItemId = 0,
            });
            AddComponent<AbilityBellowAttack>( new AbilityBellowAttack.InitData {
                AttackDelayTime = useMData.M_AttackDelayTime,
                Range = useMData.M_Range
            });
            AddComponent<ServerAbilityKnockbackGPO>(new ServerAbilityKnockbackGPO.InitData {
                Duration = 1f,
                Force = 12f
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
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