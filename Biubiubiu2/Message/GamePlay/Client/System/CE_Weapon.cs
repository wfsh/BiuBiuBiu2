using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Weapon {
        public struct StartFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StartFire>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct SetBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetBullet>();

            public int GetID() => _id;
            // 下面写你的参数
            public int BulletNum;
        }
        public struct GetFireDistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetFireDistance>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct GetFireOverHotTimer : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetFireOverHotTimer>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        
        public struct CanReload : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CanReload>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
        
        public struct GetReloadTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetReloadTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        
        public struct GetAllBulletList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetAllBulletList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<WeaponData.UseBulletData>> CallBack;
        }
        
        public struct AllBulletList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AllBulletList>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<WeaponData.UseBulletData> BulletList;
        }       
        
        public struct Fire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Fire>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 StartPoint;
            public Vector3 TargetPoint;
        }
        
        public struct EndFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<EndFire>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct OnReload : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OnReload>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsReload;
            public float ReloadTime;
        }
        
        public struct OutReloadTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OutReloadTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public float ReloadTime;
        }
        
        public struct SetUseBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetUseBullet>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UseBullet;
        }
        public struct GetUseBulletData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUseBulletData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<WeaponData.UseBulletData> CallBack;
        }
        
        public struct OnUseBulletData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OnUseBulletData>();

            public int GetID() => _id;
            // 下面写你的参数
            public WeaponData.UseBulletData UseBulletData;
        }
        
        public struct UpdatePickItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UpdatePickItem>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<ItemData.PickItemData> PickItemList;
        }
        public struct UseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public IWeapon weapon;
        }
        public struct GetUseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<IWeapon> CallBack;
        }
        public struct HasPackWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HasPackWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int WeaponId;
            public Action<int, bool> CallBack;
        }
        public struct UpdateBulletNum : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UpdateBulletNum>();

            public int GetID() => _id;
            // 下面写你的参数
            public WeaponData.UseBulletData UseBulletData;
        }
        public struct SetWeaponAttribute : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetWeaponAttribute>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ATK;
            public int MagazineNum;
            public float IntervalTime;
            public float ReloadTime;
            public float FireDistance;
            public float FireOverHotTime;
        }
        
        public struct GetFireBox : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetFireBox>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Transform> CallBack;
        }
        
        public struct OutFireOverHotTimer : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OutFireOverHotTimer>();

            public int GetID() => _id;
            // 下面写你的参数
            public float NowOverHotTimer;
            public float MaxOverHotTime;
            public bool IsCooling;
        }
        public struct Event_GetGunMoveSpeed : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGunMoveSpeed>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float> CallBack;
        }

        public struct Event_BulletSetFireGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_BulletSetFireGPO>();
            public int GetID() => _id;
            // 下面写你的参数
        }
    }
}
