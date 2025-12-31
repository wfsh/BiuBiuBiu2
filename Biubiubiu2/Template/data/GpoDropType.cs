// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// GPO掉落类型表
    /// </summary>
	public struct GpoDropType
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
		/// 是否可见
		/// </summary>
		public readonly byte IsVisible { get; }
		/// <summary>
		/// 掉落条件
		/// </summary>
		public readonly int DropCondition { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dropTypeValues"></param>
        /// <param name="isVisible"></param>
        /// <param name="dropCondition"></param> )
		public GpoDropType( int id, float[] dropTypeValues, byte isVisible, int dropCondition )
		{
			Id = id;
			DropTypeValues = dropTypeValues;
			IsVisible = isVisible;
			DropCondition = dropCondition;
		}
	}

    /// <summary>
    /// GpoDropTypeSet that holds all the table data
    /// </summary>
    public partial class GpoDropTypeSet
    {
        public static readonly GpoDropType[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoDropTypeSet()
        {
            Data = new GpoDropType[]
            {
					 new GpoDropType( 1, new float[]{}, 0, 5 )
					, new GpoDropType( 2, new float[]{0.8f,0.5f,0.3f,0f}, 0, 2 )
					, new GpoDropType( 3, new float[]{}, 0, 6 )
					, new GpoDropType( 4, new float[]{}, 0, 7 )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GpoDropType
		/// </summary>
		/// <param name="Id"></param>
		public static GpoDropType GetGpoDropTypeById( int id )
		{
			foreach (GpoDropType data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GpoDropType);
		}

		/// <summary>
		/// 根据指定条件判断单个 GpoDropType 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoDropTypeById( int id )
		{
			foreach (GpoDropType data in Data)
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
