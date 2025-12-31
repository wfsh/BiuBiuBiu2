// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代怪物行为
    /// </summary>
	public struct MonsterBehavior
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 怪物标识
		/// </summary>
		public readonly string MonsterSign { get; }
		/// <summary>
		/// 行为标识
		/// </summary>
		public readonly string BehaviorSign { get; }
		/// <summary>
		/// 攻击移动速度
		/// </summary>
		public readonly float FightRunSpeed { get; }
		/// <summary>
		/// 警戒移动速度
		/// </summary>
		public readonly float AlertWalkSpeed { get; }
		/// <summary>
		/// 巡逻移动速度
		/// </summary>
		public readonly float PatrolWalkSpeed { get; }
		/// <summary>
		/// 命中时仇恨
		/// </summary>
		public readonly float OnHitHate { get; }
		/// <summary>
		/// 开火范围
		/// </summary>
		public readonly float CatchFireRange { get; }
		/// <summary>
		/// 视野内仇恨值的累积等级
		/// </summary>
		public readonly float GpoInSightHateFillLevel { get; }
		/// <summary>
		/// 视野半径
		/// </summary>
		public readonly float SightRadius { get; }
		/// <summary>
		/// 视野角度
		/// </summary>
		public readonly float SightAngle { get; }
		/// <summary>
		/// 仇恨衰减速度
		/// </summary>
		public readonly float HateReduceSpeed { get; }
		/// <summary>
		/// 仇恨上限
		/// </summary>
		public readonly float HateUpperLimit { get; }
		/// <summary>
		/// 触觉检测角度（半角
		/// </summary>
		public readonly byte TouchAngle { get; }
		/// <summary>
		/// 触觉检测距离（m）
		/// </summary>
		public readonly byte TouchRadius { get; }
		/// <summary>
		/// 触觉检测高度（m）（玩家脚底为基准点上下高度）
		/// </summary>
		public readonly byte TouchHeight { get; }
		/// <summary>
		/// 触觉添加仇恨值（点/0.5s）
		/// </summary>
		public readonly byte TouchHateSpeed { get; }
		/// <summary>
		/// 预警时间
		/// </summary>
		public readonly float AlertTime { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="monsterSign"></param>
        /// <param name="behaviorSign"></param>
        /// <param name="fightRunSpeed"></param>
        /// <param name="alertWalkSpeed"></param>
        /// <param name="patrolWalkSpeed"></param>
        /// <param name="onHitHate"></param>
        /// <param name="catchFireRange"></param>
        /// <param name="gpoInSightHateFillLevel"></param>
        /// <param name="sightRadius"></param>
        /// <param name="sightAngle"></param>
        /// <param name="hateReduceSpeed"></param>
        /// <param name="hateUpperLimit"></param>
        /// <param name="touchAngle"></param>
        /// <param name="touchRadius"></param>
        /// <param name="touchHeight"></param>
        /// <param name="touchHateSpeed"></param>
        /// <param name="alertTime"></param> )
		public MonsterBehavior( int id, string monsterSign, string behaviorSign, float fightRunSpeed, float alertWalkSpeed, float patrolWalkSpeed, float onHitHate, float catchFireRange, float gpoInSightHateFillLevel, float sightRadius, float sightAngle, float hateReduceSpeed, float hateUpperLimit, byte touchAngle, byte touchRadius, byte touchHeight, byte touchHateSpeed, float alertTime )
		{
			Id = id;
			MonsterSign = monsterSign;
			BehaviorSign = behaviorSign;
			FightRunSpeed = fightRunSpeed;
			AlertWalkSpeed = alertWalkSpeed;
			PatrolWalkSpeed = patrolWalkSpeed;
			OnHitHate = onHitHate;
			CatchFireRange = catchFireRange;
			GpoInSightHateFillLevel = gpoInSightHateFillLevel;
			SightRadius = sightRadius;
			SightAngle = sightAngle;
			HateReduceSpeed = hateReduceSpeed;
			HateUpperLimit = hateUpperLimit;
			TouchAngle = touchAngle;
			TouchRadius = touchRadius;
			TouchHeight = touchHeight;
			TouchHateSpeed = touchHateSpeed;
			AlertTime = alertTime;
		}
	}

    /// <summary>
    /// MonsterBehaviorSet that holds all the table data
    /// </summary>
    public partial class MonsterBehaviorSet
    {
        public static readonly MonsterBehavior[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static MonsterBehaviorSet()
        {
            Data = new MonsterBehavior[]
            {
					 new MonsterBehavior( 2, "GiantDaDa", "AIBehavior_GiantDaDa", 0f, 0f, 0f, 20f, 30f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 3, "HumanMonster", "AIBehavior_CharacterAI", 4f, 2f, 2f, 20f, 15f, 60f, 20f, 90f, 20f, 150f, 180, 15, 10, 40, 1.5f )
					, new MonsterBehavior( 6, "HumanMonster_Specific", "AIBehavior_CharacterAI", 4f, 2f, 2f, 20f, 30f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 40, 1.5f )
					, new MonsterBehavior( 9, "HumanMonster_AKM", "AIBehavior_CharacterAI_Advanced", 4f, 2f, 2f, 100f, 25f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 50, 2f )
					, new MonsterBehavior( 12, "HumanMonster_Galting", "AIBehavior_CharacterAI_Galting_3", 4f, 2f, 2f, 60f, 30f, 60f, 35f, 90f, 20f, 150f, 180, 20, 10, 50, 3f )
					, new MonsterBehavior( 18, "HumanMonster_RPG", "AIBehavior_CharacterAI_RPG_3", 4f, 2f, 2f, 60f, 30f, 60f, 40f, 90f, 20f, 150f, 180, 15, 10, 50, 4f )
					, new MonsterBehavior( 39, "JokerMonster", "AIBehavior_CharacterAI", 4f, 2f, 2f, 60f, 15f, 60f, 20f, 90f, 20f, 150f, 180, 15, 10, 40, 1.5f )
					, new MonsterBehavior( 42, "JokerMonster_Specific", "AIBehavior_CharacterAI", 4f, 2f, 2f, 60f, 30f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 40, 1.5f )
					, new MonsterBehavior( 45, "JokerMonster_M416", "AIBehavior_CharacterAI_Advanced", 4f, 2f, 2f, 100f, 25f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 50, 2f )
					, new MonsterBehavior( 51, "JokerMonster_MoveMent", "AIBehavior_CharacterAI_MoveMent_3", 5f, 2f, 2f, 100f, 20f, 60f, 25f, 90f, 20f, 150f, 180, 15, 10, 100, 3f )
					, new MonsterBehavior( 72, "GoldDash_JokerDrone_Spades", "AIBehavior_JokerDrone_Spades", 8f, 6f, 4f, 60f, 20f, 60f, 25f, 90f, 20f, 300f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 73, "GoldDash_JokerDrone_Hearts", "AIBehavior_JokerDrone_Hearts", 8f, 6f, 4f, 60f, 20f, 60f, 25f, 90f, 20f, 300f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 74, "GoldDash_JokerDrone_Clubs", "AIBehavior_JokerDrone_Clubs", 8f, 6f, 4f, 60f, 25f, 60f, 30f, 90f, 20f, 300f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 75, "GoldDash_JokerDrone_Diamonds", "AIBehavior_JokerDrone_Diamonds", 8f, 6f, 4f, 60f, 25f, 60f, 30f, 90f, 20f, 300f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 84, "JokerQueen", "AIBehavior_CharacterAI_Sniper", 4f, 2f, 2f, 60f, 66f, 60f, 66f, 90f, 20f, 150f, 180, 30, 10, 40, 0f )
					, new MonsterBehavior( 90, "MonsterCamp_HumanMonster_Elite_Sniper_M24", "AIBehavior_CharacterAI_beastcamp_Sniper", 0f, 0f, 0f, 60f, 150f, 60f, 100f, 90f, 20f, 150f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 91, "MonsterCamp_HumanMonster_Elite_ShieldMan", "AIBehavior_CharacterAI_beastcamp_ShieldMan", 6f, 6f, 4f, 60f, 20f, 60f, 15f, 90f, 20f, 150f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 92, "MonsterCamp_HumanMonster_Stalker", "AIBehavior_CharacterAI_beastcamp_Stalker", 4f, 6f, 4f, 60f, 20f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 93, "MonsterCamp_HumanMonster_Thug", "AIBehavior_CharacterAI_beastcamp_Thug", 4f, 6f, 4f, 60f, 20f, 60f, 30f, 90f, 20f, 150f, 180, 15, 10, 40, 0f )
					, new MonsterBehavior( 94, "HumanMonster_Tutorial", "AIBehavior_CharacterAI_Tutorial", 4f, 2f, 2f, 80f, 30f, 100f, 25f, 90f, 20f, 150f, 180, 0, 0, 0, 1.5f )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 MonsterBehavior
		/// </summary>
		/// <param name="MonsterSign"></param>
		public static MonsterBehavior GetMonsterBehaviorByMonsterSign( string monsterSign )
		{
			foreach (MonsterBehavior data in Data)
			{
				if ( data.MonsterSign == monsterSign )
				{
					return data;
				}
			}
			return default(MonsterBehavior);
		}

		/// <summary>
		/// 根据指定条件判断单个 MonsterBehavior 是否存在
		/// </summary>
		/// <param name="MonsterSign"></param>
		public static bool HasMonsterBehaviorByMonsterSign( string monsterSign )
		{
			foreach (MonsterBehavior data in Data)
			{
				if ( data.MonsterSign == monsterSign )
				{
					return true;
				}
			}
			return false;
		}
    }
}
