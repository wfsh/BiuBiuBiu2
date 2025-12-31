using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public partial class Proto_AbilityAB_Auto {
        public struct Rpc_GiantDaDaSurgeIncoming : ITargetRpc {
            public const string ID = "Proto_Ability.Rpc_GiantDaDaSurgeIncoming";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            // public const byte FuncID = Rpc_GiantDaDaSurgeIncoming_FuncID;
            public ushort abilityModId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                // buffer.Write(FuncID);
                buffer.Write(abilityModId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
            }
        }
    }
}
