using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_MoveSpeedRateSystem : S_Ability_Base {
        private AbilityM_MoveSpeedRate useMData;
        private AbilityIn_MoveSpeedRate useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_MoveSpeedRate)MData;
            useInData = (AbilityIn_MoveSpeedRate)InData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime + useInData.In_LifeTime,
                CallBack = LifeTimeEnd,
            });
            AddComponent<EffectMoveSpeedRate>(new EffectMoveSpeedRate.InitData {
                Value = useMData.M_SpeedRate + useInData.In_SpeedRate
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
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
    }
}