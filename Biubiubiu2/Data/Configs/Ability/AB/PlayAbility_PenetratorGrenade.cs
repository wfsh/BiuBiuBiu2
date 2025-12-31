using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_PenetratorGrenade : IAbilityMData {
            public const string ID = AbilityData.SAB_PenetratorGrenade;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public float M_Speed;
            // 子弹总生命周期
            public float M_LifeTime;
            // 击中怪物后的倒计时
            public float M_BombTime;
            // 特效
            public string M_EffectSign;
            // 爆炸威力
            public int M_Power;
            // 爆炸范围
            public int M_BombRange;
            
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Quaternion In_StartRota;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_PenetratorGrenade)modMData;
                M_Speed = data.M_Speed;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
                M_BombTime = data.M_BombTime;
                M_Power = data.M_Power;
                M_BombRange = data.M_BombRange;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}