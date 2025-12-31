using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_PlayBloodSplatter : IAbilityMData {
            public const string ID = AbilityData.SAB_BloodSplatter;
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
            
            public void Select(Action callBack) {
                throw new NotImplementedException();
            }

            // 模版数据
            public float M_LifeTime;
            public string M_EffectSign;
            // 下面写你的参数
            public Vector3 In_HitPoint;
            public int In_BloodValue;
            public int In_HitGpoId;
            public int In_HitItemId;

            public void SetConfigData(IAbilityMData modData) {
                if (modData.GetTypeID() != ID) {
                    Debug.LogError($"模板数据使用错误 {ID} != {modData.GetTypeID()}");
                    return;
                }
                var data = (PlayAbility_PlayBloodSplatter)modData;
                M_LifeTime = data.M_LifeTime;
                M_EffectSign = data.M_EffectSign;
            }
        }
    }
}