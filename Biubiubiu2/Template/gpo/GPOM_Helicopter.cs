// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 直升机
    /// </summary>
	public struct GPOM_Helicopter : IGPOM {
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
		/// 攻击间隔
		/// </summary>
		public readonly float AttackIntervalTime { get; }
		/// <summary>
		/// 攻击范围
		/// </summary>
		public readonly float AttackRange { get; }
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
		/// 高度调整速度
		/// </summary>
		public readonly float HeightAdjustSpeed { get; }
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
		/// 最大射程距离
		/// </summary>
		public readonly float MaxAttackDistance { get; }
		/// <summary>
		/// 最大飞行高度
		/// </summary>
		public readonly float MaxFlyHeight { get; }
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
        /// <param name="attackIntervalTime"></param>
        /// <param name="attackRange"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="heightAdjustSpeed"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="matchMode"></param>
        /// <param name="maxAttackDistance"></param>
        /// <param name="maxFlyHeight"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="name"></param>
        /// <param name="petBehavior"></param>
        /// <param name="quality"></param>
        /// <param name="rotaSpeed"></param>
        /// <param name="sign"></param> )
		public GPOM_Helicopter( string aiBehavior, string assetSign, int atk, float attackIntervalTime, float attackRange, int[] gpoDropId, ushort gpoDropType, int gpoType, float heightAdjustSpeed, int hp, int id, int matchMode, float maxAttackDistance, float maxFlyHeight, float moveSpeed, string name, string petBehavior, byte quality, float rotaSpeed, string sign ) {
			AiBehavior = aiBehavior;
			AssetSign = assetSign;
			Atk = atk;
			AttackIntervalTime = attackIntervalTime;
			AttackRange = attackRange;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			HeightAdjustSpeed = heightAdjustSpeed;
			Hp = hp;
			Id = id;
			MatchMode = matchMode;
			MaxAttackDistance = maxAttackDistance;
			MaxFlyHeight = maxFlyHeight;
			MoveSpeed = moveSpeed;
			Name = name;
			PetBehavior = petBehavior;
			Quality = quality;
			RotaSpeed = rotaSpeed;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_HelicopterSet that holds all the table data
    /// </summary>
    public static class GPOM_HelicopterSet {
        public static readonly GPOM_Helicopter[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_Helicopter = 6;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_Helicopter = "Helicopter";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_HelicopterSet() {
            Data = new GPOM_Helicopter[] {
					 new GPOM_Helicopter( "AIBehavior_Helicopter", "Helicopter", 200, 0.025f, 5f, null, 0, 6, 1f, 5000, 6, 0, 100f, 15f, 6f, "直升机", "AIBehavior_Helicopter_Master", 2, 100f, "Helicopter" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_Helicopter
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_Helicopter GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_Helicopter data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_Helicopter);
		}
    }
}
