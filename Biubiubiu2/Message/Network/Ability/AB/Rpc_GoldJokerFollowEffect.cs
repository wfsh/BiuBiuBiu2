using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_GoldJokerFollowEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_GoldJokerFollowEffect";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_GoldJokerFollowEffect_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Vector3 startRot;
            public Vector3 startScale;
            public ushort lifeTime;
            public long playTimestamp; // unit: 1
            private byte flags;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                if (startRot != Vector3.zero) {
                    flags = 1;
                }
                if (startScale != Vector3.zero) {
                    flags += 2;
                }
                buffer.Write(startPoint);
                buffer.Write(flags);
                if (startRot != Vector3.zero) {
                    buffer.Write(startRot);
                }
                if (startScale != Vector3.zero) {
                    buffer.Write(startScale);
                }
                buffer.Write(lifeTime);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                flags = buffer.ReadByte();
                if ((flags & 1) == 1) {
                    startRot = buffer.ReadVector3();
                }
                if ((flags & 2) == 1) {
                    startScale = buffer.ReadVector3();
                } else {
                    startScale = Vector3.one;
                }
                lifeTime = buffer.ReadUShort();
                playTimestamp = buffer.ReadLong();
            }
        }
    }
}
