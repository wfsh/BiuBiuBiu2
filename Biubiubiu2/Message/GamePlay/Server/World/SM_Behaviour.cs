using System;
using BehaviorDesigner.Runtime;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Behaviour {
        public struct Event_StartBehavior : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_StartBehavior>();

            public int GetID() => _id;
            // 下面写你的参数
            public Behavior UseBehavior;
        }
        public struct Event_DisableBehavior : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DisableBehavior>();

            public int GetID() => _id;
            // 下面写你的参数
            public Behavior UseBehavior;
        }
    }
}