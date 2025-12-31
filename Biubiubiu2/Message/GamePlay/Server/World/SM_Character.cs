using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Character {

        public struct AddCharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddCharacter>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetworkCharacter INetwork;
            public ICmd ICmdData;
        }

        public struct CharacterLogin : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CharacterLogin>();

            public int GetID() => _id;

            // 下面写你的参数
            public INetworkCharacter INetwork;
            public int GpoID;
        }

        public struct GetCharacterDataByPlayerId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCharacterDataByPlayerId>();
            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public Action<SE_Mode.PlayModeCharacterData> CallBack;
        }
        
        public struct GetCharacterDataByGpoId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCharacterDataByGpoId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public Action<SE_Mode.PlayModeCharacterData> CallBack;
        }
        public struct CmdLoginStage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CmdLoginStage>();

            public int GetID() => _id;

            // 下面写你的参数
            public INetworkCharacter INetwork;
            public long PlayerId;
            public string Name;
            public string TeamId;
        }
        
        public struct AddCharacterGoldDash : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddCharacterGoldDash>();

            public int GetID() => _id;

            // 下面写你的参数
            public INetworkCharacter INetwork;
            public long PlayerId;
            public string Name;
            public string TeamId;
        }

        public struct GetCharacterForePlayerId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCharacterForePlayerId>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public Action<IGPO> CallBack;
        }
        
        public struct ChangeLockSausageRole : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ChangeLockSausageRole>();

            public int GetID() => _id;

            // 下面写你的参数
            public INetworkCharacter INetwork;
            public long PlayerId;
            public long LockPlayerId;
        }
    }
}