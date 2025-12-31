using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayRotatingRayEffectSystem : S_Ability_Base {
        public AbilityData.PlayAbility_PlayRotatingRayEffect InData;
        private IAbilityMData _modMData;

        protected override void OnAwake() {
            base.OnAwake();
            InData = (AbilityData.PlayAbility_PlayRotatingRayEffect)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            _modMData = AbilityConfig.GetAbilityModData(InData.GetConfigId());
            if (string.IsNullOrEmpty(_modMData.GetEffectSign())) {
                EndAbility();
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            iEntity.SetPoint(InData.In_StartPoint);
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerRayEffectRotateByDegree>(new ServerRayEffectRotateByDegree.InitData {
                AngularSpeed = InData.In_MoveAngularSpeed,
                StartDeg = InData.In_StartDeg,
                Length = InData.In_StartLength,
            });
            AddComponent<ServerAbilityLifeCycle>(new ServerAbilityLifeCycle.InitData {
                LifeTime = InData.In_LifeTime,
            });
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = SetSpawnProtoFunc,
            });
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void EndAbility() {
            MsgRegister.Dispatcher(new SM_Ability.BeforeRemoveAbility() {
                abSystem = this,
            });
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = this.GetAbilityId(),
            });
        }

        private void SetSpawnProtoFunc(ServerNetworkSync sync) {
            var lifeTime = InData.In_LifeTime;
            Dispatcher(new SE_Ability.GetLifeTime {
                CallBack = f => {
                    lifeTime = f;
                } 
            });
            var Len = 0f;
            var CurDeg = 0f;
            Dispatcher(new SE_Ability.GetRayEffectRotateByDegree {
                CallBack = (len, curDeg) => {
                    Len = len;
                    CurDeg = curDeg;
                }
            });
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = (ushort)FireGPO.GetGpoID(),
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_PlayRotatingRayEffect() {
                    abilityModId = InData.GetConfigId(),
                    startPoint = InData.In_StartPoint,
                    startLength = (ushort)(10 * Len),
                    startDeg = (ushort)Mathf.CeilToInt(CurDeg * 10),
                    lifeTime = (ushort)Mathf.CeilToInt(lifeTime * 10),
                    moveAngularSpeed = (short)Mathf.CeilToInt(InData.In_MoveAngularSpeed * 10),
                    endWhenFireGPODead = true,
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                })
            });
        }
    }
}