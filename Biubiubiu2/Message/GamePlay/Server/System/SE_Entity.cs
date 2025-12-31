using System;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Entity {
        public struct SyncPointToNetworkTransform : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncPointToNetworkTransform>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public bool OR_IsSync; // 同步直接下发给客户端
        }

        public struct SyncRotaToNetworkTransform : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncRotaToNetworkTransform>();

            public int GetID() => _id;

            // 下面写你的参数
            public Quaternion Rota;
            public bool OR_IsSync; // 同步直接下发给客户端
        }

        public struct SyncScaleToNetworkTransform : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncScaleToNetworkTransform>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Scale;
            public bool OR_IsSync; // 同步直接下发给客户端
        }

        public struct SyncPointAndRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncPointAndRota>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
            public bool OR_IsSync;
        }

        public struct SyncPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncPoint>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
        }

        public struct SyncRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SyncRota>();

            public int GetID() => _id;

            // 下面写你的参数
            public Quaternion Rota;
        }

        public struct Event_SetShowEntity : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetShowEntity>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool IsShow;
        }

        public struct Event_SetDeadAutoHideEntity : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetDeadAutoHideEntity>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool isDeadAutoHideEntity;
        }

        public struct Event_SetShowEntityForAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetShowEntityForAnim>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool IsShow;
        }

        public struct Event_GetShowEntity : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetShowEntity>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_IsShowEntity : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsShowEntity>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool IsShow;
        }
    }
}