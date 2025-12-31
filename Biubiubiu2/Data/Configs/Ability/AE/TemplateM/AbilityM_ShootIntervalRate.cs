using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_ShootIntervalRate : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public float M_ShootIntervalRate;
        public string M_EffectSign;
        public float M_LifeTime;
    }
    
    public class AbilityIn_ShootIntervalRate : IAbilityInData {
        public float In_LifeTime;
        public float In_ShootIntervalRate;
    }
}