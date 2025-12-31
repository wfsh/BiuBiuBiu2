using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_HurtValueRate : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public float M_HurtValueRate;
        public string M_EffectSign;
        public float M_LifeTime;
    }

    public struct AbilityIn_HurtValueRate : IAbilityEffectInData {
        [Header("In_输入数据")]
        public float In_HurtValueRate;
        public float In_LifeTime;
    }
}