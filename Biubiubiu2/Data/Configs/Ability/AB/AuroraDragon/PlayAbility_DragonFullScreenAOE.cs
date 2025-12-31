using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_DragonFullScreenAOE : IAbilityMData {
            public const string ID = SAB_DragonFullScreenAOE;
            public string GetTypeID() => ID;    
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => "";
            // 下面写你的参数
            public AbilityM_DragonFullScreenAOESpawner In_Param;
            public int In_SpawnPointIndex;
            public byte In_PlayEffectId;

            public void SetConfigData(IAbilityMData modMData) {
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}