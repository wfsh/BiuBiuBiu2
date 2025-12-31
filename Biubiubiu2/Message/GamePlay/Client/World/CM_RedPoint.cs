using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_RedPoint {
        public struct NotifyRedPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyRedPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public List<Protocol.RedPoint.RedPointData> redPointDatas;
        }
    }
}