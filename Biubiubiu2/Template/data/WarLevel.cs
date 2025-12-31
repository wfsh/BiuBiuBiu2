// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代战场等级表
    /// </summary>
	public struct WarLevel
	{
		/// <summary>
		/// 战场等级表ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 战场等级
		/// </summary>
		public readonly short Level { get; }
		/// <summary>
		/// 武器掉落概率(%)
		/// </summary>
		public readonly sbyte WeaponDropRate { get; }
		/// <summary>
		/// 击杀人数最小(包含)
		/// </summary>
		public readonly short KillsMin { get; }
		/// <summary>
		/// 击杀人数最大(包含)
		/// </summary>
		public readonly short KillsMax { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="weaponDropRate"></param>
        /// <param name="killsMin"></param>
        /// <param name="killsMax"></param> )
		public WarLevel( int id, short level, sbyte weaponDropRate, short killsMin, short killsMax )
		{
			Id = id;
			Level = level;
			WeaponDropRate = weaponDropRate;
			KillsMin = killsMin;
			KillsMax = killsMax;
		}
	}

    /// <summary>
    /// WarLevelSet that holds all the table data
    /// </summary>
    public partial class WarLevelSet
    {
        public static readonly WarLevel[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WarLevelSet()
        {
            Data = new WarLevel[]
            {
					 new WarLevel( 1, 1, 70, 1, 10 )
					, new WarLevel( 2, 1, 60, 11, 20 )
					, new WarLevel( 3, 1, 40, 21, 30 )
					, new WarLevel( 4, 1, 20, 31, 60 )
            };
        }
		/// <summary>
		/// 根据指定条件获取 WarLevel 列表
		/// </summary>
		/// <param name="Level"></param>
		public static List<WarLevel> GetWarLevelsByLevel( short level )
		{
			List<WarLevel> result = new List<WarLevel>();
			foreach (WarLevel data in Data)
			{
				if ( data.Level == level)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
