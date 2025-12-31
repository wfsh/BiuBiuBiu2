using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AncientDragonFlyFlame : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }

        public string M_SceneType = "";
        public byte M_BossType = 0;

        [Header("M_模板数据（服务端）")]
        public string M_EffectSign;

        [Header("飞行最远距离")]
        public float M_FlyDisMax = 40f; // 飞行最远距离

        [Header("移动不能超过战斗场地边缘距离")]
        public float M_RangeDis = 2f; // 移动不能超过战斗场地边缘距离

        [Header("起飞动作时间")]
        public float M_FlyUpTime = 2f;

        [Header("飞行移动速度")]
        public float M_FlySpeed = 50f;

        [Header("飞行动作时间")]
        public float M_FlyTime = 0.8f;

        [Header("喷火特效坐标偏移")]
        public Vector3 M_EffectPos = new Vector3(0.5f, 0f, 0f);

        [Header("降落动作时间")]
        public float M_FlyDownTime = 2f;

        [Header("火焰伤害范围")]
        public float M_DamageRadius = 4.5f;

        [Header("飞行完成后火焰持续时间")]
        public float M_FlameTime = 2f;

        [Header("火焰伤害判断间隔")]
        public float M_DamageInterval = 0.3f;

        [Header("火焰伤害高度")]
        public float M_DamageHeight = 0.3f;

        [Header("伤害")]
        public ushort M_ATK = 5; // 伤害
    }
    
    public class AbilityIn_AncientDragonFlyFlame : IAbilityInData {
        [Header("In_输入数据")]
        // 下面写你的参数
        public int In_SOConfigId;
        public Vector3 In_StartPos;
        public float In_StartHeight;
        public float In_DamageRadius;
        public float In_DropSpeed;
        public ushort In_HitGroundEffectId;
        public ushort In_SourceAbilityConfigId;
        public ushort In_ATK;
    }
}