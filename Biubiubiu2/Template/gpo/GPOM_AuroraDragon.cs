// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 极光呆呆龙王
    /// </summary>
	public struct GPOM_AuroraDragon : IGPOM {
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
		/// 跟随距离
		/// </summary>
		public readonly float FollowDistance { get; }
		/// <summary>
		/// 跟随时间
		/// </summary>
		public readonly float FollowTime { get; }
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
		/// 移动速度
		/// </summary>
		public readonly float MoveSpeed { get; }
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
        /// <param name="assetSign"></param>
        /// <param name="awakeTime"></param>
        /// <param name="destoryTime"></param>
        /// <param name="followDistance"></param>
        /// <param name="followTime"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="lockTime"></param>
        /// <param name="mapQuality"></param>
        /// <param name="matchMode"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="name"></param>
        /// <param name="portrait"></param>
        /// <param name="quality"></param>
        /// <param name="rotaSpeed"></param>
        /// <param name="sign"></param> )
		public GPOM_AuroraDragon( string assetSign, float awakeTime, float destoryTime, float followDistance, float followTime, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, float lockTime, byte mapQuality, int matchMode, float moveSpeed, string name, string portrait, byte quality, float rotaSpeed, string sign ) {
			AssetSign = assetSign;
			AwakeTime = awakeTime;
			DestoryTime = destoryTime;
			FollowDistance = followDistance;
			FollowTime = followTime;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			LockTime = lockTime;
			MapQuality = mapQuality;
			MatchMode = matchMode;
			MoveSpeed = moveSpeed;
			Name = name;
			Portrait = portrait;
			Quality = quality;
			RotaSpeed = rotaSpeed;
			Sign = sign;
		}
	}

    /// <summary>
    /// GPOM_AuroraDragonSet that holds all the table data
    /// </summary>
    public static class GPOM_AuroraDragonSet {
        public static readonly GPOM_AuroraDragon[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_AuroraDragon = 176;
		public const int Id_AncientDragon = 177;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_AncientDragon = "AncientDragon";
		public const string Sign_AuroraDragon = "AuroraDragon";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_AuroraDragonSet() {
            Data = new GPOM_AuroraDragon[] {
					 new GPOM_AuroraDragon( "AuroraDragon", 5f, 5f, 10f, 0.5f, new int[]{100025}, 1, 12, 10000, 176, 1f, 1, 0, 4f, "极光呆呆龙王", "Portrait_Monster_11", 4, 10f, "AuroraDragon" )
					, new GPOM_AuroraDragon( "AncientDragon", 5f, 5f, 5f, 0.5f, new int[]{100026}, 1, 12, 10000, 177, 0.5f, 1, 0, 4f, "远古呆呆龙王", "Portrait_Monster_12", 4, 10f, "AncientDragon" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_AuroraDragon
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_AuroraDragon GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_AuroraDragon data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_AuroraDragon);
		}
    }
}
