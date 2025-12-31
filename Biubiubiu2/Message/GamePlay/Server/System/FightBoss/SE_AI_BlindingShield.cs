
using System;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

public class SE_AI_BlindingShield {
    
    public struct Event_GetShieldPoint : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetShieldPoint>();
        public int GetID() => _id;

        public Action<Vector3, Quaternion> Callback;
    }
}