using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayMovingEffect : AbilityMData {
        public override string GetEffectSign() => M_EffectSign;
        [Header("M_模板数据（服务端）")]
        public string M_Sign; // 模版标识
        public string M_EffectSign;
    }
    
    public class AbilityIn_PlayMovingEffect : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Vector3 In_StartLookAt;
        public Vector3 In_StartScale;
        public float In_LifeTime;
        public Vector3 In_MoveDir;
        public float In_MoveSpeed;
        public ushort In_AudioKey;
    }
}