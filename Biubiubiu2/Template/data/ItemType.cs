// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代物品类型
    /// </summary>
	public struct ItemType
	{
		/// <summary>
		/// 物品类型ID
		/// </summary>
		public readonly short Id { get; }
		/// <summary>
		/// 名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 物品所属大类
		/// </summary>
		public readonly short ParentId { get; }
		/// <summary>
		/// 是否堆叠展示
		/// </summary>
		public readonly bool IsStacked { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="parentId"></param>
        /// <param name="isStacked"></param> )
		public ItemType( short id, string name, string sign, short parentId, bool isStacked )
		{
			Id = id;
			Name = name;
			Sign = sign;
			ParentId = parentId;
			IsStacked = isStacked;
		}
	}

    /// <summary>
    /// ItemTypeSet that holds all the table data
    /// </summary>
    public partial class ItemTypeSet
    {
        public static readonly ItemType[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const short Id_Weapon = 1;
		public const short Id_SuperWeapon = 2;
		public const short Id_HoldBall = 3;
		public const short Id_Bullet = 4;
		public const short Id_UseAbility = 5;
		public const short Id_LootBox = 6;
		public const short Id_RankScore = 7;
		public const short Id_WeaponSkin = 8;
		public const short Id_Currency = 9;
		public const short Id_SpecialItem = 10;
		public const short Id_LevelExp = 11;
		public const short Id_MeleeWeapon = 12;

        /// <summary>
        /// 构造函数
        /// </summary>
        static ItemTypeSet()
        {
            Data = new ItemType[]
            {
					 new ItemType( 1, "武器", "Weapon", 0, false )
					, new ItemType( 2, "超级武器", "SuperWeapon", 0, false )
					, new ItemType( 3, "手雷", "HoldBall", 0, false )
					, new ItemType( 4, "子弹", "Bullet", 0, false )
					, new ItemType( 5, "技能", "UseAbility", 0, false )
					, new ItemType( 6, "战利宝箱", "LootBox", 0, false )
					, new ItemType( 7, "段位积分", "RankScore", 0, false )
					, new ItemType( 8, "武器皮肤", "WeaponSkin", 0, false )
					, new ItemType( 9, "货币", "Currency", 0, true )
					, new ItemType( 10, "特殊道具", "SpecialItem", 0, true )
					, new ItemType( 11, "等级经验", "LevelExp", 0, false )
					, new ItemType( 12, "近战武器", "MeleeWeapon", 0, false )
            };
        }
    }
}
