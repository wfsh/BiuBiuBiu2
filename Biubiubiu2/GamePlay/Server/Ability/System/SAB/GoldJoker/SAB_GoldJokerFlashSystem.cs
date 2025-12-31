using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerFlashSystem : S_Ability_Base {
        public AbilityM_GoldJokerFlash useMData;
        public AbilityIn_GoldJokerFlash useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerFlash)MData;
            useInData = (AbilityIn_GoldJokerFlash)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void AddComponents() {
            AddComponent<AbilityGoldJokerFlash>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
