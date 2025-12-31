using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_AIBallSystem : C_Ability_Base {
        private Proto_Ability.Rpc_ThreadMonsterBall abilityData;
        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_ThreadMonsterBall)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(abilityData.EffectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }
        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] AB_GrenadeSystem 加载 Entity 失败:" + abilityData.EffectSign);
            }
        }
        
        private void AddComponents() {
            AddLifeTime();
            AddMove();
            AddSelect();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                CallBack = LifeTimeEnd,
                LifeTime = abilityData.LifeTime,
            });
        }

        private void AddMove() {
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Points = abilityData.points,
                Speed = abilityData.Speed,
            });
        }

        private void AddSelect() {
            AddComponent<ClientAIBallHit>(new ClientAIBallHit.InitData {
                MonsterPid = abilityData.MonsterPID,
                CallBack = LifeTimeEnd,
            });
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}