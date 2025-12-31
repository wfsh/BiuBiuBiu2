using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_WarReport {
        public struct Event_TickTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TickTime>();

            public int GetID() => _id;

            // 下面写你的参数
            public float TickTime;
        }

        public struct Event_PlayBegin : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBegin>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte[] Bytes;
        }

        public struct Event_PlayEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayEnd>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_StopPlay : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StopPlay>();

            public int GetID() => _id;
            // 下面写你的参数
        }
    }
}