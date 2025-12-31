// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 重型防御机枪
    /// </summary>
	public struct GPOM_MachineGun : IGPOM {
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
        /// <param name="atk"></param>
        /// <param name="attackIntervalTime"></param>
        /// <param name="attackRange"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="matchMode"></param>
        /// <param name="maxAttackDistance"></param>
        /// <param name="name"></param>
        /// <param name="quality"></param>
        /// <param name="sign"></param> )
		public GPOM_MachineGun( string assetSign, int atk, float attackIntervalTime, float attackRange, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, int matchMode, float maxAttackDistance, string name, byte quality, string sign ) {
			AssetSign = assetSign;
			Atk = atk;
			AttackIntervalTime = attackIntervalTime;
			AttackRange = attackRange;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			MatchMode = matchMode;
			MaxAttackDistance = maxAttackDistance;
			Name = name;
			Quality = quality;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_MachineGunSet that holds all the table data
    /// </summary>
    public static class GPOM_MachineGunSet {
        public static readonly GPOM_MachineGun[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_MachineGun = 8;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_MachineGun = "MachineGun";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_MachineGunSet() {
            Data = new GPOM_MachineGun[] {
					 new GPOM_MachineGun( "MachineGun", 200, 0.05f, 5f, null, 0, 8, 8000, 8, 0, 100f, "重型防御机枪", 1, "MachineGun" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_MachineGun
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_MachineGun GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_MachineGun data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_MachineGun);
		}
    }
}
