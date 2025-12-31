using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_MissileBomb : IAbilityMData {
            public const string ID = AbilityData.SAB_MissileBomb;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public bool M_IsHitBome;
            public string M_EffectSign;
            public float M_Speed;
            public float M_LifeTime;
            public int M_Power;
            // 下面写你的参数
            public Vector3[] In_Points;
            public int In_WeaponItemId;
            public float In_AttackRangeRange;
            public int In_ATK;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_MissileBomb)modMData;
                M_Speed = data.M_Speed;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
                M_IsHitBome = data.M_IsHitBome;
                M_Power = data.M_Power;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}