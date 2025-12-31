using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Item {
        public struct GetItemCountForPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetItemCountForPoint>();

            public int GetID() => _id;

            // 下面写你的参数
            public float Range;
            public Vector3 Point;
            public Action<int> CallBack;
        }
        public struct DropItemData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<DropItemData>();

            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public Vector3 Point;
        }
        public struct MaxGetDropItemNum : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MaxGetDropItemNum>();

            public int GetID() => _id;

            // 下面写你的参数
            public int MaxDropItemNum;
        }
        
        public struct ResourceItemNumChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ResourceItemNumChange>();

            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public int ChangeNum;
        }
        
        public struct GetResourceItemNum : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetResourceItemNum>();

            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public int Num;
        }
    }
}