using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.Data {
    public class WeaponData {
        public const int Id_AWM = 10001; // AWM
        
        private static readonly CanUseBulletData[] unlimitedGunBulletData = {
            new CanUseBulletData() {
                BulletId = BulletData.Bullet_Gun, isUnlimited = true,
            },
        };

        public enum WeaponType {
            Gun,
            Melee,
        }

        public class Data {
            public int ID;
            public int ItemId;
            public string ItemSign;
            public WeaponType WeaponType;
            public int ATK;
            public int DEF;
            public int Hp;
            public float AttackRange;
            public float HitHeadMultiplier;
            public float IntervalTime; // 攻击间隔
        }

        // 圆形准星
        public const int SightStyle_Circle = 1;

        // 横排准星
        public const int SightStyle_Row = 2;

        public class GunData : Data {
            public float StartFireWaitTime; // 开火等待时间
            public float FireRange; // 扩散
            public float ReloadTime; // 重新装弹时间
            public float FireDistance; // 射程
            public int MagazineNum; // 弹夹最大容量
            public int FireBulletNum; // 每次射击的子弹数量 (普通枪 1 ，霰弹枪 N)
            public bool IsCanAutoLock; // 是否可以自动吸附
            public bool IsCanAutoFire; // 是否可以自动开火
            public int AutoFireAngle; // 自动开火角度
            
            public float FireOverHotTime; // 过热时间
            public float FireOverHotCDTime; // 过热 CD 时间
            public float BulletSpeed; // 子弹速度
            public float MoveSpeed; // 移动速度
            public int SightStyle; // 瞄准方式
            public float SoundMaxDistance; // 枪声最大传播范围
            public CanUseBulletData[] CanUseBullet; // 可以使用的子弹类型
            public List<BulletAttnMap> BulletAttnMaps; // 子弹伤害比例
        }

        public class BulletAttnMap {
            public float MinDistanceRatio;
            public float MaxDistanceRatio;
            public float MinHurtRatio;
            public float MaxHurtRatio;
        }

        public class CanUseBulletData {
            public int BulletId;
            public bool isUnlimited;
        }

        public class UseBulletData {
            public int BulletId;
            public int UseItemId;
            public byte AbilityBulletID;
            public ushort UseMagazineNum;
            public int MaxBulletNum;
            public ushort UseBulletNum;
            public bool isUnlimited;
        }

        public static List<Data> datas = null;
        private static int advancedAttrQuality = 5;
        private static int myRedWeaponCount = 0;
        private static int myRedSuperWeaponCount = 0;

        // 已解锁的武器图鉴列表（itemid,已解锁且合出过的武器列表，只有试用装的不算）
        private static List<int> unlockCodexs;

        public static void Init() {
            if (datas != null) {
                return;
            }
            datas = new List<Data>();
            var keys = WeaponSet.Data;
            for (int i = 0, count = keys.Length; i < count; i++) {
                var cfg = keys[i];
                var gunData = new GunData();
                gunData.ID = cfg.Id;
                gunData.ItemId = cfg.ItemId;
                gunData.ItemSign = ItemSet.GetItemById(cfg.ItemId).Sign;
                gunData.ATK = Mathf.CeilToInt(cfg.Attack / (float)cfg.FireBulletNum); // 子弹伤害，固定 / 一次发射的数量
                gunData.AttackRange = cfg.BombRange; // 攻击后的爆炸范围
                gunData.IntervalTime = cfg.IntervalTime; // 射击间隔
                gunData.FireRange = cfg.FireRange; // 扩散
                gunData.HitHeadMultiplier = (float)cfg.HitheadMultiplier; // 击头倍率
                gunData.ReloadTime = cfg.ReloadTime; // 重新装弹时间
                gunData.BulletSpeed = cfg.BulletSpeed; // 子弹速度
                gunData.SightStyle = SightStyle_Circle; // 瞄准方式，默认圆形准星
                gunData.MagazineNum = cfg.MagazineNum; // 弹夹最大容量
                gunData.FireDistance = (float)cfg.FireDistance;
                gunData.FireBulletNum = cfg.FireBulletNum; // 每次射击的子弹数量 (普通枪 1 ，霰弹枪 N)
                gunData.SoundMaxDistance = (float)cfg.MaxAttenuation;
                gunData.FireOverHotTime = cfg.FireOverHotTime; // 过热
                gunData.FireOverHotCDTime = 0f; // 过热冷却时间
                gunData.MoveSpeed = cfg.MoveSpeed; // 持有不同武器时候，可能会影响移动速度
                gunData.Hp = cfg.Hp; // 使用武器召唤的怪物，会沿用武器赋值的血量
                gunData.IsCanAutoLock = true; // 是否可以自动吸附
                gunData.IsCanAutoFire = true; // 是否可以自动开火
                gunData.AutoFireAngle = 15;
                gunData.StartFireWaitTime = 0; // 开火等待时间，默认 0
                gunData.WeaponType = GetWeaponType(cfg.ItemTypeId); // 武器类型
                gunData.BulletAttnMaps = new List<BulletAttnMap>(); // 武器子弹伤害衰减
                var weaponDistanceHurt = WeaponDistanceHurtSet.GetWeaponDistanceHurtsByItemId(cfg.ItemId);
                foreach (var weaponHurt in weaponDistanceHurt) {
                    gunData.BulletAttnMaps.Add(new BulletAttnMap {
                        MinDistanceRatio = weaponHurt.MinDistanceRate * 0.01f,
                        MaxDistanceRatio = weaponHurt.MaxDistanceRate * 0.01f,
                        MinHurtRatio = weaponHurt.MinHurtRate * 0.01f,
                        MaxHurtRatio = weaponHurt.MaxHurtRate * 0.01f,
                    });
                }
                gunData.CanUseBullet = unlimitedGunBulletData;
                UpdateGunData(gunData.ItemId, gunData, true);
                datas.Add(gunData);
            }
            // AddDefaultWeapon();
        }

        private static WeaponType GetWeaponType(short itemTypeId) {
            return itemTypeId == ItemTypeSet.Id_MeleeWeapon? WeaponType.Melee : WeaponType.Gun;
        }

        private static void AddDefaultWeapon() {
            var tempWeapon = new GunData() {
                ItemId = Id_AWM, // 默认武器从 10000 开始
                ItemSign = "AWM",
                WeaponType = WeaponType.Gun,
                ATK = 100,
                IntervalTime = 0.1f,
                MagazineNum = 10,
                FireDistance = 300,
                ReloadTime = 5,
                FireRange = 2,
                BulletSpeed = 100,
                MoveSpeed =  6,
                FireBulletNum = 1,
                HitHeadMultiplier = 1.5f,
                AttackRange = 0,
                StartFireWaitTime = 0,
                IsCanAutoLock = true,
                IsCanAutoFire = true,
                FireOverHotTime = 0,
                FireOverHotCDTime = 0,
                SoundMaxDistance = 100,
                CanUseBullet = unlimitedGunBulletData,
                BulletAttnMaps = new List<BulletAttnMap>(), // 武器子弹伤害衰减
            };
            UpdateGunData(ItemSet.Id_ParticleCannon, tempWeapon, true); // 选择要测试的子弹
            datas.Add(tempWeapon);
        }

        private static void UpdateGunData(int ItemId, GunData weaponData, bool isUnlimited) {
            switch (ItemId) {
                case ItemSet.Id_Gatling:
                    weaponData.StartFireWaitTime = 1f;
                    weaponData.FireOverHotCDTime = 2f;
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_Gatling, isUnlimited = isUnlimited,
                        },
                    };
                    break;
                case ItemSet.Id_FocusGun:
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_FocusGun, isUnlimited = isUnlimited,
                        },
                    };
                    break;
                case ItemSet.Id_EnergySg:
                    weaponData.SightStyle = SightStyle_Row;
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_EnergySg, isUnlimited = isUnlimited,
                        },
                    };
                    break;
                case ItemSet.Id_ParticleCannon:
                    
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_Particlecannon, isUnlimited = isUnlimited,
                        },
                    };
                    break;
                case ItemSet.Id_M24:
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_M24, isUnlimited = isUnlimited,
                        },
                    };
                    weaponData.AutoFireAngle = 75;
                    break;
                case ItemSet.Id_Rpg:
                    weaponData.IsCanAutoLock = false;
                    weaponData.IsCanAutoFire = false;
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_Rpg, isUnlimited = isUnlimited
                        }
                    };
                    break;
                case ItemSet.Id_SplitGun:
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_SplitGun, isUnlimited = false,
                        },
                    };
                    break;
                case ItemSet.Id_FlashGun:
                    weaponData.CanUseBullet = new[] {
                        new CanUseBulletData {
                            BulletId = BulletData.Bullet_FlashGun, isUnlimited = false,
                        },
                    };
                    break;
            }
        }

        public static Data Get(int itemId) {
            for (int i = 0; i < datas.Count; i++) {
                var data = datas[i];
                if (data.ItemId == itemId) {
                    return data;
                }
            }
            Debug.LogError($"Get weapon data failed, itemId: {itemId}");
            return null;
        }

        public static Data GetID(int weaponId) {
            for (int i = 0; i < datas.Count; i++) {
                var data = datas[i];
                if (data.ID == weaponId) {
                    return data;
                }
            }
            Debug.LogError($"Get weapon data by ID failed, weaponId: {weaponId}");
            return null;
        }

        /// <summary>
        ///  获取武器基础有效射程
        /// </summary>
        /// <returns></returns>
        public static int GetEffectFireDistance(int itemId) {
            var dhurtData = WeaponDistanceHurtSet.GetWeaponDistanceHurtByItemIdMinDistanceRate(itemId, 0);
            var modeData = WeaponSet.GetWeaponByItemId(itemId);
            var ratio = 1f;
            if (dhurtData.Id != 0) {
                ratio = dhurtData.MaxDistanceRate * 0.01f;
            }
            return Mathf.CeilToInt(modeData.FireDistance * ratio);
        }

        /// <summary>
        ///  获取武器基础有效射程
        /// </summary>
        /// <returns></returns>
        public static int GetFireDistanceForRatio(int itemId, float ratio) {
            var modeData = (GunData)Get(itemId);
            return Mathf.CeilToInt(modeData.FireDistance * ratio);
        }

        /// <summary>
        /// 获取随机红色武器 Id
        /// </summary>
        /// <returns></returns>
        public static void GetRandomRedWeapon(out int weapon1Id, out int weapon2Id, out int superWeaponId) {
            var redWeaponIds = new List<int>();
            var redSuperWeaponIds = new List<int>();
            weapon1Id = weapon2Id = superWeaponId = 0;
            foreach (var weapon in datas) {
                var item = WeaponSet.GetWeaponByItemId(weapon.ItemId);
                if (item.Quality < WeaponQualitySet.Quality_Red) {
                    continue;
                }
                if (item.ItemTypeId == ItemTypeSet.Id_Weapon) {
                    redWeaponIds.Add(weapon.ItemId);
                } else if (item.ItemTypeId == ItemTypeSet.Id_SuperWeapon) {
                    redSuperWeaponIds.Add(weapon.ItemId);
                }
            }
            if (redWeaponIds.Count < 2 || redSuperWeaponIds.Count < 1) {
                Debug.LogError("红色武器数量不足");
                return;
            }
            // 随机第一把
            weapon1Id = redWeaponIds[Random.Range(0, redWeaponIds.Count)];
            // 随机第二把
            do {
                weapon2Id = redWeaponIds[Random.Range(0, redWeaponIds.Count)];
            } while (weapon1Id == weapon2Id);
            // 随机超级武器
            superWeaponId = redSuperWeaponIds[Random.Range(0, redSuperWeaponIds.Count)];
        }

        public static string GetWeaponAttrName(int itemId, int attrId) {
            var data = WeaponAttributeSet.GetWeaponAttributeById((short)attrId);
            var attrName = data.Name;
            switch (itemId, attrId) {
                case (ItemSet.Id_FireGun, WeaponAttributeSet.Id_FireDistance):
                    attrName = "喷火距离";
                    break;
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_FireDistance):
                    attrName = "扩散起点距离";
                    break;
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_BombRange):
                    attrName = "扩散范围";
                    break;
            }
            return attrName;
        }

        public static float GetWeaponAttrBaseValue(int attrId, Weapon modeData) {
            var baseValue = 0f;
            switch (attrId) {
                case WeaponAttributeSet.Id_Attack:
                    baseValue = modeData.Attack;
                    break;
                case WeaponAttributeSet.Id_MagazineNum:
                    baseValue = modeData.MagazineNum;
                    break;
                case WeaponAttributeSet.Id_ReloadTime:
                    baseValue = modeData.ReloadTime;
                    break;
                case WeaponAttributeSet.Id_Hp:
                    baseValue = modeData.Hp;
                    break;
                case WeaponAttributeSet.Id_IntervalTime:
                    baseValue = modeData.IntervalTime;
                    break;
                case WeaponAttributeSet.Id_FireRange:
                    baseValue = modeData.FireRange;
                    break;
                case WeaponAttributeSet.Id_BombRange:
                    baseValue = modeData.BombRange;
                    break;
                case WeaponAttributeSet.Id_FireOverHotTime:
                    baseValue = modeData.FireOverHotTime;
                    break;
                case WeaponAttributeSet.Id_EffectFireDistance:
                    baseValue = GetEffectFireDistance(modeData.ItemId);
                    baseValue = (float)Math.Round(baseValue * 0.01f, 2);
                    break;
                case WeaponAttributeSet.Id_FireDistance:
                    baseValue = (float)Math.Round(modeData.FireDistance * 0.01f, 2);
                    break;
            }
            return baseValue;
        }

        public static float GetWeaponAttrFinalBaseValue(int itemId, int attrId, float value) {
            var result = value;
            switch (itemId, attrId) {
                case (ItemSet.Id_Missile, WeaponAttributeSet.Id_MissileRange): {
                    var config = (AbilityData.PlayAbility_Missile)SkillData.GetHoldOnConfigId(ItemSet.Id_Missile);
                    result += config.M_AreaRadius * 100;
                    break;
                }
                case (ItemSet.Id_Missile, WeaponAttributeSet.Id_MissileDuration): {
                    var config = (AbilityData.PlayAbility_Missile)SkillData.GetHoldOnConfigId(ItemSet.Id_Missile);
                    result += config.M_BombingDuration * 1000;
                    break;
                }
                case (ItemSet.Id_Missile, WeaponAttributeSet.Id_VehicleHurtRatio): {
                    var config = (AbilityData.PlayAbility_Missile)SkillData.GetHoldOnConfigId(ItemSet.Id_Missile);
                    result += config.M_VehicleHurtRatio;
                    break;
                }
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_DiffusionReduction): {
                    var config =
                        (AbilityData.PlayAbility_SplitCone)AbilityConfig.GetAbilityModData(
                            AbilityConfig.BulletSplitCone);
                    result += config.M_ConeAngle;
                    break;
                }
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_FireDistance): {
                    var config = Get(ItemSet.Id_SplitGun);
                    result -= config.AttackRange;
                    break;
                }
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_BombRange): {
                    result /= 100;
                    break;
                }
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_Attack): {
                    var coneConfig =
                        (AbilityData.PlayAbility_SplitCone)AbilityConfig.GetAbilityModData(
                            AbilityConfig.BulletSplitCone);
                    var bulletConfig =
                        (AbilityData.PlayAbility_BulletWithStartPos)AbilityConfig.GetAbilityModData(AbilityConfig
                            .BulletSplitSubBullet);
                    result = bulletConfig.M_ATK * coneConfig.M_BulletSpreadPoints.Count;
                    break;
                }
            }
            return result;
        }

        public static string GetWeaponAttrAddValueString(int itemId, int attrId, float addValue) {
            switch (itemId, attrId) {
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_DiffusionReduction):
                    return addValue != 0 ? $"-{addValue / 10:F1}" : string.Empty;
                case (ItemSet.Id_SplitGun, WeaponAttributeSet.Id_Attack):
                    var coneConfig =
                        (AbilityData.PlayAbility_SplitCone)AbilityConfig.GetAbilityModData(
                            AbilityConfig.BulletSplitCone);
                    return addValue != 0 ? $"+{addValue * coneConfig.M_BulletSpreadPoints.Count}" : string.Empty;
                default:
                    return addValue != 0 ? $"+{addValue}" : string.Empty;
            }
        }

        public static float GetWeaponBulletMoveDistance(float fireDistance, GunData gunData) {
            var bulletMoveDistance = fireDistance;
            switch (gunData.ItemId) {
                case ItemSet.Id_SplitGun:
                    bulletMoveDistance -= gunData.AttackRange;
                    break;
            }
            return bulletMoveDistance;
        }

        public static string GetWeaponTitleDesc(int itemType) {
            switch (itemType) {
                case ItemTypeSet.Id_SuperWeapon:
                    return "新武装";
                default:
                    return "新武器";
            }
        }

        public static string GetWeaponTypeDesc(int itemType) {
            switch (itemType) {
                case ItemTypeSet.Id_SuperWeapon:
                    return "武装";
                default:
                    return "武器";
            }
        }

        public static int GetRandomWeaponSkinItemId(int weaponItemId) {
            var skins = WeaponSkinSet.GetWeaponSkinsByWeaponItemId(weaponItemId);
            return skins[Random.Range(0, skins.Count)].SkinItemId;
        }

        public static float GetHitDamageRatio(MonsterArmedCustom config) {
            return (1 + config.DamageIncRatio) * (1 + config.MapDamageIncRatio);
        }
    }
}