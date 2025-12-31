using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_BellowAttack : AbilityMData {
        [Header("M_模板数据")]
        public string M_Sign;
        public float M_LifeTime;
        public int M_Range;
        public float M_AttackDelayTime;
        public int M_Power;
        public byte M_PlayEffectAbility;
    }

    public class AbilityIn_BellowAttack : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
    }
}