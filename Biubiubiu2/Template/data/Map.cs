// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代地图
    /// </summary>
	public struct Map
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 资源标识
		/// </summary>
		public readonly string ResSign { get; }
		/// <summary>
		/// 图标标识
		/// </summary>
		public readonly string ResIcon { get; }
		/// <summary>
		/// 默认拥有
		/// </summary>
		public readonly bool IsDefault { get; }
		/// <summary>
		/// 排序
		/// </summary>
		public readonly int Sort { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="resSign"></param>
        /// <param name="resIcon"></param>
        /// <param name="isDefault"></param>
        /// <param name="sort"></param> )
		public Map( int id, string name, string sign, string resSign, string resIcon, bool isDefault, int sort )
		{
			Id = id;
			Name = name;
			Sign = sign;
			ResSign = resSign;
			ResIcon = resIcon;
			IsDefault = isDefault;
			Sort = sort;
		}
	}

    /// <summary>
    /// MapSet that holds all the table data
    /// </summary>
    public partial class MapSet
    {
        public static readonly Map[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static MapSet()
        {
            Data = new Map[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 Map
		/// </summary>
		/// <param name="Id"></param>
		public static Map GetMapById( int id )
		{
			foreach (Map data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(Map);
		}

		/// <summary>
		/// 根据指定条件判断单个 Map 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasMapById( int id )
		{
			foreach (Map data in Data)
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
