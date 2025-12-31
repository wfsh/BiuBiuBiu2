using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_AbilityEffect {
        public struct Event_AddEffect : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddEffect>();

            public int GetID() => _id;

            // 下面写你的参数
            public AbilityEffectData.Effect Effect;
            public float Value;
            public Action<IGPOAbilityEffectData> CallBack;
        }
        
        public struct Event_UpdateEffect : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateEffect>();

            public int GetID() => _id;

            // 下面写你的参数
            public AbilityEffectData.Effect Effect;
            public float Value;
        }
        
        public struct Event_ResetAbilityEffect : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ResetAbilityEffect>();

            public int GetID() => _id;

            // 下面写你的参数
        }
    }
}