using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayWarningEffectSystem : S_Ability_Base {
        public AbilityData.PlayAbility_PlayWarningEffect inData;

        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_PlayWarningEffect)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_Ability.Rpc_PlayWarningEffect() {
                    abilityModId = inData.ConfigId,
                    startPoint = inData.In_StartPoint,
                    startLookAt = inData.In_StartLookAt,
                    startScale = inData.In_StartScale * 10f,
                    fillTime = (ushort)Mathf.CeilToInt(inData.In_FillTime * 10f),
                    isCircleFill = inData.In_FillCircle,
                    lifeTime = (ushort)Mathf.CeilToInt(inData.In_LifeTime * 10f),
                    playTimestamp = DateTimeUtils.GetCurUTCTimestamp(),
                }
            );
        }

        protected override void AddComponents() {
            AddComponent<ServerAbilitySync>();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = inData.In_LifeTime,
                EndTimeCallBack = null,
            });
           
        }
    }
}