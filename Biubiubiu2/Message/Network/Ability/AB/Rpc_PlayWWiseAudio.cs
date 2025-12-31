using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_PlayWWiseAudio : IAbilityABCreateRpc {
            public const string ID = "Proto_AbilityAB_Auto.Rpc_PlayWWiseAudio";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = Rpc_PlayWWiseAudio_FuncID;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint; // 不需要坐标同步记得一定要删掉，不然浪费流量
            public ushort wwiseId;
            public ushort lifeTime;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint); // 不需要坐标同步记得一定要删掉，不然浪费流量
                buffer.Write(wwiseId);
                buffer.Write(lifeTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3(); // 不需要坐标同步记得一定要删掉，不然浪费流量
                wwiseId = buffer.ReadUShort();
                lifeTime = buffer.ReadUShort();
            }
        }
    }
}
