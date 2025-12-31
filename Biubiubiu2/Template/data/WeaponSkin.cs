// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器皮肤表
    /// </summary>
	public struct WeaponSkin
	{
		/// <summary>
		/// 皮肤ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 武器物品ID/// 关联物品表物品id
		/// </summary>
		public readonly int WeaponItemId { get; }
		/// <summary>
		/// 皮肤物品ID/// 关联物品表物品id
		/// </summary>
		public readonly int SkinItemId { get; }
		/// <summary>
		/// 皮肤品质
		/// </summary>
		public readonly int Quality { get; }
		/// <summary>
		/// 是否默认皮肤
		/// </summary>
		public readonly bool IsDefault { get; }
		/// <summary>
		/// 皮肤排序
		/// </summary>
		public readonly int Sort { get; }
		/// <summary>
		/// 皮肤来源
		/// </summary>
		public readonly string Source { get; }
		/// <summary>
		/// 低端机是否显示特效
		/// </summary>
		public readonly bool SfxLow { get; }
		/// <summary>
		/// 高端机是否显示特效
		/// </summary>
		public readonly bool SfxHigh { get; }
		/// <summary>
		/// 皮肤属性
		/// </summary>
		public readonly short AttrId { get; }
		/// <summary>
		/// 属性数值
		/// </summary>
		public readonly int AttrValue { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="weaponItemId"></param>
        /// <param name="skinItemId"></param>
        /// <param name="quality"></param>
        /// <param name="isDefault"></param>
        /// <param name="sort"></param>
        /// <param name="source"></param>
        /// <param name="sfxLow"></param>
        /// <param name="sfxHigh"></param>
        /// <param name="attrId"></param>
        /// <param name="attrValue"></param> )
		public WeaponSkin( int id, int weaponItemId, int skinItemId, int quality, bool isDefault, int sort, string source, bool sfxLow, bool sfxHigh, short attrId, int attrValue )
		{
			Id = id;
			WeaponItemId = weaponItemId;
			SkinItemId = skinItemId;
			Quality = quality;
			IsDefault = isDefault;
			Sort = sort;
			Source = source;
			SfxLow = sfxLow;
			SfxHigh = sfxHigh;
			AttrId = attrId;
			AttrValue = attrValue;
		}
	}

    /// <summary>
    /// WeaponSkinSet that holds all the table data
    /// </summary>
    public partial class WeaponSkinSet
    {
        public static readonly WeaponSkin[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponSkinSet()
        {
            Data = new WeaponSkin[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 WeaponSkin
		/// </summary>
		/// <param name="SkinItemId"></param>
		public static WeaponSkin GetWeaponSkinBySkinItemId( int skinItemId )
		{
			foreach (WeaponSkin data in Data)
			{
				if ( data.SkinItemId == skinItemId )
				{
					return data;
				}
			}
			return default(WeaponSkin);
		}

		/// <summary>
		/// 根据指定条件判断单个 WeaponSkin 是否存在
		/// </summary>
		/// <param name="SkinItemId"></param>
		public static bool HasWeaponSkinBySkinItemId( int skinItemId )
		{
			foreach (WeaponSkin data in Data)
			{
				if ( data.SkinItemId == skinItemId )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取单个 WeaponSkin
		/// </summary>
		/// <param name="WeaponItemId"></param>
		/// <param name="IsDefault"></param>
		public static WeaponSkin GetWeaponSkinByWeaponItemIdIsDefault( int weaponItemId, bool isDefault )
		{
			foreach (WeaponSkin data in Data)
			{
				if ( data.WeaponItemId == weaponItemId && data.IsDefault == isDefault )
				{
					return data;
				}
			}
			return default(WeaponSkin);
		}

		/// <summary>
		/// 根据指定条件判断单个 WeaponSkin 是否存在
		/// </summary>
		/// <param name="WeaponItemId"></param>
		/// <param name="IsDefault"></param>
		public static bool HasWeaponSkinByWeaponItemIdIsDefault( int weaponItemId, bool isDefault )
		{
			foreach (WeaponSkin data in Data)
			{
				if ( data.WeaponItemId == weaponItemId && data.IsDefault == isDefault )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 WeaponSkin 列表
		/// </summary>
		/// <param name="WeaponItemId"></param>
		public static List<WeaponSkin> GetWeaponSkinsByWeaponItemId( int weaponItemId )
		{
			List<WeaponSkin> result = new List<WeaponSkin>();
			foreach (WeaponSkin data in Data)
			{
				if ( data.WeaponItemId == weaponItemId)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
