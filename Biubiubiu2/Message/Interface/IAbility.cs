using System;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public interface IGPOAbilityEffectData {
        void Remove();
        void SetValue(float value);
        AbilityEffectData.Effect GetEffectType();
        float GetValue();
        bool IsRemove();
        IGPO TargetGPO();
    }
}