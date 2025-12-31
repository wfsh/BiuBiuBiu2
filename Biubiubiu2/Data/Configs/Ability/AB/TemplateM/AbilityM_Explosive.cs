using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_Explosive : AbilityMData {
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public float M_LifeTime;
        public bool M_IsHurtSelf;
        public bool M_IsStrikeFly;
        public byte M_PlayEffectAbility;
        public float M_MaxDistanceHurtRatio;
        public bool M_IsRayRange;
        public bool M_IsUseWeaponHurtRatio;
    }
    
    public class AbilityIn_Explosive : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public int In_Hurt;
        public float In_Range;
        public int In_WeaponId;
    }
}