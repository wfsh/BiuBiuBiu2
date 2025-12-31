using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AuroraDragonDragonCarFlameSystem : S_Ability_Base {
        private AbilityM_AuroraDragonDragonCarFlame useMData;
        private AbilityIn_AuroraDragonDragonCarFlame useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_AuroraDragonDragonCarFlame)MData;
            useInData = (AbilityIn_AuroraDragonDragonCarFlame)InData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<AbilityAuroraDragonDragonCarFlame>(new AbilityAuroraDragonDragonCarFlame.InitData() {
                Param = useInData
            });
            AddComponent<ServerNetworkTransform>();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useInData.In_Damage,
                WeaponItemId = 0,
            });
        }

        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.Rpc_SyncTransformEffect {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = iEntity.GetPoint(),
                    startRota = iEntity.GetRota(),
                    startScale = new Vector3(useInData.In_DamageRadius / 2.5f, 1f, 0f),
                    lifeTime = (ushort)(useInData.In_LifeTime * 10f),
                })
            });
        }
    }
}