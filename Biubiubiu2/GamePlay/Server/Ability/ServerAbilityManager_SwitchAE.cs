using System;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerAbilityManager {
        protected S_Ability_Base HandleAbilityAEType(string abilityTypeId, Action<S_Ability_Base> callBack) {
            S_Ability_Base system = null;
            switch (abilityTypeId) {
                case AbilityM_ChangeMat.AbilityTypeID:
                    system = AddSystem<SAE_ChangeMatSystem>(callBack);
                    break;
                case AbilityM_DownHpForTime.AbilityTypeID:
                    system = AddSystem<SAE_DownHpForTimeSystem>(callBack);
                    break;
                case AbilityM_HurtGPOByTime.AbilityTypeID:
                    system = AddSystem<SAE_HurtGPOByTimeSystem>(callBack);
                    break;
                case AbilityM_MoveSpeedRate.AbilityTypeID:
                    system = AddSystem<SAE_MoveSpeedRateSystem>(callBack);
                    break;
                case AbilityM_HurtValueRate.AbilityTypeID:
                    system = AddSystem<SAE_HurtValueRateSystem>(callBack);
                    break;
                case AbilityM_MaxHpRate.AbilityTypeID:
                    system = AddSystem<SAE_MaxHpRateSystem>(callBack);
                    break;
                case AbilityM_MoveSpeedRateByTime.AbilityTypeID:
                    system = AddSystem<SAE_MoveSpeedRateByTimeSystem>(callBack);
                    break;
                case AbilityM_ReloadRate.AbilityTypeID:
                    system = AddSystem<SAE_ReloadRateSystem>(callBack);
                    break;
                case AbilityM_ShootIntervalRate.AbilityTypeID:
                    system = AddSystem<SAE_ShootIntervalRateSystem>(callBack);
                    break;
            }
            return system;
        }
    }
}
