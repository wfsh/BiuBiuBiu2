using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_Bullet : IAbilityMData {
            public const string ID = AbilityData.SAB_Bullet;
            public string GetID() => ID;
            public ushort ConfigId;
            public string GetTypeID() {
                throw new NotImplementedException();
            }

            public byte GetRowID() {
                throw new NotImplementedException();
            }

            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => M_EffectSign;

            public void SetConfigData(IAbilityMData modMData) {
                throw new NotImplementedException();
            }

            public void Select(Action callBack) {
                throw new NotImplementedException();
            }

            // 模版数据
            public string M_EffectSign;
            public int M_Power;
            public ushort M_HitAbility;
            public string M_HitEffect;
            public ushort M_HitSplatter;
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Vector3 In_TargetPoint;
            public float In_Speed;
            public float In_MoveDistance;
            public int In_WeaponItemId;
            public List<WeaponData.BulletAttnMap> In_BulletAttnMap;
            
        }
    }
}