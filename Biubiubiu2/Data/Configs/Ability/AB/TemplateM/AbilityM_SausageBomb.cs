using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_SausageBomb : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
        public bool M_IsStrikeFly;
    }

    public class AbilityIn_SausageBomb : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public int In_Hurt;
        public float In_Range;
        public IAbilityEffectMData MData;
        public IAbilityEffectInData InData;
    }
}