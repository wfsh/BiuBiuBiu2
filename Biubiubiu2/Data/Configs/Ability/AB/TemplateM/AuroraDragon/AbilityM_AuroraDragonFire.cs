using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonFire : AbilityMData {
        public string M_SceneType = "";
        public byte M_BossType = 0;

        [Header("蓄力动作时间")]
        public float M_ChargeTime = 2f; // 蓄力动作时间

        [Header("攻击动作时间")]
        public float M_AttackTime = 6f; // 攻击动作时间

        [Header("攻击时移动速度")]
        public float M_AttackMoveSpeed = 2f; // 攻击时移动速度

        [Header("攻击时转向速度")]
        public float M_AttackRotationSpeed = 20f; // 攻击时转向速度

        [Header("移动不能超过战斗场地边缘距离")]
        public float M_MoveRangeDis = 8f; // 移动不能超过战斗场地边缘距离

        [Header("延迟创建激光特效时间")]
        public float M_CreateRayEffectTime = 0.2f; // 延迟创建激光特效时间

        [Header("攻击结束动作时间")]
        public float M_EndRayEffectTime = 0.2f; // 提前结束激光特效时间（攻击结束动作时间）

        [Header("激光特效坐标偏移")]
        public Vector3 M_EffectPos = new Vector3(0f, -0.9f, 0f);

        [Header("激光特效旋转角度")]
        public Vector3 M_EffectRota = new Vector3(90f, 0f, 0f);

        [Header("激光特效缩放")]
        public Vector3 M_EffectScale = new Vector3(1f, 1f, 1f);

        [Header("伤害判断宽度")]
        public float M_DamageRadius = 1f; // 伤害判断宽度

        [Header("伤害判定长度")]
        public float M_DamageLength = 25f; // 伤害判定长度

        [Header("伤害判断坐标偏移")]
        public Vector3 M_DamageFixPos = new Vector3(0f, -0.9f, 1f);

        [Header("伤害判定向后增加长度")]
        public float M_DamageFixLength = 5f; // 用于覆盖呆呆龙嘴下的空间

        [Header("伤害判定高度")]
        public float M_DamageHeight = 3.5f; // 伤害判定高度

        [Header("伤害判断间隔")]
        public float M_DamageInterval = 0.3f; // 伤害判断间隔

        [Header("结束动作时间")]
        public float M_WaitTime = 1f; // 结束动作时间

        [Header("伤害")]
        public ushort M_ATK = 5; // 伤害
    }
    
    public class AbilityIn_AuroraDragonFire: IAbilityInData {
        [Header("In_输入数据")]
        public ushort In_BossType;
    }
}