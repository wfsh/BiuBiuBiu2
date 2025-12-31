using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_AI_Joker {
        public struct Event_SetGoldJokerFlashTeleport : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetGoldJokerFlashTeleport>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Point;
        }
    }
}