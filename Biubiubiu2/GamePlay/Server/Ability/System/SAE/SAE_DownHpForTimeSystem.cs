using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_DownHpForTimeSystem : S_Ability_Base {
        private AbilityM_DownHpForTime useMData;
        private AbilityIn_DownHpForTime useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_DownHpForTime)MData;
            useInData = (AbilityIn_DownHpForTime)InData;
            AddComponents();
        }
        protected override void OnClear() {
            base.OnClear();
        }
        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                CallBack = OnSyncSpawnProto,
            });
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useInData.In_LifeTime,
                CallBack = LifeTimeEnd,
            });
            AddComponent<TargetDownHpForTime>(new TargetDownHpForTime.InitData {
                DownHpValue = useInData.In_DownHpValue,
                DownHpSpace = useInData.In_DownHpSpace,
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
                    rowId = RowId
                })
            });
        }
    }
}