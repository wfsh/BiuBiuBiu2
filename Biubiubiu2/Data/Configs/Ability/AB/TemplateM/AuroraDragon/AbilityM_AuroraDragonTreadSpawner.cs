using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonTreadSpawner : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }

        public string M_SceneType = "";
        public byte M_BossType = 0;
        
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;

        [Header("踩踏次数")]
        public int M_TreadAttackNum = 2; // 踩踏次数

        [Header("踩踏攻击时长")]
        public float M_TreadAttackTime = 3.7f; // 踩踏攻击时长

        [Header("踩踏喷火攻击时长")]
        public float M_TreadFireAttacKTime = 4.7f; // 踩踏喷火攻击时长
    }

    public class AbilityIn_AuroraDragonTreadSpawner : IAbilityInData {
        [Header("In_输入数据")]
        public int In_SOConfigId;
        public ushort In_BossType;
    }
}