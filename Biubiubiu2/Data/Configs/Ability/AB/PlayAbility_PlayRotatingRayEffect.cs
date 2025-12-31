using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_PlayRotatingRayEffect : IAbilityMData {
            public const string ID = SAB_PlayRotatingRayEffect;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public string M_EffectSign;
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public float In_StartLength;
            public float In_StartDeg;
            public float In_LifeTime;
            public float In_MoveAngularSpeed;
            public bool In_EndWhenFireGPODead;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_PlayRotatingRayEffect)modMData;
                M_EffectSign = data.M_EffectSign;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}