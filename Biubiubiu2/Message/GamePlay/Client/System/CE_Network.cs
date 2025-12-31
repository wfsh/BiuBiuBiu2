using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Network {
        public struct LoginServerSuccess : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<LoginServerSuccess>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetwork Network;
        }
        public struct IsOnline : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<IsOnline>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct GetIsOnline : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetIsOnline>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
    }
}
