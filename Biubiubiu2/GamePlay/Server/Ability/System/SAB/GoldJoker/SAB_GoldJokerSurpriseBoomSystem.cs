using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerSurpriseBoomSystem : S_Ability_Base {
        public AbilityM_GoldJokerSurpriseBoom useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerSurpriseBoom)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            AddComponent<AbilityGoldJokerSurpriseBoom>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
