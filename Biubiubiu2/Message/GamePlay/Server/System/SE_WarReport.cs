using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_WarReport {
        public struct Event_TickTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TickTime>();

            public int GetID() => _id;

            // 下面写你的参数
            public float TickTime;
        }

        public struct Event_SaveBegin : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SaveBegin>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_SaveReport : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SaveReport>();

            public int GetID() => _id;
            // 下面写你的参数
            public string Path;
        }
    }
}