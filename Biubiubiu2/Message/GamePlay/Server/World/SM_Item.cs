using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Item {
        
        public struct Event_DropItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DropItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public ushort ItemNum;
            public Vector3 Point;
        }

        public struct Event_DiscardItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DiscardItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
            public int PickGPOID;
        }

        public struct Event_PickUpItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_PickUpItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
            public IGPO PickGpo;
        }

        public struct Event_PickUpItemForPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_PickUpItemForPoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public float Range;
            public IGPO PickGpo;
        }

        public struct Event_AddOwnItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AddOwnItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public string ItemSign;
            public ushort ItemNum;
            public bool IsProtect;
            public IGPO OwnGPO;
            public Action<ItemData.OwnItemData> AddCallBack;
        }

        public struct Event_UseItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_UseItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public IGPO UseGPO;
            public int AutoItemId;
            public ushort UseItemNum;
            public Action<ItemData.OwnItemData> UseCallBack;
        }
        
        public struct Event_DropItemDataForPlayerId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DropItemDataForPlayerId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public long PlayerId;
        }

    }
}