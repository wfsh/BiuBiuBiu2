using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_SausageBomb : IAbilityMData {
            public const string ID = AbilityData.SAB_SausageBomb;
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
            public bool M_IsStrikeFly;

            // 下面写你的参数
            public Vector3 In_StartPoint;
            public int In_Hurt;
            public float In_Range;
            public IAbilityEffectMData In_HitEffect;
            
        }
    }
}