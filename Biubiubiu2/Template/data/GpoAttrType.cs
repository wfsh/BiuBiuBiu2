// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代GPO属性类型表
    /// </summary>
	public struct GpoAttrType
	{
		/// <summary>
		/// ID
		/// </summary>
		public readonly ushort Id { get; }
		/// <summary>
		/// 属性名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 属性标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 值类型
		/// </summary>
		public readonly string ValueType { get; }
		/// <summary>
		/// 是否默认属性
		/// </summary>
		public readonly bool IsDefault { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="valueType"></param>
        /// <param name="isDefault"></param> )
		public GpoAttrType( ushort id, string name, string sign, string valueType, bool isDefault )
		{
			Id = id;
			Name = name;
			Sign = sign;
			ValueType = valueType;
			IsDefault = isDefault;
		}
	}

    /// <summary>
    /// GpoAttrTypeSet that holds all the table data
    /// </summary>
    public partial class GpoAttrTypeSet
    {
        public static readonly GpoAttrType[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const ushort Id_Hp = 1;
		public const ushort Id_Atk = 2;
		public const ushort Id_MoveSpeed = 3;
		public const ushort Id_AttackRange = 4;
		public const ushort Id_RotaSpeed = 5;
		public const ushort Id_SkillType = 6;
		public const ushort Id_AiBehavior = 7;
		public const ushort Id_PetBehavior = 8;
		public const ushort Id_MaxFlyHeight = 9;
		public const ushort Id_MaxAttackDistance = 10;
		public const ushort Id_MaxGroundHeight = 11;
		public const ushort Id_TrajectoryPointCount = 12;
		public const ushort Id_AttackIntervalTime = 13;
		public const ushort Id_HeightAdjustSpeed = 14;
		public const ushort Id_JumpHeight = 16;
		public const ushort Id_AirJumpHeight = 17;
		public const ushort Id_Quality = 18;
		public const ushort Id_GpoDropId = 19;
		public const ushort Id_MapQuality = 20;
		public const ushort Id_Portrait = 21;
		public const ushort Id_IsRay = 22;
		public const ushort Id_ChargingTime = 23;
		public const ushort Id_Duration = 24;
		public const ushort Id_FollowSpeed = 25;
		public const ushort Id_FireRange = 26;
		public const ushort Id_ChargeEffectId = 27;
		public const ushort Id_FireEffectId = 28;
		public const ushort Id_AttackEffectId = 29;
		public const ushort Id_UpHp = 30;
		public const ushort Id_UpHpInterval = 31;
		public const ushort Id_ShieldDistance = 32;
		public const ushort Id_BulletMoveSpeed = 33;
		public const ushort Id_AwakeTime = 34;
		public const ushort Id_DestoryTime = 35;
		public const ushort Id_LockTime = 36;
		public const ushort Id_FollowDistance = 37;
		public const ushort Id_FollowTime = 38;
		public const ushort Id_MoveTime = 39;
		public const ushort Id_TeleportTime = 40;
		public const ushort Id_Random = 41;
		public const ushort Id_Category = 42;
		public const ushort Id_GpoSoConfig = 45;
		public const ushort Id_DelayTime = 46;
		public const ushort Id_TargetGpoId = 48;
		public const ushort Id_TargetGpoSign = 50;
		public const ushort Id_GpoDropType = 58;
		public const ushort Id_Id = 59;
		public const ushort Id_GpoType = 60;
		public const ushort Id_Name = 61;
		public const ushort Id_Sign = 62;
		public const ushort Id_AssetSign = 63;
		public const ushort Id_MatchMode = 64;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoAttrTypeSet()
        {
            Data = new GpoAttrType[]
            {
					 new GpoAttrType( 1, "血量", "hp", "int", false )
					, new GpoAttrType( 2, "攻击力", "atk", "int", false )
					, new GpoAttrType( 3, "移动速度", "move_speed", "float", false )
					, new GpoAttrType( 4, "攻击范围", "attack_range", "float", false )
					, new GpoAttrType( 5, "转向速度", "rota_speed", "float", false )
					, new GpoAttrType( 6, "技能类型", "skill_type", "int", false )
					, new GpoAttrType( 7, "基础 AI 行为树名称", "ai_behavior", "string", false )
					, new GpoAttrType( 8, "宠物 AI 行为树名称", "pet_behavior", "string", false )
					, new GpoAttrType( 9, "最大飞行高度", "max_fly_height", "float", false )
					, new GpoAttrType( 10, "最大射程距离", "max_attack_distance", "float", false )
					, new GpoAttrType( 11, "距离地面高度", "max_ground_height", "float", false )
					, new GpoAttrType( 12, "抛物线轨迹点数量", "trajectory_point_count", "byte", false )
					, new GpoAttrType( 13, "攻击间隔", "attack_interval_time", "float", false )
					, new GpoAttrType( 14, "高度调整速度", "height_adjust_speed", "float", false )
					, new GpoAttrType( 16, "跳跃高度", "jump_height", "float", false )
					, new GpoAttrType( 17, "二段跳高度", "air_jump_height", "float", false )
					, new GpoAttrType( 18, "GPO 品质", "quality", "byte", true )
					, new GpoAttrType( 19, "GPO掉落ID", "gpo_drop_id", "[]int", true )
					, new GpoAttrType( 20, "地图难度品质", "map_quality", "byte", false )
					, new GpoAttrType( 21, "头像", "portrait", "string", false )
					, new GpoAttrType( 22, "是否射线", "is_ray", "bool", false )
					, new GpoAttrType( 23, "蓄力时间", "charging_time", "float", false )
					, new GpoAttrType( 24, "持续时间", "duration", "float", false )
					, new GpoAttrType( 25, "跟随速度", "follow_speed", "float", false )
					, new GpoAttrType( 26, "扩散", "fire_range", "float", false )
					, new GpoAttrType( 27, "蓄力特效 id", "charge_effect_id", "byte", false )
					, new GpoAttrType( 28, "开火特效 id", "fire_effect_id", "byte", false )
					, new GpoAttrType( 29, "攻击特效 id", "attack_effect_id", "byte", false )
					, new GpoAttrType( 30, "回血量", "up_hp", "float", false )
					, new GpoAttrType( 31, "回血间隔", "up_hp_interval", "float", false )
					, new GpoAttrType( 32, "护盾距离", "shield_distance", "float", false )
					, new GpoAttrType( 33, "子弹移动速度", "bullet_move_speed", "float", false )
					, new GpoAttrType( 34, "初始化等待时间", "awake_time", "float", false )
					, new GpoAttrType( 35, "销毁等待时间", "destory_time", "float", false )
					, new GpoAttrType( 36, "锁定时间", "lock_time", "float", false )
					, new GpoAttrType( 37, "跟随距离", "follow_distance", "float", false )
					, new GpoAttrType( 38, "跟随时间", "follow_time", "float", false )
					, new GpoAttrType( 39, "移动时间", "move_time", "float", false )
					, new GpoAttrType( 40, "瞬移时间", "teleport_time", "float", false )
					, new GpoAttrType( 41, "随机概率", "random", "float", false )
					, new GpoAttrType( 42, "GPO分类类型", "category", "int", false )
					, new GpoAttrType( 45, "GpoSO配置", "gpo_so_config", "string", false )
					, new GpoAttrType( 46, "延迟时间", "delay_time", "float", false )
					, new GpoAttrType( 48, "目标 GpoId", "target_gpo_id", "int", false )
					, new GpoAttrType( 50, "目标 GpoSign", "target_gpo_sign", "string", false )
					, new GpoAttrType( 58, "GPO掉落类型", "gpo_drop_type", "ushort", true )
					, new GpoAttrType( 59, "GPOID", "id", "int", true )
					, new GpoAttrType( 60, "GPO类型", "gpo_type", "int", true )
					, new GpoAttrType( 61, "GPO名称", "name", "string", true )
					, new GpoAttrType( 62, "GPO唯一标识", "sign", "string", true )
					, new GpoAttrType( 63, "GPO资产标识", "asset_sign", "string", true )
					, new GpoAttrType( 64, "匹配模式", "match_mode", "int", true )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GpoAttrType
		/// </summary>
		/// <param name="Id"></param>
		public static GpoAttrType GetGpoAttrTypeById( ushort id )
		{
			foreach (GpoAttrType data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GpoAttrType);
		}

		/// <summary>
		/// 根据指定条件判断单个 GpoAttrType 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoAttrTypeById( ushort id )
		{
			foreach (GpoAttrType data in Data)
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
