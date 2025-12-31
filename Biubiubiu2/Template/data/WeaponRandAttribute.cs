// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器随机属性
    /// </summary>
	public struct WeaponRandAttribute
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public readonly short Id { get; }
		/// <summary>
		/// 颜色品质
		/// </summary>
		public readonly sbyte Quality { get; }
		/// <summary>
		/// 武器物品id
		/// </summary>
		public readonly int ItemId { get; }
		/// <summary>
		/// 概率
		/// </summary>
		public readonly ushort Rate { get; }
		/// <summary>
		/// 最小数值
		/// </summary>
		public readonly short MinValue { get; }
		/// <summary>
		/// 最大数值
		/// </summary>
		public readonly short MaxValue { get; }
		/// <summary>
		/// 武器属性
		/// </summary>
		public readonly short WeaponAttr { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quality"></param>
        /// <param name="itemId"></param>
        /// <param name="rate"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="weaponAttr"></param> )
		public WeaponRandAttribute( short id, sbyte quality, int itemId, ushort rate, short minValue, short maxValue, short weaponAttr )
		{
			Id = id;
			Quality = quality;
			ItemId = itemId;
			Rate = rate;
			MinValue = minValue;
			MaxValue = maxValue;
			WeaponAttr = weaponAttr;
		}
	}

    /// <summary>
    /// WeaponRandAttributeSet that holds all the table data
    /// </summary>
    public partial class WeaponRandAttributeSet
    {
        public static readonly WeaponRandAttribute[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponRandAttributeSet()
        {
            Data = new WeaponRandAttribute[]
            {
					 new WeaponRandAttribute( 52, 2, 14, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 53, 3, 14, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 54, 4, 14, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 55, 5, 14, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 56, 2, 14, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 57, 3, 14, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 58, 4, 14, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 59, 5, 14, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 60, 2, 14, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 61, 3, 14, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 62, 4, 14, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 63, 5, 14, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 64, 2, 13, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 65, 3, 13, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 66, 4, 13, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 67, 5, 13, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 68, 2, 13, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 69, 3, 13, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 70, 4, 13, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 71, 5, 13, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 72, 2, 13, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 73, 3, 13, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 74, 4, 13, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 75, 5, 13, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 76, 2, 12, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 77, 3, 12, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 78, 4, 12, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 79, 5, 12, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 80, 2, 12, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 81, 3, 12, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 82, 4, 12, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 83, 5, 12, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 84, 2, 12, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 85, 3, 12, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 106, 4, 12, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 107, 5, 12, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 108, 2, 11, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 109, 3, 11, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 110, 4, 11, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 111, 5, 11, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 112, 2, 11, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 113, 3, 11, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 114, 4, 11, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 115, 5, 11, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 116, 2, 10, 58, 10, 24, 1 )
					, new WeaponRandAttribute( 117, 3, 10, 29, 25, 49, 1 )
					, new WeaponRandAttribute( 118, 4, 10, 10, 50, 74, 1 )
					, new WeaponRandAttribute( 119, 5, 10, 3, 75, 100, 1 )
					, new WeaponRandAttribute( 120, 2, 10, 58, 100, 249, 8 )
					, new WeaponRandAttribute( 121, 3, 10, 29, 250, 499, 8 )
					, new WeaponRandAttribute( 122, 4, 10, 10, 500, 749, 8 )
					, new WeaponRandAttribute( 123, 5, 10, 3, 750, 1000, 8 )
					, new WeaponRandAttribute( 124, 2, 11, 58, 200, 399, 10 )
					, new WeaponRandAttribute( 125, 3, 11, 29, 400, 599, 10 )
					, new WeaponRandAttribute( 126, 4, 11, 10, 600, 799, 10 )
					, new WeaponRandAttribute( 127, 5, 11, 3, 800, 1000, 10 )
					, new WeaponRandAttribute( 128, 2, 10, 58, 30, 59, 11 )
					, new WeaponRandAttribute( 129, 3, 10, 29, 60, 89, 11 )
					, new WeaponRandAttribute( 130, 4, 10, 10, 90, 119, 11 )
					, new WeaponRandAttribute( 131, 5, 10, 3, 120, 150, 11 )
					, new WeaponRandAttribute( 132, 2, 35, 58, 1, 5, 1 )
					, new WeaponRandAttribute( 133, 3, 35, 29, 6, 10, 1 )
					, new WeaponRandAttribute( 134, 4, 35, 10, 11, 15, 1 )
					, new WeaponRandAttribute( 135, 5, 35, 3, 16, 20, 1 )
					, new WeaponRandAttribute( 136, 2, 35, 58, 100, 249, 8 )
					, new WeaponRandAttribute( 137, 3, 35, 29, 250, 499, 8 )
					, new WeaponRandAttribute( 138, 4, 35, 10, 500, 749, 8 )
					, new WeaponRandAttribute( 139, 5, 35, 3, 750, 1000, 8 )
					, new WeaponRandAttribute( 140, 2, 36, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 141, 3, 36, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 142, 4, 36, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 143, 5, 36, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 144, 2, 36, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 145, 3, 36, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 146, 4, 36, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 147, 5, 36, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 148, 2, 36, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 149, 3, 36, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 150, 4, 36, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 151, 5, 36, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 152, 2, 38, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 153, 3, 38, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 154, 4, 38, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 155, 5, 38, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 156, 2, 38, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 157, 3, 38, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 158, 4, 38, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 159, 5, 38, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 160, 2, 38, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 161, 3, 38, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 162, 4, 38, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 163, 5, 38, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 164, 2, 40, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 165, 3, 40, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 166, 4, 40, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 167, 5, 40, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 168, 2, 40, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 169, 3, 40, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 170, 4, 40, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 171, 5, 40, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 172, 2, 40, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 173, 3, 40, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 174, 4, 40, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 175, 5, 40, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 176, 2, 43, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 177, 3, 43, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 178, 4, 43, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 179, 5, 43, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 180, 2, 43, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 181, 3, 43, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 182, 4, 43, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 183, 5, 43, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 184, 2, 43, 58, 30, 59, 11 )
					, new WeaponRandAttribute( 185, 3, 43, 29, 60, 89, 11 )
					, new WeaponRandAttribute( 186, 4, 43, 10, 90, 119, 11 )
					, new WeaponRandAttribute( 187, 5, 43, 3, 120, 150, 11 )
					, new WeaponRandAttribute( 188, 2, 44, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 189, 3, 44, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 190, 4, 44, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 191, 5, 44, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 192, 2, 44, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 193, 3, 44, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 194, 4, 44, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 195, 5, 44, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 196, 2, 44, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 197, 3, 44, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 198, 4, 44, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 199, 5, 44, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 200, 2, 49, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 201, 3, 49, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 202, 4, 49, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 203, 5, 49, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 204, 2, 49, 58, 50, 99, 9 )
					, new WeaponRandAttribute( 205, 3, 49, 29, 100, 199, 9 )
					, new WeaponRandAttribute( 206, 4, 49, 10, 200, 299, 9 )
					, new WeaponRandAttribute( 207, 5, 49, 3, 300, 400, 9 )
					, new WeaponRandAttribute( 208, 2, 49, 58, 200, 399, 10 )
					, new WeaponRandAttribute( 209, 3, 49, 29, 400, 599, 10 )
					, new WeaponRandAttribute( 210, 4, 49, 10, 600, 799, 10 )
					, new WeaponRandAttribute( 211, 5, 49, 3, 800, 1000, 10 )
					, new WeaponRandAttribute( 212, 2, 48, 58, 20, 39, 11 )
					, new WeaponRandAttribute( 213, 3, 48, 29, 40, 59, 11 )
					, new WeaponRandAttribute( 214, 4, 48, 10, 60, 79, 11 )
					, new WeaponRandAttribute( 215, 5, 48, 3, 80, 100, 11 )
					, new WeaponRandAttribute( 216, 2, 48, 58, 2000, 2499, 12 )
					, new WeaponRandAttribute( 217, 3, 48, 29, 2500, 2999, 12 )
					, new WeaponRandAttribute( 218, 4, 48, 10, 3000, 3499, 12 )
					, new WeaponRandAttribute( 219, 5, 48, 3, 3500, 4000, 12 )
					, new WeaponRandAttribute( 220, 2, 48, 58, 20, 39, 13 )
					, new WeaponRandAttribute( 221, 3, 48, 29, 40, 59, 13 )
					, new WeaponRandAttribute( 222, 4, 48, 10, 60, 79, 13 )
					, new WeaponRandAttribute( 223, 5, 48, 3, 80, 100, 13 )
					, new WeaponRandAttribute( 224, 2, 50, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 225, 3, 50, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 226, 4, 50, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 227, 5, 50, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 228, 2, 50, 58, 1, 3, 3 )
					, new WeaponRandAttribute( 229, 3, 50, 29, 4, 6, 3 )
					, new WeaponRandAttribute( 230, 4, 50, 10, 7, 9, 3 )
					, new WeaponRandAttribute( 231, 5, 50, 3, 10, 12, 3 )
					, new WeaponRandAttribute( 232, 2, 50, 58, 3, 5, 4 )
					, new WeaponRandAttribute( 233, 3, 50, 29, 6, 8, 4 )
					, new WeaponRandAttribute( 234, 4, 50, 10, 9, 11, 4 )
					, new WeaponRandAttribute( 235, 5, 50, 3, 12, 15, 4 )
					, new WeaponRandAttribute( 236, 2, 51, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 237, 3, 51, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 238, 4, 51, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 239, 5, 51, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 240, 2, 51, 58, 1, 2, 3 )
					, new WeaponRandAttribute( 241, 3, 51, 29, 3, 4, 3 )
					, new WeaponRandAttribute( 242, 4, 51, 10, 5, 6, 3 )
					, new WeaponRandAttribute( 243, 5, 51, 3, 7, 8, 3 )
					, new WeaponRandAttribute( 244, 2, 51, 58, 1, 2, 15 )
					, new WeaponRandAttribute( 245, 3, 51, 29, 3, 4, 15 )
					, new WeaponRandAttribute( 246, 4, 51, 10, 5, 7, 15 )
					, new WeaponRandAttribute( 247, 5, 51, 3, 8, 10, 15 )
					, new WeaponRandAttribute( 248, 2, 52, 58, 8, 16, 1 )
					, new WeaponRandAttribute( 249, 3, 52, 29, 17, 24, 1 )
					, new WeaponRandAttribute( 250, 4, 52, 10, 25, 32, 1 )
					, new WeaponRandAttribute( 251, 5, 52, 3, 33, 40, 1 )
					, new WeaponRandAttribute( 252, 2, 52, 58, 60, 120, 8 )
					, new WeaponRandAttribute( 253, 3, 52, 29, 121, 180, 8 )
					, new WeaponRandAttribute( 254, 4, 52, 10, 181, 240, 8 )
					, new WeaponRandAttribute( 255, 5, 52, 3, 241, 300, 8 )
					, new WeaponRandAttribute( 256, 2, 52, 58, 9, 18, 11 )
					, new WeaponRandAttribute( 257, 3, 52, 29, 19, 27, 11 )
					, new WeaponRandAttribute( 258, 4, 52, 10, 28, 36, 11 )
					, new WeaponRandAttribute( 259, 5, 52, 3, 37, 45, 11 )
					, new WeaponRandAttribute( 260, 2, 55, 58, 5, 9, 1 )
					, new WeaponRandAttribute( 261, 3, 55, 29, 10, 14, 1 )
					, new WeaponRandAttribute( 262, 4, 55, 10, 15, 19, 1 )
					, new WeaponRandAttribute( 263, 5, 55, 3, 20, 24, 1 )
					, new WeaponRandAttribute( 264, 2, 55, 58, 100, 249, 8 )
					, new WeaponRandAttribute( 265, 3, 55, 29, 250, 499, 8 )
					, new WeaponRandAttribute( 266, 4, 55, 10, 500, 749, 8 )
					, new WeaponRandAttribute( 267, 5, 55, 3, 750, 1000, 8 )
					, new WeaponRandAttribute( 268, 2, 55, 58, 200, 399, 10 )
					, new WeaponRandAttribute( 269, 3, 55, 29, 400, 599, 10 )
					, new WeaponRandAttribute( 270, 4, 55, 10, 600, 799, 10 )
					, new WeaponRandAttribute( 271, 5, 55, 3, 800, 1000, 10 )
					, new WeaponRandAttribute( 272, 2, 63, 58, 8, 14, 1 )
					, new WeaponRandAttribute( 273, 3, 63, 29, 15, 21, 1 )
					, new WeaponRandAttribute( 274, 4, 63, 10, 22, 26, 1 )
					, new WeaponRandAttribute( 275, 5, 63, 3, 28, 32, 1 )
					, new WeaponRandAttribute( 276, 2, 63, 58, 5, 12, 11 )
					, new WeaponRandAttribute( 277, 3, 63, 29, 13, 25, 11 )
					, new WeaponRandAttribute( 278, 4, 63, 10, 26, 37, 11 )
					, new WeaponRandAttribute( 279, 5, 63, 3, 38, 50, 11 )
					, new WeaponRandAttribute( 280, 2, 63, 58, 1, 2, 3 )
					, new WeaponRandAttribute( 281, 3, 63, 29, 3, 4, 3 )
					, new WeaponRandAttribute( 282, 4, 63, 10, 5, 6, 3 )
					, new WeaponRandAttribute( 283, 5, 63, 3, 7, 8, 3 )
            };
        }
		/// <summary>
		/// 根据指定条件获取 WeaponRandAttribute 列表
		/// </summary>
		/// <param name="ItemId"></param>
		/// <param name="WeaponAttr"></param>
		public static List<WeaponRandAttribute> GetWeaponRandAttributesByItemIdWeaponAttr( int itemId, short weaponAttr )
		{
			List<WeaponRandAttribute> result = new List<WeaponRandAttribute>();
			foreach (WeaponRandAttribute data in Data)
			{
				if ( data.ItemId == itemId && data.WeaponAttr == weaponAttr)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
