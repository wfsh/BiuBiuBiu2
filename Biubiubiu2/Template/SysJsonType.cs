// Generated automatically by csv-gen: do not edit manually
using System;
using Newtonsoft.Json;

namespace Sofunny.BiuBiuBiu2.Template
{
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 扶人
    /// 成功扶触发
    /// </summary>
	public struct PickUp
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
		public PickUp( int mapmod, int groupmod, int gamemod, int showmod, int matchmod )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 舔包
    /// 捡起玩家尸体或者空投
    /// </summary>
	public struct GetItemFromBox
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 舔包
		/// 数据源 => 自定义值映射 0：个人盒子 1：空投盒子 2：怪物宝箱 3：野外啵啵
		/// </summary>
		public long Boxt { get; set; }
		/// <summary>
		/// 空投类型
		/// 数据源 => 自定义值映射 0：不限制 1：系统空投 2：超级空投
		/// </summary>
		public long AirDropBox { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="boxt"></param>
        /// <param name="airDropBox"></param>
		public GetItemFromBox( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long boxt, long airDropBox )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			Boxt = boxt;
			AirDropBox = airDropBox;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 伤害量
    /// 对敌方造成伤害
    /// </summary>
	public struct Hurt
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 伤害
		/// 数据源 => 自定义值映射 0：0
		/// </summary>
		public long H { get; set; }
		/// <summary>
		/// 爆头伤害
		/// 数据源 => 自定义值映射 0：0
		/// </summary>
		public long Hbh { get; set; }
		/// <summary>
		/// 伤害类型
		/// 数据源 => 自定义值映射 0：总伤害 1：普通伤害 2：爆头伤害
		/// </summary>
		public long Ht { get; set; }
		/// <summary>
		/// 武器类型
		/// 数据源 => 外键表：data/item_type.csv 外键列：id
		/// </summary>
		public long WeaponType { get; set; }
		/// <summary>
		/// 武器ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Weapon { get; set; }
		/// <summary>
		/// 子弹ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Bullet { get; set; }
		/// <summary>
		/// 是否战队好友
		/// 数据源 => 自定义值映射 0：不需要战队好友 1：需要战队好友
		/// </summary>
		public sbyte IsClub { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="h"></param>
        /// <param name="hbh"></param>
        /// <param name="ht"></param>
        /// <param name="weaponType"></param>
        /// <param name="weapon"></param>
        /// <param name="bullet"></param>
        /// <param name="isClub"></param>
		public Hurt( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long h, long hbh, long ht, long weaponType, long weapon, long bullet, sbyte isClub )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			H = h;
			Hbh = hbh;
			Ht = ht;
			WeaponType = weaponType;
			Weapon = weapon;
			Bullet = bullet;
			IsClub = isClub;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 战队动态
    /// 战队动态
    /// </summary>
	public struct ClubNews
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 存活时长
		/// 数据源 => 无
		/// </summary>
		public long At { get; set; }
		/// <summary>
		/// 排名
		/// 数据源 => 无
		/// </summary>
		public long R { get; set; }
		/// <summary>
		/// 是否好友
		/// 数据源 => 自定义值映射 0：不需要好友 1：需要好友
		/// </summary>
		public sbyte Isfriend { get; set; }
		/// <summary>
		/// 套装
		/// 数据源 => 外键表：data/pay_crate_topic.csv 外键列：id
		/// </summary>
		public long Suit { get; set; }
		/// <summary>
		/// 总伤害
		/// 数据源 => 无
		/// </summary>
		public long TotalHurt { get; set; }
		/// <summary>
		/// 队伍情况
		/// 数据源 => 自定义值映射 0：无 1：全队都活着 2：只有我活着
		/// </summary>
		public long GroupInfo { get; set; }
		/// <summary>
		/// 战场装备
		/// 数据源 => 自定义值映射 0：正常 1：无战场装备
		/// </summary>
		public long WarEquip { get; set; }
		/// <summary>
		/// 评分
		/// 数据源 => 无
		/// </summary>
		public long Grade { get; set; }
		/// <summary>
		/// 不受每日时间限制
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public long Ndtl { get; set; }
		/// <summary>
		/// 是否进入Y星飞碟
		/// 数据源 => 自定义值映射 0：未进入Y星飞碟 1：是进入Y星飞碟
		/// </summary>
		public sbyte IsGetIntoYscout { get; set; }
		/// <summary>
		/// 击杀数
		/// 数据源 => 无
		/// </summary>
		public long KillNums { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="coord"></param>
        /// <param name="at"></param>
        /// <param name="r"></param>
        /// <param name="isfriend"></param>
        /// <param name="suit"></param>
        /// <param name="totalHurt"></param>
        /// <param name="groupInfo"></param>
        /// <param name="warEquip"></param>
        /// <param name="grade"></param>
        /// <param name="ndtl"></param>
        /// <param name="isGetIntoYscout"></param>
        /// <param name="killNums"></param>
		public ClubNews( int mapmod, int groupmod, int gamemod, int showmod, long coord, long at, long r, sbyte isfriend, long suit, long totalHurt, long groupInfo, long warEquip, long grade, long ndtl, sbyte isGetIntoYscout, long killNums )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Coord = coord;
			At = at;
			R = r;
			Isfriend = isfriend;
			Suit = suit;
			TotalHurt = totalHurt;
			GroupInfo = groupInfo;
			WarEquip = warEquip;
			Grade = grade;
			Ndtl = ndtl;
			IsGetIntoYscout = isGetIntoYscout;
			KillNums = killNums;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 登录
    /// 登录
    /// </summary>
	public struct Login
	{
		/// <summary>
		/// 登录具体时间段
		/// 数据源 => 自定义值映射 0：不限 1：12:00～14:00 2：20:00～23:00
		/// </summary>
		public long Time { get; set; }
		/// <summary>
		/// 周登录天
		/// 数据源 => 自定义值映射 0：不限 1：周一 2：周二 3：周三 4：周四 5：周五 6：周六 7：周日
		/// </summary>
		public long WeekDay { get; set; }
		/// <summary>
		/// 平台
		/// 数据源 => 系统字典类型 platform_type
		/// </summary>
		public long Platform { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="weekDay"></param>
        /// <param name="platform"></param>
		public Login( long time, long weekDay, long platform )
		{
			Time = time;
			WeekDay = weekDay;
			Platform = platform;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 进度
    /// 进度
    /// </summary>
	public struct Progress
	{
		/// <summary>
		/// 进度类型
		/// 数据源 => 自定义值映射 0：无 1：解锁型活动进度 2：龙蛋领奖进度
		/// </summary>
		public sbyte Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
		public Progress( sbyte type )
		{
			Type = type;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 点赞
    /// 点赞
    /// </summary>
	public struct GiveLike
	{
		/// <summary>
		/// 点赞类型
		/// 数据源 => 自定义值映射 0：不限 1：结算界面点赞 2：战场内点赞 3：被赞
		/// </summary>
		public sbyte Type { get; set; }
		/// <summary>
		/// 是否战队成员
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsClub { get; set; }
		/// <summary>
		/// 是否召回好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsRecallFriend { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isClub"></param>
        /// <param name="isRecallFriend"></param>
		public GiveLike( sbyte type, sbyte isClub, sbyte isRecallFriend )
		{
			Type = type;
			IsClub = isClub;
			IsRecallFriend = isRecallFriend;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 分享
    /// 分享
    /// </summary>
	public struct Share
	{
		/// <summary>
		/// 分享方式
		/// 数据源 => 自定义值映射 0：不限 1：个人空间或结算界面 2：成功分享给玩家 3：砍一刀分享
		/// </summary>
		public long ShareType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shareType"></param>
		public Share( long shareType )
		{
			ShareType = shareType;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 关注
    /// 关注
    /// </summary>
	public struct Focus
	{
		/// <summary>
		/// 是否互相关注
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsMutual { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isMutual"></param>
		public Focus( sbyte isMutual )
		{
			IsMutual = isMutual;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 开启补给箱
    /// 开启补给箱
    /// </summary>
	public struct UseCrate
	{
		/// <summary>
		/// 补给箱类型
		/// 数据源 => 自定义值映射 0：无 1：至尊补给箱 2：甜甜圈宝箱 3：广告宝箱
		/// </summary>
		public sbyte Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
		public UseCrate( sbyte type )
		{
			Type = type;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 亲密度
    /// 亲密度
    /// </summary>
	public struct Intimacy
	{
		/// <summary>
		/// 任务类型
		/// 数据源 => 自定义值映射 0：无 1：获得亲密榜称号并且有送花行为 2：与指定好友提升亲密度
		/// </summary>
		public sbyte Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
		public Intimacy( sbyte type )
		{
			Type = type;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 收集
    /// 收集
    /// </summary>
	public struct Collect
	{
		/// <summary>
		/// 收集的物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long ItemId { get; set; }
		/// <summary>
		/// 道具分类索引
		/// 数据源 => 系统字典类型 Item_class_index
		/// </summary>
		public long ItemClassIndex { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemClassIndex"></param>
		public Collect( long itemId, long itemClassIndex )
		{
			ItemId = itemId;
			ItemClassIndex = itemClassIndex;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 完成赛季任务
    /// 完成赛季任务
    /// </summary>
	public struct CompleteSeasonTask
	{
		/// <summary>
		/// 是否周任务
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsWeekly { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isWeekly"></param>
		public CompleteSeasonTask( sbyte isWeekly )
		{
			IsWeekly = isWeekly;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// pve等级提升
    /// pve等级提升
    /// </summary>
	public struct PveLevelUp
	{
		/// <summary>
		/// pve等级
		/// 数据源 => 无
		/// </summary>
		public int Level { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level"></param>
		public PveLevelUp( int level )
		{
			Level = level;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// pve淘汰
    /// pve淘汰
    /// </summary>
	public struct PveWeedOut
	{
		/// <summary>
		/// 击杀距离
		/// 数据源 => 无
		/// </summary>
		public int Wd { get; set; }
		/// <summary>
		/// 敌人
		/// 数据源 => 系统字典类型 pve_enemy
		/// </summary>
		public int Enemy { get; set; }
		/// <summary>
		/// 武器
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Weapon { get; set; }
		/// <summary>
		/// 姿势
		/// 数据源 => 系统字典类型 pte_type
		/// </summary>
		public int Pte { get; set; }
		/// <summary>
		/// 难度
		/// 数据源 => 系统字典类型 pve_gameplay_level
		/// </summary>
		public int Difficulty { get; set; }
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 是否爆头
		/// 数据源 => 自定义值映射 0：不是 1：是
		/// </summary>
		public int Kbh { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="enemy"></param>
        /// <param name="weapon"></param>
        /// <param name="pte"></param>
        /// <param name="difficulty"></param>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="kbh"></param>
		public PveWeedOut( int wd, int enemy, long weapon, int pte, int difficulty, int mapmod, int groupmod, int gamemod, int showmod, int matchmod, int kbh )
		{
			Wd = wd;
			Enemy = enemy;
			Weapon = weapon;
			Pte = pte;
			Difficulty = difficulty;
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Kbh = kbh;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// pve通关
    /// pve通关
    /// </summary>
	public struct PvePass
	{
		/// <summary>
		/// 难度
		/// 数据源 => 系统字典类型 pve_gameplay_level
		/// </summary>
		public int Difficulty { get; set; }
		/// <summary>
		/// 损失体力值
		/// 数据源 => 无
		/// </summary>
		public int LossHp { get; set; }
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="lossHp"></param>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
		public PvePass( int difficulty, int lossHp, int mapmod, int groupmod, int gamemod, int showmod, int matchmod )
		{
			Difficulty = difficulty;
			LossHp = lossHp;
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// pve结算
    /// pve结算
    /// </summary>
	public struct PveOver
	{
		/// <summary>
		/// 体力值
		/// 数据源 => 无
		/// </summary>
		public int Hp { get; set; }
		/// <summary>
		/// 护盾
		/// 数据源 => 无
		/// </summary>
		public int Shield { get; set; }
		/// <summary>
		/// 速度
		/// 数据源 => 无
		/// </summary>
		public int Speed { get; set; }
		/// <summary>
		/// 伤害（单位K）
		/// 数据源 => 无
		/// </summary>
		public long Hurt { get; set; }
		/// <summary>
		/// 是否mvp
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public int Mvp { get; set; }
		/// <summary>
		/// 淘汰数
		/// 数据源 => 无
		/// </summary>
		public int WeedOut { get; set; }
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 是否击退boss
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public int IsBoss { get; set; }
		/// <summary>
		/// 难度
		/// 数据源 => 系统字典类型 pve_gameplay_level
		/// </summary>
		public int Difficulty { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="shield"></param>
        /// <param name="speed"></param>
        /// <param name="hurt"></param>
        /// <param name="mvp"></param>
        /// <param name="weedOut"></param>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="isBoss"></param>
        /// <param name="difficulty"></param>
		public PveOver( int hp, int shield, int speed, long hurt, int mvp, int weedOut, int mapmod, int groupmod, int gamemod, int showmod, int matchmod, int isBoss, int difficulty )
		{
			Hp = hp;
			Shield = shield;
			Speed = speed;
			Hurt = hurt;
			Mvp = mvp;
			WeedOut = weedOut;
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			IsBoss = isBoss;
			Difficulty = difficulty;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 赠礼
    /// 赠礼
    /// </summary>
	public struct Gift
	{
		/// <summary>
		/// 道具
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long ItemId { get; set; }
		/// <summary>
		/// 类型
		/// 数据源 => 自定义值映射 0：次数 1：数量
		/// </summary>
		public sbyte Type { get; set; }
		/// <summary>
		/// 是否回归好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsRecallFriend { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="type"></param>
        /// <param name="isRecallFriend"></param>
		public Gift( long itemId, sbyte type, sbyte isRecallFriend )
		{
			ItemId = itemId;
			Type = type;
			IsRecallFriend = isRecallFriend;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 玩家绑定
    /// 玩家绑定
    /// </summary>
	public struct PlayerBind
	{
		/// <summary>
		/// 绑定类型
		/// 数据源 => 自定义值映射 0：主动绑定 1：被绑定 2：相互绑定
		/// </summary>
		public sbyte BindType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bindType"></param>
		public PlayerBind( sbyte bindType )
		{
			BindType = bindType;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 使用技能
    /// 使用技能
    /// </summary>
	public struct UseSkill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 技能
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Skill { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="skill"></param>
        /// <param name="matchmod"></param>
		public UseSkill( int mapmod, int groupmod, int gamemod, int showmod, long skill, int matchmod )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Skill = skill;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 刀刃球回合结束
    /// 刀刃球回合结束
    /// </summary>
	public struct BoutOver
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 存活时长
		/// 数据源 => 无
		/// </summary>
		public long At { get; set; }
		/// <summary>
		/// 排名
		/// 数据源 => 无
		/// </summary>
		public long R { get; set; }
		/// <summary>
		/// 是否好友
		/// 数据源 => 自定义值映射 0：不需要好友 1：需要好友
		/// </summary>
		public sbyte Isfriend { get; set; }
		/// <summary>
		/// 是否胜利
		/// 数据源 => 自定义值映射 0：不是胜利 1：是胜利
		/// </summary>
		public int Win { get; set; }
		/// <summary>
		/// 连击数
		/// 数据源 => 无
		/// </summary>
		public int Combo { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="at"></param>
        /// <param name="r"></param>
        /// <param name="isfriend"></param>
        /// <param name="win"></param>
        /// <param name="combo"></param>
        /// <param name="matchmod"></param>
		public BoutOver( int mapmod, int groupmod, int gamemod, int showmod, long at, long r, sbyte isfriend, int win, int combo, int matchmod )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			At = at;
			R = r;
			Isfriend = isfriend;
			Win = win;
			Combo = combo;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 召回累登
    /// 召回累登
    /// </summary>
	public struct RecallLogin
	{
		/// <summary>
		/// 召回经验值(&gt=)
		/// 数据源 => 无
		/// </summary>
		public long RecallExp { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="recallExp"></param>
		public RecallLogin( long recallExp )
		{
			RecallExp = recallExp;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 商城转盘抽取
    /// 商城转盘抽取
    /// </summary>
	public struct StoreTurntableLottery
	{
		/// <summary>
		/// 活动id
		/// 数据源 => 外键表：data/traffic_permit_activity.csv 外键列：id
		/// </summary>
		public long ActivityId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="activityId"></param>
		public StoreTurntableLottery( long activityId )
		{
			ActivityId = activityId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// OnlyUp关卡通关成就
    /// OnlyUp关卡通关成就
    /// </summary>
	public struct OnlyUpLevel
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchmod"></param>
		public OnlyUpLevel( int matchmod )
		{
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 消费
    /// 消费
    /// </summary>
	public struct Consume
	{
		/// <summary>
		/// 提前记录天数
		/// 数据源 => 无
		/// </summary>
		public int AdvanceRecordDay { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="advanceRecordDay"></param>
		public Consume( int advanceRecordDay )
		{
			AdvanceRecordDay = advanceRecordDay;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// OnlyUp战场事件
    /// OnlyUp战场事件
    /// </summary>
	public struct OnlyUpUnet
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 事件类型
		/// 数据源 => 自定义值映射 0：小关通关 1：大关通关 2：开启宝箱 3：移动距离
		/// </summary>
		public sbyte EventType { get; set; }
		/// <summary>
		/// 关卡
		/// 数据源 => 无
		/// </summary>
		public int Level { get; set; }
		/// <summary>
		/// 主关卡
		/// 数据源 => 无
		/// </summary>
		public int RootLevel { get; set; }
		/// <summary>
		/// 宝箱ID
		/// 数据源 => 无
		/// </summary>
		public long CrateId { get; set; }
		/// <summary>
		/// OnlyUpId
		/// 数据源 => 无
		/// </summary>
		public long OnlyUpId { get; set; }
		/// <summary>
		/// 最短耗时(ms)
		/// 数据源 => 无
		/// </summary>
		public long LeastRecordTime { get; set; }
		/// <summary>
		/// 移动距离
		/// 数据源 => 无
		/// </summary>
		public long MoveDistance { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchmod"></param>
        /// <param name="eventType"></param>
        /// <param name="level"></param>
        /// <param name="rootLevel"></param>
        /// <param name="crateId"></param>
        /// <param name="onlyUpId"></param>
        /// <param name="leastRecordTime"></param>
        /// <param name="moveDistance"></param>
		public OnlyUpUnet( int matchmod, sbyte eventType, int level, int rootLevel, long crateId, long onlyUpId, long leastRecordTime, long moveDistance )
		{
			Matchmod = matchmod;
			EventType = eventType;
			Level = level;
			RootLevel = rootLevel;
			CrateId = crateId;
			OnlyUpId = onlyUpId;
			LeastRecordTime = leastRecordTime;
			MoveDistance = moveDistance;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 新手教学活动任务完成
    /// 新手教学活动任务完成
    /// </summary>
	public struct TeachingStageTaskFinish
	{
		/// <summary>
		/// 教学阶段id
		/// 数据源 => 外键表：data/beginner_teaching_stage.csv 外键列：id
		/// </summary>
		public long BeginnerTeachingStage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="beginnerTeachingStage"></param>
		public TeachingStageTaskFinish( long beginnerTeachingStage )
		{
			BeginnerTeachingStage = beginnerTeachingStage;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-淘汰
    /// 撤离模式-淘汰
    /// </summary>
	public struct GoldDashKill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="jumpId"></param>
        /// <param name="mapDifficultys"></param>
		public GoldDashKill( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long jumpId, int[] mapDifficultys )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			JumpId = jumpId;
			MapDifficultys = mapDifficultys;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-淘汰Boss
    /// 撤离模式-淘汰Boss
    /// </summary>
	public struct GoldDashKillBoss
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 头类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long HeadType { get; set; }
		/// <summary>
		/// 护甲类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long ArmorType { get; set; }
		/// <summary>
		/// 背包类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long BackPackType { get; set; }
		/// <summary>
		/// 指定枪械
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long GunId { get; set; }
		/// <summary>
		/// Boss类型
		/// 数据源 => 外键表：data/gold_dash_monster_type.csv 外键列：id
		/// </summary>
		public byte[] CommonMonsterType { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="jumpId"></param>
        /// <param name="headType"></param>
        /// <param name="armorType"></param>
        /// <param name="backPackType"></param>
        /// <param name="gunId"></param>
        /// <param name="commonMonsterType"></param>
        /// <param name="mapDifficultys"></param>
		public GoldDashKillBoss( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long jumpId, long headType, long armorType, long backPackType, long gunId, byte[] commonMonsterType, int[] mapDifficultys )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			JumpId = jumpId;
			HeadType = headType;
			ArmorType = armorType;
			BackPackType = backPackType;
			GunId = gunId;
			CommonMonsterType = commonMonsterType;
			MapDifficultys = mapDifficultys;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-商店购买道具
    /// 撤离模式-商店购买道具
    /// </summary>
	public struct GoldDashBuyShop
	{
		/// <summary>
		/// 道具id
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long ItemId { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="jumpId"></param>
		public GoldDashBuyShop( long itemId, long jumpId )
		{
			ItemId = itemId;
			JumpId = jumpId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-商店卖道具获得撤离货币数量
    /// 撤离模式-商店卖道具获得撤离货币数量
    /// </summary>
	public struct GoldDashSaleShop
	{
		/// <summary>
		/// 道具id
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long ItemId { get; set; }
		/// <summary>
		/// 货币数量
		/// 数据源 => 无
		/// </summary>
		public long Count { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <param name="jumpId"></param>
		public GoldDashSaleShop( long itemId, long count, long jumpId )
		{
			ItemId = itemId;
			Count = count;
			JumpId = jumpId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-道具交付
    /// 撤离模式-道具交付
    /// </summary>
	public struct GoldDashDeliver
	{
		/// <summary>
		/// 交付类型
		/// 数据源 => 自定义值映射 0：道具交付 1：道具类型交付
		/// </summary>
		public sbyte Type { get; set; }
		/// <summary>
		/// 道具
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] DeliverItemIds { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 道具类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long[] DeliverItemTypeIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deliverItemIds"></param>
        /// <param name="jumpId"></param>
        /// <param name="deliverItemTypeIds"></param>
		public GoldDashDeliver( sbyte type, long[] deliverItemIds, long jumpId, long[] deliverItemTypeIds )
		{
			Type = type;
			DeliverItemIds = deliverItemIds;
			JumpId = jumpId;
			DeliverItemTypeIds = deliverItemTypeIds;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-撤离结束
    /// 撤离模式-撤离结束
    /// </summary>
	public struct GoldDashGameOver
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 撤离价值
		/// 数据源 => 无
		/// </summary>
		public long Value { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 撤离总收益
		/// 数据源 => 无
		/// </summary>
		public long TotalMoney { get; set; }
		/// <summary>
		/// 撤离游戏时长
		/// 数据源 => 无
		/// </summary>
		public long TotalDuration { get; set; }
		/// <summary>
		/// 是否召回好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsRecallFriend { get; set; }
		/// <summary>
		/// 是否集结好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsAssembleFriend { get; set; }
		/// <summary>
		/// 是否好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsFriend { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchMods"></param>
        /// <param name="value"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="totalMoney"></param>
        /// <param name="totalDuration"></param>
        /// <param name="isRecallFriend"></param>
        /// <param name="isAssembleFriend"></param>
        /// <param name="isFriend"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="gamemod"></param>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="showmod"></param>
		public GoldDashGameOver( int[] matchMods, long value, long jumpId, sbyte isSuccess, long totalMoney, long totalDuration, sbyte isRecallFriend, sbyte isAssembleFriend, sbyte isFriend, int[] mapDifficultys, int gamemod, int mapmod, int groupmod, int showmod )
		{
			MatchMods = matchMods;
			Value = value;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			TotalMoney = totalMoney;
			TotalDuration = totalDuration;
			IsRecallFriend = isRecallFriend;
			IsAssembleFriend = isAssembleFriend;
			IsFriend = isFriend;
			MapDifficultys = mapDifficultys;
			Gamemod = gamemod;
			Mapmod = mapmod;
			Groupmod = groupmod;
			Showmod = showmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-跳转目标
    /// 撤离模式-跳转目标
    /// </summary>
	public struct GoldDashFixItem
	{
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否集结好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsAssembleFriend { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jumpId"></param>
        /// <param name="isAssembleFriend"></param>
		public GoldDashFixItem( long jumpId, sbyte isAssembleFriend )
		{
			JumpId = jumpId;
			IsAssembleFriend = isAssembleFriend;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 集结任务
    /// 集结任务
    /// </summary>
	public struct Assemble
	{
		/// <summary>
		/// 集结任务类型
		/// 数据源 => 自定义值映射 0：首次集结 1：集结进度 2：拉取新进/回流玩家
		/// </summary>
		public sbyte AssembleTaskType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assembleTaskType"></param>
		public Assemble( sbyte assembleTaskType )
		{
			AssembleTaskType = assembleTaskType;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-指定条件击杀
    /// 撤离模式-指定击杀
    /// </summary>
	public struct GoldDashAssignKill
	{
		/// <summary>
		/// 击杀类型
		/// 数据源 => 自定义值映射 0：无 1：怪物 2：玩家
		/// </summary>
		public sbyte Type { get; set; }
		/// <summary>
		/// 怪物id
		/// 数据源 => 外键表：biubiubiu2_data/gpo.csv 外键列：id
		/// </summary>
		public int[] MonsterIds { get; set; }
		/// <summary>
		/// 头类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long HeadType { get; set; }
		/// <summary>
		/// 护甲类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long ArmorType { get; set; }
		/// <summary>
		/// 背包类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long BackPackType { get; set; }
		/// <summary>
		/// 指定枪械
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] GunIds { get; set; }
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 普通怪物类型
		/// 数据源 => 外键表：data/gold_dash_monster_type.csv 外键列：id
		/// </summary>
		public byte[] CommonMonsterType { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="monsterIds"></param>
        /// <param name="headType"></param>
        /// <param name="armorType"></param>
        /// <param name="backPackType"></param>
        /// <param name="gunIds"></param>
        /// <param name="gamemod"></param>
        /// <param name="matchmod"></param>
        /// <param name="jumpId"></param>
        /// <param name="commonMonsterType"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
		public GoldDashAssignKill( sbyte type, int[] monsterIds, long headType, long armorType, long backPackType, long[] gunIds, int gamemod, int matchmod, long jumpId, byte[] commonMonsterType, int[] mapDifficultys, sbyte isTeamShare )
		{
			Type = type;
			MonsterIds = monsterIds;
			HeadType = headType;
			ArmorType = armorType;
			BackPackType = backPackType;
			GunIds = gunIds;
			Gamemod = gamemod;
			Matchmod = matchmod;
			JumpId = jumpId;
			CommonMonsterType = commonMonsterType;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-指定道具满足数量条件
    /// 撤离模式-指定道具满足数量条件
    /// </summary>
	public struct GoldDashItemReached
	{
		/// <summary>
		/// 道具id
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long GoldDashItemId { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goldDashItemId"></param>
        /// <param name="jumpId"></param>
		public GoldDashItemReached( long goldDashItemId, long jumpId )
		{
			GoldDashItemId = goldDashItemId;
			JumpId = jumpId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-个人等级
    /// 撤离模式-个人等级
    /// </summary>
	public struct GoldDashLevel
	{
		/// <summary>
		/// 个人等级
		/// 数据源 => 无
		/// </summary>
		public int Level { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level"></param>
		public GoldDashLevel( int level )
		{
			Level = level;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-撤离成功时携带道具
    /// 撤离模式-撤离成功时携带道具
    /// </summary>
	public struct GoldDashItemCollect
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 撤离道具
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] ItemIds { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchMods"></param>
        /// <param name="itemIds"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="mapmod"></param>
		public GoldDashItemCollect( int[] matchMods, long[] itemIds, long jumpId, sbyte isSuccess, int[] mapDifficultys, int groupmod, int gamemod, int showmod, int mapmod )
		{
			MatchMods = matchMods;
			ItemIds = itemIds;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			MapDifficultys = mapDifficultys;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Mapmod = mapmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-基建系统升级
    /// 撤离模式-基建系统升级
    /// </summary>
	public struct GoldDashInfrastructure
	{
		/// <summary>
		/// 类型
		/// 数据源 => 自定义值映射 0：子系统升级到指定等级 1：子系统升级达到指定次数
		/// </summary>
		public sbyte Type { get; set; }
		/// <summary>
		/// 子系统名称
		/// 数据源 => 外键表：data/gold_dash_infrastructure.csv 外键列：id
		/// </summary>
		public long InfraId { get; set; }
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="infraId"></param>
        /// <param name="jumpId"></param>
		public GoldDashInfrastructure( sbyte type, long infraId, long jumpId )
		{
			Type = type;
			InfraId = infraId;
			JumpId = jumpId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-累计携带物品
    /// 撤离模式-悬赏任务-累计携带物品
    /// </summary>
	public struct GoldDashOfferArewardTotalCollect
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 撤离道具
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] ItemIds { get; set; }
		/// <summary>
		/// 限定时间(秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 道具类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long[] ItemTypes { get; set; }
		/// <summary>
		/// 来源宝箱类型
		/// 数据源 => 外键表：client/loot_group.csv 外键列：type
		/// </summary>
		public long[] BoxTypeIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="itemIds"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="itemTypes"></param>
        /// <param name="boxTypeIds"></param>
		public GoldDashOfferArewardTotalCollect( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long[] itemIds, long limitTime, sbyte[] barenessTypes, long[] itemTypes, long[] boxTypeIds )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			ItemIds = itemIds;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			ItemTypes = itemTypes;
			BoxTypeIds = boxTypeIds;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-开启宝箱
    /// 撤离模式-悬赏任务-开启宝箱
    /// </summary>
	public struct GoldDashOfferArewardOpenBox
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 宝箱类型
		/// 数据源 => 外键表：client/loot_group.csv 外键列：type
		/// </summary>
		public long[] BoxTypeIds { get; set; }
		/// <summary>
		/// 限定时间（秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="boxTypeIds"></param>
        /// <param name="limitTime"></param>
		public GoldDashOfferArewardOpenBox( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long[] boxTypeIds, long limitTime )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			BoxTypeIds = boxTypeIds;
			LimitTime = limitTime;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-击杀怪物
    /// 撤离模式-悬赏任务-击杀怪物
    /// </summary>
	public struct GoldDashOfferArewardKillMonster
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 普通怪物类型
		/// 数据源 => 外键表：data/gold_dash_monster_type.csv 外键列：id
		/// </summary>
		public byte[] CommonMonsterType { get; set; }
		/// <summary>
		/// 怪物ai等级
		/// 数据源 => 外键表：sys_dict.csv 外键列：value
		/// </summary>
		public ushort[] MonsterQualitys { get; set; }
		/// <summary>
		/// 怪物id
		/// 数据源 => 外键表：biubiubiu2_data/gpo.csv 外键列：id
		/// </summary>
		public int[] MonsterIds { get; set; }
		/// <summary>
		/// 指定枪械
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] GunIds { get; set; }
		/// <summary>
		/// 子弹等级
		/// 数据源 => 外键表：data/gold_dash_bullet_level.csv 外键列：level
		/// </summary>
		public int[] BulletLevel { get; set; }
		/// <summary>
		/// 是否无配件
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte IsNoParts { get; set; }
		/// <summary>
		/// 姿势
		/// 数据源 => 自定义值映射 0：不限制 1：站立 2：趴着 3：空中
		/// </summary>
		public sbyte Posture { get; set; }
		/// <summary>
		/// 限定时间（秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 无伤击杀
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte NoHurtKill { get; set; }
		/// <summary>
		/// 击杀boss过程无伤
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte NoHurtKillBoss { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="isSuccess"></param>
        /// <param name="jumpId"></param>
        /// <param name="commonMonsterType"></param>
        /// <param name="monsterQualitys"></param>
        /// <param name="monsterIds"></param>
        /// <param name="gunIds"></param>
        /// <param name="bulletLevel"></param>
        /// <param name="isNoParts"></param>
        /// <param name="posture"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="noHurtKill"></param>
        /// <param name="noHurtKillBoss"></param>
		public GoldDashOfferArewardKillMonster( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, sbyte isSuccess, long jumpId, byte[] commonMonsterType, ushort[] monsterQualitys, int[] monsterIds, long[] gunIds, int[] bulletLevel, sbyte isNoParts, sbyte posture, long limitTime, sbyte[] barenessTypes, sbyte noHurtKill, sbyte noHurtKillBoss )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			IsSuccess = isSuccess;
			JumpId = jumpId;
			CommonMonsterType = commonMonsterType;
			MonsterQualitys = monsterQualitys;
			MonsterIds = monsterIds;
			GunIds = gunIds;
			BulletLevel = bulletLevel;
			IsNoParts = isNoParts;
			Posture = posture;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			NoHurtKill = noHurtKill;
			NoHurtKillBoss = noHurtKillBoss;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-击杀真人
    /// 撤离模式-悬赏任务-击杀真人
    /// </summary>
	public struct GoldDashOfferArewardKillPlayer
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 子弹等级
		/// 数据源 => 外键表：data/gold_dash_bullet_level.csv 外键列：level
		/// </summary>
		public int[] BulletLevel { get; set; }
		/// <summary>
		/// 姿势
		/// 数据源 => 自定义值映射 0：不限制 1：站立 2：趴着 3：空中
		/// </summary>
		public sbyte Posture { get; set; }
		/// <summary>
		/// 枪械类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long GunType { get; set; }
		/// <summary>
		/// 血量低于
		/// 数据源 => 无
		/// </summary>
		public int LessBloodPercent { get; set; }
		/// <summary>
		/// 子弹类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long BulletType { get; set; }
		/// <summary>
		/// 小于距离
		/// 数据源 => 无
		/// </summary>
		public int LessDistance { get; set; }
		/// <summary>
		/// 是否爆头
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte IsHeadshot { get; set; }
		/// <summary>
		/// 是否无配件
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte IsNoParts { get; set; }
		/// <summary>
		/// 是否不使用药物
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte NoUseDrug { get; set; }
		/// <summary>
		/// 限定时间（秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 枪械二级分类
		/// 数据源 => 外键表：data/gold_dash_second_type.csv 外键列：id
		/// </summary>
		public long GunSecondType { get; set; }
		/// <summary>
		/// 无伤击杀
		/// 数据源 => 自定义值映射 0：不限制 1：是
		/// </summary>
		public sbyte NoHurtKill { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 外键表：data/map_coordinate.csv 外键列：id
		/// </summary>
		public long[] Coords { get; set; }
		/// <summary>
		/// 击杀宝箱类型
		/// 数据源 => 外键表：client/loot_group.csv 外键列：type
		/// </summary>
		public long[] BoxTypeIds { get; set; }
		/// <summary>
		/// 击杀范围(米）
		/// 数据源 => 无
		/// </summary>
		public long KillRange { get; set; }
		/// <summary>
		/// 是否近战武器击杀
		/// 数据源 => 自定义值映射 0：不限制 1：是 2：否
		/// </summary>
		public sbyte IsMelee { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="isSuccess"></param>
        /// <param name="jumpId"></param>
        /// <param name="bulletLevel"></param>
        /// <param name="posture"></param>
        /// <param name="gunType"></param>
        /// <param name="lessBloodPercent"></param>
        /// <param name="bulletType"></param>
        /// <param name="lessDistance"></param>
        /// <param name="isHeadshot"></param>
        /// <param name="isNoParts"></param>
        /// <param name="noUseDrug"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="gunSecondType"></param>
        /// <param name="noHurtKill"></param>
        /// <param name="coords"></param>
        /// <param name="boxTypeIds"></param>
        /// <param name="killRange"></param>
        /// <param name="isMelee"></param>
		public GoldDashOfferArewardKillPlayer( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, sbyte isSuccess, long jumpId, int[] bulletLevel, sbyte posture, long gunType, int lessBloodPercent, long bulletType, int lessDistance, sbyte isHeadshot, sbyte isNoParts, sbyte noUseDrug, long limitTime, sbyte[] barenessTypes, long gunSecondType, sbyte noHurtKill, long[] coords, long[] boxTypeIds, long killRange, sbyte isMelee )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			IsSuccess = isSuccess;
			JumpId = jumpId;
			BulletLevel = bulletLevel;
			Posture = posture;
			GunType = gunType;
			LessBloodPercent = lessBloodPercent;
			BulletType = bulletType;
			LessDistance = lessDistance;
			IsHeadshot = isHeadshot;
			IsNoParts = isNoParts;
			NoUseDrug = noUseDrug;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			GunSecondType = gunSecondType;
			NoHurtKill = noHurtKill;
			Coords = coords;
			BoxTypeIds = boxTypeIds;
			KillRange = killRange;
			IsMelee = isMelee;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-调查
    /// 撤离模式-悬赏任务-调查
    /// </summary>
	public struct GoldDashOfferArewardSurvey
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 外键表：data/map_coordinate.csv 外键列：id
		/// </summary>
		public long[] Coords { get; set; }
		/// <summary>
		/// 限制时间：秒
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="coords"></param>
        /// <param name="limitTime"></param>
		public GoldDashOfferArewardSurvey( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, long[] coords, long limitTime )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			Coords = coords;
			LimitTime = limitTime;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-累计收益
    /// 撤离模式-悬赏任务-累计收益
    /// </summary>
	public struct GoldDashOfferArewardEarnings
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 限定时间(秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 宝箱类型
		/// 数据源 => 外键表：client/loot_group.csv 外键列：type
		/// </summary>
		public long[] BoxTypeIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="boxTypeIds"></param>
		public GoldDashOfferArewardEarnings( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long limitTime, sbyte[] barenessTypes, long[] boxTypeIds )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			BoxTypeIds = boxTypeIds;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-开箱收益
    /// 撤离模式-悬赏任务-开箱收益
    /// </summary>
	public struct GoldDashOfferArewardBoxEarnings
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 限定时间(秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 宝箱类型
		/// 数据源 => 外键表：client/loot_group.csv 外键列：type
		/// </summary>
		public long[] BoxTypeIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="boxTypeIds"></param>
		public GoldDashOfferArewardBoxEarnings( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long limitTime, sbyte[] barenessTypes, long[] boxTypeIds )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			BoxTypeIds = boxTypeIds;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-保险箱价值
    /// 撤离模式-悬赏任务-保险箱价值
    /// </summary>
	public struct GoldDashOfferArewardSafeBoxCost
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
		public GoldDashOfferArewardSafeBoxCost( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-提交物品
    /// 撤离模式-悬赏任务-提交物品
    /// </summary>
	public struct GoldDashOfferArrewardSubmitItem
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 外键表：data/map_coordinate.csv 外键列：id
		/// </summary>
		public long[] Coords { get; set; }
		/// <summary>
		/// 限制时间：秒
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 撤离道具
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] ItemIds { get; set; }
		/// <summary>
		/// 道具类型
		/// 数据源 => 外键表：data/gold_dash_item_type.csv 外键列：id
		/// </summary>
		public long[] ItemTypes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="coords"></param>
        /// <param name="limitTime"></param>
        /// <param name="itemIds"></param>
        /// <param name="itemTypes"></param>
		public GoldDashOfferArrewardSubmitItem( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, long[] coords, long limitTime, long[] itemIds, long[] itemTypes )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			Coords = coords;
			LimitTime = limitTime;
			ItemIds = itemIds;
			ItemTypes = itemTypes;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-天赋点分配
    /// 撤离模式-天赋点分配
    /// </summary>
	public struct GoldDashTalentAlloc
	{
		/// <summary>
		/// 跳转目标
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jumpId"></param>
		public GoldDashTalentAlloc( long jumpId )
		{
			JumpId = jumpId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 暴打猛兽营收集事件
    /// 暴打猛兽营收集事件
    /// </summary>
	public struct BeatBeastcampCollect
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 收集物类型
		/// 数据源 => 系统字典类型 beat_beastcamp_collect_type
		/// </summary>
		public int CollectType { get; set; }
		/// <summary>
		/// 收集物ID
		/// 数据源 => 无
		/// </summary>
		public int CollectId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchmod"></param>
        /// <param name="collectType"></param>
        /// <param name="collectId"></param>
		public BeatBeastcampCollect( int matchmod, int collectType, int collectId )
		{
			Matchmod = matchmod;
			CollectType = collectType;
			CollectId = collectId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 暴打猛兽营物品事件
    /// 暴打猛兽营物品事件
    /// </summary>
	public struct BeatBeastcampItem
	{
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 物品类型
		/// 数据源 => 系统字典类型 beat_beastcamp_item_type
		/// </summary>
		public int ItemType { get; set; }
		/// <summary>
		/// 数量
		/// 数据源 => 无
		/// </summary>
		public int Num { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matchmod"></param>
        /// <param name="itemType"></param>
        /// <param name="num"></param>
		public BeatBeastcampItem( int matchmod, int itemType, int num )
		{
			Matchmod = matchmod;
			ItemType = itemType;
			Num = num;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 助攻
    /// 助攻
    /// </summary>
	public struct Assists
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
		public Assists( int mapmod, int groupmod, int gamemod, int showmod, int matchmod )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 消费子弹
    /// 消费子弹
    /// </summary>
	public struct ConsumeBullet
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 子弹ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Cbid { get; set; }
		/// <summary>
		/// 子弹数量
		/// 数据源 => 自定义值映射 0：0
		/// </summary>
		public long Cbc { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="cbid"></param>
        /// <param name="cbc"></param>
		public ConsumeBullet( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long cbid, long cbc )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Cbid = cbid;
			Cbc = cbc;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 使用动作
    /// 使用动作
    /// </summary>
	public struct UseAction
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 动作类型
		/// 数据源 => 自定义值映射 0：跳舞 1：表情 2：组队指令 3：二段跳
		/// </summary>
		public long ActionType { get; set; }
		/// <summary>
		/// 跳舞数量
		/// 数据源 => 无
		/// </summary>
		public long Dance { get; set; }
		/// <summary>
		/// 表情数量
		/// 数据源 => 无
		/// </summary>
		public long Face { get; set; }
		/// <summary>
		/// 队伍指令数量
		/// 数据源 => 无
		/// </summary>
		public long GroupCmd { get; set; }
		/// <summary>
		/// 二段跳数量
		/// 数据源 => 无
		/// </summary>
		public long TwoJump { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="actionType"></param>
        /// <param name="dance"></param>
        /// <param name="face"></param>
        /// <param name="groupCmd"></param>
        /// <param name="twoJump"></param>
		public UseAction( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long actionType, long dance, long face, long groupCmd, long twoJump )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			ActionType = actionType;
			Dance = dance;
			Face = face;
			GroupCmd = groupCmd;
			TwoJump = twoJump;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 跳伞
    /// 跳伞
    /// </summary>
	public struct Jump
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
		public Jump( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 特殊
    /// 特殊行为
    /// </summary>
	public struct Special
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 使用气流
		/// 数据源 => 无
		/// </summary>
		public long UseAirflow { get; set; }
		/// <summary>
		/// 使用滑索
		/// 数据源 => 无
		/// </summary>
		public long UseStrop { get; set; }
		/// <summary>
		/// 使用波波
		/// 数据源 => 无
		/// </summary>
		public long UseBobo { get; set; }
		/// <summary>
		/// 破坏波波
		/// 数据源 => 无
		/// </summary>
		public long DestroyBobo { get; set; }
		/// <summary>
		/// 波波类型
		/// 数据源 => 自定义值映射 0：不限 1：城堡波波 2：野外波波 3：投掷波波 4：其他 5：龙宝宝
		/// </summary>
		public long BoboType { get; set; }
		/// <summary>
		/// 击碎建筑
		/// 数据源 => 无
		/// </summary>
		public long DestroyBuilding { get; set; }
		/// <summary>
		/// 破坏建筑
		/// 数据源 => 自定义值映射 0：不限 1：火山石
		/// </summary>
		public long BuildingType { get; set; }
		/// <summary>
		/// 行为类型
		/// 数据源 => 自定义值映射 0：不限 1：牵手手
		/// </summary>
		public long BehaviorType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="useAirflow"></param>
        /// <param name="useStrop"></param>
        /// <param name="useBobo"></param>
        /// <param name="destroyBobo"></param>
        /// <param name="boboType"></param>
        /// <param name="destroyBuilding"></param>
        /// <param name="buildingType"></param>
        /// <param name="behaviorType"></param>
		public Special( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long useAirflow, long useStrop, long useBobo, long destroyBobo, long boboType, long destroyBuilding, long buildingType, long behaviorType )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			UseAirflow = useAirflow;
			UseStrop = useStrop;
			UseBobo = useBobo;
			DestroyBobo = destroyBobo;
			BoboType = boboType;
			DestroyBuilding = destroyBuilding;
			BuildingType = buildingType;
			BehaviorType = behaviorType;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 累计拾取物品
    /// 累计拾取物品
    /// </summary>
	public struct TotalCollectItem
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 品质
		/// 数据源 => 外键表：data/item_quality.csv 外键列：id
		/// </summary>
		public long Quality { get; set; }
		/// <summary>
		/// 物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Itemid { get; set; }
		/// <summary>
		/// 是否下发
		/// 数据源 => 自定义值映射 0：不下发 1：下发
		/// </summary>
		public sbyte Issend { get; set; }
		/// <summary>
		/// 是否记录发送玩家ID
		/// 数据源 => 自定义值映射 0：不记录 1：记录
		/// </summary>
		public long Sendpid { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="quality"></param>
        /// <param name="itemid"></param>
        /// <param name="issend"></param>
        /// <param name="sendpid"></param>
		public TotalCollectItem( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long quality, long itemid, sbyte issend, long sendpid )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Quality = quality;
			Itemid = itemid;
			Issend = issend;
			Sendpid = sendpid;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 驾驶击杀
    /// 驾驶击杀敌人
    /// </summary>
	public struct DriveKill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 是否驾驶员
		/// 数据源 => 自定义值映射 0：不要求 1：要求
		/// </summary>
		public sbyte IsDriver { get; set; }
		/// <summary>
		/// 载具ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long CarrierId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="isDriver"></param>
        /// <param name="carrierId"></param>
		public DriveKill( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, sbyte isDriver, long carrierId )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			IsDriver = isDriver;
			CarrierId = carrierId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-条件撤离
    /// 撤离模式-悬赏任务-条件撤离
    /// </summary>
	public struct GoldDashOfferArewardConditionGameOver
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 撤离价值
		/// 数据源 => 无
		/// </summary>
		public long Value { get; set; }
		/// <summary>
		/// 限定时间内：秒
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 是否无伤
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsNoHarm { get; set; }
		/// <summary>
		/// 是否不使用药品
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte NoUseDrug { get; set; }
		/// <summary>
		/// 不使用跳跃
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte NoJump { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="value"></param>
        /// <param name="limitTime"></param>
        /// <param name="isNoHarm"></param>
        /// <param name="noUseDrug"></param>
        /// <param name="noJump"></param>
        /// <param name="barenessTypes"></param>
		public GoldDashOfferArewardConditionGameOver( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long value, long limitTime, sbyte isNoHarm, sbyte noUseDrug, sbyte noJump, sbyte[] barenessTypes )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			Value = value;
			LimitTime = limitTime;
			IsNoHarm = isNoHarm;
			NoUseDrug = noUseDrug;
			NoJump = noJump;
			BarenessTypes = barenessTypes;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-打开容器
    /// 撤离模式-打开容器
    /// </summary>
	public struct GoldDashOpenContainer
	{
		/// <summary>
		/// 容器id
		/// 数据源 => 外键表：client/loot_group.csv 外键列：id
		/// </summary>
		public long[] ContainerIds { get; set; }
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="containerIds"></param>
        /// <param name="gamemod"></param>
        /// <param name="matchmod"></param>
		public GoldDashOpenContainer( long[] containerIds, int gamemod, int matchmod )
		{
			ContainerIds = containerIds;
			Gamemod = gamemod;
			Matchmod = matchmod;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-战场带出物品
    /// 撤离模式-战场带出物品(去除开局带入)
    /// </summary>
	public struct GoldDashHarvestItem
	{
		/// <summary>
		/// 撤离道具ID列表
		/// 数据源 => 外键表：data/gold_dash_item.csv 外键列：id
		/// </summary>
		public long[] ItemIdList { get; set; }
		/// <summary>
		/// 道具数量列表
		/// 数据源 => 无
		/// </summary>
		public long[] ItemNumList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemIdList"></param>
        /// <param name="itemNumList"></param>
		public GoldDashHarvestItem( long[] itemIdList, long[] itemNumList )
		{
			ItemIdList = itemIdList;
			ItemNumList = itemNumList;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务-打开机关
    /// 撤离模式-悬赏任务-打开机关
    /// </summary>
	public struct GoldDashOfferArewardMechanism
	{
		/// <summary>
		/// 游戏模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int[] MatchMods { get; set; }
		/// <summary>
		/// 地图难度
		/// 数据源 => 系统字典类型 gold_dash_match_mode_difficulty
		/// </summary>
		public int[] MapDifficultys { get; set; }
		/// <summary>
		/// 是否小队共享
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsTeamShare { get; set; }
		/// <summary>
		/// 跳转目标（外围使用）
		/// 数据源 => 外键表：data/gold_dash_task_jump_config.csv 外键列：id
		/// </summary>
		public long JumpId { get; set; }
		/// <summary>
		/// 是否撤离成功
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsSuccess { get; set; }
		/// <summary>
		/// 限定时间(秒）
		/// 数据源 => 无
		/// </summary>
		public long LimitTime { get; set; }
		/// <summary>
		/// 裸装类型
		/// 数据源 => 自定义值映射 0：没有头 1：没有甲 2：没有包 3：没有武器
		/// </summary>
		public sbyte[] BarenessTypes { get; set; }
		/// <summary>
		/// 机关ids
		/// 数据源 => 外键表：client/mechanism_config.csv 外键列：id
		/// </summary>
		public ushort[] MechanismIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gamemod"></param>
        /// <param name="matchMods"></param>
        /// <param name="mapDifficultys"></param>
        /// <param name="isTeamShare"></param>
        /// <param name="jumpId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="limitTime"></param>
        /// <param name="barenessTypes"></param>
        /// <param name="mechanismIds"></param>
		public GoldDashOfferArewardMechanism( int gamemod, int[] matchMods, int[] mapDifficultys, sbyte isTeamShare, long jumpId, sbyte isSuccess, long limitTime, sbyte[] barenessTypes, ushort[] mechanismIds )
		{
			Gamemod = gamemod;
			MatchMods = matchMods;
			MapDifficultys = mapDifficultys;
			IsTeamShare = isTeamShare;
			JumpId = jumpId;
			IsSuccess = isSuccess;
			LimitTime = limitTime;
			BarenessTypes = barenessTypes;
			MechanismIds = mechanismIds;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 撤离模式-悬赏任务节点
    /// 撤离模式-悬赏任务节点
    /// </summary>
	public struct GoldDashOfferArewardNode
	{
		/// <summary>
		/// 撤离赛季
		/// 数据源 => 外键表：data/gold_dash_season.csv 外键列：id
		/// </summary>
		public long SeasonId { get; set; }
		/// <summary>
		/// 悬赏任务节点
		/// 数据源 => 外键表：data/gold_dash_reward_season_task_node.csv 外键列：id
		/// </summary>
		public long RewardNode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="rewardNode"></param>
		public GoldDashOfferArewardNode( long seasonId, long rewardNode )
		{
			SeasonId = seasonId;
			RewardNode = rewardNode;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 游戏结束
    /// 结算触发
    /// </summary>
	public struct GameOver
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 存活时长
		/// 数据源 => 无
		/// </summary>
		public long At { get; set; }
		/// <summary>
		/// 排名
		/// 数据源 => 无
		/// </summary>
		public long R { get; set; }
		/// <summary>
		/// 是否好友
		/// 数据源 => 自定义值映射 0：不需要好友 1：需要好友
		/// </summary>
		public sbyte Isfriend { get; set; }
		/// <summary>
		/// 套装
		/// 数据源 => 外键表：data/pay_crate_topic.csv 外键列：id
		/// </summary>
		public long Suit { get; set; }
		/// <summary>
		/// 总伤害
		/// 数据源 => 无
		/// </summary>
		public long TotalHurt { get; set; }
		/// <summary>
		/// 队伍情况
		/// 数据源 => 自定义值映射 0：无 1：全队都活着 2：只有我活着
		/// </summary>
		public long GroupInfo { get; set; }
		/// <summary>
		/// 战场装备
		/// 数据源 => 自定义值映射 0：正常 1：无战场装备
		/// </summary>
		public long WarEquip { get; set; }
		/// <summary>
		/// 评分
		/// 数据源 => 无
		/// </summary>
		public long Grade { get; set; }
		/// <summary>
		/// 不受每日时间限制
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public long Ndtl { get; set; }
		/// <summary>
		/// 是否进入Y星飞碟
		/// 数据源 => 自定义值映射 0：未进入Y星飞碟 1：是进入Y星飞碟
		/// </summary>
		public sbyte IsGetIntoYscout { get; set; }
		/// <summary>
		/// 是否MVP
		/// 数据源 => 自定义值映射 0：不是MVP 1：是MVP
		/// </summary>
		public int Mvp { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 积分
		/// 数据源 => 无
		/// </summary>
		public long Schange { get; set; }
		/// <summary>
		/// 参与乐园大冲关共几轮（不是大冲关传0）
		/// 数据源 => 无
		/// </summary>
		public long KnockotRound { get; set; }
		/// <summary>
		/// 是否战队好友
		/// 数据源 => 自定义值映射 0：不需要战队好友 1：需要战队好友
		/// </summary>
		public sbyte IsClub { get; set; }
		/// <summary>
		/// 超神
		/// 数据源 => 自定义值映射 0：没超神 1：超神
		/// </summary>
		public long SuperGod { get; set; }
		/// <summary>
		/// 冠军
		/// 数据源 => 自定义值映射 0：没冠军 1：冠军
		/// </summary>
		public long Champion { get; set; }
		/// <summary>
		/// 超级援兵
		/// 数据源 => 无
		/// </summary>
		public long MaxPut { get; set; }
		/// <summary>
		/// 淘汰王
		/// 数据源 => 自定义值映射 0：不是淘汰王 1：是淘汰王
		/// </summary>
		public long KillKing { get; set; }
		/// <summary>
		/// 是否胜利
		/// 数据源 => 自定义值映射 0：不是胜利 1：是胜利
		/// </summary>
		public int Win { get; set; }
		/// <summary>
		/// 助力类型
		/// 数据源 => 自定义值映射 0：单向助力 1：双向助力
		/// </summary>
		public sbyte AssistType { get; set; }
		/// <summary>
		/// 是否进入魔法决斗
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsGetIntoMagic { get; set; }
		/// <summary>
		/// 是否召回经验值
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsRecallExp { get; set; }
		/// <summary>
		/// 是否好友组队才加进度
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsFriendAddProgress { get; set; }
		/// <summary>
		/// 是否召回好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsRecallFriend { get; set; }
		/// <summary>
		/// 是否集结好友
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsAssembleFriend { get; set; }
		/// <summary>
		/// 每日进度上限
		/// 数据源 => 无
		/// </summary>
		public long DailyMaxProgress { get; set; }
		/// <summary>
		/// 是否是周末赛
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsWeekendCompetition { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="coord"></param>
        /// <param name="at"></param>
        /// <param name="r"></param>
        /// <param name="isfriend"></param>
        /// <param name="suit"></param>
        /// <param name="totalHurt"></param>
        /// <param name="groupInfo"></param>
        /// <param name="warEquip"></param>
        /// <param name="grade"></param>
        /// <param name="ndtl"></param>
        /// <param name="isGetIntoYscout"></param>
        /// <param name="mvp"></param>
        /// <param name="matchmod"></param>
        /// <param name="schange"></param>
        /// <param name="knockotRound"></param>
        /// <param name="isClub"></param>
        /// <param name="superGod"></param>
        /// <param name="champion"></param>
        /// <param name="maxPut"></param>
        /// <param name="killKing"></param>
        /// <param name="win"></param>
        /// <param name="assistType"></param>
        /// <param name="isGetIntoMagic"></param>
        /// <param name="isRecallExp"></param>
        /// <param name="isFriendAddProgress"></param>
        /// <param name="isRecallFriend"></param>
        /// <param name="isAssembleFriend"></param>
        /// <param name="dailyMaxProgress"></param>
        /// <param name="isWeekendCompetition"></param>
		public GameOver( int mapmod, int groupmod, int gamemod, int showmod, long coord, long at, long r, sbyte isfriend, long suit, long totalHurt, long groupInfo, long warEquip, long grade, long ndtl, sbyte isGetIntoYscout, int mvp, int matchmod, long schange, long knockotRound, sbyte isClub, long superGod, long champion, long maxPut, long killKing, int win, sbyte assistType, sbyte isGetIntoMagic, sbyte isRecallExp, sbyte isFriendAddProgress, sbyte isRecallFriend, sbyte isAssembleFriend, long dailyMaxProgress, sbyte isWeekendCompetition )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Coord = coord;
			At = at;
			R = r;
			Isfriend = isfriend;
			Suit = suit;
			TotalHurt = totalHurt;
			GroupInfo = groupInfo;
			WarEquip = warEquip;
			Grade = grade;
			Ndtl = ndtl;
			IsGetIntoYscout = isGetIntoYscout;
			Mvp = mvp;
			Matchmod = matchmod;
			Schange = schange;
			KnockotRound = knockotRound;
			IsClub = isClub;
			SuperGod = superGod;
			Champion = champion;
			MaxPut = maxPut;
			KillKing = killKing;
			Win = win;
			AssistType = assistType;
			IsGetIntoMagic = isGetIntoMagic;
			IsRecallExp = isRecallExp;
			IsFriendAddProgress = isFriendAddProgress;
			IsRecallFriend = isRecallFriend;
			IsAssembleFriend = isAssembleFriend;
			DailyMaxProgress = dailyMaxProgress;
			IsWeekendCompetition = isWeekendCompetition;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 破坏载具
    /// 破坏载具
    /// </summary>
	public struct DestroyCarrier
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 载具ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long CarrierId { get; set; }
		/// <summary>
		/// 是否皮皮球
		/// 数据源 => 自定义值映射 0：否 1：是
		/// </summary>
		public sbyte IsBall { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="carrierId"></param>
        /// <param name="isBall"></param>
		public DestroyCarrier( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long carrierId, sbyte isBall )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			CarrierId = carrierId;
			IsBall = isBall;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 战场消耗大厅物品
    /// 消耗物品
    /// </summary>
	public struct DecItem
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 道具数量
		/// 数据源 => 无
		/// </summary>
		public long ItemCount { get; set; }
		/// <summary>
		/// 物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Itemid { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="itemCount"></param>
        /// <param name="itemid"></param>
		public DecItem( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long itemCount, long itemid )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			ItemCount = itemCount;
			Itemid = itemid;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 连续击杀
    /// 连续击杀
    /// </summary>
	public struct KillStreak
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 最大连击数
		/// 数据源 => 无
		/// </summary>
		public int MaxKillStreak { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="maxKillStreak"></param>
		public KillStreak( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, int maxKillStreak )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			MaxKillStreak = maxKillStreak;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 团队击杀
    /// 团队击杀
    /// </summary>
	public struct TeamKill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 团队击杀数
		/// 数据源 => 无
		/// </summary>
		public long TeamKillCount { get; set; }
		/// <summary>
		/// 是否战队好友
		/// 数据源 => 自定义值映射 0：不需要战队好友 1：需要战队好友
		/// </summary>
		public sbyte IsClub { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="teamKillCount"></param>
        /// <param name="isClub"></param>
		public TeamKill( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long teamKillCount, sbyte isClub )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			TeamKillCount = teamKillCount;
			IsClub = isClub;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 配件击杀
    /// 配件击杀
    /// </summary>
	public struct PartsKill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 配件1ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Part1 { get; set; }
		/// <summary>
		/// 配件2ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Part2 { get; set; }
		/// <summary>
		/// 配件3ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Part3 { get; set; }
		/// <summary>
		/// 配件4ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Part4 { get; set; }
		/// <summary>
		/// 配件5ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Part5 { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <param name="part3"></param>
        /// <param name="part4"></param>
        /// <param name="part5"></param>
		public PartsKill( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long part1, long part2, long part3, long part4, long part5 )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Part1 = part1;
			Part2 = part2;
			Part3 = part3;
			Part4 = part4;
			Part5 = part5;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 淘汰
    /// 淘汰
    /// </summary>
	public struct WeedOut
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 武器类型
		/// 数据源 => 外键表：data/item_type.csv 外键列：id
		/// </summary>
		public long WeaponType { get; set; }
		/// <summary>
		/// 武器ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Weapon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="weaponType"></param>
        /// <param name="weapon"></param>
		public WeedOut( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long weaponType, long weapon )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			WeaponType = weaponType;
			Weapon = weapon;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 使用传送大炮
    /// 使用传送大炮
    /// </summary>
	public struct UseCannon
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
		public UseCannon( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 增加血量
    /// 增加血量触发
    /// </summary>
	public struct AddHp
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 增加血量
		/// 数据源 => 无
		/// </summary>
		public long Ahp { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="ahp"></param>
		public AddHp( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long ahp )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Ahp = ahp;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 2代事件-动作数据
    /// 2代事件-动作数据
    /// </summary>
	public struct EventActionData
	{
		/// <summary>
		/// 类型
		/// 数据源 => 系统字典类型 event_action_type
		/// </summary>
		public ushort ActionTypeId { get; set; }
		/// <summary>
		/// 等待时间
		/// 数据源 => 无
		/// </summary>
		public float WaitTime { get; set; }
		/// <summary>
		/// 自定义值
		/// 数据源 => 无
		/// </summary>
		public string CustomValue { get; set; }
		/// <summary>
		/// 使用次数，-1 表示无限次
		/// 数据源 => 无
		/// </summary>
		public int UseLimitCount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actionTypeId"></param>
        /// <param name="waitTime"></param>
        /// <param name="customValue"></param>
        /// <param name="useLimitCount"></param>
		public EventActionData( ushort actionTypeId, float waitTime, string customValue, int useLimitCount )
		{
			ActionTypeId = actionTypeId;
			WaitTime = waitTime;
			CustomValue = customValue;
			UseLimitCount = useLimitCount;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 拾取物品
    /// 拾取物品
    /// </summary>
	public struct CollectItem
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long CollectItemid { get; set; }
		/// <summary>
		/// 是否小肠肠
		/// 数据源 => 自定义值映射 0：不是小肠肠 1：是小肠肠
		/// </summary>
		public sbyte IsSausageBaby { get; set; }
		/// <summary>
		/// 物品类型
		/// 数据源 => 外键表：data/item_type.csv 外键列：id
		/// </summary>
		public long Itemtype { get; set; }
		/// <summary>
		/// 身份卡1ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card1 { get; set; }
		/// <summary>
		/// 身份卡2ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card2 { get; set; }
		/// <summary>
		/// 身份卡3ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card3 { get; set; }
		/// <summary>
		/// 身份卡4ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card4 { get; set; }
		/// <summary>
		/// 身份卡5ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card5 { get; set; }
		/// <summary>
		/// 是否神秘道具
		/// 数据源 => 自定义值映射 0：不是神秘道具 1：是神秘道具
		/// </summary>
		public sbyte IsMysteryItem { get; set; }
		/// <summary>
		/// 拾取物品列表
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long[] CollectItemidList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="collectItemid"></param>
        /// <param name="isSausageBaby"></param>
        /// <param name="itemtype"></param>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        /// <param name="card3"></param>
        /// <param name="card4"></param>
        /// <param name="card5"></param>
        /// <param name="isMysteryItem"></param>
        /// <param name="collectItemidList"></param>
		public CollectItem( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long collectItemid, sbyte isSausageBaby, long itemtype, long card1, long card2, long card3, long card4, long card5, sbyte isMysteryItem, long[] collectItemidList )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			CollectItemid = collectItemid;
			IsSausageBaby = isSausageBaby;
			Itemtype = itemtype;
			Card1 = card1;
			Card2 = card2;
			Card3 = card3;
			Card4 = card4;
			Card5 = card5;
			IsMysteryItem = isMysteryItem;
			CollectItemidList = collectItemidList;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 复活
    /// 复活队友
    /// </summary>
	public struct Revive
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 复活人数
		/// 数据源 => 无
		/// </summary>
		public long ManCount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="manCount"></param>
		public Revive( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long manCount )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			ManCount = manCount;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 2代事件-条件数据
    /// 2代事件-条件数据
    /// </summary>
	public struct EventConditionData
	{
		/// <summary>
		/// 条件类型
		/// 数据源 => 系统字典类型 event_condition_type
		/// </summary>
		public ushort ConditionTypeId { get; set; }
		/// <summary>
		/// 比较关系
		/// 数据源 => 系统字典类型 compare_type
		/// </summary>
		public byte CompareType { get; set; }
		/// <summary>
		/// 自定义值
		/// 数据源 => 无
		/// </summary>
		public string CustomValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conditionTypeId"></param>
        /// <param name="compareType"></param>
        /// <param name="customValue"></param>
		public EventConditionData( ushort conditionTypeId, byte compareType, string customValue )
		{
			ConditionTypeId = conditionTypeId;
			CompareType = compareType;
			CustomValue = customValue;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 2代事件-主体数据
    /// 2代事件-主体数据
    /// </summary>
	public struct EventSubjectData
	{
		/// <summary>
		/// 主体类型
		/// 数据源 => 系统字典类型 event_subject_type
		/// </summary>
		public ushort SubjectTypeId { get; set; }
		/// <summary>
		/// 自定义值
		/// 数据源 => 无
		/// </summary>
		public string CustomValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subjectTypeId"></param>
        /// <param name="customValue"></param>
		public EventSubjectData( ushort subjectTypeId, string customValue )
		{
			SubjectTypeId = subjectTypeId;
			CustomValue = customValue;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 2代事件-时间段数据
    /// 2代事件-时间段数据
    /// </summary>
	public struct EventTimeData
	{
		/// <summary>
		/// 时间类型
		/// 数据源 => 系统字典类型 event_time_type
		/// </summary>
		public byte TimeTypeId { get; set; }
		/// <summary>
		/// 开始
		/// 数据源 => 无
		/// </summary>
		public float StartValue { get; set; }
		/// <summary>
		/// 结束
		/// 数据源 => 无
		/// </summary>
		public float EndValue { get; set; }
		/// <summary>
		/// 自定义值
		/// 数据源 => 无
		/// </summary>
		public string CustomValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timeTypeId"></param>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="customValue"></param>
		public EventTimeData( byte timeTypeId, float startValue, float endValue, string customValue )
		{
			TimeTypeId = timeTypeId;
			StartValue = startValue;
			EndValue = endValue;
			CustomValue = customValue;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 使用物品
    /// 每次使用物品触发
    /// </summary>
	public struct UseItem
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Itemid { get; set; }
		/// <summary>
		/// 物品类型
		/// 数据源 => 外键表：data/item_type.csv 外键列：id
		/// </summary>
		public long Itemtype { get; set; }
		/// <summary>
		/// 身份卡1ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card1 { get; set; }
		/// <summary>
		/// 身份卡2ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card2 { get; set; }
		/// <summary>
		/// 身份卡3ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card3 { get; set; }
		/// <summary>
		/// 身份卡4ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card4 { get; set; }
		/// <summary>
		/// 身份卡5ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Card5 { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="itemid"></param>
        /// <param name="itemtype"></param>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        /// <param name="card3"></param>
        /// <param name="card4"></param>
        /// <param name="card5"></param>
		public UseItem( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long itemid, long itemtype, long card1, long card2, long card3, long card4, long card5 )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			Itemid = itemid;
			Itemtype = itemtype;
			Card1 = card1;
			Card2 = card2;
			Card3 = card3;
			Card4 = card4;
			Card5 = card5;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 移动
    /// 移动一定距离触发
    /// </summary>
	public struct Move
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 移动方式
		/// 数据源 => 系统字典类型 move_type
		/// </summary>
		public long Movetype { get; set; }
		/// <summary>
		/// 移动距离
		/// 数据源 => 自定义值映射 0：0
		/// </summary>
		public int Wd { get; set; }
		/// <summary>
		/// 载具ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long CarrierId { get; set; }
		/// <summary>
		/// 生物载具ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long BiologyCarrierId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="movetype"></param>
        /// <param name="wd"></param>
        /// <param name="carrierId"></param>
        /// <param name="biologyCarrierId"></param>
		public Move( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long movetype, int wd, long carrierId, long biologyCarrierId )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Movetype = movetype;
			Wd = wd;
			CarrierId = carrierId;
			BiologyCarrierId = biologyCarrierId;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 击杀指定对象
    /// 击杀指定对象
    /// </summary>
	public struct KillAppointObject
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// Ai对象类型
		/// 数据源 => 系统字典类型 object_type
		/// </summary>
		public long ObjectType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="objectType"></param>
		public KillAppointObject( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long objectType )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			ObjectType = objectType;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 指定对象造成伤害
    /// 对指定对象造成伤害
    /// </summary>
	public struct AppointObjectHert
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// Ai对象类型
		/// 数据源 => 系统字典类型 object_type
		/// </summary>
		public long ObjectType { get; set; }
		/// <summary>
		/// 指定对象伤害
		/// 数据源 => 无
		/// </summary>
		public long Hurt { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="objectType"></param>
        /// <param name="hurt"></param>
		public AppointObjectHert( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long objectType, long hurt )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			ObjectType = objectType;
			Hurt = hurt;
		}
	}
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 结算拾取物品
    /// 结算时拾取物品
    /// </summary>
	public struct OverCollectItem
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 品质
		/// 数据源 => 外键表：data/item_quality.csv 外键列：id
		/// </summary>
		public long Quality { get; set; }
		/// <summary>
		/// 物品
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Itemid { get; set; }
		/// <summary>
		/// 道具数量
		/// 数据源 => 无
		/// </summary>
		public long ItemCount { get; set; }
		/// <summary>
		/// 是否发送道具
		/// 数据源 => 自定义值映射 0：不发送 1：发送
		/// </summary>
		public sbyte IsSend { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="quality"></param>
        /// <param name="itemid"></param>
        /// <param name="itemCount"></param>
        /// <param name="isSend"></param>
		public OverCollectItem( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long quality, long itemid, long itemCount, sbyte isSend )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Quality = quality;
			Itemid = itemid;
			ItemCount = itemCount;
			IsSend = isSend;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 获胜装备武器
    /// 玩家获胜时穿戴装备
    /// </summary>
	public struct WinEquipWeapon
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 枪械1ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long GunOne { get; set; }
		/// <summary>
		/// 枪械2ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long GunTwo { get; set; }
		/// <summary>
		/// 枪械3ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long GunThree { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="gunOne"></param>
        /// <param name="gunTwo"></param>
        /// <param name="gunThree"></param>
		public WinEquipWeapon( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long gunOne, long gunTwo, long gunThree )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			GunOne = gunOne;
			GunTwo = gunTwo;
			GunThree = gunThree;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 击杀
    /// 击杀一个玩家触发
    /// </summary>
	public struct Kill
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 地图坐标
		/// 数据源 => 无
		/// </summary>
		public long Coord { get; set; }
		/// <summary>
		/// 武器类型
		/// 数据源 => 外键表：data/item_type.csv 外键列：id
		/// </summary>
		public long WeaponType { get; set; }
		/// <summary>
		/// 武器ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Weapon { get; set; }
		/// <summary>
		/// 爆头
		/// 数据源 => 自定义值映射 0：无 1：爆头
		/// </summary>
		public int Kbh { get; set; }
		/// <summary>
		/// 是否复仇
		/// 数据源 => 自定义值映射 0：无 1：击杀曾经打到自己的
		/// </summary>
		public long Kb { get; set; }
		/// <summary>
		/// 姿势
		/// 数据源 => 自定义值映射 0：站立 1：非洲人射击 127：不限制 2：探头射击
		/// </summary>
		public int Pte { get; set; }
		/// <summary>
		/// 杀水中人
		/// 数据源 => 自定义值映射 0：无 1：在水中
		/// </summary>
		public long Inwater { get; set; }
		/// <summary>
		/// 子弹ID
		/// 数据源 => 外键表：data/item.csv 外键列：id
		/// </summary>
		public long Bullet { get; set; }
		/// <summary>
		/// 引爆载具
		/// 数据源 => 自定义值映射 0：不引爆 1：引爆
		/// </summary>
		public long DetonateVehicle { get; set; }
		/// <summary>
		/// 击杀距离
		/// 数据源 => 自定义值映射 0：0
		/// </summary>
		public long KillDistance { get; set; }
		/// <summary>
		/// 击杀情况
		/// 数据源 => 自定义值映射 0：无 1：击杀整队
		/// </summary>
		public long KillGroup { get; set; }
		/// <summary>
		/// 飞高高
		/// 数据源 => 自定义值映射 0：无 1：飞高高
		/// </summary>
		public long InSky { get; set; }
		/// <summary>
		/// 在杂技球上
		/// 数据源 => 自定义值映射 0：无 1：在杂技球上
		/// </summary>
		public long OnBall { get; set; }
		/// <summary>
		/// 是否是变大大状态
		/// 数据源 => 自定义值映射 0：不是变大大状态 1：是变大大状态
		/// </summary>
		public long IsChangeRoleSize { get; set; }
		/// <summary>
		/// 玩法模式列表
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int[] GameModList { get; set; }
		/// <summary>
		/// 跳转模式列表
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int[] ShowModList { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="coord"></param>
        /// <param name="weaponType"></param>
        /// <param name="weapon"></param>
        /// <param name="kbh"></param>
        /// <param name="kb"></param>
        /// <param name="pte"></param>
        /// <param name="inwater"></param>
        /// <param name="bullet"></param>
        /// <param name="detonateVehicle"></param>
        /// <param name="killDistance"></param>
        /// <param name="killGroup"></param>
        /// <param name="inSky"></param>
        /// <param name="onBall"></param>
        /// <param name="isChangeRoleSize"></param>
        /// <param name="gameModList"></param>
        /// <param name="showModList"></param>
		public Kill( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long coord, long weaponType, long weapon, int kbh, long kb, int pte, long inwater, long bullet, long detonateVehicle, long killDistance, long killGroup, long inSky, long onBall, long isChangeRoleSize, int[] gameModList, int[] showModList )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			Coord = coord;
			WeaponType = weaponType;
			Weapon = weapon;
			Kbh = kbh;
			Kb = kb;
			Pte = pte;
			Inwater = inwater;
			Bullet = bullet;
			DetonateVehicle = detonateVehicle;
			KillDistance = killDistance;
			KillGroup = killGroup;
			InSky = inSky;
			OnBall = onBall;
			IsChangeRoleSize = isChangeRoleSize;
			GameModList = gameModList;
			ShowModList = showModList;
		}
	}
	/// 如果JsonFormat 列表空跳过
    /// <summary>
    /// 死亡
    /// 死亡触发
    /// </summary>
	public struct Dead
	{
		/// <summary>
		/// 地图模式
		/// 数据源 => 系统字典类型 map_mod
		/// </summary>
		public int Mapmod { get; set; }
		/// <summary>
		/// 人数模式
		/// 数据源 => 系统字典类型 group_mod
		/// </summary>
		public int Groupmod { get; set; }
		/// <summary>
		/// 玩法模式
		/// 数据源 => 系统字典类型 game_mod
		/// </summary>
		public int Gamemod { get; set; }
		/// <summary>
		/// 显示模式
		/// 数据源 => 系统字典类型 show_mode
		/// </summary>
		public int Showmod { get; set; }
		/// <summary>
		/// 匹配模式
		/// 数据源 => 系统字典类型 match_mod
		/// </summary>
		public int Matchmod { get; set; }
		/// <summary>
		/// 存活时长
		/// 数据源 => 无
		/// </summary>
		public long At { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapmod"></param>
        /// <param name="groupmod"></param>
        /// <param name="gamemod"></param>
        /// <param name="showmod"></param>
        /// <param name="matchmod"></param>
        /// <param name="at"></param>
		public Dead( int mapmod, int groupmod, int gamemod, int showmod, int matchmod, long at )
		{
			Mapmod = mapmod;
			Groupmod = groupmod;
			Gamemod = gamemod;
			Showmod = showmod;
			Matchmod = matchmod;
			At = at;
		}
	}

    /// <summary>
    /// Json 类型解析函数
    /// </summary>
    public static class JsonTypeParser
    {
        /// <summary>
        /// 解析 PickUp
        /// </summary>
        public static PickUp ParsePickUp(string jsonData) {
            return JsonConvert.DeserializeObject<PickUp>(jsonData);
        }
        /// <summary>
        /// 解析 GetItemFromBox
        /// </summary>
        public static GetItemFromBox ParseGetItemFromBox(string jsonData) {
            return JsonConvert.DeserializeObject<GetItemFromBox>(jsonData);
        }
        /// <summary>
        /// 解析 Hurt
        /// </summary>
        public static Hurt ParseHurt(string jsonData) {
            return JsonConvert.DeserializeObject<Hurt>(jsonData);
        }
        /// <summary>
        /// 解析 ClubNews
        /// </summary>
        public static ClubNews ParseClubNews(string jsonData) {
            return JsonConvert.DeserializeObject<ClubNews>(jsonData);
        }
        /// <summary>
        /// 解析 Login
        /// </summary>
        public static Login ParseLogin(string jsonData) {
            return JsonConvert.DeserializeObject<Login>(jsonData);
        }
        /// <summary>
        /// 解析 Progress
        /// </summary>
        public static Progress ParseProgress(string jsonData) {
            return JsonConvert.DeserializeObject<Progress>(jsonData);
        }
        /// <summary>
        /// 解析 GiveLike
        /// </summary>
        public static GiveLike ParseGiveLike(string jsonData) {
            return JsonConvert.DeserializeObject<GiveLike>(jsonData);
        }
        /// <summary>
        /// 解析 Share
        /// </summary>
        public static Share ParseShare(string jsonData) {
            return JsonConvert.DeserializeObject<Share>(jsonData);
        }
        /// <summary>
        /// 解析 Focus
        /// </summary>
        public static Focus ParseFocus(string jsonData) {
            return JsonConvert.DeserializeObject<Focus>(jsonData);
        }
        /// <summary>
        /// 解析 UseCrate
        /// </summary>
        public static UseCrate ParseUseCrate(string jsonData) {
            return JsonConvert.DeserializeObject<UseCrate>(jsonData);
        }
        /// <summary>
        /// 解析 Intimacy
        /// </summary>
        public static Intimacy ParseIntimacy(string jsonData) {
            return JsonConvert.DeserializeObject<Intimacy>(jsonData);
        }
        /// <summary>
        /// 解析 Collect
        /// </summary>
        public static Collect ParseCollect(string jsonData) {
            return JsonConvert.DeserializeObject<Collect>(jsonData);
        }
        /// <summary>
        /// 解析 CompleteSeasonTask
        /// </summary>
        public static CompleteSeasonTask ParseCompleteSeasonTask(string jsonData) {
            return JsonConvert.DeserializeObject<CompleteSeasonTask>(jsonData);
        }
        /// <summary>
        /// 解析 PveLevelUp
        /// </summary>
        public static PveLevelUp ParsePveLevelUp(string jsonData) {
            return JsonConvert.DeserializeObject<PveLevelUp>(jsonData);
        }
        /// <summary>
        /// 解析 PveWeedOut
        /// </summary>
        public static PveWeedOut ParsePveWeedOut(string jsonData) {
            return JsonConvert.DeserializeObject<PveWeedOut>(jsonData);
        }
        /// <summary>
        /// 解析 PvePass
        /// </summary>
        public static PvePass ParsePvePass(string jsonData) {
            return JsonConvert.DeserializeObject<PvePass>(jsonData);
        }
        /// <summary>
        /// 解析 PveOver
        /// </summary>
        public static PveOver ParsePveOver(string jsonData) {
            return JsonConvert.DeserializeObject<PveOver>(jsonData);
        }
        /// <summary>
        /// 解析 Gift
        /// </summary>
        public static Gift ParseGift(string jsonData) {
            return JsonConvert.DeserializeObject<Gift>(jsonData);
        }
        /// <summary>
        /// 解析 PlayerBind
        /// </summary>
        public static PlayerBind ParsePlayerBind(string jsonData) {
            return JsonConvert.DeserializeObject<PlayerBind>(jsonData);
        }
        /// <summary>
        /// 解析 UseSkill
        /// </summary>
        public static UseSkill ParseUseSkill(string jsonData) {
            return JsonConvert.DeserializeObject<UseSkill>(jsonData);
        }
        /// <summary>
        /// 解析 BoutOver
        /// </summary>
        public static BoutOver ParseBoutOver(string jsonData) {
            return JsonConvert.DeserializeObject<BoutOver>(jsonData);
        }
        /// <summary>
        /// 解析 RecallLogin
        /// </summary>
        public static RecallLogin ParseRecallLogin(string jsonData) {
            return JsonConvert.DeserializeObject<RecallLogin>(jsonData);
        }
        /// <summary>
        /// 解析 StoreTurntableLottery
        /// </summary>
        public static StoreTurntableLottery ParseStoreTurntableLottery(string jsonData) {
            return JsonConvert.DeserializeObject<StoreTurntableLottery>(jsonData);
        }
        /// <summary>
        /// 解析 OnlyUpLevel
        /// </summary>
        public static OnlyUpLevel ParseOnlyUpLevel(string jsonData) {
            return JsonConvert.DeserializeObject<OnlyUpLevel>(jsonData);
        }
        /// <summary>
        /// 解析 Consume
        /// </summary>
        public static Consume ParseConsume(string jsonData) {
            return JsonConvert.DeserializeObject<Consume>(jsonData);
        }
        /// <summary>
        /// 解析 OnlyUpUnet
        /// </summary>
        public static OnlyUpUnet ParseOnlyUpUnet(string jsonData) {
            return JsonConvert.DeserializeObject<OnlyUpUnet>(jsonData);
        }
        /// <summary>
        /// 解析 TeachingStageTaskFinish
        /// </summary>
        public static TeachingStageTaskFinish ParseTeachingStageTaskFinish(string jsonData) {
            return JsonConvert.DeserializeObject<TeachingStageTaskFinish>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashKill
        /// </summary>
        public static GoldDashKill ParseGoldDashKill(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashKill>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashKillBoss
        /// </summary>
        public static GoldDashKillBoss ParseGoldDashKillBoss(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashKillBoss>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashBuyShop
        /// </summary>
        public static GoldDashBuyShop ParseGoldDashBuyShop(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashBuyShop>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashSaleShop
        /// </summary>
        public static GoldDashSaleShop ParseGoldDashSaleShop(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashSaleShop>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashDeliver
        /// </summary>
        public static GoldDashDeliver ParseGoldDashDeliver(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashDeliver>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashGameOver
        /// </summary>
        public static GoldDashGameOver ParseGoldDashGameOver(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashGameOver>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashFixItem
        /// </summary>
        public static GoldDashFixItem ParseGoldDashFixItem(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashFixItem>(jsonData);
        }
        /// <summary>
        /// 解析 Assemble
        /// </summary>
        public static Assemble ParseAssemble(string jsonData) {
            return JsonConvert.DeserializeObject<Assemble>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashAssignKill
        /// </summary>
        public static GoldDashAssignKill ParseGoldDashAssignKill(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashAssignKill>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashItemReached
        /// </summary>
        public static GoldDashItemReached ParseGoldDashItemReached(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashItemReached>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashLevel
        /// </summary>
        public static GoldDashLevel ParseGoldDashLevel(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashLevel>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashItemCollect
        /// </summary>
        public static GoldDashItemCollect ParseGoldDashItemCollect(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashItemCollect>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashInfrastructure
        /// </summary>
        public static GoldDashInfrastructure ParseGoldDashInfrastructure(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashInfrastructure>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardTotalCollect
        /// </summary>
        public static GoldDashOfferArewardTotalCollect ParseGoldDashOfferArewardTotalCollect(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardTotalCollect>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardOpenBox
        /// </summary>
        public static GoldDashOfferArewardOpenBox ParseGoldDashOfferArewardOpenBox(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardOpenBox>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardKillMonster
        /// </summary>
        public static GoldDashOfferArewardKillMonster ParseGoldDashOfferArewardKillMonster(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardKillMonster>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardKillPlayer
        /// </summary>
        public static GoldDashOfferArewardKillPlayer ParseGoldDashOfferArewardKillPlayer(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardKillPlayer>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardSurvey
        /// </summary>
        public static GoldDashOfferArewardSurvey ParseGoldDashOfferArewardSurvey(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardSurvey>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardEarnings
        /// </summary>
        public static GoldDashOfferArewardEarnings ParseGoldDashOfferArewardEarnings(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardEarnings>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardBoxEarnings
        /// </summary>
        public static GoldDashOfferArewardBoxEarnings ParseGoldDashOfferArewardBoxEarnings(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardBoxEarnings>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardSafeBoxCost
        /// </summary>
        public static GoldDashOfferArewardSafeBoxCost ParseGoldDashOfferArewardSafeBoxCost(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardSafeBoxCost>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArrewardSubmitItem
        /// </summary>
        public static GoldDashOfferArrewardSubmitItem ParseGoldDashOfferArrewardSubmitItem(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArrewardSubmitItem>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashTalentAlloc
        /// </summary>
        public static GoldDashTalentAlloc ParseGoldDashTalentAlloc(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashTalentAlloc>(jsonData);
        }
        /// <summary>
        /// 解析 BeatBeastcampCollect
        /// </summary>
        public static BeatBeastcampCollect ParseBeatBeastcampCollect(string jsonData) {
            return JsonConvert.DeserializeObject<BeatBeastcampCollect>(jsonData);
        }
        /// <summary>
        /// 解析 BeatBeastcampItem
        /// </summary>
        public static BeatBeastcampItem ParseBeatBeastcampItem(string jsonData) {
            return JsonConvert.DeserializeObject<BeatBeastcampItem>(jsonData);
        }
        /// <summary>
        /// 解析 Assists
        /// </summary>
        public static Assists ParseAssists(string jsonData) {
            return JsonConvert.DeserializeObject<Assists>(jsonData);
        }
        /// <summary>
        /// 解析 ConsumeBullet
        /// </summary>
        public static ConsumeBullet ParseConsumeBullet(string jsonData) {
            return JsonConvert.DeserializeObject<ConsumeBullet>(jsonData);
        }
        /// <summary>
        /// 解析 UseAction
        /// </summary>
        public static UseAction ParseUseAction(string jsonData) {
            return JsonConvert.DeserializeObject<UseAction>(jsonData);
        }
        /// <summary>
        /// 解析 Jump
        /// </summary>
        public static Jump ParseJump(string jsonData) {
            return JsonConvert.DeserializeObject<Jump>(jsonData);
        }
        /// <summary>
        /// 解析 Special
        /// </summary>
        public static Special ParseSpecial(string jsonData) {
            return JsonConvert.DeserializeObject<Special>(jsonData);
        }
        /// <summary>
        /// 解析 TotalCollectItem
        /// </summary>
        public static TotalCollectItem ParseTotalCollectItem(string jsonData) {
            return JsonConvert.DeserializeObject<TotalCollectItem>(jsonData);
        }
        /// <summary>
        /// 解析 DriveKill
        /// </summary>
        public static DriveKill ParseDriveKill(string jsonData) {
            return JsonConvert.DeserializeObject<DriveKill>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardConditionGameOver
        /// </summary>
        public static GoldDashOfferArewardConditionGameOver ParseGoldDashOfferArewardConditionGameOver(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardConditionGameOver>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOpenContainer
        /// </summary>
        public static GoldDashOpenContainer ParseGoldDashOpenContainer(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOpenContainer>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashHarvestItem
        /// </summary>
        public static GoldDashHarvestItem ParseGoldDashHarvestItem(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashHarvestItem>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardMechanism
        /// </summary>
        public static GoldDashOfferArewardMechanism ParseGoldDashOfferArewardMechanism(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardMechanism>(jsonData);
        }
        /// <summary>
        /// 解析 GoldDashOfferArewardNode
        /// </summary>
        public static GoldDashOfferArewardNode ParseGoldDashOfferArewardNode(string jsonData) {
            return JsonConvert.DeserializeObject<GoldDashOfferArewardNode>(jsonData);
        }
        /// <summary>
        /// 解析 GameOver
        /// </summary>
        public static GameOver ParseGameOver(string jsonData) {
            return JsonConvert.DeserializeObject<GameOver>(jsonData);
        }
        /// <summary>
        /// 解析 DestroyCarrier
        /// </summary>
        public static DestroyCarrier ParseDestroyCarrier(string jsonData) {
            return JsonConvert.DeserializeObject<DestroyCarrier>(jsonData);
        }
        /// <summary>
        /// 解析 DecItem
        /// </summary>
        public static DecItem ParseDecItem(string jsonData) {
            return JsonConvert.DeserializeObject<DecItem>(jsonData);
        }
        /// <summary>
        /// 解析 KillStreak
        /// </summary>
        public static KillStreak ParseKillStreak(string jsonData) {
            return JsonConvert.DeserializeObject<KillStreak>(jsonData);
        }
        /// <summary>
        /// 解析 TeamKill
        /// </summary>
        public static TeamKill ParseTeamKill(string jsonData) {
            return JsonConvert.DeserializeObject<TeamKill>(jsonData);
        }
        /// <summary>
        /// 解析 PartsKill
        /// </summary>
        public static PartsKill ParsePartsKill(string jsonData) {
            return JsonConvert.DeserializeObject<PartsKill>(jsonData);
        }
        /// <summary>
        /// 解析 WeedOut
        /// </summary>
        public static WeedOut ParseWeedOut(string jsonData) {
            return JsonConvert.DeserializeObject<WeedOut>(jsonData);
        }
        /// <summary>
        /// 解析 UseCannon
        /// </summary>
        public static UseCannon ParseUseCannon(string jsonData) {
            return JsonConvert.DeserializeObject<UseCannon>(jsonData);
        }
        /// <summary>
        /// 解析 AddHp
        /// </summary>
        public static AddHp ParseAddHp(string jsonData) {
            return JsonConvert.DeserializeObject<AddHp>(jsonData);
        }
        /// <summary>
        /// 解析 EventActionData
        /// </summary>
        public static EventActionData ParseEventActionData(string jsonData) {
            return JsonConvert.DeserializeObject<EventActionData>(jsonData);
        }
        /// <summary>
        /// 解析 CollectItem
        /// </summary>
        public static CollectItem ParseCollectItem(string jsonData) {
            return JsonConvert.DeserializeObject<CollectItem>(jsonData);
        }
        /// <summary>
        /// 解析 Revive
        /// </summary>
        public static Revive ParseRevive(string jsonData) {
            return JsonConvert.DeserializeObject<Revive>(jsonData);
        }
        /// <summary>
        /// 解析 EventConditionData
        /// </summary>
        public static EventConditionData ParseEventConditionData(string jsonData) {
            return JsonConvert.DeserializeObject<EventConditionData>(jsonData);
        }
        /// <summary>
        /// 解析 EventSubjectData
        /// </summary>
        public static EventSubjectData ParseEventSubjectData(string jsonData) {
            return JsonConvert.DeserializeObject<EventSubjectData>(jsonData);
        }
        /// <summary>
        /// 解析 EventTimeData
        /// </summary>
        public static EventTimeData ParseEventTimeData(string jsonData) {
            return JsonConvert.DeserializeObject<EventTimeData>(jsonData);
        }
        /// <summary>
        /// 解析 UseItem
        /// </summary>
        public static UseItem ParseUseItem(string jsonData) {
            return JsonConvert.DeserializeObject<UseItem>(jsonData);
        }
        /// <summary>
        /// 解析 Move
        /// </summary>
        public static Move ParseMove(string jsonData) {
            return JsonConvert.DeserializeObject<Move>(jsonData);
        }
        /// <summary>
        /// 解析 KillAppointObject
        /// </summary>
        public static KillAppointObject ParseKillAppointObject(string jsonData) {
            return JsonConvert.DeserializeObject<KillAppointObject>(jsonData);
        }
        /// <summary>
        /// 解析 AppointObjectHert
        /// </summary>
        public static AppointObjectHert ParseAppointObjectHert(string jsonData) {
            return JsonConvert.DeserializeObject<AppointObjectHert>(jsonData);
        }
        /// <summary>
        /// 解析 OverCollectItem
        /// </summary>
        public static OverCollectItem ParseOverCollectItem(string jsonData) {
            return JsonConvert.DeserializeObject<OverCollectItem>(jsonData);
        }
        /// <summary>
        /// 解析 WinEquipWeapon
        /// </summary>
        public static WinEquipWeapon ParseWinEquipWeapon(string jsonData) {
            return JsonConvert.DeserializeObject<WinEquipWeapon>(jsonData);
        }
        /// <summary>
        /// 解析 Kill
        /// </summary>
        public static Kill ParseKill(string jsonData) {
            return JsonConvert.DeserializeObject<Kill>(jsonData);
        }
        /// <summary>
        /// 解析 Dead
        /// </summary>
        public static Dead ParseDead(string jsonData) {
            return JsonConvert.DeserializeObject<Dead>(jsonData);
        }
    }
}