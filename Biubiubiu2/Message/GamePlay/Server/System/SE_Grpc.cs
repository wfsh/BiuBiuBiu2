using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Grpc {
        public struct Event_SendConfig : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SendConfig>();
            public int GetID() => _id;
            // 下面写你的参数
            public string WarServerAddr;
            public string Area;
            public string Host;
            public ushort Port;
            public ushort MaxConnections;
            public byte ServerType;
        }
    }
}
