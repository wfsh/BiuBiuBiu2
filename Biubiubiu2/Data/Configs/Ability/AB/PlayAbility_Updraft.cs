using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_Updraft : IAbilityMData {
            public const string ID = AbilityData.SAB_Updraft;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public string M_EffectSign;
            public float M_LifeTime;
            public float M_RangeXZ;
            public float M_RangeY;
            public float M_Power;
            // 下面写你的参数
            public Vector3 In_StartPoint;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_Updraft)modMData;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
                M_RangeXZ = data.M_RangeXZ;
                M_RangeY = data.M_RangeY;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}