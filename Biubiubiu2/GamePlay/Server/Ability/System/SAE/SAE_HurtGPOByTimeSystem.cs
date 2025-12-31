using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_HurtGPOByTimeSystem : S_Ability_Base {
        private AbilityM_HurtGPOByTime useMData;
        private AbilityIn_HurtGPOByTime useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_HurtGPOByTime)MData;
            useInData = (AbilityIn_HurtGPOByTime)InData;
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddLifeTime();
            AddHit();
            AddComponent<ServerAbilityDownHpByTime>();
        }

        void OnSyncSpawnProto(ServerNetworkSync sync) {
            var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
            sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                fireGpoId = fireGpoId,
                abilityId = AbilityId,
                protoDoc = sync.SerializeProto(new Proto_Ability.TargetRpc_PlayGPOEffect {
                    gpoId = TargetGPO.GetGpoID(),
                    configId = ConfigID,
                    rowId = RowId,
                })
            });
        }


        private void AddHit() {
            AddComponent<ServerAbilityHurtGPOByValue>( new ServerAbilityHurtGPOByValue.InitData {
                Power = useMData.M_Power,
                WeaponItemId = useInData.In_WeaponItemId,
            });
        }


        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}