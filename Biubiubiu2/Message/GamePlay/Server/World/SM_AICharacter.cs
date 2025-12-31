using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_AICharacter {
        public struct AddAICharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddAICharacter>();

            public int GetID() => _id;

            // 下面写你的参数
            public int AiId;
            public string Name;
            public string TeamId;
        }
        
        public struct RemoveAICharacter : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveAICharacter>();

            public int GetID() => _id;

            // 下面写你的参数
            public int AiId;
        }
    }
}