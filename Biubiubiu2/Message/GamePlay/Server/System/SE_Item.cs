using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Item {
        // 获取所有物品（不含已经装备的武器，飞高高）
        public struct Event_GetPackItemList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetPackItemList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<ItemData.OwnItemData>> CallBack;
        }

        // 获取所有快捷物品
        public struct Event_GetQuickPackItemList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetQuickPackItemList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<ItemData.OwnItemData>> CallBack;
        }

        // 根据物品类型获取所有物品
        public struct Event_GetALLItemForType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetALLItemForType>();
            public int GetID() => _id;

            // 下面写你的参数
            public short ItemType;
            public Action<List<ItemData.OwnItemData>> CallBack;
        }

        public struct Event_GPOPickUpItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GPOPickUpItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public ItemData.OwnItemData Item;
        }

        // 添加角色物品
        public struct Event_AddPickItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddPickItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public int SkinItemId; // 可选，没有皮肤可以不填
            public ushort ItemNum;
            public bool IsQuickUse;
            public bool IsProtect;
        }

        // 物品背包是否还能够容纳物品
        public struct Event_CanInItemPack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CanInItemPack>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        // 消耗子弹
        public struct Event_DownBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DownBullet>();
            public int GetID() => _id;

            // 下面写你的参数
            public int UseBullet;
            public ushort BulletNum;
            public Action<int> CallBack;
        }

        // 掉落所有没有保护的物品
        public struct Event_DropUnprotectedItems : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DropUnprotectedItems>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsDrop; // 丢弃的物品是否掉落在地上
        }

        // 删除物品
        public struct Event_RemoveItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        // 删除物品
        public struct Event_OnRemoveItemForItemId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnRemoveItemForItemId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
        }

        // 删除快捷物品
        public struct Event_RemoveQuickPackItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveQuickPackItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        // 添加快捷物品
        public struct Event_AddQuickPackItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddQuickPackItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        //  拾取范围内的物品
        public struct Event_OnPickItemForRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnPickItemForRange>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        // 拾取物品成功
        public struct Event_PickItemSuccess : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PickItemSuccess>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
            public int ItemId;
            public int ItemNum;
        }

        //  物品数量变更
        public struct Event_ItemNumChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ItemNumChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
            public int ItemNum;
        }

        //  卸下快捷栏物品
        public struct Event_OnRemoveQuickPackItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnRemoveQuickPackItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        //  装备物品
        public struct Event_OnEquipItem: GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnEquipItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        //  使用投掷物
        public struct Event_OnUseHoldBall: GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnUseHoldBall>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
            public Vector3[] Points;
        }

        //  使用技能道具
        public struct Event_OnUseAbility: GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnUseAbility>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }
        
        // 更新物品数据
        public struct Event_UpdateItems : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateItems>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ItemData.OwnItemData> ItemList;
        }
        
        public struct Event_OnDropItemData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnDropItemData>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public IGPO DropGPO;
        }
        
        public struct Event_GetDropItemList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDropItemList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<int>> CallBack;
        }
        
        // 添加角色物品
        public struct Event_AddWeaponForMode : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddWeaponForMode>();
            public int GetID() => _id;

            // 下面写你的参数
            public SE_Mode.PlayModeCharacterWeapon WeaponData;
        }

        public struct Event_GetDropBoxId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDropBoxId>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }
        
        public struct Event_DropItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DropItem>();
            public int GetID() => _id;
            // 下面写你的参数
            public int DropTypeId;
            public IGPO DropGpo;
            public int DropItemId;
            public Vector3 OR_DropPoint;
        }

        public struct Event_SetDropData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetDropData>();
            public int GetID() => _id;
            // 下面写你的参数
            public int[] DropIdList;
            public ushort DropTypeId;
        }
    }
}