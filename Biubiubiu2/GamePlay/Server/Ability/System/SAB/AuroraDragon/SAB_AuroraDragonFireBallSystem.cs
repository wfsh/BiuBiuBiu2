using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonFireBallSystem : S_Ability_Base {
        private AbilityIn_AuroraDragonFireBall useInData;
        private AbilityM_AuroraDragonFireBall useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_AuroraDragonFireBall)InData;
            useMData = (AbilityM_AuroraDragonFireBall)MData;
            iEntity.SetPoint(useInData.In_StartPos);
            AddComponents();
        }
        
        protected override void AddComponents() {
            AddComponent<AbilityAuroraDragonFireBall>(new AbilityAuroraDragonFireBall.InitData() {
                Param = useMData
            });
            AddLifeCycle();
            AddHit();
            AddComponent<ServerNetworkTransform>();
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
        }
        
        private void AddLifeCycle() {
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useMData.M_ATK,
                WeaponItemId = 0,
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
                    startPoint = useInData.In_StartPos,
                    startScale =  Vector3.one,
                    lifeTime = (ushort)10f
                })
            });
        }
    }
}