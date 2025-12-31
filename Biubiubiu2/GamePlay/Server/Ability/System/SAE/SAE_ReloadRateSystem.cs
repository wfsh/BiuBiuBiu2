using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_ReloadRateSystem : S_Ability_Base {
        private AbilityM_ReloadRate useMData;
        private AbilityIn_ReloadRate useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_ReloadRate)MData;
            if (InData != null) {
                useInData = (AbilityIn_ReloadRate)InData;
            }
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = InData != null ? useInData.In_LifeTime : useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
            AddComponent<EffectReloadRate>( new EffectReloadRate.InitData {
                Value = InData != null ? useInData.In_ReloadRate : useMData.M_ReloadRate,
            });
            if (!string.IsNullOrEmpty(useMData.M_EffectSign)) {
                AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                    CallBack = OnSyncSpawnProto
                });
            }
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