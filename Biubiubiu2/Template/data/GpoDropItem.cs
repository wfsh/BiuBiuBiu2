// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代GPO掉落物品表
    /// </summary>
	public struct GpoDropItem
	{
		/// <summary>
		/// ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 掉落类型
		/// </summary>
		public readonly byte DropType { get; }
		/// <summary>
		/// 掉落id
		/// </summary>
		public readonly long DropId { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dropType"></param>
        /// <param name="dropId"></param> )
		public GpoDropItem( int id, byte dropType, long dropId )
		{
			Id = id;
			DropType = dropType;
			DropId = dropId;
		}
	}

    /// <summary>
    /// GpoDropItemSet that holds all the table data
    /// </summary>
    public partial class GpoDropItemSet
    {
        public static readonly GpoDropItem[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoDropItemSet()
        {
            Data = new GpoDropItem[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GpoDropItem
		/// </summary>
		/// <param name="Id"></param>
		public static GpoDropItem GetGpoDropItemById( int id )
		{
			foreach (GpoDropItem data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GpoDropItem);
		}

		/// <summary>
		/// 根据指定条件判断单个 GpoDropItem 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoDropItemById( int id )
		{
			foreach (GpoDropItem data in Data)
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
