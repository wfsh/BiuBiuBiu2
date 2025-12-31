// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代怪物伤害自定义
    /// </summary>
	public struct MonsterArmedCustom
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
		/// 武器物品id
		/// </summary>
		public readonly int WeaponItemId { get; }
		/// <summary>
		/// 匹配模式
		/// </summary>
		public readonly int MatchMode { get; }
		/// <summary>
		/// 增伤百分比(小数)
		/// </summary>
		public readonly float DamageIncRatio { get; }
		/// <summary>
		/// 地图难度增伤百分比(小数)
		/// </summary>
		public readonly float MapDamageIncRatio { get; }
		/// <summary>
		/// 基础命中率(整数)
		/// </summary>
		public readonly float Accuracy { get; }
		/// <summary>
		/// 头部命中率(整数)
		/// </summary>
		public readonly float HitHeadAccuracy { get; }
		/// <summary>
		/// 攻击间隔
		/// </summary>
		public readonly float AttackInterval { get; }
		/// <summary>
		/// 攻击持续时间
		/// </summary>
		public readonly float AttackDuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="monsterSign"></param>
        /// <param name="weaponItemId"></param>
        /// <param name="matchMode"></param>
        /// <param name="damageIncRatio"></param>
        /// <param name="mapDamageIncRatio"></param>
        /// <param name="accuracy"></param>
        /// <param name="hitHeadAccuracy"></param>
        /// <param name="attackInterval"></param>
        /// <param name="attackDuration"></param> )
		public MonsterArmedCustom( int id, string monsterSign, int weaponItemId, int matchMode, float damageIncRatio, float mapDamageIncRatio, float accuracy, float hitHeadAccuracy, float attackInterval, float attackDuration )
		{
			Id = id;
			MonsterSign = monsterSign;
			WeaponItemId = weaponItemId;
			MatchMode = matchMode;
			DamageIncRatio = damageIncRatio;
			MapDamageIncRatio = mapDamageIncRatio;
			Accuracy = accuracy;
			HitHeadAccuracy = hitHeadAccuracy;
			AttackInterval = attackInterval;
			AttackDuration = attackDuration;
		}
	}

    /// <summary>
    /// MonsterArmedCustomSet that holds all the table data
    /// </summary>
    public partial class MonsterArmedCustomSet
    {
        public static readonly MonsterArmedCustom[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static MonsterArmedCustomSet()
        {
            Data = new MonsterArmedCustom[]
            {
					 new MonsterArmedCustom( 1, "HumanMonster", 5, 73, 0f, 0f, 25f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 2, "HumanMonster", 5, 106, 0f, 1.5f, 30f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 3, "HumanMonster", 5, 108, 0f, 2.2f, 35f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 4, "HumanMonster", 5, 74, 0f, 0f, 25f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 5, "HumanMonster", 5, 107, 0f, 1.5f, 30f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 6, "HumanMonster", 5, 109, 0f, 2.2f, 35f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 7, "HumanMonster_Specific", 5, 73, 0f, 0f, 15f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 8, "HumanMonster_Specific", 5, 106, 0f, 1.5f, 35f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 9, "HumanMonster_Specific", 5, 108, 0f, 2.2f, 35f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 10, "HumanMonster_Specific", 5, 74, 0f, 0f, 15f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 11, "HumanMonster_Specific", 5, 107, 0f, 1.5f, 35f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 12, "HumanMonster_Specific", 5, 109, 0f, 2.2f, 35f, 0f, 2f, 0.75f )
					, new MonsterArmedCustom( 19, "HumanMonster_Specific", 1, 73, 0f, 0f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 20, "HumanMonster_Specific", 1, 106, 0f, 1.5f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 21, "HumanMonster_Specific", 1, 108, 0f, 2.2f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 22, "HumanMonster_Specific", 1, 74, 0f, 0f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 23, "HumanMonster_Specific", 1, 107, 0f, 1.5f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 24, "HumanMonster_Specific", 1, 109, 0f, 2.2f, 40f, 0f, 5f, 0.315f )
					, new MonsterArmedCustom( 31, "HumanMonster_Specific", 2, 73, 0f, 0f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 32, "HumanMonster_Specific", 2, 106, 0f, 1.5f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 33, "HumanMonster_Specific", 2, 108, 0f, 2.2f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 34, "HumanMonster_Specific", 2, 74, 0f, 0f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 35, "HumanMonster_Specific", 2, 107, 0f, 1.5f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 36, "HumanMonster_Specific", 2, 109, 0f, 2.2f, 70f, 0f, 8f, 0.75f )
					, new MonsterArmedCustom( 43, "HumanMonster_Specific", 13, 73, 0f, 0f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 44, "HumanMonster_Specific", 13, 106, 0f, 1.5f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 45, "HumanMonster_Specific", 13, 108, 0f, 2.2f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 46, "HumanMonster_Specific", 13, 74, 0f, 0f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 47, "HumanMonster_Specific", 13, 107, 0f, 1.5f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 48, "HumanMonster_Specific", 13, 109, 0f, 2.2f, 30f, 0f, 3f, 0.315f )
					, new MonsterArmedCustom( 55, "JokerMonster_Specific", 4, 73, 0f, 0f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 56, "JokerMonster_Specific", 4, 106, 0f, 1.5f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 57, "JokerMonster_Specific", 4, 108, 0f, 2.2f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 58, "JokerMonster_Specific", 4, 74, 0f, 0f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 59, "JokerMonster_Specific", 4, 107, 0f, 1.5f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 60, "JokerMonster_Specific", 4, 109, 0f, 2.2f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 67, "JokerMonster_Specific", 42, 73, 0f, 0f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 68, "JokerMonster_Specific", 42, 106, 0f, 1.5f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 69, "JokerMonster_Specific", 42, 108, 0f, 2.2f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 70, "JokerMonster_Specific", 42, 74, 0f, 0f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 71, "JokerMonster_Specific", 42, 107, 0f, 1.5f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 72, "JokerMonster_Specific", 42, 109, 0f, 2.2f, 40f, 0f, 2f, 0.315f )
					, new MonsterArmedCustom( 73, "JokerMonster", 3, 73, 0f, 0f, 30f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 74, "JokerMonster", 3, 106, 0f, 1.5f, 35f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 75, "JokerMonster", 3, 108, 0f, 2.2f, 40f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 76, "JokerMonster", 3, 74, 0f, 0f, 30f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 77, "JokerMonster", 3, 107, 0f, 1.5f, 35f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 78, "JokerMonster", 3, 109, 0f, 2.2f, 40f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 79, "JokerMonster_Specific", 3, 73, 0f, 0f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 80, "JokerMonster_Specific", 3, 106, 0f, 1.5f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 81, "JokerMonster_Specific", 3, 108, 0f, 2.2f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 82, "JokerMonster_Specific", 3, 74, 0f, 0f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 83, "JokerMonster_Specific", 3, 107, 0f, 1.5f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 84, "JokerMonster_Specific", 3, 109, 0f, 2.2f, 70f, 0f, 8f, 0.25f )
					, new MonsterArmedCustom( 91, "JokerMonster_Specific", 41, 73, 0f, 0f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 92, "JokerMonster_Specific", 41, 106, 0f, 1.5f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 93, "JokerMonster_Specific", 41, 108, 0f, 2.2f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 94, "JokerMonster_Specific", 41, 74, 0f, 0f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 95, "JokerMonster_Specific", 41, 107, 0f, 1.5f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 96, "JokerMonster_Specific", 41, 109, 0f, 2.2f, 30f, 0f, 3f, 0.3f )
					, new MonsterArmedCustom( 97, "HumanMonster_AKM", 8, 73, 0f, 0f, 30f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 98, "HumanMonster_AKM", 8, 106, 0f, 1.5f, 35f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 99, "HumanMonster_AKM", 8, 108, 0f, 2.2f, 40f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 100, "HumanMonster_AKM", 8, 74, 0f, 0f, 30f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 101, "HumanMonster_AKM", 8, 107, 0f, 1.5f, 35f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 102, "HumanMonster_AKM", 8, 109, 0f, 2.2f, 40f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 103, "JokerMonster_M416", 12, 73, 0f, 0f, 30f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 104, "JokerMonster_M416", 12, 106, 0f, 1.5f, 35f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 105, "JokerMonster_M416", 12, 108, 0f, 2.2f, 40f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 106, "JokerMonster_M416", 12, 74, 0f, 0f, 30f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 107, "JokerMonster_M416", 12, 107, 0f, 1.5f, 35f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 108, "JokerMonster_M416", 12, 109, 0f, 2.2f, 40f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 109, "JokerMonster_MoveMent", 44, 73, 0f, 0f, 35f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 110, "JokerMonster_MoveMent", 44, 106, 0f, 1.5f, 40f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 111, "JokerMonster_MoveMent", 44, 108, 0f, 2.2f, 45f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 112, "JokerMonster_MoveMent", 44, 74, 0f, 0f, 35f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 113, "JokerMonster_MoveMent", 44, 107, 0f, 1.5f, 40f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 114, "JokerMonster_MoveMent", 44, 109, 0f, 2.2f, 45f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 115, "HumanMonster_RPG", 18, 73, 0f, 0f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 116, "HumanMonster_RPG", 18, 106, 0f, 1.5f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 117, "HumanMonster_RPG", 18, 108, 0f, 2.2f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 118, "HumanMonster_RPG", 18, 74, 0f, 0f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 119, "HumanMonster_RPG", 18, 107, 0f, 1.5f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 120, "HumanMonster_RPG", 18, 109, 0f, 2.2f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 121, "HumanMonster_Galting", 11, 73, 0f, 0f, 15f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 122, "HumanMonster_Galting", 11, 106, 0f, 1.5f, 20f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 123, "HumanMonster_Galting", 11, 108, 0f, 2.2f, 25f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 124, "HumanMonster_Galting", 11, 74, 0f, 0f, 15f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 125, "HumanMonster_Galting", 11, 107, 0f, 1.5f, 20f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 126, "HumanMonster_Galting", 11, 109, 0f, 2.2f, 25f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 127, "HumanMonster", 5, 121, 0f, 0f, 25f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 128, "HumanMonster", 5, 119, 0f, 1.5f, 30f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 129, "HumanMonster", 5, 120, 0f, 2.2f, 35f, 0f, 2f, 0.45f )
					, new MonsterArmedCustom( 145, "JokerMonster", 3, 121, 0f, 0f, 30f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 146, "JokerMonster", 3, 119, 0f, 1.5f, 35f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 147, "JokerMonster", 3, 120, 0f, 2.2f, 40f, 0f, 3f, 0.25f )
					, new MonsterArmedCustom( 151, "HumanMonster_AKM", 8, 121, 0f, 0f, 30f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 152, "HumanMonster_AKM", 8, 119, 0f, 1.5f, 35f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 153, "HumanMonster_AKM", 8, 120, 0f, 2.2f, 40f, 0f, 3.5f, 0.6f )
					, new MonsterArmedCustom( 154, "JokerMonster_M416", 12, 121, 0f, 0f, 30f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 155, "JokerMonster_M416", 12, 119, 0f, 1.5f, 35f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 156, "JokerMonster_M416", 12, 120, 0f, 2.2f, 40f, 0f, 3.5f, 0.36f )
					, new MonsterArmedCustom( 157, "JokerMonster_MoveMent", 44, 121, 0f, 0f, 35f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 158, "JokerMonster_MoveMent", 44, 119, 0f, 1.5f, 40f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 159, "JokerMonster_MoveMent", 44, 120, 0f, 2.2f, 45f, 0f, 10f, 0.48f )
					, new MonsterArmedCustom( 160, "HumanMonster_RPG", 18, 121, 0f, 0f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 161, "HumanMonster_RPG", 18, 119, 0f, 1.5f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 162, "HumanMonster_RPG", 18, 120, 0f, 2.2f, 100f, 0f, 10f, 1f )
					, new MonsterArmedCustom( 163, "HumanMonster_Galting", 11, 121, 0f, 0f, 15f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 164, "HumanMonster_Galting", 11, 119, 0f, 1.5f, 20f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 165, "HumanMonster_Galting", 11, 120, 0f, 2.2f, 25f, 0f, 8f, 1.2f )
					, new MonsterArmedCustom( 166, "JokerQueen", 64, 73, 0f, 0f, 50f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 167, "JokerQueen", 64, 106, 0f, 1.5f, 60f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 168, "JokerQueen", 64, 108, 0f, 2.2f, 70f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 169, "JokerQueen", 64, 74, 0f, 0f, 50f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 170, "JokerQueen", 64, 107, 0f, 1.5f, 60f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 171, "JokerQueen", 64, 109, 0f, 2.2f, 70f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 172, "JokerQueen", 64, 121, 0f, 0f, 50f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 173, "JokerQueen", 64, 119, 0f, 1.5f, 60f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 174, "JokerQueen", 64, 120, 0f, 2.2f, 70f, 100f, 6f, 1.2f )
					, new MonsterArmedCustom( 184, "MonsterCamp_HumanMonster_Elite_Sniper_M24", 64, 1006, 0f, 1f, 80f, 0f, 6f, 1f )
					, new MonsterArmedCustom( 185, "MonsterCamp_HumanMonster_Elite_ShieldMan", 66, 1006, 0f, 0f, 100f, 0f, 2f, 1f )
					, new MonsterArmedCustom( 186, "MonsterCamp_HumanMonster_Stalker", 68, 1006, 0f, 0f, 100f, 0f, 1.5f, 0.5f )
					, new MonsterArmedCustom( 187, "MonsterCamp_HumanMonster_Thug", 65, 1006, 0f, 0f, 100f, 0f, 1.5f, 10.5f )
					, new MonsterArmedCustom( 188, "MonsterCamp_HumanMonster_Elite_Sniper_M24", 64, 1005, 0f, 1f, 80f, 0f, 6f, 1f )
					, new MonsterArmedCustom( 189, "MonsterCamp_HumanMonster_Elite_ShieldMan", 66, 1005, 0f, 0f, 100f, 0f, 2f, 1f )
					, new MonsterArmedCustom( 190, "MonsterCamp_HumanMonster_Stalker", 68, 1005, 0f, 0f, 100f, 0f, 1.5f, 0.5f )
					, new MonsterArmedCustom( 191, "MonsterCamp_HumanMonster_Thug", 65, 1005, 0f, 0f, 100f, 0f, 1.5f, 10.5f )
					, new MonsterArmedCustom( 192, "MonsterCamp_HumanMonster_Elite_Sniper_M24", 64, 1004, 0f, 1f, 80f, 0f, 6f, 1f )
					, new MonsterArmedCustom( 193, "MonsterCamp_HumanMonster_Elite_ShieldMan", 66, 1004, 0f, 0f, 100f, 0f, 2f, 1f )
					, new MonsterArmedCustom( 194, "MonsterCamp_HumanMonster_Stalker", 68, 1004, 0f, 0f, 100f, 0f, 1.5f, 0.5f )
					, new MonsterArmedCustom( 195, "MonsterCamp_HumanMonster_Thug", 65, 1004, 0f, 0f, 100f, 0f, 1.5f, 10.5f )
					, new MonsterArmedCustom( 196, "MonsterCamp_HumanMonster_Elite_Sniper_M24", 64, 1003, 0f, 1f, 80f, 0f, 6f, 1f )
					, new MonsterArmedCustom( 197, "MonsterCamp_HumanMonster_Elite_ShieldMan", 66, 1003, 0f, 0f, 100f, 0f, 2f, 1f )
					, new MonsterArmedCustom( 198, "MonsterCamp_HumanMonster_Stalker", 68, 1003, 0f, 0f, 100f, 0f, 1.5f, 0.5f )
					, new MonsterArmedCustom( 199, "MonsterCamp_HumanMonster_Thug", 65, 1003, 0f, 0f, 100f, 0f, 1.5f, 10.5f )
					, new MonsterArmedCustom( 205, "HumanMonster", 5, 122, 0f, -0.3f, 80f, 0f, 10f, 0.6f )
					, new MonsterArmedCustom( 206, "HumanMonster_Galting", 11, 122, 0f, -0.75f, 10f, 0f, 3f, 3f )
					, new MonsterArmedCustom( 208, "HumanMonster_Tutorial", 5, 122, 0f, -0.3f, 80f, 0f, 10f, 0.6f )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 MonsterArmedCustom
		/// </summary>
		/// <param name="MonsterSign"></param>
		public static MonsterArmedCustom GetMonsterArmedCustomByMonsterSign( string monsterSign )
		{
			foreach (MonsterArmedCustom data in Data)
			{
				if ( data.MonsterSign == monsterSign )
				{
					return data;
				}
			}
			return default(MonsterArmedCustom);
		}

		/// <summary>
		/// 根据指定条件判断单个 MonsterArmedCustom 是否存在
		/// </summary>
		/// <param name="MonsterSign"></param>
		public static bool HasMonsterArmedCustomByMonsterSign( string monsterSign )
		{
			foreach (MonsterArmedCustom data in Data)
			{
				if ( data.MonsterSign == monsterSign )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 MonsterArmedCustom 列表
		/// </summary>
		/// <param name="WeaponItemId"></param>
		public static List<MonsterArmedCustom> GetMonsterArmedCustomsByWeaponItemId( int weaponItemId )
		{
			List<MonsterArmedCustom> result = new List<MonsterArmedCustom>();
			foreach (MonsterArmedCustom data in Data)
			{
				if ( data.WeaponItemId == weaponItemId)
				{
					result.Add(data);
				}
			}
			return result;
		}
		/// <summary>
		/// 根据指定条件获取 MonsterArmedCustom 列表
		/// </summary>
		/// <param name="MonsterSign"></param>
		/// <param name="MatchMode"></param>
		public static List<MonsterArmedCustom> GetMonsterArmedCustomsByMonsterSignMatchMode( string monsterSign, int matchMode )
		{
			List<MonsterArmedCustom> result = new List<MonsterArmedCustom>();
			foreach (MonsterArmedCustom data in Data)
			{
				if ( data.MonsterSign == monsterSign && data.MatchMode == matchMode)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
