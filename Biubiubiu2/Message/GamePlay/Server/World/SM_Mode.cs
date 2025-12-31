using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Mode {
        public struct StartMode : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartMode>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<TeamInfo> TeamInfos;
        }
        
        public struct CheckCharacterLogin : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CheckCharacterLogin>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ModeId;
            public long PlayerId;
            public long NetworkVersion;
            public string WarId;
            public Action<ModeData.ModeLoginState> CallBack;
        }
        
        public struct GetCharacterList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCharacterList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<SE_Mode.PlayModeCharacterData>> CallBack;
        }
        
        public struct AddCharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddCharacter>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public string NickName;
            public IGPO CharacterGPO;
        }
        
        public struct AddCharacterEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddCharacterEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public IGPO CharacterGPO;
        }
        
        public struct AddAICharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddAICharacter>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public IGPO CharacterGPO;
            public Action<int> CallBack;
        }
        
        public struct GetStartPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetStartPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3, Quaternion> CallBack;
            public IGPO CharacterGPO;
        }
        
        public struct AddScore : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddScore>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public int AttackItemId;
            public ModeData.GetScoreChannelEnum Channel;
        }

        public struct SuperWeaponDestroyed : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SuperWeaponDestroyed>();

            public int GetID() => _id;
            // 下面写你的参数
            public int killGpoId;
            public int beKillGpoId;
            public int beKillMasterGpoId;
            public int ItemId;
        }
        
        public struct GetTeamId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetTeamId>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public Action<int> CallBack;
        }

        public struct GetKillCount : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetKillCount>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public Action<int> CallBack;
        }
        
        public struct Event_GetSaveBattleData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GetSaveBattleData>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO CharacterGPO;
            public byte[] ByteDatas;
        }
        
        public struct Event_ModeMessage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_ModeMessage>();

            public int GetID() => _id;

            // 下面写你的参数
            public int mainGpoId;
            public int subGpoId;
            public int ItemId;
            public ModeData.MessageEnum MessageState;
        }
        
        public struct SetGameState : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetGameState>();

            public int GetID() => _id;
            // 下面写你的参数
            public ModeData.GameStateEnum GameState;
        }
        
        public struct WarResultData {
            public long playerId;
            public long GroupId;
            public int deathCount;
            public int killCount;
            public bool isMvp;
            public int damage;
            public int switchWeaponTimes;
            public int slideTimes;
            public int jumpTimes;
            public int airJumpTimes;
            public int superWeaponUseTimes;
            public int originMatchId;
            public bool isTeamUp;
            public RepeatedField<int> carryItems;
            public RepeatedField<CommonItem> items;
        }
        
        public struct Event_WarEndTeamPlayerList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_WarEndTeamPlayerList>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<WarResultData> WinTeamList;
            public List<WarResultData> LoseTeamList;
        }
        
        public struct Event_PlayerLastKill : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_PlayerLastKill>();

            public int GetID() => _id;
            // 下面写你的参数
            public SE_Mode.PlayModeCharacterData lastKillCharacter;
            public int AttackItemId;
        }

        public struct UpdateGameTime : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdateGameTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GameTime; // 秒
        }
        
        public struct UpdateRoundTime : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UpdateRoundTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public int RoundTime; // 秒
        }
        
        public struct Event_DropItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DropItem>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
            public IGPO DropGpo;
            public List<IGPO> AttackGPOList;
            public int DropId;
            public int DropItemId;
            public GpoDropCondition DropType;
            public Vector3 OR_DropPoint;
        }
    }
}