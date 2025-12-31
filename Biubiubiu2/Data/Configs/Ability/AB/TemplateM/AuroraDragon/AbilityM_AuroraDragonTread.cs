using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonTread : AbilityMData {
        public string M_SceneType = "";
        public byte M_BossType = 0;

        [Header("左脚坐标")]
        public Vector3 M_FootLeftLocalPos = new Vector3(-1.5f, 0, 3.5f);

        [Header("右脚坐标")]
        public Vector3 M_FootRightLocalPos = new Vector3(0.8f, 0, 3.5f);

        [Header("伤害判断最大高度")]
        public float M_AttackHeight = 2f; // 伤害判断最大高度

        [Header("爆炸伤害半径")]
        public float M_BoomRadius = 6f; // 爆炸伤害半径

        [Header("爆炸攻击特效缩放")]
        public Vector3 M_BoomEffectScale = new Vector3(1.5f, 1.5f, 1.5f);

        [Header("扇形伤害半径")]
        public float M_AttackRadius = 15f; // 扇形伤害半径

        [Header("扇形攻击角度")]
        public float M_AttackAngle = 60f; // 扇形攻击角度

        [Header("扇形攻击特效缩放")]
        public Vector3 M_AttackEffectScale = new Vector3(1.5f, 1.5f, 1.5f);

        [Header("过多久进行伤害判断")]
        public float M_CheckDamageTime = 2f; // 过多久进行伤害判断

        [Header("总时长")]
        public float M_LifeTime = 2.2f; // 必须大于 CheckDamageTime，不然会没有伤害

        [Header("伤害")]
        public ushort M_ATK = 5; // 伤害

    }
    public class AbilityIn_AuroraDragonTread : IAbilityInData {
        [Header("In_输入数据")]
        public bool In_IsLeftFoot;
    }
}