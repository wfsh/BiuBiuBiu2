using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_GrenadeSystem : C_Ability_Base {
        private Proto_Ability.Rpc_ThreadGrenade useInData;
        private AbilityM_Grenade useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_Ability.Rpc_ThreadGrenade)InData;
            useMData = (AbilityM_Grenade)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(useMData.M_EffectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }
        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("[Error] AB_GrenadeSystem 加载 Entity 失败:" + useMData.M_EffectSign);
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
                LifeTime = useMData.M_LifeTime,
            });
        }

        private void AddMove() {
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Points = useInData.points,
                Speed = useMData.M_Speed,
            });
        }

        private void AddSelect() {
            AddComponent<GrenadeHit>(new GrenadeHit.InitData {
                IsHitBomb = useMData.M_IsHitBomb,
                FireGpoId = this.FireGpoId,
                CallBack = LifeTimeEnd,
            });
            AddComponent<MovePointRaycastHit>(new MovePointRaycastHit.InitData {
                LayerMask = ~(LayerData.ServerLayerMask),
                IgnoreGpoID = this.FireGpoId,
                HitCallBack = (o, hit) => {
                    LifeTimeEnd();
                }
            });
        }
        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}