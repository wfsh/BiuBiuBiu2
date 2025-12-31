using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

public class SE_AI_FightBoss {
    public struct Event_StartToLeave : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartToLeave>();

        public int GetID() => _id;

        // 下面写你的参数
        public float TimeToHideEntity;
    }

    public struct Event_LeaveComplete : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_LeaveComplete>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool isShowTime;
        public string languageSign;
    }

    public struct Event_CreateFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CreateFightRange>();

        public int GetID() => _id;

        // 下面写你的参数
        public IGPO GPO;
        public float FightRangeRadius;
        public float CenterOffset;
        public bool TriggerInfinitePackBullet;
        public float RemoveTimeAfterEnd;
    }
    
    public struct Event_AddBattleCnt : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddBattleCnt>();

        public int GetID() => _id;

        // 下面写你的参数
        public int AddCnt;
        public int OutCnt;
    }

    public struct Event_GetFightRangeData : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetFightRangeData>();

        public int GetID() => _id;

        // 下面写你的参数
        public Action<Vector3, float, float, bool> CallBack; // < centerPoint, radius, removeTimeAfterEnd >
    }
    
    public struct Event_CreateFightRangeData : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CreateFightRangeData>();

        public int GetID() => _id;

        // 下面写你的参数
        public Vector3 fightRangeCenterPoint;
        public float fightRangeRadius;
        public float removeTimeAfterEnd;
    }

    public struct Event_GetOneTargetInFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetOneTargetInFightRange>();

        public int GetID() => _id;

        // 下面写你的参数
        public Action<IGPO> CallBack;
    }

    public struct Event_GetAllTargetInFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAllTargetInFightRange>();

        public int GetID() => _id;

        // 下面写你的参数
        public Action<List<IGPO>> CallBack;
    }

    public struct Event_SetAllGpoInFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetAllGpoInFightRange>();

        public int GetID() => _id;

        // 下面写你的参数
        public List<IGPO> GpoList;
    }

    public struct Event_GetAliveTargetCount : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAliveTargetCount>();

        public int GetID() => _id;

        // 下面写你的参数
        public Action<int> CallBack;
    }

    public struct Event_CheckGPOInFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CheckGPOInFightRange>();

        public int GetID() => _id;

        // 下面写你的参数
        public IGPO GPO;
        public Action<bool> CallBack;
    }

    public struct Event_HurtOutOfFightRange : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HurtOutOfFightRange>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_ChangeFightRangeStage : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ChangeFightRangeStage>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool isInRange;
    }
}