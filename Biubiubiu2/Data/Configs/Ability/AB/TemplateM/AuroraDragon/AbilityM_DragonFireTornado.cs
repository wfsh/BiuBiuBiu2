using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_DragonFireTornado : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        public string M_SceneType = "";
        public byte M_BossType = 0;
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;

        [Header("地火生成轮数")]
        public int M_SpawnCout; // 地火生成轮数

        [Header("地火数量")]
        public int M_Count; // 地火数量

        [Header("地火间隔")]
        public float M_Interval; // 地火间隔

        [Header("地火半径")]
        public float M_Radius; // 地火半径

        [Header("地火高度")]
        public float M_Height; // 地火高度

        [Header("boss 前摇时间")]
        public float M_PreTime; // 地火前摇时间

        [Header("地火预警时间")]
        public float M_WarmTime; // 地火预热时间

        [Header("地火攻击特效播放时间")]
        public float M_AttackEffecPlayTime; // 地火攻击特效播放时间

        [Header("地火结束时间")]
        public float M_EndTime; // 地火结束时间

        [Header("地火持续时间")]
        public float M_LifeTime; // 地火持续时间

        [Header("地火攻击间隔")]
        public float M_AttackSpaceTime; // 地火攻击间隔

        [Header("地火伤害")]
        public int M_ATK; // 地火伤害

        [Header("地火攻击检测间隔")]
        public float M_AttackCheckTime; // 地火攻击检测时间
    }

    public class AbilityIn_DragonFireTornado : IAbilityInData {
        [Header("In_输入数据")]
        public ushort In_BossType;
    }
}