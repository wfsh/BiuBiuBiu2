using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_GameSetting {
        public struct SettingValueChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SettingValueChange>();
            public int GetID() => _id;
            public string sign;
        }
    }
}
