using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    /// <summary>
    /// 子弹种类数据
    /// </summary>
    public class BulletData {
        public const int Bullet_Gun = 1;
        public const int Bullet_Gatling = 2;
        public const int Bullet_Rpg = 3;
        public const int Bullet_Particlecannon = 4;
        public const int Bullet_EnergySg = 5;
        public const int Bullet_FocusGun = 6;
        public const int Bullet_FireGun = 7;
        public const int Bullet_SplitGun = 8;
        public const int Bullet_FlashGun = 9;
        public const int Bullet_MachineGun = 10;
        public const int Bullet_M24 = 11;
        
        public class Data {
            public int BulletId;
            public int UsedItemID;
            public string Name;
            public int MagazineGridNum;  // 弹夹格子数量
            public float IntervalTimeRatio; // 射击间隔增加的比例
            public byte AbilityBulletID;
        }

        public static List<Data> datas = new List<Data>() {
            new Data() {
                BulletId = Bullet_Gun, UsedItemID = ItemSet.Id_GunBullet, Name = "常", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletGun
            },
            new Data() {
                BulletId = Bullet_Gatling, UsedItemID = ItemSet.Id_GunBullet, Name = "常", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletGatling
            },
            new Data() {
                BulletId = Bullet_FocusGun, UsedItemID = ItemSet.Id_GunBullet, Name = "能", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletFocusGun,
            },
            new Data() {
                BulletId = Bullet_Rpg, UsedItemID = ItemSet.Id_GunBullet, Name = "RPG", IntervalTimeRatio = 10.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletRPG,
            },
            new Data() {
                BulletId = Bullet_Particlecannon, UsedItemID = ItemSet.Id_GunBullet, Name = "粒子", IntervalTimeRatio = 10.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletParticlecannon,
            },
            new Data() {
                BulletId = Bullet_EnergySg, UsedItemID = ItemSet.Id_GunBullet, Name = "弩", IntervalTimeRatio = 10.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletEnergySG,
            },
            new Data() {
                BulletId = Bullet_FireGun, UsedItemID = ItemSet.Id_GunBullet, Name = "喷火枪", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletGun,
            },
            new Data() {
                BulletId = Bullet_SplitGun, UsedItemID = ItemSet.Id_GunBullet, Name = "爆裂枪", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletGun,
            },
            new Data() {
                 BulletId = Bullet_FlashGun, UsedItemID = ItemSet.Id_GunBullet, Name = "闪电枪", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletFlashGun,
            },
            new Data() {
                BulletId = Bullet_MachineGun, UsedItemID = ItemSet.Id_GunBullet, Name = "重型防御机枪", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_BulletFlashGun,
            },
            new Data() {
                BulletId = Bullet_M24, UsedItemID = ItemSet.Id_GunBullet, Name = "NB-M24狙击枪", IntervalTimeRatio = 1.0f, MagazineGridNum = 1, AbilityBulletID = AbilityM_Bullet.ID_JokerSniperBullet,
            },
        };

        public static Data GetBulletData(int bulletId) {
            for (int i = 0; i < datas.Count; i++) {
                var data = datas[i];
                if (data.BulletId == bulletId) {
                    return data;
                }
            }
            return null;
        }
    }
}