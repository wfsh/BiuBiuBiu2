using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_WarReport {
        public struct PlayBegin : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayBegin>();
            public int GetID() => _id;
            // 下面写你的参数
            public float MaxPlayTime; // 最大游戏时间
        }
        public struct GetPlayBeginState : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetPlayBeginState>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float> Callback; // 回调函数，返回当前游戏时间
        }
        
        public struct PlayTick : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayTick>();
            public int GetID() => _id;
            // 下面写你的参数
            public float PlayTime; // 当前游戏时间
        }
        
        public struct StopPlay : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StopPlay>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsStop;
        }
        
        public struct PlaySpeedScale : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlaySpeedScale>();
            public int GetID() => _id;
            // 下面写你的参数
            public int SpeedScale; // 播放速度缩放
        }
    }
}