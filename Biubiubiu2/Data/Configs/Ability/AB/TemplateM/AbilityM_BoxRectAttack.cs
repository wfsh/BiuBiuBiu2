using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_BoxRectAttack : AbilityMData {
        [Header("M_模板数据")]
        public int M_ItemId;
        public int M_HitAbilityEffect;
        public float M_LifeTime;
        public float M_AttackDelay;
        public int M_PlayWwiseId;
        public int M_HitWwiseId;
        public Vector3 M_HalfExtend;
    }
    public class AbilityIn_BoxRectAttack : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Quaternion In_StartRotation;
        public int In_Atk;
        public float In_AttackRange;
    }
}