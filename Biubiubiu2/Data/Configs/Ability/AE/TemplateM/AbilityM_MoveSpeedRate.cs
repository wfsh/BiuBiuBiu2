using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_MoveSpeedRate : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public float M_SpeedRate;
        public string M_EffectSign;
        public float M_LifeTime;
    }

    public struct AbilityIn_MoveSpeedRate : IAbilityEffectInData {
        [Header("In_输入数据")]
        public float In_SpeedRate;
        public float In_LifeTime;
    }
}