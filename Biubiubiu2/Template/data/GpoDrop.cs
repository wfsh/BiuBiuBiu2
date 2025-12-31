// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// GPO掉落表
    /// </summary>
	public struct GpoDrop
	{
		/// <summary>
		/// ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 掉落类型值列表
		/// </summary>
		public readonly float[] DropTypeValues { get; }
		/// <summary>
		/// 掉落ID列表
		/// </summary>
		public readonly int[] DropIds { get; }
		/// <summary>
		/// 是否可见
		/// </summary>
		public readonly byte IsVisible { get; }
		/// <summary>
		/// 掉落类型
		/// </summary>
		public readonly int DropType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dropTypeValues"></param>
        /// <param name="dropIds"></param>
        /// <param name="isVisible"></param>
        /// <param name="dropType"></param> )
		public GpoDrop( int id, float[] dropTypeValues, int[] dropIds, byte isVisible, int dropType )
		{
			Id = id;
			DropTypeValues = dropTypeValues;
			DropIds = dropIds;
			IsVisible = isVisible;
			DropType = dropType;
		}
	}

    /// <summary>
    /// GpoDropSet that holds all the table data
    /// </summary>
    public partial class GpoDropSet
    {
        public static readonly GpoDrop[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoDropSet()
        {
            Data = new GpoDrop[]
            {
					 new GpoDrop( 1, new float[]{}, new int[]{}, 0, 1 )
					, new GpoDrop( 2, new float[]{0.8f,0.5f,0.3f,0}, new int[]{1}, 0, 2 )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GpoDrop
		/// </summary>
		/// <param name="Id"></param>
		public static GpoDrop GetGpoDropById( int id )
		{
			foreach (GpoDrop data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GpoDrop);
		}

		/// <summary>
		/// 根据指定条件判断单个 GpoDrop 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoDropById( int id )
		{
			foreach (GpoDrop data in Data)
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
