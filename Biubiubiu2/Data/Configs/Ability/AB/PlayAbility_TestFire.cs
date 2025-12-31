using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_TestFire : IAbilityMData {
            public const string ID = AbilityData.SAB_TestFire;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public float M_DeltaTime; // 每帧时间
            public string M_EffectSign;
            public int M_Power;
            // 下面写你的参数
            public Vector3 In_StartPoint;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_TestFire)modMData;
                M_DeltaTime = data.M_DeltaTime;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}