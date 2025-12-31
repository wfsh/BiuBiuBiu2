using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Game {
        public struct SaveItemData {
            public int ItemId;
            public ushort ItemNum;
            public bool IsQuickUse;
        }

        public struct Event_GetSaveItemData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetSaveItemData>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<SaveItemData>> CallBack;
        }

        public struct Event_GetSaveWeaponData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetSaveWeaponData>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<int>> CallBack;
        }

        public struct SaveMonsterData {
            public int MonsterLevel;
            public string MonsterSign;
        }

        public struct Event_GetSaveMonsterData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetSaveMonsterData>();
            public int GetID() => _id;
            // 下面写你的参数

            public Action<List<SaveMonsterData>> CallBack;
        }
    }
}