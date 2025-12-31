// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器距离区间伤害表
    /// </summary>
	public struct WeaponDistanceHurt
	{
		/// <summary>
		/// 主键id
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 武器物品id
		/// </summary>
		public readonly int ItemId { get; }
		/// <summary>
		/// 最近距离区间比例(%)
		/// </summary>
		public readonly ushort MinDistanceRate { get; }
		/// <summary>
		/// 最远距离区间比例(%)
		/// </summary>
		public readonly ushort MaxDistanceRate { get; }
		/// <summary>
		/// 最小伤害比例(%)
		/// </summary>
		public readonly ushort MinHurtRate { get; }
		/// <summary>
		/// 最大伤害比例(%)
		/// </summary>
		public readonly ushort MaxHurtRate { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="minDistanceRate"></param>
        /// <param name="maxDistanceRate"></param>
        /// <param name="minHurtRate"></param>
        /// <param name="maxHurtRate"></param> )
		public WeaponDistanceHurt( int id, int itemId, ushort minDistanceRate, ushort maxDistanceRate, ushort minHurtRate, ushort maxHurtRate )
		{
			Id = id;
			ItemId = itemId;
			MinDistanceRate = minDistanceRate;
			MaxDistanceRate = maxDistanceRate;
			MinHurtRate = minHurtRate;
			MaxHurtRate = maxHurtRate;
		}
	}

    /// <summary>
    /// WeaponDistanceHurtSet that holds all the table data
    /// </summary>
    public partial class WeaponDistanceHurtSet
    {
        public static readonly WeaponDistanceHurt[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponDistanceHurtSet()
        {
            Data = new WeaponDistanceHurt[]
            {
					 new WeaponDistanceHurt( 1, 14, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 2, 14, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 3, 13, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 4, 13, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 5, 12, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 6, 12, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 7, 11, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 8, 11, 5, 100, 50, 0 )
					, new WeaponDistanceHurt( 9, 9, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 10, 9, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 11, 8, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 12, 8, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 13, 7, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 14, 7, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 15, 6, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 16, 6, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 17, 24, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 18, 24, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 19, 4, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 20, 4, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 21, 25, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 22, 25, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 23, 3, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 24, 3, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 25, 5, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 26, 5, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 27, 2, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 28, 2, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 29, 1, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 30, 1, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 31, 26, 0, 30, 100, 100 )
					, new WeaponDistanceHurt( 32, 26, 30, 100, 70, 0 )
					, new WeaponDistanceHurt( 33, 27, 0, 30, 100, 100 )
					, new WeaponDistanceHurt( 34, 27, 30, 100, 70, 0 )
					, new WeaponDistanceHurt( 35, 36, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 36, 36, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 37, 38, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 38, 38, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 39, 40, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 40, 40, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 41, 43, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 42, 43, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 43, 44, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 44, 44, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 45, 37, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 46, 37, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 47, 39, 0, 50, 100, 100 )
					, new WeaponDistanceHurt( 48, 39, 50, 100, 70, 0 )
					, new WeaponDistanceHurt( 49, 41, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 50, 41, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 51, 42, 0, 40, 100, 100 )
					, new WeaponDistanceHurt( 52, 42, 40, 100, 70, 0 )
					, new WeaponDistanceHurt( 53, 49, 0, 100, 100, 100 )
					, new WeaponDistanceHurt( 55, 50, 0, 60, 100, 100 )
					, new WeaponDistanceHurt( 56, 50, 60, 100, 70, 0 )
					, new WeaponDistanceHurt( 57, 51, 0, 100, 100, 100 )
					, new WeaponDistanceHurt( 58, 63, 0, 80, 100, 100 )
					, new WeaponDistanceHurt( 59, 63, 80, 100, 100, 50 )
					, new WeaponDistanceHurt( 60, 64, 0, 100, 100, 100 )
					, new WeaponDistanceHurt( 61, 18, 0, 15, 100, 100 )
					, new WeaponDistanceHurt( 62, 18, 15, 100, 70, 20 )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 WeaponDistanceHurt
		/// </summary>
		/// <param name="ItemId"></param>
		/// <param name="MinDistanceRate"></param>
		public static WeaponDistanceHurt GetWeaponDistanceHurtByItemIdMinDistanceRate( int itemId, ushort minDistanceRate )
		{
			foreach (WeaponDistanceHurt data in Data)
			{
				if ( data.ItemId == itemId && data.MinDistanceRate == minDistanceRate )
				{
					return data;
				}
			}
			return default(WeaponDistanceHurt);
		}

		/// <summary>
		/// 根据指定条件判断单个 WeaponDistanceHurt 是否存在
		/// </summary>
		/// <param name="ItemId"></param>
		/// <param name="MinDistanceRate"></param>
		public static bool HasWeaponDistanceHurtByItemIdMinDistanceRate( int itemId, ushort minDistanceRate )
		{
			foreach (WeaponDistanceHurt data in Data)
			{
				if ( data.ItemId == itemId && data.MinDistanceRate == minDistanceRate )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 WeaponDistanceHurt 列表
		/// </summary>
		/// <param name="ItemId"></param>
		public static List<WeaponDistanceHurt> GetWeaponDistanceHurtsByItemId( int itemId )
		{
			List<WeaponDistanceHurt> result = new List<WeaponDistanceHurt>();
			foreach (WeaponDistanceHurt data in Data)
			{
				if ( data.ItemId == itemId)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
