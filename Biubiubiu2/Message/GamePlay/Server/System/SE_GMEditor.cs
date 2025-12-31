using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_GMEditor {
        public struct Event_StartActiveSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartActiveSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        public struct Event_StartChangeWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartChangeWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Weapon1Id;
            public int Weapon2Id;
            public int SupportWeaponId;
        }
    }
}