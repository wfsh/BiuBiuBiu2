using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public interface IAbilityEffectMData : IAbilityMData {
    }
    public abstract class AbilityEffectMData : AbilityMData, IAbilityEffectMData {
    }

    public interface IAbilityEffectInData : IAbilityInData {
    }

    public partial class AbilityEffectData {
        public enum Effect {
            UpdraftPoint,
            GpoMoveSpeedRate, // 移动速度
            ChangeMatType, // 材质球变换
            GpoReloadRate,
            GpoShootIntervalRate,
            GpoHurtValueRate,
            GpoMaxHpRate
        }
    }
}