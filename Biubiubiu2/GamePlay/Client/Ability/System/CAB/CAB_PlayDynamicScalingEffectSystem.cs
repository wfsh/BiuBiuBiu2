using System;
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
    public class CAB_PlayDynamicScalingEffectSystem : C_Ability_Base {
        private Proto_Ability.Rpc_PlayDynamicScalingEffect useInData;
        private Vector3 scale;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_Ability.Rpc_PlayDynamicScalingEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            if (string.IsNullOrEmpty(MData.GetEffectSign())) {
                Dispatcher(new CE_Ability.RemoveAbility() {
                    AbilityId = GetAbilityId()
                });
            } else {
                CreateEntity(MData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);

            iEntity.SetPoint(useInData.startPoint);
            if (useInData.startRota != Quaternion.identity) {
                iEntity.SetRota(useInData.startRota);
            }

            var scaleChangeSpeed = 0.1f * useInData.scaleChangeSpeed;
            var scaleTimeDiff = 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp) * scaleChangeSpeed;
            scaleTimeDiff.y = 0;
            scale = useInData.startScale * 0.1f + scaleTimeDiff;
            iEntity.SetLocalScale(scale);

            AddComponent<ClientEffectScaleUpdater>(new ClientEffectScaleUpdater.InitData {
                ScaleChangeSpeed = scaleChangeSpeed,
            });
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                WWiseID = useInData.audioKey,
                IsFollow = true,
            });
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>(new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp),
                EndCallBack = null
            });
        }
    }
}