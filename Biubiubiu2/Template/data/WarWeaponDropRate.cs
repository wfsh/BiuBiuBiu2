// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代战场武器掉落概率表
    /// </summary>
	public struct WarWeaponDropRate
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 战场等级
		/// </summary>
		public readonly short WarLevel { get; }
		/// <summary>
		/// 武器品质
		/// </summary>
		public readonly short Quality { get; }
		/// <summary>
		/// 武器物品id
		/// </summary>
		public readonly int ItemId { get; }
		/// <summary>
		/// 掉落权重
		/// </summary>
		public readonly short Rate { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warLevel"></param>
        /// <param name="quality"></param>
        /// <param name="itemId"></param>
        /// <param name="rate"></param> )
		public WarWeaponDropRate( int id, short warLevel, short quality, int itemId, short rate )
		{
			Id = id;
			WarLevel = warLevel;
			Quality = quality;
			ItemId = itemId;
			Rate = rate;
		}
	}

    /// <summary>
    /// WarWeaponDropRateSet that holds all the table data
    /// </summary>
    public partial class WarWeaponDropRateSet
    {
        public static readonly WarWeaponDropRate[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WarWeaponDropRateSet()
        {
            Data = new WarWeaponDropRate[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取 WarWeaponDropRate 列表
		/// </summary>
		/// <param name="WarLevel"></param>
		/// <param name="Quality"></param>
		public static List<WarWeaponDropRate> GetWarWeaponDropRatesByWarLevelQuality( short warLevel, short quality )
		{
			List<WarWeaponDropRate> result = new List<WarWeaponDropRate>();
			foreach (WarWeaponDropRate data in Data)
			{
				if ( data.WarLevel == warLevel && data.Quality == quality)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
