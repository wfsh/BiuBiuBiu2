using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PenetratorGrenadeSystem : C_Ability_Base {
        private Proto_Ability.Rpc_PenetratorGrenade abilityData;
        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_PenetratorGrenade)InData;
            iEntity.SetPoint(abilityData.startPoint);
            iEntity.SetRota(abilityData.startRota);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(abilityData.effectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }
        private void AddComponents() {
            AddLifeTime();
            AddAttack();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                CallBack = LifeTimeEnd,
                LifeTime = abilityData.lifeTime,
            });
        }

        private void AddAttack() {
            AddComponent<ClientAbilityPenetratorGrenade>( new ClientAbilityPenetratorGrenade.InitData {
                Speed = this.abilityData.speed,
            });
        }
        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}