using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonTreadFire : AbilityMData {
        public string M_SceneType = "";
        [Header("Boss类型")]
        public int M_BossType; // Boss类型

        [Header("预警特效时长")]
        public float M_WarringEffectTime = 2f;

        [Header("预警特效宽度")]
        public float M_WarringEffectWidth = 38f;

        [Header("过多久进行伤害判断")]
        public float M_CheckDamageTime = 2f; // 过多久进行伤害判断

        [Header("伤害间隔")]
        public float M_DamageInterval = 0.3f; // 伤害间隔

        [Header("伤害时间")]
        public float M_DamageTime = 1.3f; // 伤害时间

        [Header("伤害高度")]
        public float M_AttackHeight = 5f; // 伤害高度

        [Header("攻击偏移距离")]
        public float M_AttackFixDis = 5f; // 攻击偏移距离

        [Header("最远伤害距离")]
        public float M_AttackRadius = 10f; // 最远伤害距离

        [Header("伤害角度")]
        public float M_AttackAngle = 15f; // 伤害角度

        [Header("火焰初始旋转")]
        public float M_AttackRotationStart = 40f; // 火焰初始旋转

        [Header("火焰旋转速度")]
        public float M_AttackRotationSpeed = 58f; // 火焰旋转速度

        [Header("伤害")]
        public ushort M_ATK = 5; // 伤害

        [Header("总时长")]
        public float M_LifeTime = 4.5f; // 必须大于 CheckDamageTime，不然会没有伤害
    }

    public class AbilityIn_AuroraDragonTreadFire : IAbilityInData {
        // [Header("In_输入数据")]
    }
}