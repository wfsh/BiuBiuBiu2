using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_AI {
        public struct Event_GetArmedCustomData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetArmedCustomData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool, MonsterArmedCustom> CallBack; // <isInit, data>
        }

        public struct Event_TakeBack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeBack>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_CatchAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CatchAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO CatchGPO;
        }

        public struct Event_SkillType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SkillType>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsSkillType;
        }

        public struct Event_CatchAIState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CatchAIState>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsSuccess;
            public GPOData.AttributeData CatchAIData;
        }

        public struct Event_GetFollowList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetFollowList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IAI>> CallBack;
        }

        public struct Event_ChangeFollowAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ChangeFollowAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public IAI Iai;
        }

        public struct Event_SummonFollowAIEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SummonFollowAIEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public IAI Iai;
        }
        public struct Event_IsMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsMovePoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
            public AIData.MoveType MoveType;
        }
        
        public struct Event_PlayJokerDroneAttackAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayJokerDroneAttackAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool isRayAttack;
        }
        
        public struct Event_SetJokerDroneAttackState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetJokerDroneAttackState>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool isAttack;
        }

        public struct Event_PlayAttackAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAttackAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_PlayBellowAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBellowAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_FireAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_FireBallAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireBallAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_DragonCarAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarAnim>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_DragonCarAnim_Run : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarAnim_Run>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_GetMasterGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMasterGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }

        // 获取最近目标的距离
        public struct Event_GetMinDistanceGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMinDistanceGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_DriverPointRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DriverPointRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
        }

        public struct Event_DriveState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DriveState>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDrive;
            public GPOData.GPOType DriveGpoType;
        }

        public struct Event_GetDriveState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDriveState>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<GPOData.GPOType> CallBack;
        }

        // 获取和 Master 之间的距离
        public struct Event_MasterGPODistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MasterGPODistance>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_PlayerDriveSetTankUpperBodyRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayerDriveSetTankUpperBodyRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Rota;
        }

        public struct Event_PlayerDriveFireForPoints : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayerDriveFireForPoints>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3[] Points;
            public Action<bool> CallBack;
        }
        public struct Event_SetPatrolPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetPatrolPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public PatrolPointData PatrolPointData;
            public float Area;
        }

        public struct Event_OnRemoveAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnRemoveAI>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_DisabledDeadToRemove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DisabledDeadToRemove>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_JumpOverObstacle : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_JumpOverObstacle>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_JumpOverMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_JumpOverMove>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_TriggerAlertStatus : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TriggerAlertStatus>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool isEnabled;
        }

        public struct Event_TriggerFightStatus : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TriggerFightStatus>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool isEnabled;
        }

        public struct Event_SetFireCycle : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetFireCycle>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool isEnabled;
        }

        public struct StartKittyRadar : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StartKittyRadar>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public float Range;
            public int KittyRadarId;
        }

        public struct StartSkirmisher : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StartSkirmisher>();

            public int GetID() => _id;

            // 下面写你的参数
            public string SausageTeamId;
            public float LifeTime;
        }

        public struct Event_GetAlertStartPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAlertStartPoint>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<Vector3> CallBack; // < 进入警戒状态时的位置 >
        }

        public struct Event_CaughtFireEvent : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CaughtFireEvent>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO FireGPO;
        }

        public struct Event_GetActivateStatus : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetActivateStatus>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool, bool> CallBack; // < IsInAlertStatus, IsInFightStatus >
        }
        public struct Event_SetStartPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetStartPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 StartPoint;
        }
        
        
        public struct Event_OnAIFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnAIFire>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsFire;
            public IGPO TargetGPO;
        }
        public struct Event_GetInsightTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetInsightTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }
        public struct Event_SetInsightTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetInsightTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
        }
        
        public struct Event_PlayDragonSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayDragonSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public AIDragonSkillType SkillType;
        }
        
        public struct Event_GoldJokerLookAtTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GoldJokerLookAtTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsLooking;
        }
        
        public struct Event_PlayBossAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBossAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Id;
        }
        
        public struct Event_SetShieldParam : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetShieldParam>();

            public int GetID() => _id;
            // 下面写你的参数
            public ushort ConfigId;
            public ushort UpHpConfigId;
            public int UpHp;
            public float UpHpInterval;
            public float Distance;
        }
        
        public struct Event_ChangeAIState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ChangeAIState>();

            public int GetID() => _id;

            // 下面写你的参数
            public AIBehaviourData.FightStateEnum State;
            public float WarningMaxHate;
        }
        
        public struct Event_PlayGoldJokerSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayGoldJokerSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public GoldJokerSkillType SkillType;
        }
        
        public struct Event_PlayBossTeleport : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBossTeleport>();

            public int GetID() => _id;
            public Vector3 EndPoint;
        }
        
        public struct Event_GetBossFightArea : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetBossFightArea>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3, float> Callback;
        }
        
        public struct Event_AIGoldJokerShieldStateChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AIGoldJokerShieldStateChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool isOpen;
        }
        
        public struct Event_PlayBossInMusic : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBossInMusic>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct Event_PlayFightRandomMusic : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayFightRandomMusic>();

            public int GetID() => _id;
            // 下面写你的参数  这个随机是 wwise 控制的，我们只计算时间进行播放
        }

        public struct Event_PlayFightFailedMusic : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayFightFailedMusic>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct Event_PlayBossDeadMusic : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayBossDeadMusic>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_BossAbilityHit : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_BossAbilityHit>();

            public int GetID() => _id;
            // 下面写你的参数
            public string sourceAbilityType;
        }

        public struct Event_GetIsBoss : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsBoss>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        // 获取最近目标的距离
        public struct Event_SetBaseHateTarget : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetBaseHateTarget>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO TargetGPO;
        }
        
        public struct Event_IsSectorAreaHasGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsSectorAreaHasGPO>();
            public int GetID() => _id;
            public float SweepAngle;
            public Action<bool> CallBack;
        }
    }
}