using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_AI {
        public struct Event_GetAIList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GetAIList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IAI>> CallBack;
        }
        
        public struct Event_DisplayAttackAlert : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DisplayAttackAlert>();
            public int GetID() => _id;

            public int GpoID;
            public Vector3 Position;
            public float FillAmount;
            public bool Show;
        }
    }
}