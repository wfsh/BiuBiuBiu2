using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_MissileBombSystem : C_Ability_Base {
        private Proto_Ability.Rpc_MissileBomb abilityData;
        private AbilityData.PlayAbility_MissileBomb modData;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_MissileBomb)InData;
            modData = (AbilityData.PlayAbility_MissileBomb)AbilityConfig.GetAbilityModData(abilityData.abilityModId);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityToPool(modData.M_EffectSign);
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] AB_GrenadeSystem 加载 Entity 失败:" + modData.M_EffectSign);
            } else {
                var entityBase = iEntity as EntityBase;
                var collider = entityBase.GetComponent<SphereCollider>();
                if (collider != null) {
                    GameObject.Destroy(collider);
                }
            }
        }

        private void AddComponents() {
            AddLifeTime();
            AddMove();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                CallBack = LifeTimeEnd,
                LifeTime = modData.M_LifeTime,
            });
        }

        private void AddMove() {
            AddComponent<ClientAbilityMissileMove>(new ClientAbilityMissileMove.InitData {
                Points = abilityData.points,
                Speed = modData.M_Speed,
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