using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonDragonCar : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        public string M_SceneType = "";
        public byte M_BossType = 0;
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;

        [Header("冲撞最远距离")]
        public float M_DisMax = 40f; // 冲撞最远距离

        [Header("移动不能超过战斗场地边缘距离")]
        public float M_RangeDis = 2f; // 移动不能超过战斗场地边缘距离

        [Header("蓄力时间")]
        public float M_ChargeTime = 2.667f; // 蓄力时间

        [Header("蓄力追踪目标转向时间")]
        public float M_ChargeTrackTime = 2f; // 蓄力追踪目标转向时间

        [Header("蓄力追踪目标转向速度")]
        public float M_ChargeTrackSpeed = 60f; // 蓄力追踪目标转向速度

        [Header("冲撞移动速度")]
        public float M_RushSpeed = 50f; // 冲撞移动速度

        [Header("冲撞持续时间")]
        public float M_RushTime = 0.8f; // 冲撞持续时间

        [Header("冲撞身体特效坐标")]
        public Vector3 M_RushEffectPos = new Vector3(-3f, 0f, 0f);

        [Header("冲撞身体特效缩放")]
        public Vector3 M_RushEffectScale = new Vector3(1.5f, 1.5f, 1.5f);

        [Header("冲撞伤害范围")]
        public float M_RushDamageRadius = 4.5f; // 冲撞伤害范围

        [Header("冲撞和下落撞击伤害")]
        public ushort M_ATK = 5; // 冲撞和下落撞击伤害

        [Header("冲撞完成后火焰持续时间")]
        public float M_FlameTime = 2f; // 冲撞完成后火焰持续时间

        [Header("火焰伤害判断间隔")]
        public float M_FlameDamageInterval = 0.3f; // 火焰伤害判断间隔

        [Header("火焰伤害高度")]
        public float M_FlameDamageHeight = 0.3f; // 火焰伤害高度

        [Header("火焰伤害")]
        public ushort M_FlameDamage = 5; // 火焰伤害

        [Header("下落撞击伤害时间")]
        public float M_DownDamageTime = 1.4f; // 下落撞击伤害时间

        [Header("下落撞击伤害范围")]
        public float M_DownDamageRadius = 10f; // 下落撞击伤害范围

        [Header("下落动画时长")]
        public float M_DownTime = 2f; // 下落动画时长

        [Header("结束动作时间")]
        public float M_WaitTime = 3.667f; // 结束动作时间
    }
    
    public class AbilityIn_AuroraDragonDragonCar : IAbilityInData {
        [Header("In_输入数据")]
        public ushort In_BossType;
    }
}