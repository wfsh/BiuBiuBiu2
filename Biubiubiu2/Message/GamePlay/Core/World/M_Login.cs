using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Login {
        public struct LoginGameServerState : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoginGameServerState>();

            public int GetID() => _id;

            // 下面写你的参数
            public int LoginState;
        }

        public struct LoadBaseAssetFinish : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadBaseAssetFinish>();

            public int GetID() => _id;
            // 下面写你的参数
        }
    }
}