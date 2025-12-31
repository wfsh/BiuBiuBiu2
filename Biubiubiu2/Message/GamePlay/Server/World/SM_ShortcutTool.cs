using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_ShortcutTool {
        public struct Event_GameOver : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GameOver>();

            public int GetID() => _id;

            // 下面写你的参数
        }
        public struct Event_TeamScore : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_TeamScore>();

            public int GetID() => _id;

            // 下面写你的参数
            public int TeamId;  
            public int TeamScore;
        }
        
        public struct Event_MonsterFire : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_MonsterFire>();

            public int GetID() => _id;

            // 下面写你的参数
        }
        
        public struct Event_CharacterLockBlood : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_CharacterLockBlood>();

            public int GetID() => _id;

            // 下面写你的参数
            public long GpoId;
            public bool IsLocked;
        }
        
        public struct Event_EquipWeaponChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_EquipWeaponChange>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public int Index;
            public int WeaponItemId;
        }
        
        public struct Event_EquipSkillChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_EquipSkillChange>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public int WeaponItemId;
        }
        
        public struct Event_ActivateSuperWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_ActivateSuperWeapon>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
        }
    }
}