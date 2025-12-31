using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_GoldJokerDollBomb : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_GoldJokerDollBomb";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_GoldJokerDollBomb_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Vector3 startRot;
            public ushort lifeTime;
            public long playTimestamp; // unit: 1
            public float attackRange;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(startRot);
                buffer.Write(lifeTime);
                buffer.Write(playTimestamp);
                buffer.Write(attackRange);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                startRot = buffer.ReadVector3();
                lifeTime = buffer.ReadUShort();
                playTimestamp = buffer.ReadLong();
                attackRange = buffer.ReadFloat();
            }
        }
    }
}
