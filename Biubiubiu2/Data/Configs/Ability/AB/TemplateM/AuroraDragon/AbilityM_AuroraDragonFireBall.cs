using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonFireBall : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }

        public string M_SceneType = "";
        public byte M_BossType = 0;
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;

        [Header("过多久显示地面爆炸特效")]
        public float M_CreateHitEffectTime = 1.5f; // 过多久显示地面爆炸特效

        [Header("伤害半径")]
        public float M_Radius = 2f; // 伤害半径

        [Header("过多久进行伤害判断")]
        public float M_CheckDamageTime = 2f; // 过多久进行伤害判断（和显示爆炸特效间隔0.5）

        [Header("总时长")]
        public float M_LifeTime = 2.2f; // 必须大于 CheckDamageTime，不然会没有伤害

        [Header("伤害")]
        public ushort M_ATK = 5; // 伤害
    }

    public class AbilityIn_AuroraDragonFireBall : IAbilityInData {
        [Header("In_输入数据")]
        // 下面写你的参数
        public Vector3 In_StartPos;
        public Vector3 In_EndPos;
    }
}