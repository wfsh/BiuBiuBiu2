using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayBloodSplatter : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public float M_LifeTime;
        public string M_EffectSign;
    }
    public class AbilityIn_PlayBloodSplatter : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_HitPoint;
        public int In_BloodValue;
        public int In_HitGpoId;
        public int In_HitItemId;
        public Vector3 In_DiffPos;
    }
}