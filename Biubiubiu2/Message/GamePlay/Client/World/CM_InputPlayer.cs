using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_InputPlayer {
        public struct Move : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Move>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Px;
            public float Pz;
        }
        public struct MoveUp : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MoveUp>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct MoveDown : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MoveDown>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct Jump : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Jump>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Dodge : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Dodge>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Crouch : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Crouch>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Prone : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Prone>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Wirebug : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Wirebug>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Reload : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Reload>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Fire : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Fire>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDown;
        }
        public struct Fire2 : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Fire2>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDown;
        }

        public struct Throw : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Throw>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDown;
        }

        public struct UseWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UseWeapon>();
            public int GetID() => _id;
            // 下面写你的参数

            public IWeapon Weapon;
        }
        
        public struct UseMelee : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UseMelee>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct HoldOn : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HoldOn>();
            public int GetID() => _id;
            // 下面写你的参数
            public ItemData.PickItemData ItemData;
        }
        public struct EnabledMelee : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledMelee>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct EnabledShowPickItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledShowPickItem>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct EnabledGun : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledGun>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
            public IWeapon Weapon;
        }
        public struct EnabledDevice : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledDevice>();
            public int GetID() => _id;
            // 下面写你的参数
            public IGPO Device;
        }
        public struct EnabledHoldOn : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledHoldOn>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct EnabledTakeOnMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EnabledTakeOnMonster>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct MoveCanceled : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MoveCanceled>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct PickItem : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PickItem>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct AutoLockAngle : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AutoLockAngle>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Angle;
        }
        
        public struct AutoLockSpeed : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AutoLockSpeed>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Speed;
        }
        
        public struct AutoFireAngle : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AutoFireAngle>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Angle;
        }
        
        public struct CameraRatio : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CameraRatio>();
            public int GetID() => _id;
            // 下面写你的参数
            public float speed;
        }
    }
}