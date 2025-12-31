using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayRotatingEffectSystem : S_Ability_Base {
        public AbilityData.PlayAbility_PlayRotatingEffect InData;
        private ServerNetworkSync serverNetworkSync;

        protected override void OnAwake() {
            base.OnAwake();
            InData = (AbilityData.PlayAbility_PlayRotatingEffect)MData;
            AddComponents();
            FireGPO.Register<SE_GPO.Event_SetIsDead>(OnSetDeadCallBack);
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = InData.In_LifeTime,
                CallBack = LifeTimeEnd
            });
            AddComponent<ServerEffectRotateByDegree>();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = SetSpawnProtoFunc,
            });
        }

        protected override void OnClear() {
            base.OnClear();
            FireGPO.Unregister<SE_GPO.Event_SetIsDead>(OnSetDeadCallBack);
        }

        private void OnSetDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                LifeTimeEnd();
            }
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.BeforeRemoveAbility() {
                abSystem = this,
            });
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private void SetSpawnProtoFunc(ServerNetworkSync sync) {
            var countLifeTime = InData.In_LifeTime;
            Dispatcher(new SE_Ability.GetCountLifeTime {
                CallBack = lifeTime => {
                    countLifeTime = lifeTime;
                }
            });
            var CurDeg = 0f;
            var Radius = 0f;
            var AroundPoint = Vector3.zero;
            var MoveAngularSpeed = 0f;
            Dispatcher(new SE_Ability.GetServerEffectRotateByDegreeData {
                CallBack = (curDeg, radius, aroundPoint, moveAngularSpeed) => {
                    CurDeg = curDeg;
                    Radius = radius;
                    AroundPoint = aroundPoint;
                    MoveAngularSpeed = moveAngularSpeed;
                }
            });
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = (ushort)FireGPO.GetGpoID(),
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_PlayRotatingEffect() {
                    abilityModId = InData.GetConfigId(),
                    startDeg = (ushort)Mathf.CeilToInt(CurDeg * 10),
                    startRadius = (ushort)Mathf.CeilToInt(Radius * 10),
                    rotateAroundPoint = AroundPoint,
                    lifeTime = (ushort)Mathf.CeilToInt(countLifeTime * 10),
                    moveAngularSpeed = (short)Mathf.CeilToInt(MoveAngularSpeed * 10),
                    endWhenFireGPODead = InData.In_EndWhenFireGPODead,
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                })
            });
        }
    }
}