using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public class PlayAbility_RangeFlash : IAbilityMData {
            public const string ID = AbilityData.SAB_RangeFlash;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;

            public string GetEffectSign() => M_EffectSign;

            // 模版数据
            public string M_EffectSign;
            public float M_LifeTime;

            public float In_DelayCheckTime;
            public float In_Range;
            // 下面写你的参数
            public int M_Power;
            public float In_Speed;
            public int In_WeaponId;
            public Vector3 In_StartPoint;
            public int In_IngoreGPOId;


            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }

                var data = (PlayAbility_RangeFlash)modMData;
                M_EffectSign = data.M_EffectSign;
                M_LifeTime = data.M_LifeTime;
                In_DelayCheckTime = data.In_DelayCheckTime;
                In_Range = data.In_Range;//这个估计不可以
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}