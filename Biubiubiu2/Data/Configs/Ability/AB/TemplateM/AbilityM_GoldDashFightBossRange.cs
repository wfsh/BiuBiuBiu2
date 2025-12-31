using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_GoldDashFightBossRange : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        [Header("M_模板数据（服务端）")]
        public string M_Sign;
        public string M_EffectSign;
        public string M_ServerEffectSign;
        public float M_FightRangeRadius;
        public float M_FightRangeHeight;
        public float M_FightRangeCenterForwardOffset;
        public float M_RemoveTimeAfterDead;
        public float M_RemoveTimeNoTarget;// 全部方式重置时间  10
        public bool M_TriggerInfinitePackBullet;
        public bool M_IsCheckBossFailForGpoEmpty; // 检测范围内没有目标时，是否判定为BOSS失败
        public string M_FightBGMSign;
        public string[] M_SceneSign;
        public float[] M_FactorValue;
        public float[] M_RoleAddHpFactorValue; // 角色添加的血量系数
    }
    
    public class AbilityIn_GoldDashFightBossRange : IAbilityInData {
        public Vector3 In_FightRangeCenterPoint;
    }
}