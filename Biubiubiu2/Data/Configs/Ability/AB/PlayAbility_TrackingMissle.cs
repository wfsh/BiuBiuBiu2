using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_TrackingMissle : IAbilityMData {
            public const string ID = AbilityData.SAB_TrackingMissle;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;

            public string GetEffectSign() => M_EffectSign;

            // 模版数据
            public string M_EffectSign;
            public int M_Power;
            public byte M_HitAbility;

            public string M_HitEffect;
            public float M_TrackSpeed;
            public float M_LockSpeed;
            public float M_MoveDistance;
            public float M_StopTrackDistance;
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_TargetPoint;

            public int In_WeaponItemId;
            public int In_TargetGPOId;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }

                var data = (PlayAbility_TrackingMissle)modMData;
                M_EffectSign = data.M_EffectSign;
                M_Power = data.M_Power;
                M_TrackSpeed = data.M_TrackSpeed;
                M_LockSpeed = data.M_LockSpeed;
                M_MoveDistance = data.M_MoveDistance;
                M_StopTrackDistance = data.M_StopTrackDistance;
                M_HitAbility = data.M_HitAbility;
                M_HitEffect = data.M_HitEffect;
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }   
    }
}