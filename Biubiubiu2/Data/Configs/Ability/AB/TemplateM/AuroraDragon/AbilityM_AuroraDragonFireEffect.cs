using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonFireEffect : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        [Header("M_模板数据（服务端）")]
        public string M_Sign = "";
        public byte M_BossType = 0;
        public string M_EffectSign;
    }
    public class AbilityIn_AuroraDragonFireEffect : IAbilityInData {
        [Header("In_输入数据")]
        public Transform In_AttackBoxTran;
        public float In_LifeTime;
        public bool In_IsUpdate;
        public bool In_IsFollowHead;
        public Vector3 In_StartPos;
        public Quaternion In_StartRota;
        public Vector3 In_StartScale;
    }
}