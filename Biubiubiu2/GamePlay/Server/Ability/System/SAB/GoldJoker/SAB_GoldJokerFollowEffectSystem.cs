using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerFollowEffectSystem : S_Ability_Base {
        public AbilityM_GoldJokerFollowEffect useMData;
        public AbilityIn_GoldJokerFollowEffect useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerFollowEffect)MData;
            useInData = (AbilityIn_GoldJokerFollowEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            var parent = FireGPO.GetBodyTran(useInData.In_BodyPart);
            iEntity.SetParent(parent);
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
        }

        protected override void AddComponents() {
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<ServerNetworkTransform>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
            });
        }
        
        private void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_AbilityAB_Auto.Rpc_GoldJokerFollowEffect() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = iEntity.GetPoint(),
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_LifeTime * 10f),
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                })
            });
        }
    }
}
