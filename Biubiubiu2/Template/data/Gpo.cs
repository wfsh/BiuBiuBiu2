// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代GPO表
    /// </summary>
	public struct Gpo
	{
		/// <summary>
		/// ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// GPO类型
		/// </summary>
		public readonly int GpoType { get; }
		/// <summary>
		/// 
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 
		/// </summary>
		public readonly string AssetSign { get; }
		/// <summary>
		/// 
		/// </summary>
		public readonly string Desc { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gpoType"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="assetSign"></param>
        /// <param name="desc"></param> )
		public Gpo( int id, int gpoType, string name, string sign, string assetSign, string desc )
		{
			Id = id;
			GpoType = gpoType;
			Name = name;
			Sign = sign;
			AssetSign = assetSign;
			Desc = desc;
		}
	}

    /// <summary>
    /// GpoSet that holds all the table data
    /// </summary>
    public partial class GpoSet
    {
        public static readonly Gpo[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const int Id_Tank = 1;
		public const int Id_FeiYu = 2;
		public const int Id_WuGui = 3;
		public const int Id_RexKing = 4;
		public const int Id_SwordTiger = 5;
		public const int Id_Helicopter = 6;
		public const int Id_Uav = 7;
		public const int Id_MachineGun = 8;
		public const int Id_Character = 10;
		public const int Id_Hero1 = 11;
		public const int Id_Hero2 = 12;
		public const int Id_GoldenEgg = 13;
		public const int Id_GoldenBigEgg = 14;
		public const int Id_Hero3 = 15;
		public const int Id_GiantDaDa = 100;
		public const int Id_JokerMonster = 101;
		public const int Id_JokerMonsterSpecific = 102;
		public const int Id_HumanMonsterAkm = 107;
		public const int Id_JokerMonsterM416 = 109;
		public const int Id_HumanMonster = 110;
		public const int Id_HumanMonsterRpg = 112;
		public const int Id_HumanMonsterGalting = 113;
		public const int Id_JokerMonsterMoveMent = 114;
		public const int Id_HumanMonsterSpecific = 117;
		public const int Id_JokerQueen = 169;
		public const int Id_AuroraDragon = 176;
		public const int Id_AncientDragon = 177;
		public const int Id_GoldJoker = 178;
		public const int Id_AceJoker = 179;
		public const int Id_GoldDashJokerDroneSpades = 180;
		public const int Id_GoldDashJokerDroneHearts = 181;
		public const int Id_GoldDashJokerDroneClubs = 182;
		public const int Id_GoldDashJokerDroneDiamonds = 183;
		public const int Id_GpospawnerDefense = 200;
		public const int Id_GpospawnerClearGpo = 201;
		public const int Id_GpospawnerGoldRush = 202;
		public const int Id_GoldDashBlindingShield = 204;
		public const int Id_MonsterCampHumanMonsterEliteSniperM24 = 207;
		public const int Id_MonsterCampHumanMonsterEliteShieldMan = 208;
		public const int Id_MonsterCampHumanMonsterStalker = 209;
		public const int Id_MonsterCampHumanMonsterThug = 210;
		public const int Id_GoldDashRoleAicharacter = 211;
		public const int Id_HumanMonsterTutorial = 220;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoSet()
        {
            Data = new Gpo[]
            {
					 new Gpo( 1, 1, "坦克", "Tank", "Tank", "" )
					, new Gpo( 2, 2, "飞鱼", "FeiYu", "FeiYu", "" )
					, new Gpo( 3, 3, "乌龟", "WuGui", "WuGui", "" )
					, new Gpo( 4, 4, "霸王龙", "RexKing", "RexKing", "" )
					, new Gpo( 5, 5, "剑齿虎", "SwordTiger", "SwordTiger", "" )
					, new Gpo( 6, 6, "直升机", "Helicopter", "Helicopter", "" )
					, new Gpo( 7, 7, "无人机", "Uav", "Uav", "" )
					, new Gpo( 8, 8, "重型防御机枪", "MachineGun", "MachineGun", "" )
					, new Gpo( 10, 9, "基础鸭子", "Character", "Character", "" )
					, new Gpo( 11, 9, "嘎嘎鸭", "Hero1", "Hero1", "本是农场普通鸭子，偶然捡到神奇锅盖后自封”天选之鸭“，戴着锅盖勇闯战场。它战力平平，但总以滑稽动作化险为夷，成为团队开心果。" )
					, new Gpo( 12, 9, "爆烈鸟", "Hero2", "Hero2", "“天才爆破专家”，痴迷爆炸实验。一次意外炸飞实验室，却因防火羽毛幸存。如今带着自制燃烧弹参战，把每场战斗当作实验。" )
					, new Gpo( 13, 10, "夺金-蛋", "GoldenEgg", "GoldenEgg", "" )
					, new Gpo( 14, 10, "夺金-巨蛋", "GoldenBigEgg", "GoldenBigEgg", "" )
					, new Gpo( 15, 9, "战斗鸡", "Hero3", "Hero3", "古老“斗鸡门”的最后传鸡。毕生追求拳法的至高境界，出山只为寻找能与它痛快切磋、打出完美对攻的对手！" )
					, new Gpo( 100, 11, "巨像达达", "GiantDaDa", "GiantDaDa", "" )
					, new Gpo( 101, 9, "小丑帮员工", "JokerMonster", "JokerMonster_Normal", "" )
					, new Gpo( 102, 9, "Jorker-P90变异怪", "JokerMonster_Specific", "JokerMonster_Normal_Specific", "" )
					, new Gpo( 107, 9, "猛兽营先锋", "HumanMonster_AKM", "HumanMonster_Advanced_GreenFish", "" )
					, new Gpo( 109, 9, "小丑帮队长", "JokerMonster_M416", "JokerMonster_Advanced", "" )
					, new Gpo( 110, 9, "猛兽营喽啰", "HumanMonster", "HumanMonster_Normal_Frog", "" )
					, new Gpo( 112, 9, "金钱豹头领", "HumanMonster_RPG", "HumanMonster_Elite_RPG", "" )
					, new Gpo( 113, 9, "野猪头领", "HumanMonster_Galting", "HumanMonster_Elite_Galting", "" )
					, new Gpo( 114, 9, "白帽子大师", "JokerMonster_MoveMent", "JokerMonster_Elite_MoveMent", "" )
					, new Gpo( 117, 9, "Human-TommyGun变异怪", "HumanMonster_Specific", "HumanMonster_Normal_Frog_Specific", "" )
					, new Gpo( 169, 33, "双马尾大师", "JokerQueen", "JokerMonster_M24Master", "" )
					, new Gpo( 176, 12, "极光呆呆龙王", "AuroraDragon", "AuroraDragon", "" )
					, new Gpo( 177, 12, "远古呆呆龙王", "AncientDragon", "AncientDragon", "" )
					, new Gpo( 178, 13, "至尊小丑", "GoldJoker", "GoldJoker", "" )
					, new Gpo( 179, 13, "王牌小丑", "AceJoker", "AceJoker", "" )
					, new Gpo( 180, 14, "小丑无人机-黑桃", "GoldDash_JokerDrone_Spades", "GoldDash_JokerDrone_Spades", "" )
					, new Gpo( 181, 14, "小丑无人机-红心", "GoldDash_JokerDrone_Hearts", "GoldDash_JokerDrone_Hearts", "" )
					, new Gpo( 182, 14, "小丑无人机-梅花", "GoldDash_JokerDrone_Clubs", "GoldDash_JokerDrone_Clubs", "" )
					, new Gpo( 183, 14, "小丑无人机-方片", "GoldDash_JokerDrone_Diamonds", "GoldDash_JokerDrone_Diamonds", "" )
					, new Gpo( 200, 24, "GPO 生成器 - 防守", "GPOSpawnerDefense", "GPOSpawnerDefense", "" )
					, new Gpo( 201, 24, "GPO 生成器 - 清怪", "GPOSpawnerClearGPO", "GPOSpawnerClearGPO", "" )
					, new Gpo( 202, 24, "GPO 生成器 - 夺金", "GPOSpawnerGoldRush", "", "" )
					, new Gpo( 204, 25, "闪光盾", "GoldDash_BlindingShield", "GoldDash_BlindingShield", "" )
					, new Gpo( 207, 9, "暴打猛兽营狙击手", "MonsterCamp_HumanMonster_Elite_Sniper_M24", "HumanMonster_Elite_RPG", "" )
					, new Gpo( 208, 9, "暴打猛兽营盾牌兵", "MonsterCamp_HumanMonster_Elite_ShieldMan", "HumanMonster_Elite_BeastCamp_ShieldMan", "" )
					, new Gpo( 209, 9, "暴打猛兽营潜伏者", "MonsterCamp_HumanMonster_Stalker", "HumanMonster_Normal_Frog", "" )
					, new Gpo( 210, 9, "暴打猛兽营暴徒", "MonsterCamp_HumanMonster_Thug", "HumanMonster_Normal_Frog_Specific", "" )
					, new Gpo( 211, 9, "撤离-拟人AI映射", "GoldDashRoleAICharacter", "GoldDashRoleAICharacter", "" )
					, new Gpo( 220, 9, "猛兽营喽啰", "HumanMonster_Tutorial", "HumanMonster_Normal_Frog", "" )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 Gpo
		/// </summary>
		/// <param name="Id"></param>
		public static Gpo GetGpoById( int id )
		{
			foreach (Gpo data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(Gpo);
		}

		/// <summary>
		/// 根据指定条件判断单个 Gpo 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoById( int id )
		{
			foreach (Gpo data in Data)
			{
				if ( data.Id == id )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取单个 Gpo
		/// </summary>
		/// <param name="Sign"></param>
		public static Gpo GetGpoBySign( string sign )
		{
			foreach (Gpo data in Data)
			{
				if ( data.Sign == sign )
				{
					return data;
				}
			}
			return default(Gpo);
		}

		/// <summary>
		/// 根据指定条件判断单个 Gpo 是否存在
		/// </summary>
		/// <param name="Sign"></param>
		public static bool HasGpoBySign( string sign )
		{
			foreach (Gpo data in Data)
			{
				if ( data.Sign == sign )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取 Gpo 列表
		/// </summary>
		/// <param name="GpoType"></param>
		public static List<Gpo> GetGposByGpoType( int gpoType )
		{
			List<Gpo> result = new List<Gpo>();
			foreach (Gpo data in Data)
			{
				if ( data.GpoType == gpoType)
				{
					result.Add(data);
				}
			}
			return result;
		}
    }
}
