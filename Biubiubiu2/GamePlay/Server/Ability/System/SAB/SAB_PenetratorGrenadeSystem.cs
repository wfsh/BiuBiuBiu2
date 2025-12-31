using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 穿甲榴弹
    public class SAB_PenetratorGrenadeSystem : S_Ability_Base {
        private AbilityData.PlayAbility_PenetratorGrenade _abilityData;

        protected override void OnAwake() {
            base.OnAwake();
            _abilityData = (AbilityData.PlayAbility_PenetratorGrenade)MData;
            iEntity.SetPoint(_abilityData.In_StartPoint);
            iEntity.SetRota(_abilityData.In_StartRota);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddAttack();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_Ability.Rpc_PenetratorGrenade {
                    speed = _abilityData.M_Speed,
                    startPoint = _abilityData.In_StartPoint,
                    startRota = _abilityData.In_StartRota,
                    effectSign = _abilityData.M_EffectSign,
                    lifeTime = _abilityData.M_LifeTime,
                }
            );
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = _abilityData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddAttack() {
            AddComponent<AbilityPenetratorGrenade>(new AbilityPenetratorGrenade.InitData {
                IgnoreGpoID = FireGPO.GetGpoID(),
                MoveSpeed = _abilityData.M_Speed,
                BombTime = _abilityData.M_BombTime,
                Power = _abilityData.M_Power,
                Range = _abilityData.M_BombRange,
                PlayEndCallBack = LifeTimeEnd,
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}