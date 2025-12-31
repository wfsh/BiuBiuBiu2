using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_RPG : IAbilityMData {
            public const string ID = AbilityData.SAB_RPG;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public float M_Speed;
            public float M_LifeTime;
            public string M_EffectSign;
            public int M_Power;
            public int M_AbilityExplosive;
            
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_TargetPoint;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_RPG)modMData;
                M_Speed = data.M_Speed;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
                M_AbilityExplosive = data.M_AbilityExplosive;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}