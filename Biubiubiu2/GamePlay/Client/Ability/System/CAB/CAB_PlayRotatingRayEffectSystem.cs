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
    public class CAB_PlayRotatingRayEffectSystem : C_Ability_Base {
        public Proto_Ability.Rpc_PlayRotatingRayEffect AbilityData;
        private IAbilityMData _modMData;

        protected override void OnAwake() {
            base.OnAwake();
            AbilityData = (Proto_Ability.Rpc_PlayRotatingRayEffect)InData;
            AddLifeTime();
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
            iEntity.SetPoint(AbilityData.startPoint);
            AddRotate();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddRotate() {
            AddComponent<ClientRayEffectRotateByDegree>( new ClientRayEffectRotateByDegree.InitData {
                StartDeg = AbilityData.startDeg * 0.1f,
                AngularSpeed = AbilityData.moveAngularSpeed * 0.1f,
                Length = 0.1f * AbilityData.startLength,
                PlayTimestamp = AbilityData.playTimestamp
            });
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>( new ClientAbilityLifeCycle.InitData {
                Duration = AbilityData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - AbilityData.playTimestamp),
            });
        }
    }
}