using System;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Camera {
        public struct SetDelta : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetDelta>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector2 Delta;
        }
        public struct StartAutoLookTarget : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartAutoLookTarget>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct LookNearestGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LookNearestGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public float LimitAngle;
            public float Speed;
        }
        public struct LockTarget : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LockTarget>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Speed;
            public IGPO TargetGPO;
        }
        public struct CheckObstacleForGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CheckObstacleForGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 startPoint;
            public IGPO TargetGPO;
            public Action<bool> CallBack;
        }
        public struct CheckAngleAnDistance : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CheckAngleAnDistance>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Angle;
            public float Distance;
            public IGPO TargetGPO;
            public Action<bool> CallBack;
        }
        /// <summary>
        /// 获取一定范围内没有遮挡的最近的 GPO(角度 + 距离)
        /// </summary>
        public struct FindNearestGPOInFrontAngle : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<FindNearestGPOInFrontAngle>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Angle;
            public float Distance;
            public Action<IGPO> CallBack;
        }
        
        /// <summary>
        /// 获取一定范围内没有遮挡的最近的 GPO(屏幕距离 + 距离)
        /// </summary>
        public struct FindNearestGPOInFrontScreen : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<FindNearestGPOInFrontScreen>();
            public int GetID() => _id;
            // 下面写你的参数
            public float MaxScreenDistance;
            public float MinScreenDistance;
            public float ScreenCheckDistance;
            public float Distance;
            public Action<IGPO> CallBack;
        }
        public struct CameraHVRota : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CameraHVRota>();
            public int GetID() => _id;
            // 下面写你的参数
            public Quaternion HRota;
            public Quaternion VRota;
        }
        public struct GetCameraCenterObjPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCameraCenterObjPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 CheckForwardPoint;
            public float FarDistance;
            public int IgnoreTeamId;
            public Action<Vector3, bool> CallBack;
        }
        public struct GetScreenCenterGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetScreenCenterGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public float FarDistance;
            public int IgnoreGpoID;
            public Action<IGPO> CallBack;
        }
        public struct GetCameraForward : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCameraForward>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3> CallBack;
        }

        public struct GetScreenCenterBarrier : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetScreenCenterBarrier>();
            public int GetID() => _id;
            // 下面写你的参数
            public float FarDistance;
            public Vector3 CheckForwardPoint;
            public Action<Vector3, bool> CallBack;
        }

        public struct SetAutoLookGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetAutoLookGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public IGPO GPO;
        }
        
        public struct SetUIAutoLockDistance : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetUIAutoLockDistance>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Distance;
        }
        
        public struct ShakeCamera : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShakeCamera>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Duration;
            public float Magnitude;
            public float DampingSharpness;
            public float MaxRotationAngle;
            public Vector3 NoiseFrequency;
            public bool AffectRotation;
        }
    }
}