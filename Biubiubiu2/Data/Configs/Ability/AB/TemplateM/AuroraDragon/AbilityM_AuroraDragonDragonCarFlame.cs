using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonDragonCarFlame : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        [Header("M_模板数据（服务端）")]
        public byte M_BossType = 0;
        public string M_EffectSign;
    }
    
    public class AbilityIn_AuroraDragonDragonCarFlame : IAbilityInData {
        [Header("In_输入数据")]
        // 下面写你的参数
        public Vector3 In_StartPos;
        public Vector3 In_EndPos;
        public float In_Speed;
        public float In_DamageRadius;
        public float In_DamageInterval;
        public float In_DamageHeight;
        public ushort In_Damage;
        public float In_LifeTime;
    }
}