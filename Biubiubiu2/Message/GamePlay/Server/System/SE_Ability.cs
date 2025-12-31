using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Ability {
        public struct RPCAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<RPCAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public IProto_Doc ProtoData;
        }
        public struct EnableRPCRemoveAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<EnableRPCRemoveAbility>();
            public int GetID() => _id;
        }
        public struct SetLifeTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetLifeTime>();
            public int GetID() => _id;
            // 下面写你的参数
            public float LifeTime;
            public Action OnLifeTimeEnd;
        }
        public struct GetServerEffectRotateByDegreeData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetServerEffectRotateByDegreeData>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float, Vector3, float> CallBack;
        }
        public struct SetMovePointsLoop : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetMovePointsLoop>();
            public int GetID() => _id;
            // 下面写你的参数
            public List<Vector3> Points;
            public float MoveSpeed;
        }

        public struct PlatformMovement : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlatformMovement>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 move;
        }

        public struct MovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<MovePoint>();
            public int GetID() => _id;
            // 下面写你的参数

            public Vector3 movePoint;
            public bool isRun;
            public float moveSpeed;
        }

        public struct CanceMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CanceMovePoint>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct HitGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HitGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public IGPO hitGPO;
            public bool isHead;
            public Vector3 hitPoint;
            public float HurtRatio;
            public string SourceAbilityType;
            public Action<int> CallBack;
        }

        public struct Ability_BellowAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_BellowAttack>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 point;
        }

        public struct Ability_DragonCar : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_DragonCar>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 startPoint;
            public Vector3 endPoint;
        }

        public struct Ability_HurtRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_HurtRange>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 point;
        }

        public struct Ability_RexKinFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_RexKinFire>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 firePoint;
            public Vector3 startPoint;
            public Quaternion startRota;
        }

        
        
        public struct Ability_FireGunFireRangeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_FireGunFireRangeChange>();
            public int GetID() => _id;
            // 下面写你的参数
            public float range;
            public float initRange;
        }
        
        public struct Ability_InitGetHurtRatioFunc : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_InitGetHurtRatioFunc>();
            public int GetID() => _id;
            // 下面写你的参数
            public Func<IGPO, float, float> getHurtRatioFunc;
        }

        public struct Ability_StartMissileBombing : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_StartMissileBombing>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Ability_EndMissileBombing : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_EndMissileBombing>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Ability_MoveGrenadeEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_MoveGrenadeEnd>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Ability_GetMissileMoveing : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_GetMissileMoveing>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Ability_SetClearCallback : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_SetClearCallback>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action Callback;
        }

        public struct Ability_LifeTimeEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_LifeTimeEnd>();
            public int GetID() => _id;
        }
        
        public struct Ability_SetRangeChangeCallBack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_SetRangeChangeCallBack>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> Callback;
        }
        
        public struct Ability_RaycastStartCheck : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Ability_RaycastStartCheck>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct SetLifeCycleDuration : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetLifeCycleDuration>();
            public int GetID() => _id;
            // 下面写你的参数
            public float LifeTime;
        }
        
        public struct GetLifeTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetLifeTime>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct GetCountLifeTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetCountLifeTime>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct GetRayEffectRotateByDegree : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetRayEffectRotateByDegree>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float> CallBack;
        }
        public struct HasConnAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HasConnAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action CallBack;
        }
    }
}