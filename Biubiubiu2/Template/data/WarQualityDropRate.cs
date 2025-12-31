// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代战场品质掉落概率表
    /// </summary>
	public struct WarQualityDropRate
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
		/// 品质
		/// </summary>
		public readonly short Quality { get; }
		/// <summary>
		/// 掉落权重
		/// </summary>
		public readonly short DropRate { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="warLevel"></param>
        /// <param name="quality"></param>
        /// <param name="dropRate"></param> )
		public WarQualityDropRate( int id, short warLevel, short quality, short dropRate )
		{
			Id = id;
			WarLevel = warLevel;
			Quality = quality;
			DropRate = dropRate;
		}
	}

    /// <summary>
    /// WarQualityDropRateSet that holds all the table data
    /// </summary>
    public partial class WarQualityDropRateSet
    {
        public static readonly WarQualityDropRate[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WarQualityDropRateSet()
        {
            Data = new WarQualityDropRate[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取 WarQualityDropRate 列表
		/// </summary>
		/// <param name="WarLevel"></param>
		public static List<WarQualityDropRate> GetWarQualityDropRatesByWarLevel( short warLevel )
		{
			List<WarQualityDropRate> result = new List<WarQualityDropRate>();
			foreach (WarQualityDropRate data in Data)
			{
				if ( data.WarLevel == warLevel)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
