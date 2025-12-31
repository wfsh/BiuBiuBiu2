using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Game {
        public struct GameEngineClose : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GameEngineClose>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct LoginGameScene : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoginGameScene>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct LoginGameServer : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoginGameServer>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct HideScene : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HideScene>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct HideTerrain : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HideTerrain>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct HideAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HideAbility>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct AddGameTestObj : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddGameTestObj>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GameTestIndex;
            public GameObject GameObj;
        }
        public struct ShowWarEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowWarEnd>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct HandlerErrorUseTime : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HandlerErrorUseTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public float UseTime;
        }
    }
}