using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_PlayEffect : IAbilityMData {
            public const string ID = AbilityData.SAB_PlayEffect;
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
            public float M_LifeTime;
            public string M_EffectSign;
            public string M_AudioSign;
            
            // 下面写你的参数
            public Vector3 In_StartPoint;
            public Quaternion In_StartRota;
            public float In_LifeTime;
            public ushort In_AudioKey;
        }
    }
}