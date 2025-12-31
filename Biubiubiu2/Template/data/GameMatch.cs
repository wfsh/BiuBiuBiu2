// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代模式匹配配置
    /// </summary>
	public struct GameMatch
	{
		/// <summary>
		/// 匹配 ID
		/// </summary>
		public readonly int Id { get; }
		/// <summary>
		/// 游戏模式
		/// </summary>
		public readonly int GameMode { get; }
		/// <summary>
		/// 性能平台展示名称
		/// </summary>
		public readonly string Desc { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// 地图(0代表随机地图)
		/// </summary>
		public readonly int MapId { get; }
		/// <summary>
		/// 客户端显示名称
		/// </summary>
		public readonly string ShowName { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gameMode"></param>
        /// <param name="desc"></param>
        /// <param name="sign"></param>
        /// <param name="mapId"></param>
        /// <param name="showName"></param> )
		public GameMatch( int id, int gameMode, string desc, string sign, int mapId, string showName )
		{
			Id = id;
			GameMode = gameMode;
			Desc = desc;
			Sign = sign;
			MapId = mapId;
			ShowName = showName;
		}
	}

    /// <summary>
    /// GameMatchSet that holds all the table data
    /// </summary>
    public partial class GameMatchSet
    {
        public static readonly GameMatch[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static GameMatchSet()
        {
            Data = new GameMatch[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 GameMatch
		/// </summary>
		/// <param name="Id"></param>
		public static GameMatch GetGameMatchById( int id )
		{
			foreach (GameMatch data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(GameMatch);
		}

		/// <summary>
		/// 根据指定条件判断单个 GameMatch 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasGameMatchById( int id )
		{
			foreach (GameMatch data in Data)
			{
				if ( data.Id == id )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取单个 GameMatch
		/// </summary>
		/// <param name="GameMode"></param>
		/// <param name="MapId"></param>
		public static GameMatch GetGameMatchByGameModeMapId( int gameMode, int mapId )
		{
			foreach (GameMatch data in Data)
			{
				if ( data.GameMode == gameMode && data.MapId == mapId )
				{
					return data;
				}
			}
			return default(GameMatch);
		}

		/// <summary>
		/// 根据指定条件判断单个 GameMatch 是否存在
		/// </summary>
		/// <param name="GameMode"></param>
		/// <param name="MapId"></param>
		public static bool HasGameMatchByGameModeMapId( int gameMode, int mapId )
		{
			foreach (GameMatch data in Data)
			{
				if ( data.GameMode == gameMode && data.MapId == mapId )
				{
					return true;
				}
			}
			return false;
		}
    }
}
