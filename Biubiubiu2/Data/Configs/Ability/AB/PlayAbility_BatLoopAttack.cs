using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        /// <summary>
        /// 以攻击者为中心。范围内的角色在攻击持续时间内不断受伤。
        /// </summary>
        public struct PlayAbility_BatLoopAttack : IAbilityMData {
            public const string ID = AbilityData.SAB_BatLoopAttack;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public int M_Power;
            public float M_LifeTime; // 保底时间，正常的消亡走技能结束
            public string M_EffectSign;
            
            // 下面写你的参数

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_BatLoopAttack)modMData;
                M_Power = data.M_Power;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}