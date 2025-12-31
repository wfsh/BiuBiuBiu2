using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_HurtGPOByTime : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public float M_LifeTime;
        public string M_EffectSign;
        public int M_Power;
        public float M_DeltaTime; // 每帧时间
    }

    public struct AbilityIn_HurtGPOByTime : IAbilityEffectInData {
        [Header("In_输入数据")]
        public int In_WeaponItemId;
    }
}