using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayWWiseAudioSystem : S_Ability_Base {
        private AbilityM_PlayWWiseAudio useMData;
        private AbilityIn_PlayWWiseAudio useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_PlayWWiseAudio)MData;
            useInData = (AbilityIn_PlayWWiseAudio)InData;
            // 设置 AB 初始位置
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        protected override void OnClear() {
            useMData = null;
            useInData = null;
        }
        
        override protected void AddComponents() {
            base.AddComponents();
             // 示例：生命周期组件中设置时间，时间到后调用技能删除
             AddComponent<TimeReduce>(new TimeReduce.InitData {
                 LifeTime = useInData.In_LifeTime,
                 CallBack = () => {
                    // 删除服务端上的技能，在调用该方法后，会同步通知客户端进行删除，请自己调整该事件的调用位置
                    MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                        AbilityId = AbilityId
                    });
                 }
             });
             if (useInData.In_IsFollow) {
                 // 长效 AB 调用的方法，会自动进行重连，和位置同步，如果只是播放一下特效不需要考虑重连可以删除下面相关的代码
                 // 长效 AB 的组件，需要继承 ServerNetworkComponentBase 而不是 ComponentBase
                 AddComponent<ServerNetworkSync>(new ServerNetworkSync.InitData {
                     CallBack = OnSyncSpawnProto, // 常驻类型
                 });
                 AddComponent<ServerNetworkTransform>();
                 AddComponent<MoveFollowFireGPO>();
             } else {
                 RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio() {
                     configId = ConfigID,
                     rowId = RowId,
                     startPoint = useInData.In_StartPoint,
                     lifeTime = (ushort)(useInData.In_LifeTime * 10f),
                     wwiseId = useInData.In_WWiseId,
                 });
             }
        }
        
        // 重连同步的函数，不需要可以删除
        void OnSyncSpawnProto(ServerNetworkSync sync) {
             var fireGpoId = FireGPO == null ? (ushort)0 : (ushort)FireGPO.GpoID;
             sync.SetSpawnRPC(new Proto_Ability.TargetRpc_PlayAbility {
                  fireGpoId = fireGpoId,
                  abilityId = AbilityId,
                  protoDoc = sync.SerializeProto(new Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio {
                       startPoint = iEntity.GetPoint(), // 不需要同步位置可以删除(RPC 里面也要删掉)， 主要这边要使用 iEntity.GetPoint() 而不是 inData.In_StartPoint
                       configId = ConfigID,
                       rowId = RowId,
                       lifeTime = (ushort)(useInData.In_LifeTime * 10f) ,
                       wwiseId = useInData.In_WWiseId,
                  })
              });
        }
    }
}
