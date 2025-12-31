using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_Bullet : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
        public int M_Power;
        public string M_HitEffect;
        public byte M_Explosive;
        public byte M_HitSplatter;
    }
    public class AbilityIn_Bullet : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Vector3 In_TargetPoint;
        public float In_Speed;
        public float In_MoveDistance;
        public int In_WeaponItemId;
        public List<WeaponData.BulletAttnMap> In_BulletAttnMap;
    }
}