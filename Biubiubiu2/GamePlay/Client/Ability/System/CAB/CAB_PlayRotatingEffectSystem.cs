using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlayRotatingEffectSystem : C_Ability_Base {
        public Proto_Ability.Rpc_PlayRotatingEffect AbilityData;
        private IAbilityMData _modMData;

        protected override void OnAwake() {
            base.OnAwake();
            AbilityData = (Proto_Ability.Rpc_PlayRotatingEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            _modMData = AbilityConfig.GetAbilityModData(AbilityData.abilityModId);
            if (string.IsNullOrEmpty(_modMData.GetEffectSign())) {
                Dispatcher(new CE_Ability.RemoveAbility() {
                    AbilityId = GetAbilityId()
                });
            } else {
                CreateEntity(_modMData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            AddRotate();
        }

        private void AddComponents() {
            AddLifeTime();
        }

        private void AddRotate() {
            AddComponent<ClientEffectRotateByDegree>(new ClientEffectRotateByDegree.InitData {
                StartDeg = AbilityData.startDeg * 0.1f,
                StartRadius = AbilityData.startRadius * 0.1f,
                MoveAngularSpeed = AbilityData.moveAngularSpeed * 0.1f,
                PlayTimestamp = AbilityData.playTimestamp
            });
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = AbilityData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - AbilityData.playTimestamp),
                EndCallBack = null
            });
        }
    }
}