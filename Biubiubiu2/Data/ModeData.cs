using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class ModeData {
        public const int Id_ModeExplore = 10000;
        public const int Id_ModeBoss = 10001;
        public const int Id_Mode1V1 = 10003;
        public const int Id_Mode5V5 = 10004;
         
        public static int ModeId {
            get;
            private set;
        }
        
        public static int MatchId {
            get;
            private set;
        }

        public static int SceneId {
            get;
            private set;
        }

        public static ModeEnum PlayMode {
            get;
            private set;
        } = ModeEnum.None;

        public static GameStateEnum PlayGameState {
            get;
            private set;
        } = GameStateEnum.None;

        public static WeaponSourceEnum WeaponSource {
            get;
            private set;
        }
        
        public static bool IsIntoMode {
            get;
            private set;
        }
        
        public static Data PlayData {
            get;
            private set;
        }
        
        // 游戏状态
        public enum GameStateEnum {
            None,
            Wait,
            WaitStartDownTime,
            WaitRoundStart,
            RoundStart,
            RoundEnd,
            WaitNextRound,
            WaitModeOver,
            ModeOver,
            SaveReport,
            QuitApp,
        }

        // 模式类型
        public enum ModeEnum {
            None = 0,
            ModeExplore = 1,  // 探索搜集模式
            ModeBoss = 2,   // Boss 模式
            Mode1V1 = 3,   // 1V1对战模式
            Mode2V2 = 4,   // 2V2对战模式
            Mode5V5ReLife = 5,   // 5V5 复活模式
            Mode5V5ReLife2 = 6,   // 5V5 复活模式2
            SausageGoldDash = 7,   // 香肠摸金模式
            SausageBeastCamp = 8,   // 香肠国服猛兽营
            SausageFastGoldDash = 9,   // 香肠街机摸金团竞
        }

        public static bool IsSausageMode() {
            return PlayMode == ModeEnum.SausageGoldDash ||
                   PlayMode == ModeEnum.SausageBeastCamp ||
                   PlayMode == ModeEnum.SausageFastGoldDash;
        }

        // MessageType
        public enum MessageEnum {
            WeaponKillRole = 1, // 击杀对手
            SuperWeaponKillRole = 2, // 超级武器击杀对手
            UseItem = 3, // 使用道具
        }

        // 武器来源
        public enum WeaponSourceEnum {
            Room, // 战场大厅
            RandomRedWeapon, // 随机红色武器
        }
        // 模式登录状态
        public enum ModeLoginState {
            Success = 1,
            MaxRoleNum = 2,
            ModeError = 3,
            TeamIdNull = 4,
            TeamFull = 5,  // 队伍已满
            TeamRoleFull = 6, // 队伍角色已满
            GameStart = 7, // 游戏已经开始
            ModeCharacterListNull = 8, // 模式角色列表为空
            NetworkVersionError = 9, // 网络版本错误
            WarIdError = 10, // 战场ID错误
            WarEnd = 11, // 战斗结束
        }
        
        // 获取积分渠道
        public enum GetScoreChannelEnum {
            FinalSurvivalTeam, // 最终存活队伍
            KillRole, // 击杀对手
            KillRoleAI, // 击杀对手 AI
            KillBoss, // 击杀 Boss
            KillMonster, // 击杀怪物
            KillMasterMonster, // 击杀宠物怪物
            NaturalResource, // 采集资源
        }
        // 回合获胜条件
        public enum RoundWinStateEnum {
            PersonalScoreTop = 1, // 个人积分 最高
            TeamScoreTop = 2, // 队伍积分 最高
        }
        
        // 模式获胜条件
        public enum ModeWinStateEnum {
            PersonalScoreTop = 1, // 个人总积分 最高
            TeamScoreTop = 2, // 队伍总积分 最高
            RoundWinTop = 3, // 回合获胜次数最高
        }
        
        // 模式数据
        public class Data {
            public int Id;  // 模式 Id
            public string ModeName; // 模式名称
            public int MaxRoleNum; // 模式可容纳的总人数
            public int MaxRoleNumPerTeam; // 一队最多有多少人
            public int MinStartModeTeamNum; // 最小开始队伍数量
            public int MinStartPlayCharacterNum;// 最小开启人数
            public float StartModeDownTime; // 达到最小开始队伍数量后开始倒计时
            public float StartRoundDownTime; // 每回合开始时候的倒计时
            public float WaitNextRoundTime; // 等待下回合开始的时间
            public float WaitModeOverTime; // 等待模式结算时间
            public int RoundTime; // 每回合持续时间 -1 为无限时间 (秒)
            public int WinScore; // 获胜需要的分数
            public int ModeWinRoundCount; // 模式胜利回合次数
            public int MaxRoundCount; // 最大回合数
            public bool PerRoundRandWeapon; // 是否每回合随机武器
            public ModeEnum Mode; // 模式类型
            public RoundWinStateEnum RoundWinState; // 回合胜利方式
            public ModeWinStateEnum ModeWinState; // 模式胜利方式
            public ScoreChannelData[] ScoreChannelDatas; // 获取积分渠道
        }
        
        public class ScoreChannelData {
            public GetScoreChannelEnum Channel; // 获取积分渠道
            public int Score; // 获取的积分
        }
        public static List<Data> Datas;
        
        public static void Init() {
            if (Datas != null) {
                return;
            }
            Datas = new List<Data>(GameModeSet.Data.Length);
            foreach (var gameMode in GameModeSet.Data) {
                var modeData = new Data {
                    Id = gameMode.Id,
                    ModeName = gameMode.Desc,
                    MaxRoleNum = gameMode.MaxRoleNum,
                    MinStartPlayCharacterNum = gameMode.MinStartPlayCharacterNum,
                    MaxRoleNumPerTeam = gameMode.MaxRoleNumPerTeam,
                    MinStartModeTeamNum = gameMode.MinStartModeTeamNum,
                    StartModeDownTime = gameMode.StartModeDownTime,
                    StartRoundDownTime = gameMode.StartRoundDownTime,
                    WaitNextRoundTime = gameMode.WaitNextRoundTime,
                    RoundTime = gameMode.RoundTime,
                    WinScore = gameMode.WinScore,
                    ModeWinRoundCount = gameMode.ModeWinRoundCount,
                    MaxRoundCount = gameMode.MaxRoundCount,
                    WaitModeOverTime = gameMode.WaitModeOverTime,
                    PerRoundRandWeapon = gameMode.PerRoundRandWeapon
                };
                // 非模板数据
                modeData.RoundWinState = RoundWinStateEnum.TeamScoreTop;
                modeData.ModeWinState = ModeWinStateEnum.RoundWinTop;
                modeData.ScoreChannelDatas = new[] {
                    new ScoreChannelData {
                        Channel = GetScoreChannelEnum.KillRole, Score = 1,
                    },
                    new ScoreChannelData {
                        Channel = GetScoreChannelEnum.KillRoleAI, Score = 1,
                    },
                };

                Datas.Add(modeData);
            }
            AddTestMode();
        }


        public static void SetGameState(GameStateEnum state) {
            PlayGameState = state;
        }


        public static void SetPlayMode(ModeEnum mode) {
            PlayMode = mode;
        }
        
        public static void SetMatchId(int matchId) {
            MatchId = matchId;
        }

        public static void SetIsIntoMode(bool isTrue) {
            IsIntoMode = isTrue;
        }

        public static bool SetPlayMode(int modeId) {
            var playData = GetModeData(modeId);
            if (playData == null) {
                PlayMode = ModeEnum.None;
                PlayData = null;
                return false;
            }
            ModeId = modeId;
            PlayMode = playData.Mode;
            PlayData = playData;
            Debug.Log(" SetPlayMode:" + PlayMode + " ModeId:" + modeId);
            // SentrySDKAgent.Instance.SetMode(PlayMode.ToString());
            return true;
        }
        
        public static void ResetModeData() {
            ModeId = 0;
            MatchId = 0;
            SceneId = 0;
            PlayMode = ModeEnum.None;
            PlayGameState = GameStateEnum.None;
            WeaponSource = WeaponSourceEnum.Room;
            IsIntoMode = false;
            PlayData = null;
        }

        public static void SetSceneId(int sceneId) {
            SceneId = sceneId;
        }

        public static void SetWeaponSource(bool randomWeapon) {
            WeaponSource = randomWeapon ? WeaponSourceEnum.RandomRedWeapon : WeaponSourceEnum.Room;
        }

        public static Data GetModeData(int modeId) {
            for (int i = 0; i < Datas.Count; i++) {
                var data = Datas[i];
                if (data.Id == modeId) {
                    return data;
                }
            }
#if UNITY_EDITOR
            Debug.LogError("2 代架构缺少模式数据:" + modeId);
#endif
            return null;
        }

        private static void AddTestMode() {
            Datas.Add(new Data {
                Id = Id_ModeExplore,
                Mode = ModeEnum.ModeExplore, MaxRoleNum = 20, MinStartPlayCharacterNum = 1, MaxRoleNumPerTeam = 1, MinStartModeTeamNum = 1,
                StartModeDownTime = 0f, StartRoundDownTime = 0f, WaitNextRoundTime = 0f,RoundTime = 600,
                RoundWinState = RoundWinStateEnum.PersonalScoreTop, WinScore = -1,
                ModeWinRoundCount = 1, MaxRoundCount = 1, ModeWinState = ModeWinStateEnum.RoundWinTop, WaitModeOverTime = 5f, PerRoundRandWeapon = false,
                ScoreChannelDatas = new ScoreChannelData[0]
            });
            Datas.Add(new Data() {
                Id = Id_ModeBoss,
                Mode = ModeEnum.ModeBoss, MaxRoleNum = 4, MinStartPlayCharacterNum = 4, MaxRoleNumPerTeam = 4, MinStartModeTeamNum = 1,
                StartModeDownTime = 100f, StartRoundDownTime = 5f, WaitNextRoundTime = 0f, RoundTime = 300,
                RoundWinState = RoundWinStateEnum.TeamScoreTop, WinScore = 1,
                ModeWinRoundCount = 1, MaxRoundCount = 1, ModeWinState = ModeWinStateEnum.RoundWinTop, WaitModeOverTime = 5f, PerRoundRandWeapon = false,
                ScoreChannelDatas = new []{
                    new ScoreChannelData() {
                        Channel = GetScoreChannelEnum.KillBoss, Score = 1,
                    },
                }
            });
            Datas.Add(new Data {
                Id = (int)GameMod.GoldDash, Mode = ModeEnum.SausageGoldDash,
            });
            Datas.Add(new Data {
                Id = (int)GameMod.BeatCamp, Mode = ModeEnum.SausageBeastCamp,
            });
            Datas.Add(new Data {
                Id = (int)GameMod.TkvElimination, Mode = ModeEnum.SausageFastGoldDash,
            });
            Datas.Add(new Data() {
                Id = Id_Mode1V1,
                Mode = ModeEnum.Mode1V1, MaxRoleNum = 2, MinStartPlayCharacterNum = 2, MaxRoleNumPerTeam = 1, MinStartModeTeamNum = 1, 
                StartModeDownTime = 5f, StartRoundDownTime = 3f, WaitNextRoundTime = 3f, RoundTime = 300,  
                RoundWinState = RoundWinStateEnum.TeamScoreTop, WinScore = 1, 
                ModeWinRoundCount = 2, ModeWinState = ModeWinStateEnum.RoundWinTop, WaitModeOverTime = 5f,
                ScoreChannelDatas = new []{
                    new ScoreChannelData() {
                        Channel = GetScoreChannelEnum.FinalSurvivalTeam, Score = 1,
                    },
                }
            });
            Datas.Add(new Data() {
                Id = Id_Mode5V5,
                Mode = ModeEnum.Mode5V5ReLife, MaxRoleNum = 10,  MinStartPlayCharacterNum = 10, MaxRoleNumPerTeam = 5, MinStartModeTeamNum = 1, 
                StartModeDownTime = 10f, StartRoundDownTime = 0f, WaitNextRoundTime = 4f, RoundTime = 300,  
                RoundWinState = RoundWinStateEnum.TeamScoreTop, WinScore = 60, 
                ModeWinRoundCount = 1, ModeWinState = ModeWinStateEnum.RoundWinTop, WaitModeOverTime = 5f,
                ScoreChannelDatas = new []{
                    new ScoreChannelData() {
                        Channel = GetScoreChannelEnum.KillRole, Score = 1,
                    },
                    new ScoreChannelData() {
                        Channel = GetScoreChannelEnum.KillRoleAI, Score = 1,
                    },
                },
            });
        }

        public static List<GameMatch> GetAllGameMatches() {
            var list = GameMatchSet.Data.ToList();
            list.Insert(0, new GameMatch(10000, 10000, "探索模式", "ModeExplore", SceneData.ShipwreckBay_Test,"探索模式"));
            return list;
        }

        public static Data GetModeDataForModeEnum(ModeEnum mode) {
            for (int i = 0; i < Datas.Count; i++) {
                var data = Datas[i];
                if (data.Mode == mode) {
                    return data;
                }
            }
            Debug.LogError("缺少模式数据:" + mode);
            return null;
        }
    }
}