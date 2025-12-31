using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_ShootIntervalRateSystem : S_Ability_Base {
        private AbilityM_ShootIntervalRate useMData;
        private AbilityIn_ShootIntervalRate useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_ShootIntervalRate)MData;
            if (InData != null) {
                useInData = (AbilityIn_ShootIntervalRate)InData;
            }
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = InData != null ? useInData.In_LifeTime : useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
            AddComponent<EffectShootIntervalRate>( new EffectShootIntervalRate.InitData {
                Value = InData != null ? useInData.In_ShootIntervalRate : useMData.M_ShootIntervalRate,
            });
            if (!string.IsNullOrEmpty(useMData.M_EffectSign)) {
                AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
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