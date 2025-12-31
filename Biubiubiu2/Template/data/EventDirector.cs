// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 事件导演数据
    /// </summary>
	public struct EventDirector
	{
		/// <summary>
		/// ID
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
		/// 模式id
		/// </summary>
		public readonly int[] MatchIds { get; }
		/// <summary>
		/// 时间类型逻辑关系
		/// </summary>
		public readonly byte TimeLogicType { get; }
		/// <summary>
		/// 时间段列表
		/// </summary>
		public readonly EventTimeData[] Times { get; }
		/// <summary>
		/// 事件主体
		/// </summary>
		public readonly EventSubjectData Subject { get; }
		/// <summary>
		/// 进入条件逻辑类型
		/// </summary>
		public readonly byte EnterConditionLogicType { get; }
		/// <summary>
		/// 进入条件
		/// </summary>
		public readonly EventConditionData[] EnterConditions { get; }
		/// <summary>
		/// 是否随机执行动作
		/// </summary>
		public readonly bool IsActionRandom { get; }
		/// <summary>
		/// 进入事件提示类型
		/// </summary>
		public readonly byte EnterActionTipType { get; }
		/// <summary>
		/// 退出事件提示类型
		/// </summary>
		public readonly byte QuitActionTipType { get; }
		/// <summary>
		/// 执行动作
		/// </summary>
		public readonly EventActionData[] Actions { get; }
		/// <summary>
		/// 退出条件逻辑类型
		/// </summary>
		public readonly byte QuitConditionLogicType { get; }
		/// <summary>
		/// 退出条件
		/// </summary>
		public readonly EventConditionData[] QuitConditions { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sign"></param>
        /// <param name="matchIds"></param>
        /// <param name="timeLogicType"></param>
        /// <param name="times"></param>
        /// <param name="subject"></param>
        /// <param name="enterConditionLogicType"></param>
        /// <param name="enterConditions"></param>
        /// <param name="isActionRandom"></param>
        /// <param name="enterActionTipType"></param>
        /// <param name="quitActionTipType"></param>
        /// <param name="actions"></param>
        /// <param name="quitConditionLogicType"></param>
        /// <param name="quitConditions"></param> )
		public EventDirector( int id, string name, string sign, int[] matchIds, byte timeLogicType, EventTimeData[] times, EventSubjectData subject, byte enterConditionLogicType, EventConditionData[] enterConditions, bool isActionRandom, byte enterActionTipType, byte quitActionTipType, EventActionData[] actions, byte quitConditionLogicType, EventConditionData[] quitConditions )
		{
			Id = id;
			Name = name;
			Sign = sign;
			MatchIds = matchIds;
			TimeLogicType = timeLogicType;
			Times = times;
			Subject = subject;
			EnterConditionLogicType = enterConditionLogicType;
			EnterConditions = enterConditions;
			IsActionRandom = isActionRandom;
			EnterActionTipType = enterActionTipType;
			QuitActionTipType = quitActionTipType;
			Actions = actions;
			QuitConditionLogicType = quitConditionLogicType;
			QuitConditions = quitConditions;
		}
	}

    /// <summary>
    /// EventDirectorSet that holds all the table data
    /// </summary>
    public partial class EventDirectorSet
    {
        public static readonly EventDirector[] Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        static EventDirectorSet()
        {
            Data = new EventDirector[]
            {
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 EventDirector
		/// </summary>
		/// <param name="Id"></param>
		public static EventDirector GetEventDirectorById( int id )
		{
			foreach (EventDirector data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(EventDirector);
		}

		/// <summary>
		/// 根据指定条件判断单个 EventDirector 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasEventDirectorById( int id )
		{
			foreach (EventDirector data in Data)
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
