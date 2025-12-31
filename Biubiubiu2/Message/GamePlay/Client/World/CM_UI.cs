using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_UI {
        public struct IsFullScreenUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<IsFullScreenUI>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        
        public struct ShowConnect : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowConnect>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsShow;
        }
        
        public struct ShowDialog : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowDialog>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Message;
            public string BtnSureText;  // 确认按钮文字
            public string BtnCancelText;    // 取消按钮文字
            public Action OnSure; // 确认按钮回调
            public Action OnCancel; // 取消按钮回调
        }
        
        public struct GotoEquipDepot : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GotoEquipDepot>();
            public int GetID() => _id;
            // 下面写你的参数
            public long EquipPlayerItemId;
        }
        
        public struct SetConnectMessage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetConnectMessage>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Message;
        }
        public struct GetNewWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetNewWeapon>();
            public int GetID() => _id;
            // 下面写你的参数
            public Protocol.Common.Weapon WeaponProto;
        }

        public struct ShowToast : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowToast>();
            public int GetID() => _id;
            // 下面写你的参数
            public string Message;
        }
        
        public struct GetNewPlayerReward : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetNewPlayerReward>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct GetEquipWeaponNoSurWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetEquipWeaponNoSurWeapon>();
            public int GetID() => _id;
            // 下面写你的参数
            public int count;
        }

        public struct ShowMaintainAnnouncement : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowMaintainAnnouncement>();
            public int GetID() => _id;
            // 下面写你的参数
            public string MaintainInfo;
        }
        
        public struct GetRewardedVideoAdCount : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetRewardedVideoAdCount>();
            public int GetID() => _id;
            // 下面写你的参数
            public int CurAdCount;
            public int Limit;
            public int AdId;
        }
        
        public struct ShowSeasonRewardHub : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowSeasonRewardHub>();
            public int GetID() => _id;
            // 下面写你的参数
            public List<int> RewardIds;
        }

        public struct OpenShowRankSettleInfo : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenShowRankSettleInfo>();
            public int GetID() => _id;
        }

        public struct ShowAcquiredItems : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowAcquiredItems>();
            public const int TYPE_NORMAL = 0;
            public const int TYPE_RED_WEAPON = 1;
            public const int TYPE_LOOT_BOX = 2;
            public const int TYPE_STACKED_ITEM = 3;

            public int GetID() => _id;
            // 下面写你的参数
            public List<int> RewardIds;
            public int Type; // 0.普通武器，可堆叠物品 1.红色武器 2.宝箱 3.堆叠道具 
            public string Title;
            public List<ItemData.RewardItemData> StackedItems; // 叠加物品
        }

        public struct ShowAcquiredRedWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowAcquiredRedWeapon>();
            public int GetID() => _id;
            // 下面写你的参数
            public Protocol.Common.Weapon WeaponProto;
            public bool isSkip;
        }

        public struct CloseAllEmailRedWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseAllEmailRedWeapon>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct CloseUIView : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseUIView>();
            public int GetID() => _id;
            public string FunctionSign;
        }
        
        public struct CloseWarUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseWarUI>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct CloseRoomUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseRoomUI>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct OpenWarUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenWarUI>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct OpenUIView : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenUIView>();

            public int GetID() => _id;
            // 下面写你的参数
            public string FunctionSign;
        }
        
        public struct OpenRoomUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenRoomUI>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct EnterWarFailed : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnterWarFailed>();

            public int GetID() => _id;
            // 下面写你的参数
            public ModeData.ModeLoginState State;
            public string WarId;
        }

        public struct ShowWarEndItemShowcase : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowWarEndItemShowcase>();

            public int GetID() => _id;
            public List<int> ItemIds;
        }
        
        public struct ShareCustomScreenShot : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShareCustomScreenShot>();

            public int GetID() => _id;
            // 下面写你的参数
            public RectTransform RectTransform;
            public string Title;
            public string Query;
        }
        
    }
}