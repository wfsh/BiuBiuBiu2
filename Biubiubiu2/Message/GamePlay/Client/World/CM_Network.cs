using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Network {
        public struct AddWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public IClientWorldNetwork IWorldNetwork;
        }

        public struct RemoveWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnID;
        }

        public struct GetWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnID;
            public IProto_Doc ProtoDoc;
            public Action<IClientWorldNetwork, IProto_Doc> CallBack;
        }

        public struct SendPing : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SendPing>();

            public int GetID() => _id;
            // 下面写你的参数
            public ushort Ping;
        }

        public struct SendPhysicsPing : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SendPhysicsPing>();

            public int GetID() => _id;
            // 下面写你的参数
            public ushort Ping;
        }
        public struct SpawnWorldNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SpawnWorldNetwork>();
            public int GetID() => _id;
            // 下面写你的参数
            public int ConnID;  // syncId
            public NetworkData.SpawnConnType ConnType;  // syncType
            public IProto_Doc ProtoDoc;  // 初始协议
        }
    }
}