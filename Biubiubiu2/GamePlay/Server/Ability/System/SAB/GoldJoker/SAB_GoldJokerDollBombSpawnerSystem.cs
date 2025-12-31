using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GoldJokerDollBombSpawnerSystem : S_Ability_Base {
        private AbilityM_GoldJokerDollBombSpawner useMData;
        public AbilityIn_GoldJokerDollBombSpawner useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_GoldJokerDollBombSpawner)MData;
            useInData = (AbilityIn_GoldJokerDollBombSpawner)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void AddComponents() {
            AddComponent<AbilityGoldJokerDollBombSpawner>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useMData.M_LifeTime,
                EndTimeCallBack = null,
            });
        }
    }
}
