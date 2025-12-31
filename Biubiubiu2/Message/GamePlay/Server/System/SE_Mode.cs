using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Mode {
        public class PlayModeCharacterData {
            public long PlayerId;
            public int GPOMId; // 角色模板 ID
            public string AssetSign; // 角色资源标识
            public string NickName;
            public string Avatar;
            public bool IsAI;
            public int AILevel;
            public int TeamId;
            public int Score;
            public int ContinueScore;
            public int SameTypeWeaponContinueScore; //同类型武器连续击杀，切换超级武器会重置
            public int AllScore;
            public int WinCount;
            public int DeadCount;
            public int HurtValue;
            public int KillCount;
            public int RealKillCount; // 真人击杀数
            public int WinLootBoxItemId;
            public long GroupId;
            public bool IsTeamUp;
            public int RankScore;
            public int HiddenScore;
            public float MoveDistance;
            public int SlideNum;
            public int JumpNum;
            public int AirJumpNum;
            public int SwitchWeaponNum;
            public int SuperWeaponUseTime;
            public int originMatchId;
            public IGPO CharacterGPO;
            public AiConfig AiConfig;
            public IGPOInData InData;
            public List<PlayModeCharacterWeapon> WeaponList;
        }

        public class PlayModeCharacterWeapon {
            public int WeaponItemId;
            public int WeaponSkinItemId;
            public int Index;
            public int RandBulletSpeed;
            public int RandAttack;
            public int RandMagazineNum;
            public bool IsSuperWeapon;
            public float RandIntervalTime;
            public float RandFireOverHotTime;
            public float RandAttackRange;
            public float RandFireRange;
            public float RandFireDistance;
            public float RandReloadTime;
            public float RandAddEffectFireDistanceRate;
            public float RandMissileRange;
            public float RandMissileDuration;
            public float RandDiffusionReduction;
            public int RandHp;
            public float HitHeadMultiplier;
            public int Level; // 白 ~ 红: 1 ~ 6, 红 2 以上: 7 ~ 10
            public IWeapon iWeapon;
        }

        public struct Event_GameState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GameState>();
            public int GetID() => _id;

            // 下面写你的参数
            public ModeData.GameStateEnum GameState;
        }

        public struct Event_AddCharacterFinish : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddCharacterFinish>();
            public int GetID() => _id;

            // 下面写你的参数
            public PlayModeCharacterData Data;
        }

        public struct Event_GetCharacterList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetCharacterList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<PlayModeCharacterData>> CallBack;
        }

        public struct Event_GetGpoIdToPlayerIdDic : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGpoIdToPlayerIdDic>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Dictionary<int, long>> CallBack;
        }

        public struct Event_GetWinTeamList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetWinTeamList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<int>> CallBack;
        }

        public struct Event_OnSetCreateAIList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnSetCreateAIList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<PlayModeCharacterData> AIList;
        }

        public struct Event_OnSetCreateAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnSetCreateAI>();
            public int GetID() => _id;

            // 下面写你的参数
            public PlayModeCharacterData Data;
        }

        public struct Event_SendCharacterScore : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SendCharacterScore>();

            public int GetID() => _id;

            // 下面写你的参数
            public PlayModeCharacterData Data;
            public int Score;
        }

        public struct Event_OnCloseWaitAutoAddAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnCloseWaitAutoAddAI>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        

        public struct Event_ServiceQuitGame : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ServiceQuitGame>();

            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_GetWeaponResult : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetWeaponResult>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public Action<List<SM_FunnyDB.WeaponResultInfo>> CallBack;
        }

        public struct Event_OverGameMode : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OverGameMode>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<int> WinTeamList;
            public PlayModeCharacterData TriggerData;
        }

        public struct Event_GetCharacterData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetCharacterData>();
            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public Action<PlayModeCharacterData> CallBack;
        }
    }
}