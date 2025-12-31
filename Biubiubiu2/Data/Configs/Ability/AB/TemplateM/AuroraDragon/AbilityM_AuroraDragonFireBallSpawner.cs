using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonFireBallSpawner : AbilityMData {
        public override string GetEffectSign() {
            return M_EffectSign;
        }
        public string M_SceneType = "";
        public byte M_BossType = 0;
        [Header("M_模板数据（服务端）")]
        public string M_EffectSign; // Boss类型
        [Header("升空动画时长")] 
        public float M_FireBallStartAnimTime = 3f; // 升空动画时长
        [Header("发射次数")] 
        public int M_FireBallAttackNum = 3; // 发射次数
        [Header("发射动画时长")] 
        public float M_FireBallAnimTime = 1f; // 发射动画时长
        [Header("结束动画时长")] 
        public float M_FireBallEndAnimTime = 2f; // 结束动画时长
    }
    public class AbilityIn_AuroraDragonFireBallSpawner : IAbilityInData {
        [Header("In_输入数据")]
        // 下面写你的参数
        public ushort In_BossType;
    }
}