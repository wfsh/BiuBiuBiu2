// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代游戏模式配置
    /// </summary>
	public struct GameMode
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 总人数
		/// </summary>
		public readonly int MaxRoleNum { get; }
		/// <summary>
		/// 队伍人数
		/// </summary>
		public readonly int MaxRoleNumPerTeam { get; }
		/// <summary>
		/// 最小开始队伍数量
		/// </summary>
		public readonly int MinStartModeTeamNum { get; }
		/// <summary>
		/// 最小开启人数
		/// </summary>
		public readonly int MinStartPlayCharacterNum { get; }
		/// <summary>
		/// 达到最小开始队伍数量后开始倒计时
		/// </summary>
		public readonly float StartModeDownTime { get; }
		/// <summary>
		/// 每回合开始时候的倒计时
		/// </summary>
		public readonly float StartRoundDownTime { get; }
		/// <summary>
		/// 等待下回合开始的时间
		/// </summary>
		public readonly float WaitNextRoundTime { get; }
		/// <summary>
		/// 等待模式结算时间
		/// </summary>
		public readonly float WaitModeOverTime { get; }
		/// <summary>
		/// 每回合持续时间 -1 为无限时间 (秒)
		/// </summary>
		public readonly int RoundTime { get; }
		/// <summary>
		/// 获胜需要的分数
		/// </summary>
		public readonly int WinScore { get; }
		/// <summary>
		/// 模式胜利回合次数
		/// </summary>
		public readonly int ModeWinRoundCount { get; }
		/// <summary>
		/// 最大回合数
		/// </summary>
		public readonly int MaxRoundCount { get; }
		/// <summary>
		/// 模式名称
		/// </summary>
		public readonly string Name { get; }
		/// <summary>
		/// 描述信息
		/// </summary>
		public readonly string Desc { get; }
		/// <summary>
		/// 是否每回合随机武器
		/// </summary>
		public readonly bool PerRoundRandWeapon { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxRoleNum"></param>
        /// <param name="maxRoleNumPerTeam"></param>
        /// <param name="minStartModeTeamNum"></param>
        /// <param name="minStartPlayCharacterNum"></param>
        /// <param name="startModeDownTime"></param>
        /// <param name="startRoundDownTime"></param>
        /// <param name="waitNextRoundTime"></param>
        /// <param name="waitModeOverTime"></param>
        /// <param name="roundTime"></param>
        /// <param name="winScore"></param>
        /// <param name="modeWinRoundCount"></param>
        /// <param name="maxRoundCount"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="perRoundRandWeapon"></param>
        /// <param name="sign"></param> )
		public GameMode( int id, int maxRoleNum, int maxRoleNumPerTeam, int minStartModeTeamNum, int minStartPlayCharacterNum, float startModeDownTime, float startRoundDownTime, float waitNextRoundTime, float waitModeOverTime, int roundTime, int winScore, int modeWinRoundCount, int maxRoundCount, string name, string desc, bool perRoundRandWeapon, string sign )
		{
			Id = id;
			MaxRoleNum = maxRoleNum;
			MaxRoleNumPerTeam = maxRoleNumPerTeam;
			MinStartModeTeamNum = minStartModeTeamNum;
			MinStartPlayCharacterNum = minStartPlayCharacterNum;
			StartModeDownTime = startModeDownTime;
			StartRoundDownTime = startRoundDownTime;
			WaitNextRoundTime = waitNextRoundTime;
			WaitModeOverTime = waitModeOverTime;
			RoundTime = roundTime;
			WinScore = winScore;
			ModeWinRoundCount = modeWinRoundCount;
			MaxRoundCount = maxRoundCount;
			Name = name;
			Desc = desc;
			PerRoundRandWeapon = perRoundRandWeapon;
			Sign = sign;
		}
	}

    /// <summary>
    /// GameModeSet that holds all the table data
    /// </summary>
    public partial class GameModeSet
    {
        public static readonly GameMode[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GameModeSet()
        {
            Data = new GameMode[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GameMode
		/// </summary>
		/// <param name="Id"></param>
		public static GameMode GetGameModeById( int id )
		{
			foreach (GameMode data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GameMode);
		}

		/// <summary>
		/// 根据指定条件判断单个 GameMode 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGameModeById( int id )
		{
			foreach (GameMode data in Data)
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
