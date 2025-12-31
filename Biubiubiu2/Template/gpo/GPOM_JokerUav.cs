// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 小丑无人机
    /// </summary>
	public struct GPOM_JokerUav : IGPOM {
		/// <summary>
		/// GPO资产标识
		/// </summary>
		public readonly string AssetSign { get; }
		/// <summary>
		/// 攻击力
		/// </summary>
		public readonly int Atk { get; }
		/// <summary>
		/// 攻击特效 id
		/// </summary>
		public readonly byte AttackEffectId { get; }
		/// <summary>
		/// 攻击间隔
		/// </summary>
		public readonly float AttackIntervalTime { get; }
		/// <summary>
		/// 子弹移动速度
		/// </summary>
		public readonly float BulletMoveSpeed { get; }
		/// <summary>
		/// 蓄力特效 id
		/// </summary>
		public readonly byte ChargeEffectId { get; }
		/// <summary>
		/// 蓄力时间
		/// </summary>
		public readonly float ChargingTime { get; }
		/// <summary>
		/// 持续时间
		/// </summary>
		public readonly float Duration { get; }
		/// <summary>
		/// 开火特效 id
		/// </summary>
		public readonly byte FireEffectId { get; }
		/// <summary>
		/// 扩散
		/// </summary>
		public readonly float FireRange { get; }
		/// <summary>
		/// 跟随速度
		/// </summary>
		public readonly float FollowSpeed { get; }
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
		/// 是否射线
		/// </summary>
		public readonly bool IsRay { get; }
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
		/// 护盾距离
		/// </summary>
		public readonly float ShieldDistance { get; }
		/// <summary>
		/// GPO唯一标识
		/// </summary>
		public readonly string Sign { get; }
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
        /// <param name="atk"></param>
        /// <param name="attackEffectId"></param>
        /// <param name="attackIntervalTime"></param>
        /// <param name="bulletMoveSpeed"></param>
        /// <param name="chargeEffectId"></param>
        /// <param name="chargingTime"></param>
        /// <param name="duration"></param>
        /// <param name="fireEffectId"></param>
        /// <param name="fireRange"></param>
        /// <param name="followSpeed"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="isRay"></param>
        /// <param name="mapQuality"></param>
        /// <param name="matchMode"></param>
        /// <param name="maxAttackDistance"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="name"></param>
        /// <param name="portrait"></param>
        /// <param name="quality"></param>
        /// <param name="shieldDistance"></param>
        /// <param name="sign"></param>
        /// <param name="upHp"></param>
        /// <param name="upHpInterval"></param> )
		public GPOM_JokerUav( string assetSign, int atk, byte attackEffectId, float attackIntervalTime, float bulletMoveSpeed, byte chargeEffectId, float chargingTime, float duration, byte fireEffectId, float fireRange, float followSpeed, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, bool isRay, byte mapQuality, int matchMode, float maxAttackDistance, float moveSpeed, string name, string portrait, byte quality, float shieldDistance, string sign, float upHp, float upHpInterval ) {
			AssetSign = assetSign;
			Atk = atk;
			AttackEffectId = attackEffectId;
			AttackIntervalTime = attackIntervalTime;
			BulletMoveSpeed = bulletMoveSpeed;
			ChargeEffectId = chargeEffectId;
			ChargingTime = chargingTime;
			Duration = duration;
			FireEffectId = fireEffectId;
			FireRange = fireRange;
			FollowSpeed = followSpeed;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			IsRay = isRay;
			MapQuality = mapQuality;
			MatchMode = matchMode;
			MaxAttackDistance = maxAttackDistance;
			MoveSpeed = moveSpeed;
			Name = name;
			Portrait = portrait;
			Quality = quality;
			ShieldDistance = shieldDistance;
			Sign = sign;
			UpHp = upHp;
			UpHpInterval = upHpInterval;
		}
	}

    /// <summary>
    /// GPOM_JokerUavSet that holds all the table data
    /// </summary>
    public static class GPOM_JokerUavSet {
        public static readonly GPOM_JokerUav[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_GoldDashJokerDroneSpades = 180;
		public const int Id_GoldDashJokerDroneHearts = 181;
		public const int Id_GoldDashJokerDroneClubs = 182;
		public const int Id_GoldDashJokerDroneDiamonds = 183;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_GoldDashJokerDroneClubs = "GoldDash_JokerDrone_Clubs";
		public const string Sign_GoldDashJokerDroneDiamonds = "GoldDash_JokerDrone_Diamonds";
		public const string Sign_GoldDashJokerDroneHearts = "GoldDash_JokerDrone_Hearts";
		public const string Sign_GoldDashJokerDroneSpades = "GoldDash_JokerDrone_Spades";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_JokerUavSet() {
            Data = new GPOM_JokerUav[] {
					 new GPOM_JokerUav( "GoldDash_JokerDrone_Spades", 12, 11, 0.1f, 300f, 31, 3f, 5f, 32, 3f, 5.6f, new int[]{100029}, 1, 14, 2300, 180, false, 1, 0, 50f, 4f, "小丑无人机-黑桃", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Spades", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Spades", 30, 11, 0.1f, 300f, 31, 3f, 5f, 32, 3f, 5.6f, new int[]{100029}, 1, 14, 3250, 180, false, 2, 119, 50f, 4f, "小丑无人机-黑桃", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Spades", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Spades", 38, 11, 0.1f, 300f, 31, 3f, 5f, 32, 3f, 5.6f, new int[]{100029}, 1, 14, 4000, 180, false, 3, 120, 50f, 4f, "小丑无人机-黑桃", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Spades", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Hearts", 25, 1, 1f, 100f, 27, 3f, 5f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 2300, 181, true, 1, 0, 40f, 4f, "小丑无人机-红心", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Hearts", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Hearts", 62, 1, 1f, 100f, 27, 3f, 5f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 3250, 181, true, 2, 119, 40f, 4f, "小丑无人机-红心", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Hearts", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Hearts", 80, 1, 1f, 100f, 27, 3f, 5f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 4000, 181, true, 3, 120, 40f, 4f, "小丑无人机-红心", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Hearts", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Clubs", 20, 2, 1f, 100f, 25, 3f, 3f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 2300, 182, true, 1, 0, 25f, 4f, "小丑无人机-梅花", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Clubs", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Clubs", 50, 2, 1f, 100f, 25, 3f, 3f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 3250, 182, true, 2, 119, 25f, 4f, "小丑无人机-梅花", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Clubs", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Clubs", 64, 2, 1f, 100f, 25, 3f, 3f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 4000, 182, true, 3, 120, 25f, 4f, "小丑无人机-梅花", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Clubs", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Diamonds", 24, 3, 1f, 100f, 14, 3f, 4f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 2300, 183, true, 1, 0, 25f, 4f, "小丑无人机-方片", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Diamonds", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Diamonds", 60, 3, 1f, 100f, 14, 3f, 4f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 3250, 183, true, 2, 119, 25f, 4f, "小丑无人机-方片", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Diamonds", 10f, 1f )
					, new GPOM_JokerUav( "GoldDash_JokerDrone_Diamonds", 76, 3, 1f, 100f, 14, 3f, 4f, 0, 1f, 5.6f, new int[]{100029}, 1, 14, 4000, 183, true, 3, 120, 25f, 4f, "小丑无人机-方片", "Portrait_Monster_13", 3, 35f, "GoldDash_JokerDrone_Diamonds", 10f, 1f )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_JokerUav
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_JokerUav GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_JokerUav data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_JokerUav);
		}
    }
}
