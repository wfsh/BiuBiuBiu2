using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_FunnyDB {
        public struct WeaponResultInfo {
            public string Weapon;
            public int WeaponLevel;
            public int FireBullets;
            public int HitBullets;
        }

        public struct RewardInfo {
            public int ItemNum;
            public int ItemId;
        }

        public struct ReportEnterWar : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportEnterWar>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
            public int MatchId;
            public int Gmod;
            public int GMap;
            public int MapSide;
            public int MaxEquipLevel;
            public int RealPlayerNum;
        }

        public struct ReportPlayerBeKill : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportPlayerBeKill>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
            public int Gmod;
            public int GMap;
            public int GradeScore;
            public int HideScore;
            public string FightTime;
            public float FightTimeF;
            public string KillLocationZ;
            public float KillLocationZF;
            public string KillLocationX;
            public float KillLocationXF;
            public string BeKillLocationZ;
            public float BeKillLocationZF;
            public string BeKillLocationX;
            public float BeKillLocationXF;
            public string KilledDistance;
            public float KilledDistanceF;
            public long BeKillPid;
            public int MultiKillNum;
            public string DeadType;
            public string DeadDetail;
            public int DeadDetailLevel;
        }

        public struct ReportPlayerUseSuperWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportPlayerUseSuperWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
            public int Gmod;
            public int HideScore;
            public int GradeScore;
            public string SuperWeaponType;
        }

        public struct ReportSuperWeaponDestroy : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportSuperWeaponDestroy>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
            public int Gmod;
            public int HideScore;
            public int GradeScore;
            public string SuperWeaponType;
            public string SuperWeaponDuration;
            public float SuperWeaponDurationF;
            public string KillLocationZ;
            public float KillLocationZF;
            public string KillLocationX;
            public float KillLocationXF;
            public string BeKillLocationZ;
            public float BeKillLocationZF;
            public string BeKillLocationX;
            public float BeKillLocationXF;
            public string KilledDistance;
            public float KilledDistanceF;
            public long BePid;   // 击杀者
            public string DeadType;
            public string DeadDetail;
            public int DeadDetailLevel;
        }

        public struct ReportWarEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportWarEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
            public int MatchId;
            public int Gmod;
            public int GMap;
            public int GradeScore;
            public int HideScore;
            public float WarStartTime;
            public float WarDuration; 
            public float BattleResult;
            public int MapSide; //1-A侧；2-B侧
            public string BattleScore; //记为己方比分 / 对方比分，保留2位小数，如50（己方）:60（敌方）记为0.83
            public List<WeaponResultInfo> FireGuns;
            public int DamageNum;
            public int DeadNum;
            public int KillNum;
            public int RealKillNum;
            public int TotalBullets;
            public int HitBullets;
            public string MoveDistance;
            public float MoveDistanceF;
            public float SlideNum;
            public int JumpNum;
            public List<RewardInfo> Award;
        }

        public struct ReportGunChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportGunChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public long Pid;
            public string WarId;
        }
        
        public struct ReportLog : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SM_FunnyDB.ReportLog>();

            public int GetID() => _id;
            // 下面写你的参数
            public string EventName;
            public string Value;
        }
    }
}