using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

using System;
namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_PlayWarningEffect : IAbilityMData {
            public const string ID = SAB_PlayWarningEffect;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public string M_EffectSign;
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_StartLookAt;
            public Vector3 In_StartScale;
            public float In_Angle;
            public float In_FillTime;
            public bool In_FillCircle;
            public float In_LifeTime;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_PlayWarningEffect)modMData;
                M_EffectSign = data.M_EffectSign;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}
