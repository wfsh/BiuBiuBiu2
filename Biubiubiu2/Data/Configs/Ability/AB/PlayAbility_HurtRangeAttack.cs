using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_HurtRangeAttack : IAbilityMData {
            public const string ID = AbilityData.SAB_HurtRangeAttack;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public int M_Range;
            public string M_EffectSign;
            public float M_LifeTime;
            public int M_Power;
            public byte M_PlayEffectAbility;
            // 下面写你的参数
            public Vector3 In_FirePoint;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_HurtRangeAttack)modMData;
                M_Range = data.M_Range;
                M_EffectSign = data.M_EffectSign;
                M_LifeTime = data.M_LifeTime;
                M_Power = data.M_Power;
                M_PlayEffectAbility = data.M_PlayEffectAbility;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}