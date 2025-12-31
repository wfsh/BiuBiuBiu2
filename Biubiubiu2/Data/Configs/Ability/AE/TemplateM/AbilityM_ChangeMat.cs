using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_ChangeMat : AbilityEffectMData {
        override public string GetEffectSign() => "";
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
    }
    public struct AbilityIn_ChangeMat : IAbilityEffectInData {
        [Header("In_输入数据")]
        public byte In_LifeTime;
        public byte In_MatType;
    }
}