using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_SausageBullet : IAbilityMData {
            public struct BulletSpeedAndGravity {
                public float gravity;
                public float bulletSpeed;
                public float flyTime;
            }

            public const string ID = AbilityData.SAB_SausageBullet;
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;
            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => "";
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public float In_SpeedRatio;
            public Quaternion In_StartRota;
            public BulletSpeedAndGravity[] In_SpeedAndGravity;
            public float In_LifeTime;
            public float In_MaxFlyDistance;
            public int In_GunAutoItemId;
            public int In_BuffDamage;
            public GPOData.GPOType In_IgnoreGPOType;

            public void SetConfigData(IAbilityMData modMData) {
                if (modMData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modMData.GetTypeID()}");
                    return;
                }
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}