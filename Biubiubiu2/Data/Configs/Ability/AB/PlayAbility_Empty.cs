using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityData {
        public struct PlayAbility_Empty : IAbilityMData {
            public const string ID = "";
            public string GetID() => "";
            public string GetTypeID() => ID;
            public byte GetRowID() => 0;

            public ushort ConfigId;
            public ushort GetConfigId() => ConfigId;
            public string GetEffectSign() => "";

            public void SetConfigData(IAbilityMData modData) {
            }

            public void Select(Action callBack) {
                callBack?.Invoke();
            }
        }
    }
}
