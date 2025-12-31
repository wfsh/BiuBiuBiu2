using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class WeaponSkinData {
        private static readonly Dictionary<int, string> itemIdToSfxSuffix = new Dictionary<int, string> {
            // [ItemSet.Id_MissileBronze] = "_Bronze",
            // [ItemSet.Id_MissileSilver] = "_Silver",
            // [ItemSet.Id_MissileGold] = "_Gold",
            // [ItemSet.Id_MissileDiamond] = "_Diamond",
        };

        public static int GetWeaponItemId(int skinItemId) {
            var skinInfo = WeaponSkinSet.GetWeaponSkinBySkinItemId(skinItemId);
            return skinInfo.WeaponItemId;
        }

        public static int GetDefaultSkinItemId(int weaponItemId) {
            foreach (var weaponSkin in WeaponSkinSet.Data) {
                if (weaponSkin.WeaponItemId == weaponItemId && weaponSkin.IsDefault) {
                    return weaponSkin.SkinItemId;
                }
            }
            return 0;
        }

        public static string GetSkinSfx(int skinItemId) {
            var sfxName = string.Empty;
            var skinInfo = WeaponSkinSet.GetWeaponSkinBySkinItemId(skinItemId);
            var weaponItem = ItemData.GetData(skinInfo.WeaponItemId);
            var weaponSkinItem = ItemData.GetData(skinInfo.SkinItemId);

            switch (QualityData.DeviceLevel) {
                case QualityData.DEVICE_ULTRA:
                case QualityData.DEVICE_HIGHT:
                    switch (QualityData.GetQualityType()) {
                        case QualityData.QualityType.High:
                            if (!skinInfo.SfxHigh) {
                                return string.Empty;
                            }
                            sfxName = $"fx_{weaponSkinItem.Sign.ToLower()}_high";
                            break;
                        case QualityData.QualityType.Medium:
                            if (!skinInfo.SfxLow) {
                                return string.Empty;
                            }
                            sfxName = $"fx_{weaponSkinItem.Sign.ToLower()}_low";
                            break;
                        case QualityData.QualityType.Low:
                            return string.Empty;
                    }
                    break;
                case QualityData.DEVICE_MIDDLE:
                    if (QualityData.GetQualityType() != QualityData.QualityType.High || !skinInfo.SfxLow) {
                        return string.Empty;
                    }
                    sfxName = $"fx_{weaponSkinItem.Sign.ToLower()}_low";
                    break;
                default:
                    return string.Empty;
            }

            return AssetURL.GetEffect($"Weapon/{weaponItem.Sign}/{sfxName}");
        }

        public static string GetSkinSfx(int skinItemId, string sfxName) {
            return itemIdToSfxSuffix.TryGetValue(skinItemId, out var suffix) ? $"{sfxName}{suffix}" : sfxName;
        }

        public static string GetUISkinSfx(int skinItemId) {
            var skinInfo = WeaponSkinSet.GetWeaponSkinBySkinItemId(skinItemId);
            var weaponItem = ItemData.GetData(skinInfo.WeaponItemId);
            if (weaponItem.ItemTypeId == ItemTypeSet.Id_SuperWeapon) {
                return string.Empty;
            }
            var weaponSkinItem = ItemData.GetData(skinInfo.SkinItemId);
            if (!skinInfo.SfxHigh) {
                return string.Empty;
            }
            string sfxName = $"fx_{weaponSkinItem.Sign.ToLower()}_high";
            return $"Effects/Weapon/{weaponItem.Sign}/{sfxName}";
        }

        public static string GetWeaponURL(int itemId, bool equippedInRole) {
            var item = ItemData.GetData(itemId);
            var weaponItem = item.ItemTypeId == ItemTypeSet.Id_WeaponSkin ? ItemData.GetData(GetWeaponItemId(itemId)) : item;
            var url = "";
            switch (weaponItem.Id) {
                case ItemSet.Id_Uav:
                    url = equippedInRole ? $"Items/{item.ResSign}" : $"AI/Client/{item.ResSign}";
                    break;
                case ItemSet.Id_Missile:
                    url = $"Items/{item.ResSign}";
                    break;
                default:
                    switch (weaponItem.ItemTypeId) {
                        case ItemTypeSet.Id_Weapon:
                            url = $"Weapon/Gun/{item.ResSign}";
                            break;
                        case ItemTypeSet.Id_SuperWeapon:
                            url = $"AI/Client/{item.ResSign}";
                            break;
                        default:
                            Debug.LogError($"没有对应武器地址 Type:{item.ItemTypeId}  Sign: {item.Sign}  ResSign: {item.ResSign}");
                            break;
                    }
                    break;
            }
            return url;
        }
    }
}