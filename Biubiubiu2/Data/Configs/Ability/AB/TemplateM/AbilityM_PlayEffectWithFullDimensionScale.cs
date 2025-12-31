using System.Collections;
using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayEffectWithFullDimensionScale : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;
        public string M_Sign = "";
    }

    public class AbilityIn_PlayEffectWithFullDimensionScale : IAbilityInData {
        [Header("In_输入数据")]
        public Vector3 In_StartPoint;
        public Quaternion In_StartRota;
        public Vector3 In_StartScale;
        public float In_LifeTime;
        public ushort In_AudioKey;
    }
}