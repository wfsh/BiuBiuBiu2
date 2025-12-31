using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_PlayBloodSplatter : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public byte GetRowID() => rowId;
            public ushort GetConfigID() => configId;
            public const byte FuncID = Rpc_PlayBloodSplatter_FuncID;
            public ushort configId;
            public byte rowId;
            public int bloodValue;
            public int hitGpoId;
            public int hitItemId;
            public Vector3 diffPos;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(bloodValue);
                buffer.Write(hitGpoId);
                buffer.Write(hitItemId);
                buffer.Write(diffPos);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                bloodValue = buffer.ReadInt();
                hitGpoId = buffer.ReadInt();
                hitItemId = buffer.ReadInt();
                diffPos = buffer.ReadVector3();
            }
        }
    }
}
