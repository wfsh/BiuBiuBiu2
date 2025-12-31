using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_Missile : IAbilityMData {
            public const string ID = AbilityData.SAB_Missile;
            public ushort ConfigId;
            // 模版数据
            public string M_EffectSign;
            public float M_Speed;
            public float M_AreaRadius; // 轰炸范围
            public float M_BombSpawnHeight; // 导弹初始的高度
            public float M_BombingDuration; // 持续轰炸时间
            public float M_DelayBombingDuration; // 导弹延迟 spawn 时间
            public float M_VehicleHurtRatio; // 载具伤害系数

            // 下面写你的参数
            public Vector3[] In_Points;
            public int In_SkinItemId;
            public int In_ATK;
            public float In_BombingInterval; // 持续轰炸间隔
            public float In_RandDuration;    // 额外持续轰炸时间
            public float In_AttackRange;     // 导弹伤害范围
            public float In_FinalAreaRadius; // 最终轰炸范围

            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_Missile)modMData;
                M_Speed = data.M_Speed;
                M_EffectSign = data.M_EffectSign;
                M_AreaRadius = data.M_AreaRadius;
                M_BombSpawnHeight = data.M_BombSpawnHeight;
                M_BombingDuration = data.M_BombingDuration;
                M_DelayBombingDuration = data.M_DelayBombingDuration;
                M_VehicleHurtRatio = data.M_VehicleHurtRatio;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}