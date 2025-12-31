using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_AbilityEffect {
        public struct Event_UpdateEffect : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateEffect>();
            public int GetID() => _id;

            // 下面写你的参数
            public AbilityEffectData.Effect Effect;
            public float Value;
        }
    }
}