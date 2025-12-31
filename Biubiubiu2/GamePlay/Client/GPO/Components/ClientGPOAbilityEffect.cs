using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOAbilityEffect : ComponentBase {
        const float TOLERANCE = 0.01f; // 设置一个合理的误差范围
        private Dictionary<AbilityEffectData.Effect, float> effectData = new Dictionary<AbilityEffectData.Effect, float>();

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Ability.TargetRpc_AbilityEffect.ID, OnTargetRpcAbilityEffectCallBack);
            RemoveProtoCallBack(Proto_Ability.Rpc_AbilityEffect.ID, OnRpcAbilityEffectCallBack);
            effectData.Clear();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Ability.TargetRpc_AbilityEffect.ID, OnTargetRpcAbilityEffectCallBack);
            AddProtoCallBack(Proto_Ability.Rpc_AbilityEffect.ID, OnRpcAbilityEffectCallBack);
        }

        private void OnTargetRpcAbilityEffectCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Ability.TargetRpc_AbilityEffect)proto;
            SetEffectData((AbilityEffectData.Effect)data.abilityEffect, data.value);
        }

        private void OnRpcAbilityEffectCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Ability.Rpc_AbilityEffect)proto;
            SetEffectData((AbilityEffectData.Effect)data.abilityEffect, data.value);
        }

        private void SetEffectData(AbilityEffectData.Effect effect, float value) {
            var oldValue = 0f;
            if (effectData.TryGetValue(effect, out oldValue) == false) {
                effectData[effect] = value;
            }
            if (Mathf.Abs(oldValue - value) > TOLERANCE) {
                effectData[effect] = value;
                mySystem.Dispatcher(new CE_AbilityEffect.Event_UpdateEffect() {
                    Effect = effect, Value = value,
                });
            }
        }
    }
}