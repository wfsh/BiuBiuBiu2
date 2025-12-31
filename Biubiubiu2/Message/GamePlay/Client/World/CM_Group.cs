using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Protocol.Common;
using Sofunny.BiuBiuBiu2.Protocol.Match;
using Sofunny.BiuBiuBiu2.Protocol.Player;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Group {
        public struct OutGetGroupInfo : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OutGetGroupInfo>();

            public int GetID() => _id;
            // 下面写你的参数
            public GetGroupInfoOut GroupInfo;
            public int GameMode;
        }

        public struct RequestGetGroupInfo : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RequestGetGroupInfo>();

            public int GetID() => _id;
            public int GameMode;
        }

        public struct NotifyGroupStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyGroupStatus>();

            public int GetID() => _id;
            // 下面写你的参数
            public long LeaderId;
            public long GroupId;
            public GroupStatus GroupStatus;
            public int GameMode;
        }
        
        public struct OutGetPlayerInfo : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OutGetPlayerInfo>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<PlayerInfo> PlayerInfos;
            public int GameMode;
        }
        
        public struct NotifyPlayerStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyPlayerStatus>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public PlayerStatus PlayerStatus;
            public int GameMode;
        }
        
        public struct NotifyPlayerWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyPlayerWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public long GroupId;
            public int WeaponId1;
            public int WeaponId2;
        }
        
        public struct InKickRoom : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<InKickRoom>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public long GroupId;
            public int GameMode;
        }
        
        public struct InQuitGroup : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<InQuitGroup>();

            public int GetID() => _id;
            // 下面写你的参数
            public long GroupId;
            public int GameMode;
        }
        
        public struct OutQuitGroup : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OutQuitGroup>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GameMode;
        }
        
        public struct RequestOpenInviteHub : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RequestOpenInviteHub>();

            public int GetID() => _id;
        }
        
        public struct Matchmaking : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Matchmaking>();

            public int GetID() => _id;
            public int GameMode;
            public int MapId;
            public bool isMatchmaking;
        }
        
        public struct OutStartMatch : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OutStartMatch>();

            public int GetID() => _id;
            public StartMatchStatus StartMatchStatus;
        }
        
        public struct OutCancelMatch : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OutCancelMatch>();

            public int GetID() => _id;
            public bool isSuceess;
        }
            
        public struct NotifyMatchResult : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyMatchResult>();

            public int GetID() => _id;
            public MatchResult MatchResult;
            public string WarId;
        }
        
        public struct ChangePlayMode : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ChangePlayMode>();

            public int GetID() => _id;
            public int GameMode;
        }
        
        public struct InviteFriendTest : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<InviteFriendTest>();

            public int GetID() => _id;
            public int GameMode;
            public long PlayerId;
            public long GroupId;
        }
        
        public struct GetGroupMemberCount : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGroupMemberCount>();

            public int GetID() => _id;
            // 下面写你的参数
            public int MemberCount;
            public int GameMode;
        }

        public struct EnterWar : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnterWar>();

            public int GetID() => _id;

            // 下面写你的参数
            public string WarId;
            public string Host;
            public ushort Port;
            public int MatchId;
            public string ProxyAddr;
            public Hashtable NetConfig;
        }
        
        public struct NotifyMatchmakingStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyMatchmakingStatus>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsMatchmaking;
            public int GameMode;
        }
        
        public struct StartMatchmaking : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartMatchmaking>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsMatchmaking;
            public int GameMode;
        }
        
        public struct NotifyCancelMatch : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyCancelMatch>();

            public int GetID() => _id;
        }
    }
}

