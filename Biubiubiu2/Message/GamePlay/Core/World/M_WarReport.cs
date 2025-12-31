using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_WarReport {
        public struct SetRpcReportData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetRpcReportData>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
            public byte TargetNetworkId;
            public byte[] Bytes;
            public int ConnID;
            public byte Channel;
        }
        
        public struct SetTargetRpcReportData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetTargetRpcReportData>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
            public byte TargetNetworkId;
            public byte[] Bytes;
            public int ConnID;
            public byte Channel;
        }
        public struct SetCharacterSyncData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetCharacterSyncData>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
            public WarReportData.CharacterSyncType SyncType;
            public string Value;
        }
        public struct SetCharacterPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetCharacterPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
            public Vector3 Point;
        }
        public struct SetCharacterRota : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetCharacterRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
            public Quaternion Rota;
        }
    }
}