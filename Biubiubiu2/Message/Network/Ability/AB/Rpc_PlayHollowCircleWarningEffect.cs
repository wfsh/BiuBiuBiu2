using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_PlayHollowCircleWarningEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_PlayHollowCircleWarningEffect";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_PlayHollowCircleWarningEffect_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public float lifeTime;
            public byte maxDistance;
            public ushort attackOffset;
            

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(lifeTime);
                buffer.Write(maxDistance);
                buffer.Write(attackOffset);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                lifeTime = buffer.ReadFloat();
                maxDistance = buffer.ReadByte();
                attackOffset = buffer.ReadUShort();
            }
        }
    }
}
