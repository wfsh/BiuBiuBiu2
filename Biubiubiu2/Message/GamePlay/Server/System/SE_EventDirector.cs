using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_EventDirector {
        public struct SetEventList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetEventList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<EventDirectorData.Data> List;
        }
        public struct HasItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HasItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public Action<bool> CallBack;
        }
    }
}