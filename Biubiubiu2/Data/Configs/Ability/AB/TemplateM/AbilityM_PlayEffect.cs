using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayEffect : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign; // 模版标识
        public float M_LifeTime;
        public string M_EffectSign;
        public string M_AudioSign;
    }
    
    public class AbilityIn_PlayEffect : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Quaternion In_StartRota;
        public ushort In_AudioKey;
        public float In_LifeTime;
    }
}