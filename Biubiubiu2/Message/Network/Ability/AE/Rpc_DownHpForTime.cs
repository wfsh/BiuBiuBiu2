using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAE_Auto {
        public struct Rpc_DownHpForTime : IAbilityAECreateRpc {
            public const string ID = "Proto_AbilityAE_Auto.Rpc_DownHpForTime";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_DownHpForTime_FuncID;
            public ushort configId;
            public byte rowId;
            public int gpoId; // 目标GPO ID

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(gpoId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                gpoId = buffer.ReadInt();
            }
        }
    }
}
