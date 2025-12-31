using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Camera {
        public struct SetLockTargetDelta : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetLockTargetDelta>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector2 Delta;
        }
        public struct SetLockTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetLockTarget>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct EndCameraLocalPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<EndCameraLocalPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 LocalPoint;
        }
        public struct EndFarPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<EndFarPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 FarPoint;
        }

        public struct SetCameraOffset : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetCameraOffset>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Offset;
            public float LerpRate;
        }
    }
}