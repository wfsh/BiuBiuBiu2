// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 巨像达达
    /// </summary>
	public struct GPOM_GiantDaDa : IGPOM {
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
		/// 地图难度品质
		/// </summary>
		public readonly byte MapQuality { get; }
		/// <summary>
		/// 匹配模式
		/// </summary>
		public readonly int MatchMode { get; }
		/// <summary>
		/// GPO名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 头像
		/// </summary>
		public readonly string Portrait { get; }
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
        /// <param name="mapQuality"></param>
        /// <param name="matchMode"></param>
        /// <param name="name"></param>
        /// <param name="portrait"></param>
        /// <param name="quality"></param>
        /// <param name="sign"></param> )
		public GPOM_GiantDaDa( string assetSign, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, byte mapQuality, int matchMode, string name, string portrait, byte quality, string sign ) {
			AssetSign = assetSign;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			MapQuality = mapQuality;
			MatchMode = matchMode;
			Name = name;
			Portrait = portrait;
			Quality = quality;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_GiantDaDaSet that holds all the table data
    /// </summary>
    public static class GPOM_GiantDaDaSet {
        public static readonly GPOM_GiantDaDa[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_GiantDaDa = 100;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_GiantDaDa = "GiantDaDa";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_GiantDaDaSet() {
            Data = new GPOM_GiantDaDa[] {
					 new GPOM_GiantDaDa( "GiantDaDa", new int[]{100024}, 1, 11, 10000, 100, 1, 0, "巨像达达", "Portrait_Monster_2", 4, "GiantDaDa" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_GiantDaDa
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_GiantDaDa GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_GiantDaDa data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_GiantDaDa);
		}
    }
}
