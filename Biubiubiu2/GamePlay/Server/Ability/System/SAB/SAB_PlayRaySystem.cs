using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayRaySystem : S_Ability_Base {
        public AbilityIn_PlayRay useInData;
        public AbilityM_PlayRay useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_PlayRay)InData;
            useMData = (AbilityM_PlayRay)MData;
            iEntity.SetPoint(useInData.In_StartPoint);
            iEntity.SetRota(Quaternion.LookRotation(useInData.In_Dir));
            AddComponents();
        }

        
        protected override void OnClear() {
            base.OnClear();
            useMData = null;
            useInData = null;
        }

        protected override void AddComponents() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
                EndTimeCallBack = LifeTimeEnd
            });
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<ServerNetworkTransform>();
            AddComponent<AbilityPlayRay>(new AbilityPlayRay.InitData() {
                IsFollowFireGPO = useInData.In_IsFollowFireGPO,
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useInData.In_RayATK,
                WeaponItemId = 0,
            });
        }
        
        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_AbilityAB_Auto.Rpc_PlayRay() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    direction = useInData.In_Dir,
                    maxDistance = useInData.In_MaxDistance,
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_LifeTime * 10f),
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                })
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}