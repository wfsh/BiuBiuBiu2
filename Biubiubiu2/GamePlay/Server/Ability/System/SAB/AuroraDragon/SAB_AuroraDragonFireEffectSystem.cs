using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonFireEffectSystem : S_Ability_Base {
        private AbilityIn_AuroraDragonFireEffect useInData;
        private AbilityM_AuroraDragonFireEffect useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_AuroraDragonFireEffect)InData;
            useMData = (AbilityM_AuroraDragonFireEffect)MData;
            AddComponents();
        }
        
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonFireEffect>(new AbilityAuroraDragonFireEffect.InitData() {
                Param = useInData
            });
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<ServerNetworkTransform>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime
            });
        }

        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_SyncTransformEffect() {
                    configId = useMData.GetConfigId(),
                    rowId = useMData.GetRowID(),
                    startPoint = iEntity.GetPoint(),
                    startRota = iEntity.GetRota(),
                    startScale = iEntity.GetLocalScale(),
                    lifeTime = (ushort)(useInData.In_LifeTime * 10f),
                })
            });
        }
    }
}