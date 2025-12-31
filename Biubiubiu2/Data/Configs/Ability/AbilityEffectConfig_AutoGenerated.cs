using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityEffectConfig {
        // 自动生成的技能 ID
        // 从 20000 开始
        public const ushort StartIndex = 20000;
        public const ushort ChangeMat = 20001;
        public const ushort DownHpForTime = 20002;
        public const ushort HurtGPOByTime = 20003;
        public const ushort MoveSpeedRate = 20004;
        public const ushort HurtValueRate = 20005;
        public const ushort MaxHpRate = 20006;
        public const ushort MoveSpeedRateByTime = 20007;
        public const ushort ReloadRate = 20008;
        public const ushort ShootIntervalRate = 20009;

        // 自动生成的技能类型标识
        public const string AE_ChangeMat = "AE_ChangeMat";
        public const string AE_DownHpForTime = "AE_DownHpForTime";
        public const string AE_HurtGPOByTime = "AE_HurtGPOByTime";
        public const string AE_MoveSpeedRate = "AE_MoveSpeedRate";
        public const string AE_HurtValueRate = "AE_HurtValueRate";
        public const string AE_MaxHpRate = "AE_MaxHpRate";
        public const string AE_MoveSpeedRateByTime = "AE_MoveSpeedRateByTime";
        public const string AE_ReloadRate = "AE_ReloadRate";
        public const string AE_ShootIntervalRate = "AE_ShootIntervalRate";

        public static void GetAbilityEffectConfig(ushort configId, byte rowId, Action<IAbilityEffectMData> callBack) {
            var mData = GetAbilityEffectConfig(configId, rowId);
            if (mData != null) {
                mData.Select(() => {
                    callBack(mData);
                });
            } else {
                Debug.LogError($"没有对应的 AbilityM 配置 configId: {configId}  rowId: {rowId}");
                callBack(null);
            }
        }

        public static void GetAbilityEffectConfig(string configSign, string rowSign, Action<IAbilityEffectMData> callBack) {
            var mData = GetAbilityEffectConfig(configSign, rowSign);
            if (mData != null) {
                mData.Select(() => {
                    callBack(mData);
                });
            } else {
                Debug.LogError($"没有对应的 AbilityM 配置 configSign: {configSign}  rowSign: {rowSign}");
                callBack(null);
            }
        }

        public static IAbilityEffectMData GetAbilityEffectConfig(string configSign, string rowSign) {
            IAbilityEffectMData mData = null;
            switch (configSign) {
                case AE_ChangeMat:
                    mData = AbilityM_ChangeMat.CreateForSign(rowSign);
                    break;
                case AE_DownHpForTime:
                    mData = AbilityM_DownHpForTime.CreateForSign(rowSign);
                    break;
                case AE_HurtGPOByTime:
                    mData = AbilityM_HurtGPOByTime.CreateForSign(rowSign);
                    break;
                case AE_MoveSpeedRate:
                    mData = AbilityM_MoveSpeedRate.CreateForSign(rowSign);
                    break;
                case AE_HurtValueRate:
                    mData = AbilityM_HurtValueRate.CreateForSign(rowSign);
                    break;
                case AE_MaxHpRate:
                    mData = AbilityM_MaxHpRate.CreateForSign(rowSign);
                    break;
                case AE_MoveSpeedRateByTime:
                    mData = AbilityM_MoveSpeedRateByTime.CreateForSign(rowSign);
                    break;
                case AE_ReloadRate:
                    mData = AbilityM_ReloadRate.CreateForSign(rowSign);
                    break;
                case AE_ShootIntervalRate:
                    mData = AbilityM_ShootIntervalRate.CreateForSign(rowSign);
                    break;
            }
            return mData;
         }

        public static IAbilityEffectMData GetAbilityEffectConfig(ushort configId, byte rowId) {
            IAbilityEffectMData mData = null;
            switch (configId) {
                case ChangeMat:
                    mData = AbilityM_ChangeMat.CreateForID(rowId);
                    break;
                case DownHpForTime:
                    mData = AbilityM_DownHpForTime.CreateForID(rowId);
                    break;
                case HurtGPOByTime:
                    mData = AbilityM_HurtGPOByTime.CreateForID(rowId);
                    break;
                case MoveSpeedRate:
                    mData = AbilityM_MoveSpeedRate.CreateForID(rowId);
                    break;
                case HurtValueRate:
                    mData = AbilityM_HurtValueRate.CreateForID(rowId);
                    break;
                case MaxHpRate:
                    mData = AbilityM_MaxHpRate.CreateForID(rowId);
                    break;
                case MoveSpeedRateByTime:
                    mData = AbilityM_MoveSpeedRateByTime.CreateForID(rowId);
                    break;
                case ReloadRate:
                    mData = AbilityM_ReloadRate.CreateForID(rowId);
                    break;
                case ShootIntervalRate:
                    mData = AbilityM_ShootIntervalRate.CreateForID(rowId);
                    break;
            }
            return mData;
         }
    }
}
