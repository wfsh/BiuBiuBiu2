// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代物品表
    /// </summary>
	public struct Item
	{
		/// <summary>
		/// 物品ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 显示名称
		/// </summary>
		public readonly string ShowName { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 资源标识
		/// </summary>
		public readonly string ResSign { get; }
		/// <summary>
		/// 物品类型ID
		/// </summary>
		public readonly short ItemTypeId { get; }
		/// <summary>
		/// 品质
		/// </summary>
		public readonly short Quality { get; }
		/// <summary>
		/// 图集
		/// </summary>
		public readonly string Atlas { get; }
		/// <summary>
		/// 物品描述
		/// </summary>
		public readonly string Desc { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="showName"></param>
        /// <param name="sign"></param>
        /// <param name="resSign"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="quality"></param>
        /// <param name="atlas"></param>
        /// <param name="desc"></param> )
		public Item( int id, string name, string showName, string sign, string resSign, short itemTypeId, short quality, string atlas, string desc )
		{
			Id = id;
			Name = name;
			ShowName = showName;
			Sign = sign;
			ResSign = resSign;
			ItemTypeId = itemTypeId;
			Quality = quality;
			Atlas = atlas;
			Desc = desc;
		}
	}

    /// <summary>
    /// ItemSet that holds all the table data
    /// </summary>
    public partial class ItemSet
    {
        public static readonly Item[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_Uzi = 1;
		public const int Id_S1897 = 2;
		public const int Id_S12K = 3;
		public const int Id_Vector = 4;
		public const int Id_TommyGun = 5;
		public const int Id_M249 = 6;
		public const int Id_Scarl = 7;
		public const int Id_Akm = 8;
		public const int Id_Mp5 = 9;
		public const int Id_Tank = 10;
		public const int Id_Gatling = 11;
		public const int Id_M416 = 12;
		public const int Id_Ak12 = 13;
		public const int Id_Ump9 = 14;
		public const int Id_GunBullet = 15;
		public const int Id_FocusGunBullet = 16;
		public const int Id_Rpgbullet = 17;
		public const int Id_Rpg = 18;
		public const int Id_Bat = 19;
		public const int Id_FocusGun = 20;
		public const int Id_Grenade = 21;
		public const int Id_ExplosiveBullet = 22;
		public const int Id_FirstAidKit = 23;
		public const int Id_Qbz95 = 24;
		public const int Id_Ksg = 25;
		public const int Id_R1895 = 26;
		public const int Id_P1911 = 27;
		public const int Id_Helicopter = 35;
		public const int Id_Aug = 36;
		public const int Id_Dp28 = 37;
		public const int Id_EnergySg = 38;
		public const int Id_Galil = 39;
		public const int Id_Groza = 40;
		public const int Id_M16A4 = 41;
		public const int Id_P90 = 42;
		public const int Id_ParticleCannon = 43;
		public const int Id_Pp19 = 44;
		public const int Id_ParticlecannonBullet = 45;
		public const int Id_EnergySgbullet = 46;
		public const int Id_Missile = 48;
		public const int Id_FireGun = 49;
		public const int Id_FlashGun = 50;
		public const int Id_SplitGun = 51;
		public const int Id_Uav = 52;
		public const int Id_MachineGun = 55;
		public const int Id_GrenadeGun = 63;
		public const int Id_M24 = 64;
		public const int Id_BatBeastcamp = 65;
		public const int Id_ShieldBeastcamp = 66;
		public const int Id_PlungerBeastcamp = 68;

        /// <summary>
        /// 构造函数
        /// </summary>
        static ItemSet()
        {
            Data = new Item[]
            {
					 new Item( 1, "UZI", "UZI", "UZI", "UZI", 1, 2, "Items", "" )
					, new Item( 2, "S1897", "S1897", "S1897", "S1897", 1, 2, "Items", "" )
					, new Item( 3, "S12K", "S12K", "S12K", "S12K", 1, 3, "Items", "" )
					, new Item( 4, "Vector", "Vector", "Vector", "Vector", 1, 4, "Items", "" )
					, new Item( 5, "TommyGun", "TommyGun", "TommyGun", "TommyGun", 1, 3, "Items", "" )
					, new Item( 6, "M249", "M249机枪", "M249", "M249", 1, 5, "Items", "" )
					, new Item( 7, "SCAR_L", "SCAR步枪", "SCARL", "SCARL", 1, 5, "Items", "" )
					, new Item( 8, "AKM", "AKM", "AKM", "AKM", 1, 5, "Items", "" )
					, new Item( 9, "MP5", "MP5冲锋", "MP5", "MP5", 1, 5, "Items", "" )
					, new Item( 10, "Tank", "坦克", "Tank", "Tank", 2, 6, "Items", "可获得随机属性的坦克" )
					, new Item( 11, "Galting", "Galting", "Gatling", "Gatling", 1, 6, "Items", "可获得随机属性的加特林机枪" )
					, new Item( 12, "M416", "M416", "M416", "M416", 1, 6, "Items", "可获得随机属性的HK416步枪" )
					, new Item( 13, "AK12", "AK12", "AK12", "AK12", 1, 6, "Items", "可获得随机属性的AK12步枪" )
					, new Item( 14, "UMP45", "UMP45冲锋", "UMP9", "UMP9", 1, 6, "Items", "可获得随机属性的UMP45冲锋" )
					, new Item( 15, "GunBullet", "GunBullet", "GunBullet", "GunBullet", 4, 0, "Items", "" )
					, new Item( 16, "FocusGunBullet", "FocusGunBullet", "FocusGunBullet", "FocusGunBullet", 4, 0, "Items", "" )
					, new Item( 17, "RPGBullet", "RPGBullet", "RPGBullet", "RPGBullet", 4, 0, "Items", "" )
					, new Item( 18, "RPG", "RPG", "RPG", "RPG", 1, 6, "Items", "" )
					, new Item( 19, "Bat", "Bat", "Bat", "Bat", 2, 6, "Items", "" )
					, new Item( 20, "FocusGun", "FocusGun", "FocusGun", "FocusGun", 1, 1, "Items", "" )
					, new Item( 21, "Grenade", "Grenade", "Grenade", "Grenade", 3, 0, "Items", "" )
					, new Item( 22, "ExplosiveBullet", "ExplosiveBullet", "ExplosiveBullet", "ExplosiveBullet", 4, 0, "Items", "" )
					, new Item( 23, "FirstAidKit", "FirstAidKit", "FirstAidKit", "FirstAidKit", 5, 0, "Items", "" )
					, new Item( 24, "QBZ95", "QBZ95步枪", "QBZ95", "QBZ95", 1, 4, "Items", "" )
					, new Item( 25, "KSG", "KSG", "KSG", "KSG", 1, 4, "Items", "" )
					, new Item( 26, "R1895", "R1895手枪", "R1895", "R1895", 1, 1, "Items", "" )
					, new Item( 27, "P1911", "P1911手枪", "P1911", "P1911", 1, 1, "Items", "" )
					, new Item( 35, "Helicopter", "直升机", "Helicopter", "Helicopter", 2, 6, "Items", "可获得随机属性的直升机" )
					, new Item( 36, "AUG", "AUG步枪", "AUG", "AUG", 1, 6, "Items", "可获得随机属性的AUG步枪" )
					, new Item( 37, "DP28", "DP28机枪", "DP28", "DP28", 1, 5, "Items", "" )
					, new Item( 38, "EnergySG", "能量弩", "EnergySG", "EnergySG", 1, 6, "Items", "可获得随机属性的能量弩" )
					, new Item( 39, "Galil", "Galil步枪", "Galil", "Galil", 1, 5, "Items", "" )
					, new Item( 40, "Groza", "Groza", "Groza", "Groza", 1, 6, "Items", "可获得随机属性的Groza步枪" )
					, new Item( 41, "M16A4", "M16A4", "M16A4", "M16A4", 1, 4, "Items", "" )
					, new Item( 42, "P90", "P90", "P90", "P90", 1, 5, "Items", "" )
					, new Item( 43, "ParticleCannon", "粒子炮", "ParticleCannon", "ParticleCannon", 1, 6, "Items", "可获得随机属性的粒子炮" )
					, new Item( 44, "PP19", "PP19", "PP19", "PP19", 1, 6, "Items", "可获得随机属性的PP19" )
					, new Item( 45, "ParticlecannonBullet", "ParticlecannonBullet", "ParticlecannonBullet", "ParticlecannonBullet", 4, 0, "Items", "" )
					, new Item( 46, "EnergySGBullet", "EnergySGBullet", "EnergySGBullet", "EnergySGBullet", 4, 0, "Items", "" )
					, new Item( 48, "Missile", "空袭导弹", "Missile", "Missile", 2, 6, "Items", "可获得随机属性的空袭导弹" )
					, new Item( 49, "喷火枪", "喷火枪", "FireGun", "FireGun", 1, 6, "Items", "可获得随机属性的喷火枪" )
					, new Item( 50, "闪电枪", "闪电枪", "FlashGun", "FlashGun", 1, 6, "Items", "可获得随机属性的闪电枪" )
					, new Item( 51, "爆裂枪", "爆裂枪", "SplitGun", "SplitGun", 1, 6, "Items", "可获得随机属性的爆裂枪" )
					, new Item( 52, "导弹无人机", "导弹无人机", "Uav", "Uav", 2, 6, "Items", "可获得随机属性的导弹无人机" )
					, new Item( 55, "重型防御机枪", "重型防御机枪", "MachineGun", "MachineGun", 2, 6, "Items", "可获得随机属性的重型防御机枪" )
					, new Item( 63, "榴弹枪", "榴弹枪", "GrenadeGun", "GrenadeGun", 1, 6, "Items", "" )
					, new Item( 64, "M24", "M24", "M24", "M24", 1, 6, "Items", "" )
					, new Item( 65, "batBeastcamp", "棒球棍", "batBeastcamp", "batBeastcamp", 12, 4, "Items", "" )
					, new Item( 66, "shieldBeastcamp", "冲击盾", "shieldBeastcamp", "shieldBeastcamp", 12, 4, "Items", "" )
					, new Item( 68, "plungerBeastcamp", "马桶搋", "plungerBeastcamp", "plungerBeastcamp", 12, 4, "Items", "" )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 Item
		/// </summary>
		/// <param name="Id"></param>
		public static Item GetItemById( int id )
		{
			foreach (Item data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(Item);
		}

		/// <summary>
		/// 根据指定条件判断单个 Item 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasItemById( int id )
		{
			foreach (Item data in Data)
			{
				if ( data.Id == id )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 Item 列表
		/// </summary>
		/// <param name="ItemTypeId"></param>
		public static List<Item> GetItemsByItemTypeId( short itemTypeId )
		{
			List<Item> result = new List<Item>();
			foreach (Item data in Data)
			{
				if ( data.ItemTypeId == itemTypeId)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
