using System;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Mono {
        public struct Destroy : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Destroy>();

            public int GetID() => _id;
            // 下面写你的参数
        }
    }
}