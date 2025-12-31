using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayWWiseAudio : AbilityMData {
        public override string GetEffectSign() => "";
        // [Header("D_固定数据")]
    }

    public class AbilityIn_PlayWWiseAudio : IAbilityInData {
        [Header("In_输入数据")]
        public ushort In_WWiseId;
        public Vector3 In_StartPoint;
        public float In_LifeTime; // 如果是 0 则使用 M_LifeTime 数据
        public bool In_IsFollow; // 是否跟随释放者
    }
}