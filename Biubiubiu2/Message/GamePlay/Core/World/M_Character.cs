using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Character {
        public struct SetNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public INetworkCharacter iNetwork;
        }
        public struct DestoryNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<DestoryNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public byte NetworkId;
        }
    }
}