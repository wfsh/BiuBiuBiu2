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
    public class CAB_PlayDynamicScalingRippleEffectSystem : C_Ability_Base {
        private Proto_Ability.Rpc_PlayDynamicScalingRippleEffect abilityData;
        private IAbilityMData _modMData;
        private Vector3 scale;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_PlayDynamicScalingRippleEffect)InData;
            iEntity.SetPoint(abilityData.startPoint);
            if (abilityData.startRota != Quaternion.identity) {
                iEntity.SetRota(abilityData.startRota);
            }
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            _modMData = AbilityConfig.GetAbilityModData(abilityData.abilityModId);
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
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                WWiseID = abilityData.audioKey,
                IsFollow = true,
            });
            AddComponent<ClientRippleEffectDraw>(new ClientRippleEffectDraw.InitData {
                PlayTimestamp = abilityData.playTimestamp,
                ScaleChangeSpeed = abilityData.scaleChangeSpeed
            });
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = abilityData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - abilityData.playTimestamp),
                EndCallBack = null
            });
        }
    }
}