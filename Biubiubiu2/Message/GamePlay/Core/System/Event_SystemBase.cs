using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class Event_SystemBase {
        public struct SetNetwork : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetNetwork>();
            public int GetID() => _id;
            // 下面写你的参数
            public INetwork network;
        }
        
        public struct SetEntityObj : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetEntityObj>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct Test : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Test>();
            public int GetID() => _id;
            // 下面写你的参数
        }
    }
}
