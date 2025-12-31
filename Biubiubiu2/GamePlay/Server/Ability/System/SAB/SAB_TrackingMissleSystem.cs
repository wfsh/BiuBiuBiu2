using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_TrackingMissleSystem : S_Ability_Base {
        private static float startCheckTime = 0f;
        private AbilityData.PlayAbility_TrackingMissle _abilityData;
        private float lifeTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            _abilityData = (AbilityData.PlayAbility_TrackingMissle)MData;
            lifeTime = _abilityData.M_MoveDistance / _abilityData.M_TrackSpeed;
            iEntity.SetPoint(_abilityData.In_StartPoint);
            iEntity.SetRota(Quaternion.LookRotation((_abilityData.In_TargetPoint + Vector3.up - _abilityData.In_StartPoint).normalized));
            AddComponents();
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            var mGPOId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = mGPOId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_EffectFollowServerTransform() {
                    abilityModId = _abilityData.ConfigId,
                })
            });
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            // 移动同步组件
            AddComponent<ServerNetworkTransform>();
            AddLifeTime();
            AddMove();
            AddSelect();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = lifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveTrackGPO>(new MoveTrackGPO.InitData {
                TargetGPOId = _abilityData.In_TargetGPOId,
                TrackSpeed = _abilityData.M_TrackSpeed,
                LockSpeed = _abilityData.M_LockSpeed,
                StopTrackDistance = _abilityData.M_StopTrackDistance,
            });
        }

        private void AddSelect() {
            AddComponent<TrackingMissleSelectGPO>(new TrackingMissleSelectGPO.MissleSelectGPOInitData {
                IgnoreGpoID = FireGPO.GetGpoID(),
                AbilityData = _abilityData,
                IgnoreTeamId = FireGPO.GetTeamID(),
                LayerMask = ~(LayerData.ClientLayerMask),
                HitCallBack = (o, hit) => {
                    Dispatcher(new SE_Ability.SetLifeTime {
                        LifeTime = 0.2f,
                    });
                }
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}