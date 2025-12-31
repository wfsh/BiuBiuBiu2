using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Ability {
        public struct PlayAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public string sign;
            public int fireGpoId;
            public int abilityId;
            public int connId;
            public IProto_Doc abilityData;
        }
        
        public struct RemoveAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<RemoveAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public int AbilityId;
        }
        public struct SetLifeTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetLifeTime>();
            public int GetID() => _id;
            // 下面写你的参数
            public float LifeTime;
            public Action OnLifeTimeEnd;
        }
        public struct PlatformMovement : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlatformMovement>();
            public int GetID() => _id;
            // 下面写你的参数
            public IEntity entity;
            public Vector3 point;
        }
        
        public struct StayPlatformMovement : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StayPlatformMovement>();
            public int GetID() => _id;
            // 下面写你的参数
            public int elementId;
            public bool isStay;
        }
        
        public struct InSlipRope : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<InSlipRope>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool isInSlipRope;
        }
        
        public struct OutSlipRope : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OutSlipRope>();
            public int GetID() => _id;
            // 下面写你的参数
            public int gpoId;
        }
        
        public struct SlipRopeMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SlipRopeMove>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 forward;
        }
        
        public struct AddBulletDecal : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AddBulletDecal>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 TargetPoint;
            public bool IsShowDecal;
            public string BulletDecal;
            public Vector3 TargetNormal;
        }
        
        public struct SetEntityStartPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetEntityStartPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 StartPoint;
        }
        
        public struct MissileMoveEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<MissileMoveEnd>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct HasConnAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HasConnAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action CallBack;
        }
    }
}
