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

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityDragonFullScreenAOESpawner : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_DragonFullScreenAOESpawner Param;
        }
        private SAB_DragonFullScreenAOESpawnerSystem abSystem;
        private AbilityM_DragonFullScreenAOESpawner config;
        private float timer;
        private int index;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_DragonFullScreenAOESpawnerSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = config.GetTypeID(),
            });
            #endregion
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (timer > 0) {
                timer -= deltaTime;
            } else {
                if (index < config.M_SpawnPoints.Length) {
                    var effectId = index == 0 ? AbilityM_PlayEffect.ID_PlayDragonAOEEffect01 : AbilityM_PlayEffect.ID_PlayDragonAOEEffect02;
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld() {
                        FireGPO = abSystem.FireGPO,
                        AbilityMData = new AbilityData.PlayAbility_DragonFullScreenAOE() {
                            ConfigId = AbilityConfig.DragonFullScreenAOE,
                            In_Param = config,
                            In_SpawnPointIndex = index,
                            In_PlayEffectId = effectId
                        }
                    });
                    timer = config.M_NextTime;
                    index++;
                }
            }
        }
    }
}