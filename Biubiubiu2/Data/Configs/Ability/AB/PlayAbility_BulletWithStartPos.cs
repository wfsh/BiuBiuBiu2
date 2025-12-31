using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_BulletWithStartPos : IAbilityMData {
            public const string ID = AbilityData.SAB_BulletWithStartPoint;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;
            // 模版数据
            public int M_ATK;
            public string M_EffectSign;
            public int M_Power;
            public string M_HitEffect;
            public byte M_HitSplatter;
            public List<WeaponData.BulletAttnMap> M_BulletAttnMap;

            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_TargetPoint;
            public float In_Speed;
            public float In_MoveDistance;
            public int In_WeaponItemId;
            public int In_RandomAtk;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_BulletWithStartPos)modMData;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
                M_HitEffect = data.M_HitEffect;
                M_BulletAttnMap = data.M_BulletAttnMap;
                M_ATK = data.M_ATK;
                M_HitSplatter = data.M_HitSplatter;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}