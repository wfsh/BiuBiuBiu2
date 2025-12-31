using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Level {
        public struct NotifyPlayerExpUp : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyPlayerExpUp>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct RefreshLevelInfo : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RefreshLevelInfo>();
            public int GetID() => _id;
            // 下面写你的参数
            public short level;
            public int exp;
        }
        
        public struct NotifyTaskComplete : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyTaskComplete>();
            public int GetID() => _id;
            // 下面写你的参数
            public Protocol.Task.LevelTask task;
        }
        
        public struct RefreshReceivedStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RefreshReceivedStatus>();
            public int GetID() => _id;
            // 下面写你的参数
            public short level;
            public int exp;
        }
        
        public struct RefreshGameFunctionStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RefreshGameFunctionStatus>();
            public int GetID() => _id;
            // 下面写你的参数
        } 
    }
}
