using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Behaviour {
        public struct Event_HateLockTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HateLockTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
            public float Distance;
        }

        public struct Event_HateFindTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HateFindTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
            public float Distance;
        }

        public struct Event_HateAttackTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HateAttackTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
        }
        public struct Event_AfterBehaviorConfigInit : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AfterBehaviorConfigInit>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_SetKeepEyesOnGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetKeepEyesOnGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsEnabled;
            public IGPO TargetGPO;
        }

        public struct Event_GetGPOListInSight : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGPOListInSight>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
        }

        public struct Event_FillHateToValue : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FillHateToValue>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO CasterGPO;
            public float Value;
        }

        public struct Event_ChainStimulus : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ChainStimulus>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO CasterGPO;
        }

        public struct Event_ClearGPOSelfHate : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ClearGPOSelfHate>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoID;
        }

        public struct Event_MaxHateTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MaxHateTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
            public float HateValue;
        }

        public struct Event_GetMaxHateTargetData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMaxHateTargetData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO, float> CallBack; // < HateGPO, Value >
        }

        public struct Event_GetMaxHateTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMaxHateTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }

        public struct Event_MovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MovePoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 movePoint;
            public AIData.MoveType MoveType;
        }

        public struct Event_GetNextPatrolPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetNextPatrolPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3> CallBack;
        }

        public struct Event_StopMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StopMove>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_ComeBack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ComeBack>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_InFirstFind : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_InFirstFind>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_GetInFirstFind : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetInFirstFind>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_IsUseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsUseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_IsAllSkillCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsAllSkillCD>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_GetIsUseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsUseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_GetIsAllSkillCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsAllSkillCD>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_PlayAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAttack>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_AttackTargetGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AttackTargetGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
        }

        public struct Event_isMaxHateGPOObstacle : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_isMaxHateGPOObstacle>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsObstacle;
        }

        public struct Event_IsObstacleInSightRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsObstacleInSightRange>();

            public int GetID() => _id;
            // 下面写你的参数
            public float checkDistance;
            public Action<bool> Callback;
        }

        public struct Event_OnJump : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnJump>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_OnAirJump : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnAirJump>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_OnSlide : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnSlide>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_LookTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_LookTarget>();

            public int GetID() => _id;
            public bool isLookTarget;
            public IGPO lookTarget;
            // 下面写你的参数
        }

        public struct Event_GetLatestStimulusState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetLatestStimulusState>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO, Vector3> CallBack; // < SourceGPO, StimulusPoint >
        }
        
        public struct Event_GetForceAttacking : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetForceAttacking>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
        
        public struct Event_CanMoveToTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CanMoveToTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_ClearAllHateTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ClearAllHateTarget>();
      		public int GetID() => _id;
        }
        
        public struct Event_ActionSweepChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ActionSweepChange>();
            public int GetID() => _id;
            public float SweepAngle;
            public float Duration;
            public bool IsSweep;
        }

        public struct Event_ActionSweepEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ActionSweepEnd>();
            public int GetID() => _id;
        }
        

        public struct Event_EnabledBehavior : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_EnabledBehavior>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsEnabled;
        }
    }
}