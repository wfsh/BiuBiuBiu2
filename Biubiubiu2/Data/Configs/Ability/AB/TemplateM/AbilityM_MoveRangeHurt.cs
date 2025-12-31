using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_MoveRangeHurt : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
    }

    public class AbilityIn_MoveRangeHurt : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Vector3 In_StartDir;
        public float In_MoveSpeed;
        public float In_Rangle;
        public float In_MaxDistance;
        public int In_ATK;
        public float In_LifeTime;
    }
}