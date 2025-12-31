// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器表
    /// </summary>
	public struct Weapon
	{
		/// <summary>
		/// 武器ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 物品ID
		/// </summary>
		public readonly int ItemId { get; }
		/// <summary>
		/// 武器类型id
		/// </summary>
		public readonly short ItemTypeId { get; }
		/// <summary>
		/// 武器品质
		/// </summary>
		public readonly short Quality { get; }
		/// <summary>
		/// 单发子弹伤害
		/// </summary>
		public readonly ushort Attack { get; }
		/// <summary>
		/// 射击间隔(s)
		/// </summary>
		public readonly float IntervalTime { get; }
		/// <summary>
		/// 弹夹容量
		/// </summary>
		public readonly ushort MagazineNum { get; }
		/// <summary>
		/// 射程
		/// </summary>
		public readonly ushort FireDistance { get; }
		/// <summary>
		/// 换弹时间(s)
		/// </summary>
		public readonly float ReloadTime { get; }
		/// <summary>
		/// 精准
		/// </summary>
		public readonly ushort FireRange { get; }
		/// <summary>
		/// 子弹速度(m/s)
		/// </summary>
		public readonly ushort BulletSpeed { get; }
		/// <summary>
		/// 移动速度(um)
		/// </summary>
		public readonly ushort MoveSpeed { get; }
		/// <summary>
		/// 是否启用
		/// </summary>
		public readonly bool IsEnabled { get; }
		/// <summary>
		/// 是否初始化武器
		/// </summary>
		public readonly bool IsInit { get; }
		/// <summary>
		/// 开火子弹数
		/// </summary>
		public readonly ushort FireBulletNum { get; }
		/// <summary>
		/// 血量
		/// </summary>
		public readonly ushort Hp { get; }
		/// <summary>
		/// 射击过热时间(ms)
		/// </summary>
		public readonly ushort FireOverHotTime { get; }
		/// <summary>
		/// 爆炸范围(m)
		/// </summary>
		public readonly ushort BombRange { get; }
		/// <summary>
		/// 武器描述
		/// </summary>
		public readonly string Desc { get; }
		/// <summary>
		/// 枪声最大传播范围
		/// </summary>
		public readonly float MaxAttenuation { get; }
		/// <summary>
		/// 命中头部伤害倍数
		/// </summary>
		public readonly float HitheadMultiplier { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="quality"></param>
        /// <param name="attack"></param>
        /// <param name="intervalTime"></param>
        /// <param name="magazineNum"></param>
        /// <param name="fireDistance"></param>
        /// <param name="reloadTime"></param>
        /// <param name="fireRange"></param>
        /// <param name="bulletSpeed"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isInit"></param>
        /// <param name="fireBulletNum"></param>
        /// <param name="hp"></param>
        /// <param name="fireOverHotTime"></param>
        /// <param name="bombRange"></param>
        /// <param name="desc"></param>
        /// <param name="maxAttenuation"></param>
        /// <param name="hitheadMultiplier"></param> )
		public Weapon( int id, int itemId, short itemTypeId, short quality, ushort attack, float intervalTime, ushort magazineNum, ushort fireDistance, float reloadTime, ushort fireRange, ushort bulletSpeed, ushort moveSpeed, bool isEnabled, bool isInit, ushort fireBulletNum, ushort hp, ushort fireOverHotTime, ushort bombRange, string desc, float maxAttenuation, float hitheadMultiplier )
		{
			Id = id;
			ItemId = itemId;
			ItemTypeId = itemTypeId;
			Quality = quality;
			Attack = attack;
			IntervalTime = intervalTime;
			MagazineNum = magazineNum;
			FireDistance = fireDistance;
			ReloadTime = reloadTime;
			FireRange = fireRange;
			BulletSpeed = bulletSpeed;
			MoveSpeed = moveSpeed;
			IsEnabled = isEnabled;
			IsInit = isInit;
			FireBulletNum = fireBulletNum;
			Hp = hp;
			FireOverHotTime = fireOverHotTime;
			BombRange = bombRange;
			Desc = desc;
			MaxAttenuation = maxAttenuation;
			HitheadMultiplier = hitheadMultiplier;
		}
	}

    /// <summary>
    /// WeaponSet that holds all the table data
    /// </summary>
    public partial class WeaponSet
    {
        public static readonly Weapon[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponSet()
        {
            Data = new Weapon[]
            {
					 new Weapon( 1, 1, 1, 2, 15, 0.105f, 40, 80, 2f, 15, 300, 65, true, false, 1, 0, 0, 0, "UZI是一把冲锋枪，主要用于近距离战斗。UZI拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 2, 2, 1, 2, 25, 0.75f, 5, 50, 0.642f, 30, 300, 65, true, false, 1, 0, 0, 0, "M1897是一把霰弹枪，主要用于近距离战斗。M1897单发可发射多枚子弹，造成较高的伤害，但有效距离较短。", 150f, 2f )
					, new Weapon( 3, 3, 1, 3, 15, 0.25f, 5, 50, 3f, 30, 300, 65, true, false, 1, 0, 0, 0, "S12K是一把霰弹枪，主要用于近距离战斗。S12K单发可发射多枚子弹，造成较高的伤害，但有效距离较短。", 150f, 2f )
					, new Weapon( 4, 4, 1, 4, 10, 0.051f, 25, 80, 2f, 15, 300, 65, true, false, 1, 0, 0, 0, "Vector是一把冲锋枪，主要用于近距离战斗。Vector拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 5, 5, 1, 3, 7, 0.15f, 40, 80, 2.6f, 15, 300, 65, true, false, 1, 0, 0, 0, "汤姆逊是一把冲锋枪，主要用于近距离战斗。汤姆逊拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 6, 6, 1, 5, 80, 75f, 100, 10000, 3800f, 35, 400, 50, true, false, 1, 0, 0, 0, "M249是一把机枪，主要用于中距离火力压制。M249具有较高的射速，不俗的威力，但子弹扩散较大。", 150f, 2f )
					, new Weapon( 7, 7, 1, 5, 110, 96f, 50, 10000, 1800f, 23, 600, 60, true, false, 1, 0, 0, 0, "SCAR-L是一把突击步枪，主要用于中远程战斗。SCAR-L具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 8, 8, 1, 5, 14, 0.2f, 30, 100, 3.5f, 15, 500, 60, true, true, 1, 0, 0, 0, "AKM是一把突击步枪，主要用于中远程战斗。AKM具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 9, 9, 1, 5, 90, 75f, 45, 10000, 1400f, 17, 330, 65, true, false, 1, 0, 0, 0, "MP5是一把冲锋枪，主要用于近距离战斗。MP5拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 10, 10, 2, 6, 1000, 1000f, 0, 3000, 0f, 100, 150, 50, true, true, 1, 8000, 0, 500, "坦克是一个超级武装，能够适用全场景战斗。使用后，将会召唤一台可以移动的坦克，通过抛物线发射炮弹造成范围伤害。", 150f, 2f )
					, new Weapon( 11, 11, 1, 6, 18, 0.12f, 10000, 100, 1f, 15, 450, 50, true, false, 1, 0, 5000, 0, "加特林是一把机枪，主要用于中距离火力压制。加特林具有极高的射速，不俗的威力，但子弹扩散较大。加特林无弹匣概念，但在持续射击后会进入一段不可射击的冷却状态", 150f, 2f )
					, new Weapon( 12, 12, 1, 6, 14, 0.12f, 30, 100, 3.8f, 15, 500, 60, true, false, 1, 0, 0, 0, "HK416是一把突击步枪，主要用于中远程战斗。HK416具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 13, 13, 1, 6, 20, 0.105f, 30, 100, 3.6f, 15, 500, 60, true, false, 1, 0, 0, 0, "AK12是一把突击步枪，主要用于中远程战斗。AK12具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 14, 14, 1, 6, 100, 60f, 45, 10000, 1300f, 17, 290, 65, true, false, 1, 0, 0, 0, "UMP45是一把冲锋枪，主要用于近距离战斗。UMP45拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 15, 18, 2, 6, 35, 1f, 1, 80, 3f, 15, 150, 60, false, false, 1, 0, 0, 200, "RPG", 150f, 2f )
					, new Weapon( 16, 19, 2, 6, 0, 0f, 0, 0, 0f, 0, 0, 0, false, false, 1, 0, 0, 0, "", 150f, 2f )
					, new Weapon( 17, 20, 1, 1, 0, 0f, 0, 10000, 0f, 0, 0, 0, false, false, 1, 0, 0, 0, "", 150f, 2f )
					, new Weapon( 18, 26, 1, 1, 70, 400f, 7, 10000, 1600f, 12, 150, 70, true, false, 1, 0, 0, 0, "R1895是一把手枪，主要用于近距离战斗，R1895各方面属性较为平庸。", 150f, 2f )
					, new Weapon( 19, 27, 1, 1, 30, 150f, 7, 10000, 400f, 11, 200, 70, true, false, 1, 0, 0, 0, "P1911是一把手枪，主要用于近距离战斗，P1911各方面属性较为平庸。", 150f, 2f )
					, new Weapon( 20, 25, 1, 4, 800, 250f, 8, 3000, 2000f, 70, 200, 65, true, false, 1, 0, 0, 0, "KSG是一把霰弹枪，主要用于近距离战斗。KSG单发可发射多枚子弹，造成较高的伤害，但有效距离较短。", 150f, 2f )
					, new Weapon( 21, 24, 1, 4, 100, 92f, 45, 10000, 2000f, 30, 650, 60, true, true, 1, 0, 0, 0, "QBZ95是一把突击步枪，主要用于中远程战斗。QBZ95具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 22, 35, 2, 6, 200, 50f, 0, 10000, 0f, 30, 400, 0, true, false, 1, 5000, 0, 0, "直升机是一个超级武装，能够适用全场景战斗。使用后，将会召唤一台可以全向移动的直升机，通过两把机枪进行火力压制。", 150f, 2f )
					, new Weapon( 23, 36, 1, 6, 140, 84f, 50, 10000, 1400f, 18, 670, 60, true, false, 1, 0, 0, 0, "AUG是一把突击步枪，主要用于中远程战斗。AUG具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 24, 37, 1, 5, 90, 75f, 50, 10000, 2500f, 25, 350, 60, true, false, 1, 0, 0, 0, "DP28是一把机枪，主要用于中距离火力压制。DP28具有较高的射速，不俗的威力，但子弹扩散较大。", 150f, 2f )
					, new Weapon( 25, 38, 1, 6, 2000, 500f, 25, 4000, 1700f, 50, 200, 60, true, false, 1, 0, 0, 0, "能量弩是一把霰弹枪，主要用于近距离战斗。能量弩具有较高的伤害，但射速较低。能量弩单次以扇形发射多发子弹", 150f, 2f )
					, new Weapon( 26, 39, 1, 5, 130, 113f, 45, 10000, 1800f, 22, 650, 60, true, false, 1, 0, 0, 0, "Galil是一把突击步枪，主要用于中远程战斗。Galil具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 27, 40, 1, 6, 20, 0.1f, 30, 100, 3.4f, 10, 600, 60, true, false, 1, 0, 0, 0, "Groza是一把突击步枪，主要用于中远程战斗。Groza具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 28, 41, 1, 4, 10, 0.1f, 30, 120, 3.7f, 15, 500, 60, true, false, 1, 0, 0, 0, "M16A4是一把突击步枪，主要用于中远程战斗。M16A4具有较高的伤害，较高的精准度，但射速相对较低。", 150f, 2f )
					, new Weapon( 29, 42, 1, 5, 10, 0.135f, 50, 80, 3.2f, 15, 300, 60, true, false, 1, 0, 0, 0, "P90是一把冲锋枪，主要用于近距离战斗。P90拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 30, 43, 1, 6, 600, 400f, 20, 10000, 1700f, 15, 350, 60, true, false, 1, 0, 0, 200, "粒子炮是一把特种枪械，主要用于中远距离战斗。粒子炮具有极高的单发伤害，但射速较低。粒子炮发射能量粒子造成爆炸范围伤害。", 150f, 2f )
					, new Weapon( 31, 44, 1, 6, 15, 0.12f, 30, 100, 2.4f, 15, 500, 60, true, false, 1, 0, 0, 0, "PP19是一把冲锋枪，主要用于近距离战斗。PP19拥有较高的射速，但单发伤害相对较低。", 150f, 2f )
					, new Weapon( 32, 49, 1, 6, 120, 60f, 1, 2000, 0f, 30, 400, 50, true, false, 1, 0, 6000, 0, "喷火枪是一把特种枪械，主要用于近距离战斗。喷火枪具有极高的爆发伤害，但有效距离较短。被喷火枪命中后，将造成持续的灼烧伤害。", 150f, 2f )
					, new Weapon( 33, 48, 2, 6, 1000, 300f, 0, 50, 0f, 0, 40, 0, true, false, 1, 0, 0, 300, "空袭导弹是一个超级武装，主要用于区域性火力压制。空袭导弹使用信标进行召唤，投掷信标后，将在信标所处区域投放持续的空袭，造成持续的范围伤害。", 150f, 2f )
					, new Weapon( 34, 50, 1, 6, 150, 150f, 30, 10000, 1800f, 20, 650, 60, true, false, 1, 0, 0, 0, "闪电枪是一把特种枪械，主要用于中远程战斗。闪电枪具有较高的单发伤害，但射速较低。闪电枪能够在命中对方后链接到附近的其余对手，造成闪电链伤害。", 150f, 2f )
					, new Weapon( 35, 51, 1, 6, 200, 500f, 12, 6000, 1800f, 0, 220, 60, true, false, 1, 0, 0, 5000, "爆裂枪是一把特种枪械，主要用于中距离战斗。爆裂枪具有极高的单发伤害，但射速较低。爆裂枪具有两段伤害，主要伤害来源是第二段伤害，需精确控制距离。", 150f, 2f )
					, new Weapon( 36, 52, 2, 6, 200, 600f, 0, 10000, 0f, 0, 20, 60, true, false, 1, 2000, 0, 300, "导弹无人机是一个超级武装，主要用于中远距离战斗。使用后将会召唤两台自动锁定、自动攻击的无人机，无人机发射导弹造成范围伤害。", 150f, 2f )
					, new Weapon( 37, 55, 2, 6, 100, 50f, 0, 10000, 0f, 30, 400, 0, true, false, 1, 8000, 5000, 0, "重型防御机枪是一个超级武装，主要用于中距离战斗。使用后将会召唤一台自动锁定、自动攻击的重型机枪，机枪能造成高额的伤害。", 150f, 2f )
					, new Weapon( 38, 63, 1, 6, 350, 500f, 10, 10000, 1500f, 0, 38, 58, true, false, 1, 0, 0, 300, "榴弹枪是一把特种枪械，能够适用全场景战斗，榴弹可碰墙反弹且爆炸后造成范围伤害，但榴弹飞速偏低需要一定预判。", 150f, 2f )
					, new Weapon( 39, 64, 1, 6, 35, 1.2f, 1, 500, 2.3f, 5, 500, 65, true, false, 1, 0, 0, 0, "", 300f, 2f )
					, new Weapon( 40, 65, 12, 4, 25, 1f, 0, 1, 0f, 0, 0, 70, false, false, 1, 0, 0, 0, "", 20f, 2f )
					, new Weapon( 41, 66, 12, 4, 200, 1f, 0, 2, 0f, 0, 0, 50, false, false, 1, 0, 0, 0, "", 20f, 2f )
					, new Weapon( 42, 68, 12, 4, 30, 1f, 0, 1, 0f, 0, 0, 70, false, false, 1, 0, 0, 0, "", 150f, 2f )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 Weapon
		/// </summary>
		/// <param name="ItemId"></param>
		public static Weapon GetWeaponByItemId( int itemId )
		{
			foreach (Weapon data in Data)
			{
				if ( data.ItemId == itemId )
				{
					return data;
				}
			}
			return default(Weapon);
		}

		/// <summary>
		/// 根据指定条件判断单个 Weapon 是否存在
		/// </summary>
		/// <param name="ItemId"></param>
		public static bool HasWeaponByItemId( int itemId )
		{
			foreach (Weapon data in Data)
			{
				if ( data.ItemId == itemId )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 Weapon 列表
		/// </summary>
		/// <param name="ItemTypeId"></param>
		/// <param name="Quality"></param>
		/// <param name="IsEnabled"></param>
		public static List<Weapon> GetWeaponsByItemTypeIdQualityIsEnabled( short itemTypeId, short quality, bool isEnabled )
		{
			List<Weapon> result = new List<Weapon>();
			foreach (Weapon data in Data)
			{
				if ( data.ItemTypeId == itemTypeId && data.Quality == quality && data.IsEnabled == isEnabled)
				{
					result.Add(data);
				}
			}
			return result;
		}
		/// <summary>
		/// 根据指定条件获取 Weapon 列表
		/// </summary>
		/// <param name="Quality"></param>
		public static List<Weapon> GetWeaponsByQuality( short quality )
		{
			List<Weapon> result = new List<Weapon>();
			foreach (Weapon data in Data)
			{
				if ( data.Quality == quality)
				{
					result.Add(data);
				}
			}
			return result;
		}
		/// <summary>
		/// 根据指定条件获取 Weapon 列表
		/// </summary>
		/// <param name="IsEnabled"></param>
		public static List<Weapon> GetWeaponsByIsEnabled( bool isEnabled )
		{
			List<Weapon> result = new List<Weapon>();
			foreach (Weapon data in Data)
			{
				if ( data.IsEnabled == isEnabled)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
