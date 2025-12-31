using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BossFightMusicSystem : S_Ability_Base {
        private AbilityM_BossFightMusic useMData;
        private AbilityIn_BossFightMusic useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_BossFightMusic)MData;
            useInData = (AbilityIn_BossFightMusic)InData;
            // 设置 AB 初始位置
            //iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        protected override void OnClear() {
            useMData = null;
            useInData = null;
        }

        protected override void OnStart() {
             // 此技能不同步到客户端 (SyncCreateCAB=false), 如果有播放特效同步到客户端的需求，可以使用 RPCAbility(new Proto_Ability.Rpc_PlayEffect()
        }

        override protected void AddComponents() {
            base.AddComponents();
             // 示例：生命周期组件中设置时间，时间到后调用技能删除
             // AddComponent<TimeReduce>(new TimeReduce.InitData {
             //     LifeTime = useMData.M_LifeTime,
             //     CallBack = () => {
             //        // 删除服务端上的技能，在调用该方法后，会同步通知客户端进行删除，请自己调整该事件的调用位置
             //        MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
             //            AbilityId = AbilityId
             //        });
             //     }
             // });
        }

    }
}
