// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;


namespace Sofunny.BiuBiuBiu2.Template {
    /// <summary>
    /// 基础角色
    /// </summary>
	public struct GPOM_Character : IGPOM {
		/// <summary>
		/// 基础 AI 行为树名称
		/// </summary>
		public readonly string AiBehavior { get; }
		/// <summary>
		/// 二段跳高度
		/// </summary>
		public readonly float AirJumpHeight { get; }
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
		/// 跳跃高度
		/// </summary>
		public readonly float JumpHeight { get; }
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
        /// <param name="aiBehavior"></param>
        /// <param name="airJumpHeight"></param>
        /// <param name="assetSign"></param>
        /// <param name="gpoDropId"></param>
        /// <param name="gpoDropType"></param>
        /// <param name="gpoType"></param>
        /// <param name="hp"></param>
        /// <param name="id"></param>
        /// <param name="jumpHeight"></param>
        /// <param name="mapQuality"></param>
        /// <param name="matchMode"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="name"></param>
        /// <param name="portrait"></param>
        /// <param name="quality"></param>
        /// <param name="rotaSpeed"></param>
        /// <param name="sign"></param> )
		public GPOM_Character( string aiBehavior, float airJumpHeight, string assetSign, int[] gpoDropId, ushort gpoDropType, int gpoType, int hp, int id, float jumpHeight, byte mapQuality, int matchMode, float moveSpeed, string name, string portrait, byte quality, float rotaSpeed, string sign ) {
			AiBehavior = aiBehavior;
			AirJumpHeight = airJumpHeight;
			AssetSign = assetSign;
			GpoDropId = gpoDropId;
			GpoDropType = gpoDropType;
			GpoType = gpoType;
			Hp = hp;
			Id = id;
			JumpHeight = jumpHeight;
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
    /// GPOM_CharacterSet that holds all the table data
    /// </summary>
    public static class GPOM_CharacterSet {
        public static readonly GPOM_Character[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_Character = 10;
		public const int Id_JokerMonster = 101;
		public const int Id_JokerMonsterSpecific = 102;
		public const int Id_HumanMonsterAkm = 107;
		public const int Id_JokerMonsterM416 = 109;
		public const int Id_Hero1 = 11;
		public const int Id_HumanMonster = 110;
		public const int Id_HumanMonsterRpg = 112;
		public const int Id_HumanMonsterGalting = 113;
		public const int Id_JokerMonsterMoveMent = 114;
		public const int Id_HumanMonsterSpecific = 117;
		public const int Id_Hero2 = 12;
		public const int Id_Hero3 = 15;
		public const int Id_MonsterCampHumanMonsterEliteSniperM24 = 207;
		public const int Id_MonsterCampHumanMonsterEliteShieldMan = 208;
		public const int Id_MonsterCampHumanMonsterStalker = 209;
		public const int Id_MonsterCampHumanMonsterThug = 210;
		public const int Id_GoldDashRoleAicharacter = 211;
		public const int Id_HumanMonsterTutorial = 220;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const string Sign_Character = "Character";
		public const string Sign_GoldDashRoleAicharacter = "GoldDashRoleAICharacter";
		public const string Sign_Hero1 = "Hero1";
		public const string Sign_Hero2 = "Hero2";
		public const string Sign_Hero3 = "Hero3";
		public const string Sign_HumanMonster = "HumanMonster";
		public const string Sign_HumanMonsterAkm = "HumanMonster_AKM";
		public const string Sign_HumanMonsterGalting = "HumanMonster_Galting";
		public const string Sign_HumanMonsterRpg = "HumanMonster_RPG";
		public const string Sign_HumanMonsterSpecific = "HumanMonster_Specific";
		public const string Sign_HumanMonsterTutorial = "HumanMonster_Tutorial";
		public const string Sign_JokerMonster = "JokerMonster";
		public const string Sign_JokerMonsterM416 = "JokerMonster_M416";
		public const string Sign_JokerMonsterMoveMent = "JokerMonster_MoveMent";
		public const string Sign_JokerMonsterSpecific = "JokerMonster_Specific";
		public const string Sign_MonsterCampHumanMonsterEliteShieldMan = "MonsterCamp_HumanMonster_Elite_ShieldMan";
		public const string Sign_MonsterCampHumanMonsterEliteSniperM24 = "MonsterCamp_HumanMonster_Elite_Sniper_M24";
		public const string Sign_MonsterCampHumanMonsterStalker = "MonsterCamp_HumanMonster_Stalker";
		public const string Sign_MonsterCampHumanMonsterThug = "MonsterCamp_HumanMonster_Thug";

        /// <summary>
        /// 构造函数
        /// </summary>
        static GPOM_CharacterSet() {
            Data = new GPOM_Character[] {
					 new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "Character", new int[]{0}, 1, 9, 1000, 10, 1.5f, 1, 0, 7f, "基础鸭子", "0", 1, 50f, "Character" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100011}, 1, 9, 100, 101, 1.5f, 1, 0, 4f, "小丑帮员工", "Portrait_Monster_9", 1, 50f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 150, 101, 1.5f, 2, 106, 15f, "小丑帮员工", "Portrait_Monster_9", 1, 61f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 150, 101, 1.5f, 2, 107, 48f, "小丑帮员工", "Portrait_Monster_9", 1, 94f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 200, 101, 1.5f, 3, 108, 26f, "小丑帮员工", "Portrait_Monster_9", 1, 72f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 200, 101, 1.5f, 3, 109, 59f, "小丑帮员工", "Portrait_Monster_9", 1, 105f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 150, 101, 1.5f, 2, 119, 81f, "小丑帮员工", "Portrait_Monster_9", 1, 127f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100009}, 1, 9, 200, 101, 1.5f, 3, 120, 92f, "小丑帮员工", "Portrait_Monster_9", 1, 138f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100011}, 1, 9, 100, 101, 1.5f, 1, 121, 70f, "小丑帮员工", "Portrait_Monster_9", 1, 116f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal", new int[]{100011}, 1, 9, 100, 101, 1.5f, 1, 74, 37f, "小丑帮员工", "Portrait_Monster_9", 1, 83f, "JokerMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 100, 102, 1.5f, 1, 0, 5f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 51f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100013}, 1, 9, 150, 102, 1.5f, 2, 106, 16f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 62f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100013}, 1, 9, 150, 102, 1.5f, 2, 107, 49f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 95f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 200, 102, 1.5f, 3, 108, 27f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 73f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 200, 102, 1.5f, 3, 109, 60f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 106f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100013}, 1, 9, 150, 102, 1.5f, 2, 119, 82f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 128f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 200, 102, 1.5f, 3, 120, 93f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 139f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 120, 102, 1.5f, 1, 121, 71f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 117f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "JokerMonster_Normal_Specific", new int[]{100011}, 1, 9, 120, 102, 1.5f, 1, 74, 38f, "Jorker-P90变异怪", "Portrait_Monster_8", 1, 84f, "JokerMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 500, 107, 1.5f, 1, 0, 6f, "猛兽营先锋", "Portrait_Monster_6", 2, 52f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 750, 107, 1.5f, 2, 106, 17f, "猛兽营先锋", "Portrait_Monster_6", 2, 63f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 750, 107, 1.5f, 2, 107, 50f, "猛兽营先锋", "Portrait_Monster_6", 2, 96f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 1000, 107, 1.5f, 3, 108, 28f, "猛兽营先锋", "Portrait_Monster_6", 2, 74f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 1000, 107, 1.5f, 3, 109, 61f, "猛兽营先锋", "Portrait_Monster_6", 2, 107f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 750, 107, 1.5f, 2, 119, 83f, "猛兽营先锋", "Portrait_Monster_6", 2, 129f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 1000, 107, 1.5f, 3, 120, 94f, "猛兽营先锋", "Portrait_Monster_6", 2, 140f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 500, 107, 1.5f, 1, 121, 72f, "猛兽营先锋", "Portrait_Monster_6", 2, 118f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "HumanMonster_Advanced_GreenFish", new int[]{100017}, 1, 9, 500, 107, 1.5f, 1, 74, 39f, "猛兽营先锋", "Portrait_Monster_6", 2, 85f, "HumanMonster_AKM" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 500, 109, 1.5f, 1, 0, 7f, "小丑帮队长", "Portrait_Monster_7", 2, 53f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100023}, 1, 9, 750, 109, 1.5f, 2, 106, 18f, "小丑帮队长", "Portrait_Monster_7", 2, 64f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100023}, 1, 9, 750, 109, 1.5f, 2, 107, 51f, "小丑帮队长", "Portrait_Monster_7", 2, 97f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 1000, 109, 1.5f, 3, 108, 29f, "小丑帮队长", "Portrait_Monster_7", 2, 75f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 1000, 109, 1.5f, 3, 109, 62f, "小丑帮队长", "Portrait_Monster_7", 2, 108f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100023}, 1, 9, 750, 109, 1.5f, 2, 119, 84f, "小丑帮队长", "Portrait_Monster_7", 2, 130f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 1000, 109, 1.5f, 3, 120, 95f, "小丑帮队长", "Portrait_Monster_7", 2, 141f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 500, 109, 1.5f, 1, 121, 73f, "小丑帮队长", "Portrait_Monster_7", 2, 119f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Advanced", 1.0f, "JokerMonster_Advanced", new int[]{100019}, 1, 9, 500, 109, 1.5f, 1, 74, 40f, "小丑帮队长", "Portrait_Monster_7", 2, 86f, "JokerMonster_M416" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "Hero1", new int[]{0}, 1, 9, 2500, 11, 1.5f, 1, 0, 6f, "嘎嘎鸭", "0", 1, 50f, "Hero1" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 100, 110, 1.5f, 1, 0, 8f, "猛兽营喽啰", "Portrait_Monster_5", 1, 54f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 150, 110, 1.5f, 2, 106, 19f, "猛兽营喽啰", "Portrait_Monster_5", 1, 65f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 150, 110, 1.5f, 2, 107, 52f, "猛兽营喽啰", "Portrait_Monster_5", 1, 98f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100005}, 1, 9, 200, 110, 1.5f, 3, 108, 30f, "猛兽营喽啰", "Portrait_Monster_5", 1, 76f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100005}, 1, 9, 200, 110, 1.5f, 3, 109, 63f, "猛兽营喽啰", "Portrait_Monster_5", 1, 109f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 150, 110, 1.5f, 2, 119, 85f, "猛兽营喽啰", "Portrait_Monster_5", 1, 131f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100005}, 1, 9, 200, 110, 1.5f, 3, 120, 96f, "猛兽营喽啰", "Portrait_Monster_5", 1, 142f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 120, 110, 1.5f, 1, 121, 74f, "猛兽营喽啰", "Portrait_Monster_5", 1, 120f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{100001}, 1, 9, 120, 110, 1.5f, 1, 74, 41f, "猛兽营喽啰", "Portrait_Monster_5", 1, 87f, "HumanMonster" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 3000, 112, 1.5f, 1, 0, 9f, "金钱豹头领", "Portrait_Monster_1", 3, 55f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 4500, 112, 1.5f, 2, 106, 20f, "金钱豹头领", "Portrait_Monster_1", 3, 66f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 4500, 112, 1.5f, 2, 107, 53f, "金钱豹头领", "Portrait_Monster_1", 3, 99f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 6000, 112, 1.5f, 3, 108, 31f, "金钱豹头领", "Portrait_Monster_1", 3, 77f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 6000, 112, 1.5f, 3, 109, 64f, "金钱豹头领", "Portrait_Monster_1", 3, 110f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 4500, 112, 1.5f, 2, 119, 86f, "金钱豹头领", "Portrait_Monster_1", 3, 132f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 6000, 112, 1.5f, 3, 120, 97f, "金钱豹头领", "Portrait_Monster_1", 3, 143f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 3000, 112, 1.5f, 1, 121, 75f, "金钱豹头领", "Portrait_Monster_1", 3, 121f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_RPG_3", 1.0f, "HumanMonster_Elite_RPG", new int[]{100021}, 1, 9, 3000, 112, 1.5f, 1, 74, 42f, "金钱豹头领", "Portrait_Monster_1", 3, 88f, "HumanMonster_RPG" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 3000, 113, 1.5f, 1, 0, 10f, "野猪头领", "Portrait_Monster_10", 3, 56f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 4500, 113, 1.5f, 2, 106, 21f, "野猪头领", "Portrait_Monster_10", 3, 67f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 4500, 113, 1.5f, 2, 107, 54f, "野猪头领", "Portrait_Monster_10", 3, 100f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 6000, 113, 1.5f, 3, 108, 32f, "野猪头领", "Portrait_Monster_10", 3, 78f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 6000, 113, 1.5f, 3, 109, 65f, "野猪头领", "Portrait_Monster_10", 3, 111f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 4500, 113, 1.5f, 2, 119, 87f, "野猪头领", "Portrait_Monster_10", 3, 133f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 6000, 113, 1.5f, 3, 120, 98f, "野猪头领", "Portrait_Monster_10", 3, 144f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 3000, 113, 1.5f, 1, 121, 76f, "野猪头领", "Portrait_Monster_10", 3, 122f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI 2", 1.0f, "HumanMonster_Elite_Galting", new int[]{1093}, 1, 9, 900, 113, 1.5f, 1, 122, 10f, "野猪头领", "Portrait_Monster_10", 3, 56f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_Galting_3", 1.0f, "HumanMonster_Elite_Galting", new int[]{100022}, 1, 9, 3000, 113, 1.5f, 1, 74, 43f, "野猪头领", "Portrait_Monster_10", 3, 89f, "HumanMonster_Galting" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 2, 9, 3000, 114, 1.5f, 1, 0, 11f, "白帽子大师", "Portrait_Monster_4", 3, 57f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 4500, 114, 1.5f, 2, 106, 22f, "白帽子大师", "Portrait_Monster_4", 3, 68f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 4500, 114, 1.5f, 2, 107, 55f, "白帽子大师", "Portrait_Monster_4", 3, 101f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 6000, 114, 1.5f, 3, 108, 33f, "白帽子大师", "Portrait_Monster_4", 3, 79f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 6000, 114, 1.5f, 3, 109, 66f, "白帽子大师", "Portrait_Monster_4", 3, 112f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 4500, 114, 1.5f, 2, 119, 88f, "白帽子大师", "Portrait_Monster_4", 3, 134f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 6000, 114, 1.5f, 3, 120, 99f, "白帽子大师", "Portrait_Monster_4", 3, 145f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 3000, 114, 1.5f, 1, 121, 77f, "白帽子大师", "Portrait_Monster_4", 3, 123f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI_MoveMent_3", 1.0f, "JokerMonster_Elite_MoveMent", new int[]{100023}, 1, 9, 3000, 114, 1.5f, 1, 74, 44f, "白帽子大师", "Portrait_Monster_4", 3, 90f, "JokerMonster_MoveMent" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100001}, 1, 9, 100, 117, 1.5f, 1, 0, 12f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 58f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100003}, 1, 9, 150, 117, 1.5f, 2, 106, 23f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 69f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100003}, 1, 9, 150, 117, 1.5f, 2, 107, 56f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 102f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100005}, 1, 9, 200, 117, 1.5f, 3, 108, 34f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 80f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100005}, 1, 9, 200, 117, 1.5f, 3, 109, 67f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 113f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100003}, 1, 9, 150, 117, 1.5f, 2, 119, 89f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 135f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100005}, 1, 9, 200, 117, 1.5f, 3, 120, 100f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 146f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100001}, 1, 9, 120, 117, 1.5f, 1, 121, 78f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 124f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog_Specific", new int[]{100001}, 1, 9, 120, 117, 1.5f, 1, 74, 45f, "Human-TommyGun变异怪", "Portrait_Monster_5", 1, 91f, "HumanMonster_Specific" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "Hero2", new int[]{0}, 1, 9, 2000, 12, 1.5f, 1, 0, 6f, "爆烈鸟", "0", 1, 50f, "Hero2" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "Hero3", new int[]{0}, 1, 9, 2500, 15, 1.5f, 1, 0, 6f, "战斗鸡", "0", 1, 50f, "Hero3" )
					, new GPOM_Character( "AIBehavior_CharacterAI_beastcamp_Sniper", 1f, "HumanMonster_Elite_RPG", null, 0, 9, 100, 207, 1.5f, 1, 0, 4f, "暴打猛兽营狙击手", "", 1, 60f, "MonsterCamp_HumanMonster_Elite_Sniper_M24" )
					, new GPOM_Character( "AIBehavior_CharacterAI_beastcamp_ShieldMan", 1f, "HumanMonster_Elite_BeastCamp_ShieldMan", new int[]{0}, 0, 9, 300, 208, 1.5f, 1, 0, 4f, "暴打猛兽营盾牌兵", "", 1, 120f, "MonsterCamp_HumanMonster_Elite_ShieldMan" )
					, new GPOM_Character( "AIBehavior_CharacterAI_beastcamp_Stalker", 1f, "HumanMonster_Normal_Frog", new int[]{0}, 0, 9, 100, 209, 1.5f, 1, 0, 4f, "暴打猛兽营潜伏者", "", 1, 120f, "MonsterCamp_HumanMonster_Stalker" )
					, new GPOM_Character( "AIBehavior_CharacterAI_beastcamp_Thug", 1f, "HumanMonster_Normal_Frog_Specific", new int[]{0}, 0, 9, 100, 210, 1.5f, 1, 0, 4f, "暴打猛兽营暴徒", "", 1, 120f, "MonsterCamp_HumanMonster_Thug" )
					, new GPOM_Character( "", 0.0f, "GoldDashRoleAICharacter", null, 0, 9, 0, 211, 0.0f, 0, 0, 0.0f, "撤离-拟人AI映射", "", 0, 0.0f, "GoldDashRoleAICharacter" )
					, new GPOM_Character( "AIBehavior_CharacterAI", 1.0f, "HumanMonster_Normal_Frog", new int[]{1092}, 1, 9, 100, 220, 1.5f, 1, 122, 74f, "猛兽营喽啰", "Portrait_Monster_5", 1, 120f, "HumanMonster_Tutorial" )
            };
        }

		/// <summary>
		/// 根据指定条件获取单个 GPOM_Character
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="MatchMode"></param>
		public static GPOM_Character GetGPOMByIdAndMatchMode(int id, int matchMode = 0) {
			foreach (GPOM_Character data in Data) {
				if (data.Id == id && data.MatchMode == matchMode) {
					return data;
				}
			}
			return default(GPOM_Character);
		}
    }
}
