// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 精英小丑
    /// </summary>
	public struct GPOM_AceJoker : IGPOM {
		/// <summary>
		/// GPO资产标识
		/// </summary>
		public readonly string AssetSign { get; }
		/// <summary>
		/// 初始化等待时间
		/// </summary>
		public readonly float AwakeTime { get; }
		/// <summary>
		/// 销毁等待时间
		/// </summary>
		public readonly float DestoryTime { get; }
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
		/// 锁定时间
		/// </summary>
		public readonly float LockTime { get; }
		/// <summary>
		/// 地图难度品质
		/// </summary>
		public readonly byte MapQuality { get; }
		/// <summary>
		/// 匹配模式
		/// </summary>
		public readonly int MatchMode { get; }
		/// <summary>
		/// 最大射程距离
		/// </summary>
		public readonly float MaxAttackDistance { get; }
		/// <summary>
		/// 距离地面高度
		/// </summary>
		public readonly float MaxGroundHeight { get; }
		/// <summary>
		/// 移动速度
		/// </summary>
		public readonly float MoveSpeed { get; }
		/// <summary>
		/// 移动时间
		/// </summary>
		public readonly float MoveTime { get; }
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
		/// 随机概率
		/// </summary>
		public readonly float Random { get; }
		/// <summary>
		/// 护盾距离
		/// </summary>
		public readonly float ShieldDistance { get; }
		/// <summary>
		/// GPO唯一标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 瞬移时间
		/// </summary>
		public readonly float TeleportTime { get; }
		/// <summary>
		/// 回血量
		/// </summary>
		public readonly float UpHp { get; }
		/// <summary>
		/// 回血间隔
		/// </summary>
		public readonly float UpHpInterval { get; }

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
        /// <param name="awakeTime"></param>
        /// <param name="destoryTime"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="lockTime"></param>
        /// <param name="mapQuality"></param>
        /// <param name="matchMode"></param>
        /// <param name="maxAttackDistance"></param>
        /// <param name="maxGroundHeight"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="moveTime"></param>
        /// <param name="name"></param>
        /// <param name="portrait"></param>
        /// <param name="quality"></param>
        /// <param name="random"></param>
        /// <param name="shieldDistance"></param>
        /// <param name="sign"></param>
        /// <param name="teleportTime"></param>
        /// <param name="upHp"></param>
        /// <param name="upHpInterval"></param> )
		public GPOM_AceJoker( string assetSign, float awakeTime, float destoryTime, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, float lockTime, byte mapQuality, int matchMode, float maxAttackDistance, float maxGroundHeight, float moveSpeed, float moveTime, string name, string portrait, byte quality, float random, float shieldDistance, string sign, float teleportTime, float upHp, float upHpInterval ) {
			AssetSign = assetSign;
			AwakeTime = awakeTime;
			DestoryTime = destoryTime;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			LockTime = lockTime;
			MapQuality = mapQuality;
			MatchMode = matchMode;
			MaxAttackDistance = maxAttackDistance;
			MaxGroundHeight = maxGroundHeight;
			MoveSpeed = moveSpeed;
			MoveTime = moveTime;
			Name = name;
			Portrait = portrait;
			Quality = quality;
			Random = random;
			ShieldDistance = shieldDistance;
			Sign = sign;
			TeleportTime = teleportTime;
			UpHp = upHp;
			UpHpInterval = upHpInterval;
		}
	}

    /// <summary>
    /// GPOM_AceJokerSet that holds all the table data
    /// </summary>
    public static class GPOM_AceJokerSet {
        public static readonly GPOM_AceJoker[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_GoldJoker = 178;
		public const int Id_AceJoker = 179;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_AceJoker = "AceJoker";
		public const string Sign_GoldJoker = "GoldJoker";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_AceJokerSet() {
            Data = new GPOM_AceJoker[] {
					 new GPOM_AceJoker( "GoldJoker", 0f, 1f, new int[]{100028}, 1, 13, 10000, 178, 0.5f, 1, 0, 2f, 3.6f, 3f, 1f, "至尊小丑", "Portrait_Monster_15", 4, 34f, 23f, "GoldJoker", 0.5f, 200f, 0.5f )
					, new GPOM_AceJoker( "AceJoker", 0f, 5f, new int[]{100027}, 1, 13, 10000, 179, 0.5f, 1, 0, 2f, 3.6f, 3f, 1f, "王牌小丑", "Portrait_Monster_14", 4, 34f, 30f, "AceJoker", 0.5f, 88f, 0.5f )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_AceJoker
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_AceJoker GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_AceJoker data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_AceJoker);
		}
    }
}
