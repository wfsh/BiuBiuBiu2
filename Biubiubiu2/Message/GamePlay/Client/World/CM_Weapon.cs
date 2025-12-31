using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Weapon {
        public struct AddWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WeaponItemId;
            public int WeaponSkinItemId;
            public int WeaponId;
            public IGPO UseGPO;
            public Action<IWeapon> CallBack;
        }

        public struct UseWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<UseWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WeaponId;
            public Action<bool, IWeapon> CallBack;
        }

        public struct RemoveWeapon : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveWeapon>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WeaponId;
        }

        public struct ShowGunSight : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ShowGunSight>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Index;
        }

        public struct HideGunSight : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HideGunSight>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Index;
        }

        public struct AllBulletList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AllBulletList>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<WeaponData.UseBulletData> BulletList;
        }

        public struct SetUseBullet : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetUseBullet>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UseBullet;
        }

        public struct BulletChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BulletChange>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct OnClickUseBullet : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OnClickUseBullet>();

            public int GetID() => _id;
            // 下面写你的参数
            public int BulletId;
        }

        public struct BulletFire : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BulletFire>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO FireGPO;
        }
    }
}