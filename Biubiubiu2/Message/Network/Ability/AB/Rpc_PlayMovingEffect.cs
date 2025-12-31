using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_PlayMovingEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_PlayMovingEffect";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_PlayMovingEffect_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint; // 不需要坐标同步记得一定要删掉，不然浪费流量
            public Vector3 startLookAt;
            public Vector3 startScale; // unit: 0.1
            public ushort lifeTime; // unit: 0.1s
            public Vector3 moveDir;
            public ushort moveSpeed; // unit: 0.1m/s
            public ushort audioKey;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint); // 不需要坐标同步记得一定要删掉，不然浪费流量
                buffer.Write(startLookAt);
                buffer.Write(startScale); // unit: 0.1
                buffer.Write(lifeTime); // unit: 0.1s
                buffer.Write(moveDir);
                buffer.Write(moveSpeed); // unit: 0.1m/s
                buffer.Write(audioKey);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3(); // 不需要坐标同步记得一定要删掉，不然浪费流量
                startLookAt = buffer.ReadVector3();
                startScale = buffer.ReadVector3(); // unit: 0.1
                lifeTime = buffer.ReadUShort(); // unit: 0.1s
                moveDir = buffer.ReadVector3();
                moveSpeed = buffer.ReadUShort(); // unit: 0.1m/s
                audioKey = buffer.ReadUShort();
            }
        }
    }
}
