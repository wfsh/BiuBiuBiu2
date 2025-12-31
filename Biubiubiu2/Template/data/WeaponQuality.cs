// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器品质
    /// </summary>
	public struct WeaponQuality
	{
		/// <summary>
		/// 物品品质ID
		/// </summary>
		public readonly ushort Id { get; }
		/// <summary>
		/// 名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 品质
		/// </summary>
		public readonly short Quality { get; }
		/// <summary>
		/// 最大强化次数
		/// </summary>
		public readonly sbyte MaxReinforceNum { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="quality"></param>
        /// <param name="maxReinforceNum"></param> )
		public WeaponQuality( ushort id, string name, string sign, short quality, sbyte maxReinforceNum )
		{
			Id = id;
			Name = name;
			Sign = sign;
			Quality = quality;
			MaxReinforceNum = maxReinforceNum;
		}
	}

    /// <summary>
    /// WeaponQualitySet that holds all the table data
    /// </summary>
    public partial class WeaponQualitySet
    {
        public static readonly WeaponQuality[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const short Quality_White = 1;
		public const short Quality_Green = 2;
		public const short Quality_Blue = 3;
		public const short Quality_Purple = 4;
		public const short Quality_Golden = 5;
		public const short Quality_Red = 6;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponQualitySet()
        {
            Data = new WeaponQuality[]
            {
					 new WeaponQuality( 1, "白色品质", "White", 1, 0 )
					, new WeaponQuality( 2, "绿色品质", "Green", 2, 0 )
					, new WeaponQuality( 3, "蓝色品质", "Blue", 3, 0 )
					, new WeaponQuality( 4, "紫色品质", "Purple", 4, 0 )
					, new WeaponQuality( 5, "金色品质", "Golden", 5, 0 )
					, new WeaponQuality( 6, "红色品质", "Red", 6, 5 )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 WeaponQuality
		/// </summary>
		/// <param name="Quality"></param>
		public static WeaponQuality GetWeaponQualityByQuality( short quality )
		{
			foreach (WeaponQuality data in Data)
			{
				if ( data.Quality == quality )
				{
					return data;
				}
			}
			return default(WeaponQuality);
		}

		/// <summary>
		/// 根据指定条件判断单个 WeaponQuality 是否存在
		/// </summary>
		/// <param name="Quality"></param>
		public static bool HasWeaponQualityByQuality( short quality )
		{
			foreach (WeaponQuality data in Data)
			{
				if ( data.Quality == quality )
				{
					return true;
				}
			}
			return false;
		}
    }
}
