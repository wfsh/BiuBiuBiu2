using System;
using Sofunny.BiuBiuBiu2.Data;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Network {
        public struct NetworkConfigInit : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NetworkConfigInit>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct CMDSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CMDSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public IProto_Doc ProtoDoc;
            public int Channel;
            public Action<int, byte[]> CallBack;
        }

        public struct RPCUnSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RPCUnSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte[] Datas;
            public int ConnID;
            public Action<int, IProto_Doc> CallBack;
        }

        public struct RPCSyncUnSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RPCSyncUnSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte[] Datas;
            public int ConnID;
            public NetworkData.SpawnConnType SpawnType;
            public Action<int, NetworkData.SpawnConnType, IProto_Doc> CallBack;
        }

        public struct RPCSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RPCSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public IProto_Doc ProtoDoc;
            public int Channel;
            public int ConnID;
            public Action<int, int, byte[]> CallBack;
        }
        public struct TargetRPCSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<TargetRPCSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public IProto_Doc ProtoDoc;
            public INetwork TargetNetwork;
            public int ConnID;
            public int Channel;
            public Action<int, int, byte[], INetwork, string, bool> CallBack;
        }


        public struct CMDUnSerialize : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CMDUnSerialize>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte[] Datas;
            public Action<IProto_Doc> CallBack;
        }

        public struct SetNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetwork iNetwork;
        }

        public struct ClientConnect : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ClientConnect>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct SetSpawnPrefabs : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetSpawnPrefabs>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<GameObject> SpawnPrefabs;
        }

        public struct Spawn : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Spawn>();

            public int GetID() => _id;
            // 下面写你的参数
            public string Sign;
            public Action<GameObject> CallBack;
        }

        public struct ServerStart : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ServerStart>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct ServerDisconnect : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ServerDisconnect>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnId;
        }

        public struct ClientDisconnect : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ClientDisconnect>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct DisconnectNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<DisconnectNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetwork INetwork;
        }

        public struct GameServerConnectSuccess : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GameServerConnectSuccess>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct GetAllNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetAllNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<INetworkCharacter>> CallBack;
        }

        public struct GetAllNetworkForPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetAllNetworkForPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<INetworkCharacter>> CallBack;
            public Vector3 Point;
            public float Distance;
        }
    }
}