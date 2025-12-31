using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Network {
        public struct SetWorldNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetWorldNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetwork network;
            public IWorldSync worldSync;
        }

        public struct AddWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public IServerWorldNetwork network;
        }

        public struct RemoveWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnId;
        }

        public struct GetWorldNetworkBehaviour : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetWorldNetworkBehaviour>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnId;
            public Action<IServerWorldNetwork> CallBack;
        }
    }
}