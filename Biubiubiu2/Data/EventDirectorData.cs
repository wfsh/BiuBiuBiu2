using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.Data {
    /// <summary>
    /// Time       什么时候触发	控制时间约束
    /// Subject	   谁在参与事件	提供上下文与作用域
    /// Condition  条件判断	    控制逻辑约束
    /// Action	   触发后做什么	定义结果行为
    /// </summary>
    public class EventDirectorData {
        public enum ShowTipType {
            None,
            DialogTip, // 对话提示
            ToastTip, // 软提示
        }
        public enum LogicType {
            And,  // 都是 True 就是 True
            Or, //  有一个是 True 就是 True
            Random, // 随机一个是 True 就是 True
        }
        public enum TimeType {
            AnyTime,          // 永远生效
            GameStartTime,  // 游戏经过时间段
            RoundStartTime, // 当前回合时间段
            RoundIndex,       // 指定回合次数
        }

        public struct TimeData {
            public TimeType Type;
            public float StartValue;
            public float EndValue;
            public string Value;
        }
        
        public enum SubjectType {
            None,
            World, // 全局
            Player, // 单个玩家
            PlayerAndPlayerAI, // 单个玩家+ AI
            AI, // 单个怪物
            PlayerGroup, // 玩家组
            PlayerAndPlayerAIGroup, // 玩家和 AI 组
            AIGroup, // 怪物组
            PlayerTeam, // 玩家队伍
            GPOMId, // 指定模板 ID 的 GPO
            GPOMTypeId, // 指定模板类型 ID 的 GPO
            GPOMIdGroup, // 指定模板 ID 的 GPO
        }

        public struct SubjectData {
            public SubjectType Type;
            public string Value;
        }
        
        public enum ConditionType {
            None,
            KillMTypeAI, // 击杀怪物(类型)
            KillAI, // 击杀怪物
            KillRangePointAI, // 击杀指定位置范围的怪物
            FireCount, // 开火次数
            SlideCount, // 滑铲次数
            HasItem, // 拥有物品
            HasBoss, // 拥有 Boss
            WaitTime, // 等待时间
            GPOMDead, // 指定模板 ID 的 GPO 死亡
            GPOMTypeDead, // 指定模板类型 ID 的 GPO 死亡
            PlayerEnterArea, // 玩家进入区域
        }
        public enum CompareType {
            Equal,  // 包含 = 
            NotEqual,  // 不包含 ！=
            GreaterEquip,  // 大于等于
            Less, // 小于
        }

        public struct ConditionData {
            public ConditionType Type;
            public CompareType CompareType;
            public string Value;
        }

        public enum ActionType {
            None,
            SpawnAI, // 生成怪物
            SpawnTriggerGPOTeamAI, // 生成召唤怪物
            GiveItem, // 给予物品
            UpHpPlayer, // 治疗玩家
            DamagePlayer, // 伤害玩家
            SpawnSummerJokerStone, // 生成小丑石碑
            ATKUpRate, // 提升攻击力百分比
        }

        public struct ActionData {
            public ActionType Type;
            public float WaitTime; // 等待时间
            public string Value;
            public int UseLimitCount; // 使用次数，-1 表示无限次
        }
        
        public struct Data {
            public int EventId;
            public string EventName;
            public string EventSign;
            public int[] MatchIds;
            // 时间段
            public LogicType TimeLogicType;
            public TimeData[] Times; 
            // 事件主体
            public SubjectData Subject;
            // 进入条件
            public LogicType EnterConditionLogicType;
            public ConditionData[] EnterConditions;
            // 执行动作
            public bool IsActionRandom; 
            public ShowTipType EnterActionTipType; // 进入事件提示类型
            public ShowTipType QuitActionTipType; // 退出事件提示类型
            public ActionData[] Actions; 
            // 退出条件
            public LogicType QuitConditionLogicType;
            public ConditionData[] QuitConditions;
        }
        
        public static List<Data> EventList = new List<Data> {
            new Data {
                EventId = 1,
                EventName = "小丑石碑生成",
                MatchIds = new int[] {
                    (int)MatchMod.GoldDash3, (int)MatchMod.GoldDash3A, (int)MatchMod.GoldDash3B
                },
                TimeLogicType = LogicType.And, // 时间和模式是与的关系
                Times = new TimeData[] {
                    new TimeData {
                        Type = TimeType.AnyTime,
                    }
                },
                Subject = new SubjectData {
                    Type = SubjectType.World,
                },
                EnterActionTipType = ShowTipType.None,
                QuitActionTipType = ShowTipType.None,
                EnterConditionLogicType = LogicType.And,
                EnterConditions = new ConditionData[]{},
                Actions = new [] {
                    new ActionData {
                        Type = ActionType.SpawnSummerJokerStone,
                        Value = $"{GPOM_AceJokerSet.Id_AceJoker}&{GPOM_AceJokerSet.Id_GoldJoker}&0.18;51.18;-10.96&3.6&{GpoTypeSet.Id_JokerUav}&4&60", // 触发一次
                        UseLimitCount = 1,
                    },
                }
            }
        };
        
        public static List<Data> GetEventList(int matchId) {
            var list = new List<Data>();
            foreach (var data in EventList) {
                if (data.MatchIds.Length <= 0) {
                    list.Add(data);
                } else {
                    foreach (var id in data.MatchIds) {
                        if (id == matchId) {
                            list.Add(data);
                            break;
                        }
                    }
                }
            }
            return list;
        }
        public static Data GetEventData(int eventId) {
            foreach (var data in EventList) {
                if (data.EventId == eventId) {
                    return data;
                }
            }
            return default;
        }
    }
}