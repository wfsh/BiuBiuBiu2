using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_DragonDelayBlastSpawner : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        [Header("M_模板数据（服务端）")]
        public string M_SceneType = "";
        public byte M_BossType = 0;
        public string M_EffectSign;
        public int M_AttackNum;
        public float M_CreateInterval = 0.5f; // 生成间隔
        public float M_MinDistance = 5f;
        public float M_MaxDistance = 16f;
        public float M_GroundY = 17.5f; // 地面高度
        public float M_Radius = 2f; // 球体伤害半径
        public float M_BallCreateHeight; // 球体生成高度
        public float M_BoomTime = 5f; // 过多久地面爆炸
        public float M_BoomRadius = 5f; // 爆炸伤害半径
        public float M_CreateTime = 3f; // 生成球的时间
        public float M_FallBallCheckTime = 0.3f; // 火球伤害间隔
        public ushort M_ATK = 5; // 伤害
        public float M_BoomAtkRatio = 10; // 爆炸伤害倍率（基于基础伤害）
        public float M_BoomWidth; // 远古伤害宽度
        public float M_BoomLong; // 远古伤害长度
        public float M_BoomHeight; // 远古伤害高度
    }
    
    public class AbilityIn_DragonDelayBlastSpawner: IAbilityInData {
        [Header("In_输入数据")]
        public string In_MonsterSign;
    }
}