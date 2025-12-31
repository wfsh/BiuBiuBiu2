// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代武器属性表
    /// </summary>
	public struct WeaponAttribute
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public readonly short Id { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public readonly string Name { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sign"></param>
        /// <param name="name"></param> )
		public WeaponAttribute( short id, string sign, string name )
		{
			Id = id;
			Sign = sign;
			Name = name;
		}
	}

    /// <summary>
    /// WeaponAttributeSet that holds all the table data
    /// </summary>
    public partial class WeaponAttributeSet
    {
        public static readonly WeaponAttribute[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const short Id_Attack = 1;
		public const short Id_IntervalTime = 2;
		public const short Id_MagazineNum = 3;
		public const short Id_EffectFireDistance = 4;
		public const short Id_ReloadTime = 5;
		public const short Id_FireRange = 6;
		public const short Id_BulletSpeed = 7;
		public const short Id_Hp = 8;
		public const short Id_FireDistance = 9;
		public const short Id_FireOverHotTime = 10;
		public const short Id_BombRange = 11;
		public const short Id_MissileDuration = 12;
		public const short Id_MissileRange = 13;
		public const short Id_VehicleHurtRatio = 14;
		public const short Id_DiffusionReduction = 15;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WeaponAttributeSet()
        {
            Data = new WeaponAttribute[]
            {
					 new WeaponAttribute( 1, "Attack", "火力" )
					, new WeaponAttribute( 2, "IntervalTime", "射击间隔(ms)" )
					, new WeaponAttribute( 3, "MagazineNum", "弹匣容量" )
					, new WeaponAttribute( 4, "EffectFireDistance", "有效射程" )
					, new WeaponAttribute( 5, "ReloadTime", "换弹时间(ms)" )
					, new WeaponAttribute( 6, "FireRange", "精准" )
					, new WeaponAttribute( 7, "BulletSpeed", "子弹速度(m/s)" )
					, new WeaponAttribute( 8, "HP", "血量" )
					, new WeaponAttribute( 9, "FireDistance", "射程" )
					, new WeaponAttribute( 10, "FireOverHotTime", "过热时间(ms)" )
					, new WeaponAttribute( 11, "BombRange", "爆炸范围(cm)" )
					, new WeaponAttribute( 12, "MissileDuration", "轰炸时间(ms)" )
					, new WeaponAttribute( 13, "MissileRange", "轰炸区范围(cm)" )
					, new WeaponAttribute( 14, "VehicleHurtRatio", "超级武装伤害倍数" )
					, new WeaponAttribute( 15, "DiffusionReduction", "扩散角度" )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 WeaponAttribute
		/// </summary>
		/// <param name="Id"></param>
		public static WeaponAttribute GetWeaponAttributeById( short id )
		{
			foreach (WeaponAttribute data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(WeaponAttribute);
		}

		/// <summary>
		/// 根据指定条件判断单个 WeaponAttribute 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasWeaponAttributeById( short id )
		{
			foreach (WeaponAttribute data in Data)
			{
				if ( data.Id == id )
				{
					return true;
				}
			}
			return false;
		}
    }
}
