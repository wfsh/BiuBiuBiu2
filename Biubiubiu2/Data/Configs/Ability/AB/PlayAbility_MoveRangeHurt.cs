using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        // public struct PlayAbility_MoveRangeHurt : IAbilityMData {
        //     public const string ID = SAB_MoveRangeHurt;
        //     public string GetTypeID() => ID;
        //     public byte GetRowID() => 0;
        //
        //     public ushort ConfigId;
        //     public ushort GetConfigId() => ConfigId;
        //     public string GetEffectSign() => "";
        //     // 模版数据
        //     public string M_EffectSign;
        //     // 下面写你的参数
        //     public Vector3 In_StartPoint;
        //     public Vector3 In_StartDir;
        //     public float In_MoveSpeed;
        //     public float In_Rangle;
        //     public float In_MaxDistance;
        //     public int In_ATK;
        //     public float In_LifeTime;
        //
        //     public void SetConfigData(IAbilityMData modMData) {
        //         if (modMData.GetTypeID() != ID) {
        //             Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
        //             return;
        //         }
        //         var data = (PlayAbility_MoveRangeHurt)modMData;
        //         M_EffectSign = data.M_EffectSign;
        //     }
        //     
        //     public void Select(Action callBack) {
        //         callBack?.Invoke();
        //     }
        // }
    }
}