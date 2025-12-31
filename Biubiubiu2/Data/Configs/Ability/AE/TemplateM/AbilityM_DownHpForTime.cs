using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_DownHpForTime : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
    }

    public struct AbilityIn_DownHpForTime : IAbilityEffectInData {
        [Header("In_输入数据")]
        public float In_DownHpValue;
        public float In_LifeTime;
        public float In_DownHpSpace;
    }
}