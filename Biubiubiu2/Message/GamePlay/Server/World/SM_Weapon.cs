using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Weapon {
        public struct Event_AddWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AddWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WeaponItemId;
            public int WeaponSkinItemId;
            public IGPO UseGPO;
            public Action<IWeapon> CallBack;
        }

        public struct Event_RemoveWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_RemoveWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WeaponId;
        }

        public struct Event_Fire : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_Fire>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO FireGpo;
        }
    }
}