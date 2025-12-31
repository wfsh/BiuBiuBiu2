using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_PlayEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_PlayEffect";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public byte GetRowID() => rowId;
            public ushort GetConfigID() => configId;
            public const byte FuncID = Rpc_PlayEffect_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Quaternion startRota;
            public ushort lifeTime;
            public byte scale; // unit: 0.1
            public ushort audioKey;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(lifeTime);
                buffer.Write(scale);
                buffer.Write(audioKey);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                lifeTime = buffer.ReadUShort();
                scale = buffer.ReadByte();
                audioKey = buffer.ReadUShort();
            }
        }
    }
}
