using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_SplitCone : IAbilityMData {
            public const string ID = AbilityData.SAB_SplitCone;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;

            public string GetEffectSign() => M_EffectSign;

            // 模版数据
            public string M_EffectSign;
            public string M_AudioSign;
            public List<Vector2> M_BulletSpreadPoints;
            public float M_ConeAngle;

            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_ForwardAngle;
            public float In_MoveDistance;
            public float In_Speed;
            public int In_WeaponItemId;
            public float In_DiffusionReductionAngle;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_SplitCone)modMData;
                M_EffectSign = data.M_EffectSign;
                M_AudioSign = data.M_AudioSign;
                M_BulletSpreadPoints = data.M_BulletSpreadPoints;
                M_ConeAngle = data.M_ConeAngle;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}