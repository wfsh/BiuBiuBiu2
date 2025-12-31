// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代GPO类型表
    /// </summary>
	public struct GpoType
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param> )
		public GpoType( int id, string name, string sign )
		{
			Id = id;
			Name = name;
			Sign = sign;
		}
	}

    /// <summary>
    /// GpoTypeSet that holds all the table data
    /// </summary>
    public partial class GpoTypeSet
    {
        public static readonly GpoType[] Data;
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
		public const int Id_Character = 9;
		public const int Id_GoldenEgg = 10;
		public const int Id_GiantDaDa = 11;
		public const int Id_AuroraDragon = 12;
		public const int Id_AceJoker = 13;
		public const int Id_JokerUav = 14;
		public const int Id_Gpospawner = 24;
		public const int Id_BlindingShield = 25;
		public const int Id_Sniper = 33;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GpoTypeSet()
        {
            Data = new GpoType[]
            {
					 new GpoType( 1, "坦克", "Tank" )
					, new GpoType( 2, "飞鱼", "FeiYu" )
					, new GpoType( 3, "乌龟", "WuGui" )
					, new GpoType( 4, "霸王龙", "RexKing" )
					, new GpoType( 5, "剑齿虎", "SwordTiger" )
					, new GpoType( 6, "直升机", "Helicopter" )
					, new GpoType( 7, "无人机", "Uav" )
					, new GpoType( 8, "重型防御机枪", "MachineGun" )
					, new GpoType( 9, "基础角色", "Character" )
					, new GpoType( 10, "夺金-蛋", "GoldenEgg" )
					, new GpoType( 11, "巨像达达", "GiantDaDa" )
					, new GpoType( 12, "极光呆呆龙王", "AuroraDragon" )
					, new GpoType( 13, "精英小丑", "AceJoker" )
					, new GpoType( 14, "小丑无人机", "JokerUAV" )
					, new GpoType( 24, "GPO生成器", "GPOSpawner" )
					, new GpoType( 25, "闪光盾", "BlindingShield" )
					, new GpoType( 33, "狙击手", "Sniper" )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GpoType
		/// </summary>
		/// <param name="Id"></param>
		public static GpoType GetGpoTypeById( int id )
		{
			foreach (GpoType data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GpoType);
		}

		/// <summary>
		/// 根据指定条件判断单个 GpoType 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGpoTypeById( int id )
		{
			foreach (GpoType data in Data)
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
