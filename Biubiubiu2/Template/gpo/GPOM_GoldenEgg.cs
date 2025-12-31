// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 夺金-蛋
    /// </summary>
	public struct GPOM_GoldenEgg : IGPOM {
		/// <summary>
		/// GPO资产标识
		/// </summary>
		public readonly string AssetSign { get; }
		/// <summary>
		/// GPO掉落ID
		/// </summary>
		public readonly int[] GpoDropId { get; }
		/// <summary>
		/// GPO掉落类型
		/// </summary>
		public readonly ushort GpoDropType { get; }
		/// <summary>
		/// GPO类型
		/// </summary>
		public readonly int GpoType { get; }
		/// <summary>
		/// 血量
		/// </summary>
		public readonly int Hp { get; }
		/// <summary>
		/// GPOID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 匹配模式
		/// </summary>
		public readonly int MatchMode { get; }
		/// <summary>
		/// GPO名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// GPO 品质
		/// </summary>
		public readonly byte Quality { get; }
		/// <summary>
		/// GPO唯一标识
		/// </summary>
		public readonly string Sign { get; }

		// 实现IGPOM接口
		public string GetAssetSign() => AssetSign;
		public int[] GetGpoDropId() => GpoDropId;
		public ushort GetGpoDropType() => GpoDropType;
		public int GetGpoType() => GpoType;
		public int GetId() => Id;
		public int GetMatchMode() => MatchMode;
		public string GetName() => Name;
		public byte GetQuality() => Quality;
		public string GetSign() => Sign;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assetSign"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="matchMode"></param>
        /// <param name="name"></param>
        /// <param name="quality"></param>
        /// <param name="sign"></param> )
		public GPOM_GoldenEgg( string assetSign, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, int matchMode, string name, byte quality, string sign ) {
			AssetSign = assetSign;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			MatchMode = matchMode;
			Name = name;
			Quality = quality;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_GoldenEggSet that holds all the table data
    /// </summary>
    public static class GPOM_GoldenEggSet {
        public static readonly GPOM_GoldenEgg[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_GoldenEgg = 13;
		public const int Id_GoldenBigEgg = 14;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_GoldenBigEgg = "GoldenBigEgg";
		public const string Sign_GoldenEgg = "GoldenEgg";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_GoldenEggSet() {
            Data = new GPOM_GoldenEgg[] {
					 new GPOM_GoldenEgg( "GoldenEgg", null, 0, 10, 1000, 13, 0, "夺金-蛋", 1, "GoldenEgg" )
					, new GPOM_GoldenEgg( "GoldenBigEgg", null, 0, 10, 10000, 14, 0, "夺金-巨蛋", 1, "GoldenBigEgg" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_GoldenEgg
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_GoldenEgg GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_GoldenEgg data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_GoldenEgg);
		}
    }
}
