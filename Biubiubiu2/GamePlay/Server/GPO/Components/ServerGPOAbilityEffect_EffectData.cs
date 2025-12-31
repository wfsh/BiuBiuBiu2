using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerGPOAbilityEffect {
        public class EffectData : IGPOAbilityEffectData {
            private float value;
            private AbilityEffectData.Effect effect;
            private bool isRemove;
            public Action OnRemove;
            public Action OnValueChanged;
            private IGPO targetGPO;

            public void Init(AbilityEffectData.Effect effect, float value, IGPO targetGPO) {
                this.effect = effect;
                this.value = value;
                this.targetGPO = targetGPO;
            }

            public AbilityEffectData.Effect GetEffectType() {
                return effect;
            }

            public void Remove() {
                if (isRemove) {
                    return;
                }
                isRemove = true;
                OnRemove?.Invoke();
                targetGPO = null;
            }

            public void SetValue(float value) {
                this.value = value;
                OnValueChanged?.Invoke();
            }

            public float GetValue() {
                return value;
            }

            public bool IsRemove() {
                return isRemove;
            }

            public IGPO TargetGPO() {
                return targetGPO;
            }
        }
    }
}