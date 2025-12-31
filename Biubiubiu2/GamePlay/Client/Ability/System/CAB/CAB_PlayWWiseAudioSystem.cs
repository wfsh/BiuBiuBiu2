using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlayWWiseAudioSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio useInData;
        private AbilityM_PlayWWiseAudio useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio)InData;
            useMData = (AbilityM_PlayWWiseAudio)MData;
            // 设置 AB 初始位置，有需要可以打开下面的注释，需要从网络协议上进行下发
            iEntity.SetPoint(useInData.startPoint);
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
            useMData = null;
        }

        private void AddComponents() {
            // 长效 AB 调用的方法，会自动进行网络状态，重连相关的逻辑对接
            AddComponent<ClientAbilityNetworkBehaviour>();
            // 长效 AB 调用的方法，会自动和服务器进行位置同步
            AddComponent<ClientNetworkTransform>();
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useInData.lifeTime, 
                CallBack = LifeTimeEnd
            });
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                WWiseID = useInData.wwiseId,
                IsFollow = true,
            });
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}