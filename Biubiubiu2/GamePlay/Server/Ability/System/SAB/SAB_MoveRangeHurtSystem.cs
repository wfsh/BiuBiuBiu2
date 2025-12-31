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
    public class SAB_MoveRangeHurtSystem : S_Ability_Base {
        public AbilityIn_MoveRangeHurt useInData;
        public AbilityM_MoveRangeHurt useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_MoveRangeHurt)InData;
            useMData = (AbilityM_MoveRangeHurt)MData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useInData = null;
            useMData = null;
        }

        protected override void AddComponents() {
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });

            AddComponent<AbilityMoveRangeHurt>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useInData.In_ATK,
                WeaponItemId = 0,
            });
        }
        
        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_AbilityAB_Auto.Rpc_PlayMovingEffect() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    startLookAt = useInData.In_StartDir,
                    moveDir = useInData.In_StartDir,
                    moveSpeed = (ushort)Mathf.CeilToInt(useInData.In_MoveSpeed * 10),
                    startScale = Vector3.one * useInData.In_Rangle * 2,
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_LifeTime * 10),
                })
            });
        }
    }
}
