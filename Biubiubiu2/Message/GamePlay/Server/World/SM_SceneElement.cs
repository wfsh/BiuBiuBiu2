using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_SceneElement { 
        public struct AddNaturalResource : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddNaturalResource>();
            public int GetID() => _id;
            // 下面写你的参数
            // 资源标识
            public string ResourceSign;
            // 采集点
            public Vector3 Point;
        }
        
        // 采集状态
        public struct Event_StartGathering : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_StartGathering>();
            public int GetID() => _id;

            // 下面写你的参数
            public IGPO IGpo;
            public int CreateId;
        }
        
        // 采集状态
        public struct Event_CancelGathering : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_CancelGathering>();
            public int GetID() => _id;

            // 下面写你的参数
            public IGPO IGpo;
        }
    }
}