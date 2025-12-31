using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Network {
        public struct Event_GetSpawnNetworks : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetSpawnNetworks>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<INetworkCharacter>> CallBack;
        }
        public struct Event_EnabledSyncNetworkTransform : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_EnabledSyncNetworkTransform>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct Event_InDistanceNetwork : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_InDistanceNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<INetworkCharacter> NetworkList;
        }
        public struct Event_SetIsOnline : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetIsOnline>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public bool IsOnline;
        }

        public struct Event_GetIsOnline : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsOnline>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
    }
}