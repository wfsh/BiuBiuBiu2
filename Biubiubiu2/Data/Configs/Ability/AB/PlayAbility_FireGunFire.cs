using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public class PlayAbility_FireGunFire : IAbilityMData {
            public const string ID = AbilityData.SAB_FireGunFire;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;

            public string GetEffectSign() => M_EffectSign;

            // 模版数据
            public string M_EffectSign;

            // 下面写你的参数
            public float M_Range;
            public float M_InitRange;
            public float M_Radius;
            public int M_Power;
            public byte M_HitAbilityEffect;

            public string M_HitEffect;

            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_TargetPoint;
            public float In_Speed;
            public float In_MoveDistance;
            public int In_WeaponItemId;
            public List<WeaponData.BulletAttnMap> In_BulletAttnMap;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }

                var data = (PlayAbility_FireGunFire)modMData;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
                M_HitAbilityEffect = data.M_HitAbilityEffect;
                M_HitEffect = data.M_HitEffect;
                M_Radius = data.M_Radius;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}