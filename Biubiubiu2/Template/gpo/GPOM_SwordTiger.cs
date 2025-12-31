// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 剑齿虎
    /// </summary>
	public struct GPOM_SwordTiger : IGPOM {
		/// <summary>
		/// 基础 AI 行为树名称
		/// </summary>
		public readonly string AiBehavior { get; }
		/// <summary>
		/// GPO资产标识
		/// </summary>
		public readonly string AssetSign { get; }
		/// <summary>
		/// 攻击力
		/// </summary>
		public readonly int Atk { get; }
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
		/// 移动速度
		/// </summary>
		public readonly float MoveSpeed { get; }
		/// <summary>
		/// GPO名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 宠物 AI 行为树名称
		/// </summary>
		public readonly string PetBehavior { get; }
		/// <summary>
		/// GPO 品质
		/// </summary>
		public readonly byte Quality { get; }
		/// <summary>
		/// 转向速度
		/// </summary>
		public readonly float RotaSpeed { get; }
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
        /// <param name="aiBehavior"></param>
        /// <param name="assetSign"></param>
        /// <param name="atk"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="matchMode"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="name"></param>
        /// <param name="petBehavior"></param>
        /// <param name="quality"></param>
        /// <param name="rotaSpeed"></param>
        /// <param name="sign"></param> )
		public GPOM_SwordTiger( string aiBehavior, string assetSign, int atk, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, int matchMode, float moveSpeed, string name, string petBehavior, byte quality, float rotaSpeed, string sign ) {
			AiBehavior = aiBehavior;
			AssetSign = assetSign;
			Atk = atk;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			MatchMode = matchMode;
			MoveSpeed = moveSpeed;
			Name = name;
			PetBehavior = petBehavior;
			Quality = quality;
			RotaSpeed = rotaSpeed;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_SwordTigerSet that holds all the table data
    /// </summary>
    public static class GPOM_SwordTigerSet {
        public static readonly GPOM_SwordTiger[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_SwordTiger = 5;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_SwordTiger = "SwordTiger";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_SwordTigerSet() {
            Data = new GPOM_SwordTiger[] {
					 new GPOM_SwordTiger( "AIBehavior_SwordTiger_Master", "SwordTiger", 50, null, 0, 5, 500, 5, 0, 4f, "剑齿虎", "AIBehavior_SwordTiger", 2, 100f, "SwordTiger" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_SwordTiger
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_SwordTiger GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_SwordTiger data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_SwordTiger);
		}
    }
}
