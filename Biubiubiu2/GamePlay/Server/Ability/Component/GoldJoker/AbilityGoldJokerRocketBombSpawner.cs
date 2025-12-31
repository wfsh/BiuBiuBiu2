using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerRocketBombSpawner : ComponentBase {
        private IGPO fireGPO;
        private AbilityM_GoldJokerRocketBombSpawner useMData;
        private int index;
        private float timer;

        protected override void OnAwake() {
            base.OnAwake();
            var abSystem = (SAB_GoldJokerRocketBombSpawnerSystem)mySystem;
            useMData = (AbilityM_GoldJokerRocketBombSpawner)abSystem.MData;
            fireGPO = abSystem.FireGPO;
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            useMData = null;
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
            if (timer > 0) {
                timer -= delta;
            } else {
                if (index < useMData.M_LifeTime.Length) {
                    timer = useMData.M_LifeTime[index];
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = fireGPO,
                        MData = AbilityM_GoldJokerRocketBomb.CreateForID(0),
                        InData = new AbilityIn_GoldJokerRocketBomb() {
                            In_Param = useMData,
                            In_Index = index++
                        }
                    });
                    fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                        Id = useMData.M_AnimId
                    });
                }
            }
        }
    }
}
