using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayRay : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
        public bool M_IsFixScale;
        public string M_HitEffect;
    }

    public class AbilityIn_PlayRay : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;

        public Vector3 In_Dir;
        public int In_MaxDistance;
        public int In_RayATK;
        public float In_RayATKInterval;
        public float In_LifeTime;
        public bool In_IsFollowFireGPO;
    }
}