using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_ExpandBoom : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        // 模版数据
        public string M_Sign;
        public string M_EffectSign;
    }

    public class AbilityIn_ExpandBoom : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public float In_CheckHight;
        public float In_ExpandSpeed;
        public float In_MaxDistance;
        public bool In_CheckBlock;
        public int In_ATK;
        public float In_LifeTime;
    }
}