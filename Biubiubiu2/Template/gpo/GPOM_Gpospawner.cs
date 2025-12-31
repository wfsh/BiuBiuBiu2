// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// GPO生成器
    /// </summary>
	public struct GPOM_Gpospawner : IGPOM {
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
		/// GpoSO配置
		/// </summary>
		public readonly string GpoSoConfig { get; }
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
		/// <summary>
		/// 目标 GpoSign
		/// </summary>
		public readonly string TargetGpoSign { get; }

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
        /// <param name="gpoSoConfig"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="matchMode"></param>
        /// <param name="name"></param>
        /// <param name="quality"></param>
        /// <param name="sign"></param>
        /// <param name="targetGpoSign"></param> )
		public GPOM_Gpospawner( string assetSign, int[] gpoDropId, ushort gpoDropType, string gpoSoConfig, int gpoType, int hp, int id, int matchMode, string name, byte quality, string sign, string targetGpoSign ) {
			AssetSign = assetSign;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoSoConfig = gpoSoConfig;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			MatchMode = matchMode;
			Name = name;
			Quality = quality;
			Sign = sign;
			TargetGpoSign = targetGpoSign;
		}
	}

    /// <summary>
    /// GPOM_GpospawnerSet that holds all the table data
    /// </summary>
    public static class GPOM_GpospawnerSet {
        public static readonly GPOM_Gpospawner[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_GpospawnerDefense = 200;
		public const int Id_GpospawnerClearGpo = 201;
		public const int Id_GpospawnerGoldRush = 202;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_GpospawnerClearGpo = "GPOSpawnerClearGPO";
		public const string Sign_GpospawnerDefense = "GPOSpawnerDefense";
		public const string Sign_GpospawnerGoldRush = "GPOSpawnerGoldRush";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_GpospawnerSet() {
            Data = new GPOM_Gpospawner[] {
					 new GPOM_Gpospawner( "GPOSpawnerDefense", new int[]{100024}, 3, "GPOSpawnerDefense", 24, 10000, 200, 0, "GPO 生成器 - 防守", 1, "GPOSpawnerDefense", "Tank" )
					, new GPOM_Gpospawner( "GPOSpawnerClearGPO", new int[]{100024}, 3, "GPOSpawnerClear", 24, 0, 201, 0, "GPO 生成器 - 清怪", 1, "GPOSpawnerClearGPO", "" )
					, new GPOM_Gpospawner( "", new int[]{100024}, 3, "GPOSpawnerGoldRush", 24, 0, 202, 0, "GPO 生成器 - 夺金", 1, "GPOSpawnerGoldRush", "" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_Gpospawner
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_Gpospawner GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_Gpospawner data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_Gpospawner);
		}
    }
}
