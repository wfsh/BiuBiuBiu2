using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayHollowCircleWarningEffect : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign; // 模版标识
        public string M_EffectSign;
    }
    
    public class AbilityIn_PlayHollowCircleWarningEffect : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public float In_LifeTime;
        public float In_MaxDistance;
        public float In_AttackOffset;
    }
}
