using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Skill {
        public struct Event_SetSuperWeaponTip : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_SetSuperWeaponTip>();
        
            public int GetID() => _id;
            // 下面写你的参数
            public string Tip;
        }
    }
}