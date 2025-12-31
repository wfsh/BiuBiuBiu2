using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_MoveSpeedRateByTime : AbilityEffectMData {
        override public string GetEffectSign() => M_EffectSign;

        [Header("M_模板数据（服务端）")]
        public float M_InitSpeedRate;
        public float M_TargetSpeedRate;
        public float M_ReachTargetSpeedRateTime;
        public string M_EffectSign;
        public float M_LifeTime;
    }
    
    public class AbilityIn_MoveSpeedRateByTime : IAbilityInData {
        public float In_LifeTime;
        public float In_InitSpeedRate;
        public float In_TargetSpeedRate;
        public float In_ReachTargetSpeedRateTime;
    }
}