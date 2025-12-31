using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Character {
        public struct AddCharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddCharacter>();
            public int GetID() => _id;
            // 下面写你的参数
            public INetwork iNetwork;
            public IProto_Doc InDataDoc;
        }
        public struct AddLocalCharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddLocalCharacter>();
            public int GetID() => _id;
            // 下面写你的参数
            public INetwork iNetwork;
            public IProto_Doc RoleInfoDoc;
            public IProto_Doc InDataDoc;
        }

        public struct Event_GetAutoFire : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GetAutoFire>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_GetAutoLock : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GetAutoLock>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_InputSetAuto : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_InputSetAuto>();
            public int GetID() => _id;

            // 下面写你的参数
        }
    }
}