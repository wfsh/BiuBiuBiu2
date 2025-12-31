using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_GPO {
        public struct AddGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO IGpo;
        }

        public struct RemoveGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        public struct GetGPOList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
        }

        public struct AddLocalGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddLocalGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO LocalGPO;
        }

        public struct GetLocalGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetLocalGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }

        public struct GetGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public Action<IGPO> CallBack;
        }

        public struct GetLookGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetLookGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }

        public struct AddLookGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddLookGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO LookGPO;
            public bool IsDrive;
        }

        public struct SendBloodSplatterData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SendBloodSplatterData>();

            public int GetID() => _id;
            // 下面写你的参数
            public int FireGpoId;
            public int HitGpoId;
            public Vector3 HitPoint;
            public int BloodValue;
            public int HitItemId;
        }

        public struct Event_AfterDownHP : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AfterDownHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
            public IGPO TargetGPO;
            public float ActualDownHp;
            public bool IsHead;
        }

    }
}