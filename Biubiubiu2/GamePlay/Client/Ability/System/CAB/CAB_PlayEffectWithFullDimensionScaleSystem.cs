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
    public class CAB_PlayEffectWithFullDimensionScaleSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayEffectWithFullDimensionScale useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayEffectWithFullDimensionScale)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            if (string.IsNullOrEmpty(MData.GetEffectSign())) {
                LifeTimeEnd();
            } else {
                CreateEntity(MData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            iEntity.SetLocalScale(useInData.scale * 0.1f);
            iEntity.SetPoint(useInData.startPoint);
            if (useInData.startRota != Quaternion.identity) {
                iEntity.SetRota(useInData.startRota);
            }
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                WWiseID = useInData.audioKey,
                IsFollow = true,
            });
        }

        private void AddLifeTime() {
            var duration = useInData.lifeTime * 0.1f - 0.001f * (TimeUtil.GetCurUTCTimestamp() - useInData.playTimestamp);
            if (duration <= 0) {
                LifeTimeEnd();
            } else {
                AddComponent<TimeReduce>(new TimeReduce.InitData {
                    LifeTime = useInData.lifeTime * 0.1f,
                    CallBack = LifeTimeEnd
                });
            }
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}