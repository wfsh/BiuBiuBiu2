using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    // 自动生成的代码，不要修改
    public partial class Proto_AbilityAE_Auto {
        public const byte ModID = 15;
        public const byte Rpc_HurtValueRate_FuncID = 1;
        public const byte Rpc_ChangeMat_FuncID = 2;
        public const byte Rpc_DownHpForTime_FuncID = 3;
        public const byte Rpc_HurtGPOByTime_FuncID = 4;
        public const byte Rpc_MaxHpRate_FuncID = 5;
        public const byte Rpc_MoveSpeedRate_FuncID = 6;
        public const byte Rpc_MoveSpeedRateByTime_FuncID = 7;
        public const byte Rpc_ReloadRate_FuncID = 8;
        public const byte Rpc_ShootIntervalRate_FuncID = 9;

        public static IProto_Doc ReadRpcBuffer(byte funcId) {
            IProto_Doc doc = null;
            switch (funcId) {
                case Rpc_HurtValueRate_FuncID:
                    doc = new Rpc_HurtValueRate();
                    break;
                case Rpc_ChangeMat_FuncID:
                    doc = new Rpc_ChangeMat();
                    break;
                case Rpc_DownHpForTime_FuncID:
                    doc = new Rpc_DownHpForTime();
                    break;
                case Rpc_HurtGPOByTime_FuncID:
                    doc = new Rpc_HurtGPOByTime();
                    break;
                case Rpc_MaxHpRate_FuncID:
                    doc = new Rpc_MaxHpRate();
                    break;
                case Rpc_MoveSpeedRate_FuncID:
                    doc = new Rpc_MoveSpeedRate();
                    break;
                case Rpc_MoveSpeedRateByTime_FuncID:
                    doc = new Rpc_MoveSpeedRateByTime();
                    break;
                case Rpc_ReloadRate_FuncID:
                    doc = new Rpc_ReloadRate();
                    break;
                case Rpc_ShootIntervalRate_FuncID:
                    doc = new Rpc_ShootIntervalRate();
                    break;
                default:
                    Debug.LogError("Proto_Ability:ReadRpcBuffer 没有注册对应 ID:" + funcId);
                    return null;
            }

            return doc;
        }
    }
}
