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
    public class SAB_GoldJokerFloatingGunSystem : S_Ability_Base {
        public AbilityIn_GoldJokerFloatingGun useInData;
        public AbilityM_GoldJokerFloatingGun useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_GoldJokerFloatingGun)InData;
            useMData = (AbilityM_GoldJokerFloatingGun)MData;
            iEntity.SetPoint(useInData.In_StartPoint);
            iEntity.SetRota(useInData.In_StartRot);
            AddComponents();
        }

        protected override void AddComponents() {
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<ServerNetworkTransform>();
            AddComponent<AbilityGoldJokerFloatingGun>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_Param.M_LifeTime - useInData.In_Param.M_StartTime,
            });
        }
        
        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_AbilityAB_Auto.Rpc_GoldJokerFloatingGun() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    startRot = useInData.In_StartRot.eulerAngles,
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_Param.M_LifeTime * 10f),
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                })
            });
        }
    }
}
