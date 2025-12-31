using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_ReloadRate : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;
        public float M_ReloadRate;
        public float M_LifeTime;
    }

    public struct AbilityIn_ReloadRate : IAbilityEffectInData {
        [Header("In_输入数据")]
        public float In_ReloadRate;
        public float In_LifeTime;
    }
}