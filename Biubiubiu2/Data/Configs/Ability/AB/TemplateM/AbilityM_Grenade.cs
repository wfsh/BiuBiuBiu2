using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_Grenade : AbilityMData {
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public bool M_IsHitBomb;
        public string M_EffectSign;
        public float M_Speed;
        public float M_LifeTime;
        public int M_Power;
    }
    public class AbilityIn_Grenade : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3[] In_Points;
        public int In_WeaponItemId;
    }
}