using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldDashFightBossRangeSystem : S_Ability_Base {
        public AbilityM_GoldDashFightBossRange useMData;
        public AbilityIn_GoldDashFightBossRange useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldDashFightBossRange)MData;
            useInData = (AbilityIn_GoldDashFightBossRange)InData;
            AddComponents();
            var startPoint = useInData.In_FightRangeCenterPoint + FireGPO.GetForward() * useMData.M_FightRangeCenterForwardOffset;
            iEntity.SetPoint(startPoint);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        private void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = SetSpawnProtoFunc,
                OR_SyncDistance = 250
            });
            AddComponent<ServerGoldDashFightBossRange>();
        }

        private void SetSpawnProtoFunc(ServerNetworkSync sync) {
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = (ushort)FireGPO.GetGpoID(),
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.TargetRpc_GoldDashFightBossRange() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = iEntity.GetPoint(),
                    startScale = new Vector3(useMData.M_FightRangeRadius, 1, useMData.M_FightRangeRadius),
                    removeTimeAfterDead = useMData.M_RemoveTimeAfterDead,
                })
            });
        }
    }
}