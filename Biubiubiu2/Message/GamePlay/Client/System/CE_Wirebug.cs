using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Wirebug {
        public struct GetWirebugList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetWirebugList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<float>> CallBack;
        }
        public struct GetWirebugMaxCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetWirebugMaxCD>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct SetWirebugList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetWirebugList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<float> WirebugList;
        }
        public struct SetWirebugMaxCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetWirebugMaxCD>();
            public int GetID() => _id;

            // 下面写你的参数
            public float MaxCD;
        }
    }
}