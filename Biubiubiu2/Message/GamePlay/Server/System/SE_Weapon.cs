using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Weapon {
        public struct Event_BatLoopAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_BatLoopAttack>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool IsAttack;
        }

        public struct Event_BatEndAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_BatEndAttack>();

            public int GetID() => _id;

            // 下面写你的参数
            public Quaternion StartRota;
            public Vector3 StartPoint;
        }

        public struct Event_PlungerAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlungerAttack>();

            public int GetID() => _id;

            // 下面写你的参数
            public Quaternion StartRota;
        }

        public struct Event_PlungerEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlungerEnd>();

            public int GetID() => _id;

            // 下面写你的参数
            public Quaternion StartRota;
            public Vector3 StartPoint;
        }

        public struct Event_GunFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GunFire>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 StartPoint;
            public Vector3 TargetPoint;
        }

        public struct Event_OnReload : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnReload>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_ReloadAllBulletNoTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ReloadAllBulletNoTime>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_PlayFireAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayFireAbility>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 StartPoint;
            public Vector3 TargetPoint;
        }

        public struct Event_Fire: GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_Fire>();

            public int GetID() => _id;

            // 下面写你的参数
            public WeaponData.UseBulletData BulletData; // 使用的子弹类型
        }

        public struct Event_FireEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireEnd>();

            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_Reload : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_Reload>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool isTrue;
        }

        public struct Event_PutOn : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PutOn>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_SetALLBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetALLBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public int UseItemId;
            public int BulletNum;
        }

        public struct Event_UseBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public WeaponData.UseBulletData Data;
        }

        public struct Event_EnabledAutoFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_EnabledAutoFire>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
            public float StartFireWaitTime;
        }

        public struct Event_SetUseBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetUseBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public int UseBullet;
        }

        public struct Event_NowBulletNum : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_NowBulletNum>();

            public int GetID() => _id;

            // 下面写你的参数
            public ushort BulletNum;
            public int UseBullet;
            public int WeaponId;
        }

        public struct Event_OnDownBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnDownBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public int UseBullet;
            public ushort BulletNum;
            public Action<int> CallBack;
        }

        public struct Event_GetALLBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetALLBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public IWeapon iWeapon;
        }

        public struct Event_GetUseBulletData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetUseBulletData>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<WeaponData.UseBulletData> CallBack;
        }

        public struct Event_UpdateItems : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateItems>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ItemData.OwnItemData> ItemList;
        }

        public struct Event_SetRandomAttributeData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetRandomAttributeData>();
            public int GetID() => _id;

            // 下面写你的参数
            public SE_Mode.PlayModeCharacterWeapon Data;
        }

        public struct Event_GetATK : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetATK>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<int> CallBack;
        }
        
        public struct Event_GetHitHeadMultiplier : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHitHeadMultiplier>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_GetRandomATK : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetRandomATK>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<int> CallBack;
        }

        public struct Event_GetRandomDiffusionReduction : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetRandomDiffusionReduction>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_GetAttackRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAttackRange>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_GetAddEffectFireDistanceRate : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAddEffectFireDistanceRate>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_GetGunMoveSpeed : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGunMoveSpeed>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_GetMagazineNum : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMagazineNum>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<int> CallBack;
        }

        public struct Event_UpdateMagazineNum : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateMagazineNum>();
            public int GetID() => _id;

            // 下面写你的参数
            public int MagazineNum;
        }

        public struct Event_UpdateReloadTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateReloadTime>();
            public int GetID() => _id;

            // 下面写你的参数
            public float ReloadTime;
        }

        public struct Event_UpdateIntervalTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateIntervalTime>();
            public int GetID() => _id;

            // 下面写你的参数
            public float IntervalTime;
        }

        public struct Event_FireOverHotTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireOverHotTime>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FireOverHotTime;
        }

        public struct Event_UpdateFireRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateFireRange>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FireRange;
        }

        public struct Event_UpdateFireDistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateFireDistance>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FireDistance;
        }

        public struct Event_UpdateFireAngle : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateFireAngle>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FireAngle;
        }

        public struct Event_GetGunAttribute : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGunAttribute>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<int, int, float, float, float, float, float> CallBack;
        }

        public struct Event_GetGunFireRecord : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGunFireRecord>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<int, int> CallBack; // 1 fireCount, 2 hitCount
        }
    }
}