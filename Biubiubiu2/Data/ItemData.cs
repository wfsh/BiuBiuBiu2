using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.Data {
    public class ItemData {
        private static Dictionary<int, ResourceItemData> resourceItems = new Dictionary<int, ResourceItemData>();
        
        public static int[] WeaponAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_MagazineNum, WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_ReloadTime,
            WeaponAttributeSet.Id_IntervalTime, WeaponAttributeSet.Id_FireRange, 
        };
        
        public static int[] GatlingAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_FireOverHotTime,
            WeaponAttributeSet.Id_IntervalTime, WeaponAttributeSet.Id_FireRange, 
        };

        public static int[] FireGunAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_FireDistance, WeaponAttributeSet.Id_FireOverHotTime, WeaponAttributeSet.Id_EffectFireDistance,
            WeaponAttributeSet.Id_IntervalTime, WeaponAttributeSet.Id_FireRange,
        };

        public static  int[] TankAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_Hp, WeaponAttributeSet.Id_BombRange,WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_IntervalTime
        }; 

        public static  int[] HelicopterAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_Hp, WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_IntervalTime,  WeaponAttributeSet.Id_FireRange, 
        };
        
        public static int[] ParticleCannonAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_MagazineNum, WeaponAttributeSet.Id_BombRange, WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_ReloadTime,
            WeaponAttributeSet.Id_IntervalTime, 
        };

        public static  int[] MissileAttrIds = new int[] {
            WeaponAttributeSet.Id_BombRange, WeaponAttributeSet.Id_MissileRange, WeaponAttributeSet.Id_MissileDuration, WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_IntervalTime, WeaponAttributeSet.Id_VehicleHurtRatio
        };

        public static  int[] SplitGunAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_MagazineNum, WeaponAttributeSet.Id_DiffusionReduction, WeaponAttributeSet.Id_FireDistance, WeaponAttributeSet.Id_BombRange, WeaponAttributeSet.Id_ReloadTime, WeaponAttributeSet.Id_IntervalTime
        };

        
        public static  int[] UavAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_Hp, WeaponAttributeSet.Id_BombRange, WeaponAttributeSet.Id_IntervalTime,
        };
        
        public static  int[] MachineGunAttrIds = new int[] {
            WeaponAttributeSet.Id_Attack, WeaponAttributeSet.Id_Hp, WeaponAttributeSet.Id_FireOverHotTime, WeaponAttributeSet.Id_EffectFireDistance, WeaponAttributeSet.Id_IntervalTime, WeaponAttributeSet.Id_FireRange,
        };
        
        public class OwnItemData {
            public ulong CreateId;
            public int ItemId;
            public int ItemGroupId;
            public bool IsProtect;
            public int AutoItemId;
            public int OwnGPOID;
            public ushort ItemNum;
            public Vector3 Point;
        }
        

        public class PickItemData {
            public int ItemId;
            public int AutoItemId;
            public ushort ItemNum;
        }
        
        public class RewardItemData {
            public int ItemId;
            public int ItemNum;
            public int ExpireType;
            public long ExpireValue;
        }
        
        public struct AdItemData {
            public int AdId;
            public int ExpireItemId;
            public List<RewardItemData> RewardItemData;
        }
        
        public class ResourceItemData{
            public int ItemId;
            public int ItemNum;
        }

        public static bool CanPackUseType(short itemType) {
            if (itemType == ItemTypeSet.Id_Bullet) {
                return false;
            }
            return true;
        }


        public static bool QuickUseItem(short itemType) {
            if (itemType == ItemTypeSet.Id_HoldBall || itemType == ItemTypeSet.Id_UseAbility) {
                return true;
            }
            return false;
        }

        public static ushort MaxPackageNum(short itemType) {
            if (itemType == ItemTypeSet.Id_Bullet) {
                return 999;
            }
            if (itemType == ItemTypeSet.Id_UseAbility) {
                return 10;
            }
            return 1;
        }

        public static ushort GetWeight(short itemType) {
            return 1;
        }

        public static Item GetData(int itemId) {
            if (itemId == 0) {
                return new Item();
            }
            for (int i = 0; i < ItemSet.Data.Length; i++) {
                var item = ItemSet.Data[i];
                if (item.Id == itemId) {
                    return item;
                }
            }
            Debug.LogError("缺少物品数据:" + itemId);
            return new Item();
        }

        public static Item GetData(string sign) {
            for (int i = 0; i < ItemSet.Data.Length; i++) {
                var item = ItemSet.Data[i];
                if (item.Sign == sign) {
                    return item;
                }
            }
            Debug.LogError("缺少物品数据:" + sign);
            return new Item();
        }
        
        public static Color GetQualityColor(int quality) {
            switch (quality) {
                case -1:
                    return new Color(0.63f, 0.62f, 0.3513726f);
                case 1:
                    return new Color(0.8313726f, 0.8352941f, 0.8313726f);
                case 2:
                    return new Color(0.6941177f, 0.8078431f, 0.1607843f);
                case 3:
                    return new Color(0.4470588f, 0.8f, 1f);
                case 4:
                    return new Color(0.83f, 0.21f, 0.99f);
                case 5:
                    return new Color(1f,0.6f, 0f);
                default:
                    return new Color(0.91f,0.23f, 0.24f);
            }
        }
        
        public static int[] GetAttrIds(int itemId) {
            switch (itemId) {
                case ItemSet.Id_Gatling:
                    return GatlingAttrIds;
                case ItemSet.Id_Tank:
                    return TankAttrIds;
                case ItemSet.Id_Helicopter:
                    return HelicopterAttrIds;
                case ItemSet.Id_Missile:
                    return MissileAttrIds;
                case ItemSet.Id_ParticleCannon:
                    return ParticleCannonAttrIds;
                case ItemSet.Id_FireGun:
                    return FireGunAttrIds;
                case ItemSet.Id_SplitGun:
                    return SplitGunAttrIds;
                case ItemSet.Id_Uav:
                    return UavAttrIds;
                case ItemSet.Id_MachineGun:
                    return MachineGunAttrIds;
            }
            return WeaponAttrIds;
        }
        
        public static Color GetAttrColor(int itemId, int attrId, int value) {
            return GetQualityColor(GetAttrQuality(itemId, attrId, value));
        }

        public static sbyte GetAttrQuality(int itemId, int attrId, int value) {
            sbyte quality = 1;
            var list = WeaponRandAttributeSet.GetWeaponRandAttributesByItemIdWeaponAttr(itemId, (short)attrId);
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                if (data.MinValue <= value && value <= data.MaxValue) {
                    quality = data.Quality;
                    break;
                }
            }
            return quality;
        }

        public static int GetAttrQualityByRandomList(List<int> qualitys) {
            if (qualitys != null && qualitys.Count == 1) {
                return qualitys[0];
            }

            return -1;
        }

        public static bool IsStackedItem(int itemType) {
            foreach (var itemTypeInfo in ItemTypeSet.Data) {
                if (itemTypeInfo.Id == itemType) {
                    return itemTypeInfo.IsStacked;
                }
            }

            return false;
       }
       
       public static void SetAllResourceItemData(List<ResourceItemData> resourceItemList) {
            resourceItems.Clear();
            for (int i = 0; i < resourceItemList.Count; i++) {
                var item = resourceItemList[i];
                resourceItems.Add(item.ItemId, new ResourceItemData() {
                    ItemId = item.ItemId,
                    ItemNum = item.ItemNum
                });
            }
        }
        
        public static void SetResourceItemData(int itemId, int itemNum) {
            if (resourceItems.TryGetValue(itemId, out var data)) {
                data.ItemNum = itemNum;
            }
            else {
                // 如果没有找到对应的特殊物品，则忽略
                // 这里只记录和同步GetResourceItemsOut中提供的物品类型
                return;
            }
        }
        
        public static bool IsResourceItem(int itemId){
            return resourceItems.ContainsKey(itemId);
        }

        public static int GetResourceItemNum(int itemId){
            if (resourceItems.TryGetValue(itemId, out var data)) {
                return data.ItemNum;
            }
            
            return 0;
        }
    }
}
