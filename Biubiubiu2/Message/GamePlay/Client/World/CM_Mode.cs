using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Mode {
        public class PlayModeCharacterData {
            public int GPOId;
            public long PlayerId;
            public int TeamId;
            public int Score;
            public int KillCount;
            public int ContinueScore;
            public int HurtValue;
            public string AvatarURL;
            public string NickName;
            
            // 结算后下发
            public int AllScore;
            public int DeadNum;  
        }
        public struct SendSaveBattleData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SendSaveBattleData>();
            public int GetID() => _id;
            // 下面写你的参数
            public byte[] ByteDatas;
        }
        
        public struct SaveData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveData>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct GetPlayModeCharacterData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetPlayModeCharacterData>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public Action<PlayModeCharacterData> CallBack;
        }    
        
        public struct SetGameState : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetGameState>();

            public int GetID() => _id;
            // 下面写你的参数
            public ModeData.GameStateEnum GameState;
        }
        
        public struct SetDownTime : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetDownTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public int DownTime;
        }
        
        public struct UpdatePlayCharacterData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdatePlayCharacterData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Dictionary<long, PlayModeCharacterData> CharacterDataDic;
        }
        
        public struct UpdateCharacterScore : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdateCharacterScore>();
            public int GetID() => _id;
            // 下面写你的参数
            public PlayModeCharacterData CharacterData;
        }
        
        public struct UpdateCharacterKillNum : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdateCharacterKillNum>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool CheckFirstKill;
            public PlayModeCharacterData CharacterData;
        }
        
        public struct UpdateCharacterHurt : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdateCharacterHurt>();
            public int GetID() => _id;
            // 下面写你的参数
            public PlayModeCharacterData CharacterData;
        }
        
        public struct ModeMessage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ModeMessage>();

            public int GetID() => _id;
            // 下面写你的参数
            public string MainText;
            public int MainTeamId;
            public string SubText;
            public int UseId;
            public ModeData.MessageEnum MessageState;
        }
        public struct GetWarEndData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetWarEndData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<int>, List<PlayModeCharacterData>, List<int>> CallBack;
        }       
           
        public struct WinTeamList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<WinTeamList>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<int> TeamList;
            public int TriggerEndGpoId;
        }
        
        public struct ShowWarEndUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowWarEndUI>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct SetTeamsWinCount : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetTeamsWinCount>();

            public int GetID() => _id;
            // 下面写你的参数
            public int roundCount;
            public Dictionary<int, int> teamToWinCount;
        }

        public struct TeamWin : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<TeamWin>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<int> teamList;
        }

        public struct GetTeamsWinCount : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetTeamsWinCount>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Dictionary<int, int>> callBack;
        }

        public struct OnlineChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OnlineChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public bool IsOnline;
            public float ReconnectDuration;
        }
    }
}