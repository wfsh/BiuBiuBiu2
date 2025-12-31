using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAE_MoveSpeedRateByTimeSystem : S_Ability_Base {
        private AbilityM_MoveSpeedRateByTime useMData;
        private AbilityIn_MoveSpeedRateByTime useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_MoveSpeedRateByTime)MData;
            if (InData != null) {
                useInData = (AbilityIn_MoveSpeedRateByTime)InData;
            }
            // 设置 AB 初始位置
            AddComponents();
        }

        protected override void OnClear() {
            useMData = null;
        }

        override protected void AddComponents() {
            base.AddComponents();
             // 示例：生命周期组件中设置时间，时间到后调用技能删除
             AddComponent<TimeReduce>(new TimeReduce.InitData {
                 LifeTime = InData != null ? useInData.In_LifeTime : useMData.M_LifeTime,
                 CallBack = () => {
                    // 删除服务端上的技能，在调用该方法后，会同步通知客户端进行删除，请自己调整该事件的调用位置
                    MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                        AbilityId = AbilityId
                    });
                 }
             });
             AddComponent<EffectMoveSpeedRateByTime>( new EffectMoveSpeedRateByTime.InitData {
                 InitSpeedRate = InData != null ? useInData.In_InitSpeedRate : useMData.M_InitSpeedRate,
                 TargetSpeedRate = InData != null ? useInData.In_TargetSpeedRate : useMData.M_TargetSpeedRate,
                 ReachTargetSpeedRateTime = InData != null ? useInData.In_ReachTargetSpeedRateTime : useMData.M_ReachTargetSpeedRateTime,
             });
             if (!string.IsNullOrEmpty(useMData.M_EffectSign)) {
                 AddComponent<ServerNetworkSync>( new ServerNetworkSync.InitData {
                     CallBack = OnSyncSpawnProto
                 });
             }
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
