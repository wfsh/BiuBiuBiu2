using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_SceneElement {
        public struct StartGatheringResource : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartGatheringResource>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct EnabledShowGatheringResource : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledShowGatheringResource>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        
        public struct ShowGatheringTime : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowGatheringTime>();
            public int GetID() => _id;
            // 下面写你的参数
            public float startTime;
            public float endTime;
        }
    }
}
