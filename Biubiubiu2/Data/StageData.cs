using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class StageData {
        public enum GameWorldLayerType : byte {
            None = 0,
            Base = 1,
            Character = 2,
            Item = 3,
            Ability = 4,
            AI = 5,
        }

        public enum LoadEnum {
            None,
            Room,
            Game,
            RoomUI,
            WarUI,
            AddServerGameScene,
            AddClientGameScene,
            Connect,
            LoginWar,
            ChangeQuality,
            SendLoginInfo,
            SetLookRole,
            ConnectGameServer,
            DisconnectGameServer,
        }

        public enum StageType {
            Room,
            ClientGame,
            ServerGame,
        }

        public const string GAME = "Game";
        public const string ROOM = "Room";
        public const string GoldDash_01_Remnant = "GoldDash_01_Remnant";
        public const string GoldDash_02_Remnant = "GoldDash_02_Remnant";
        public const string GoldDash_03_Remnant = "GoldDash_03_Remnant";
        public const string BeastCamp_Tree = "BeastCamp_Tree";
        public const string BeastCamp_Fire = "BeastCamp_Fire";
        public const string BeastCamp_Water = "BeastCamp_Water";
        public const string BeastCamp_Sand = "BeastCamp_Sand";
        public const string GoldDash_00_Tutorial = "GoldDash_00_Tutorial";
        public const string FastGoldDash_SORemnant_01 = "GoldDash_01_Remnant";
        public const string FastGoldDash_SOForest_02 = "GoldDash_02_Remnant";
        public const string FastGoldDash_SOJokerBase_03 = "GoldDash_03_Remnant";
        public static float SceneWidth = 0f;
        public static float SceneHeight = 0f;

        public static StageType NowStageType {
            get {
                return nowStageType;
            }
        }
        private static StageType nowStageType = StageType.Room;
        public static void SetStageType(StageType stageType) {
            nowStageType = stageType;
        }

        public static List<LoadEnum> GetStageLoadList(StageType stageType) {
            switch (stageType) {
                case StageType.ClientGame:
                    return new List<LoadEnum> {
                        // LoadEnum.DisconnectGameServer,
                        LoadEnum.Game,
                        LoadEnum.Connect,
                        LoadEnum.WarUI,
                        LoadEnum.AddClientGameScene,
                        LoadEnum.LoginWar,
                        LoadEnum.ChangeQuality,
                        LoadEnum.SendLoginInfo,
                        LoadEnum.SetLookRole,
                    };
                case StageType.ServerGame:
                    return new List<LoadEnum> {
                        LoadEnum.Game, LoadEnum.Connect, LoadEnum.AddServerGameScene,
                    };
                case StageType.Room:
                    return new List<LoadEnum> {
                       LoadEnum.Room, LoadEnum.RoomUI, 
                    };
            }
            Debug.LogError("没有对应场景需要的加载列表");
            return null;
        }

        public static string GetServerStage(string clientStage) {
            return $"Server{clientStage}";
        }
        
        public static string GetStateSign(LoadEnum state) {
            var info = "";
            switch (state) {
                case LoadEnum.Room:
                    info = "进入大厅";
                    break;
                case LoadEnum.Game:
                    info = "进入游戏";
                    break;
                case LoadEnum.RoomUI:
                    info = "加载大厅UI";
                    break;
                case LoadEnum.WarUI:
                    info = "加载战斗UI";
                    break;
                case LoadEnum.AddServerGameScene:
                    info = "加载服务器游戏场景";
                    break;
                case LoadEnum.AddClientGameScene:
                    info = "加载客户端游戏场景";
                    break;
                case LoadEnum.Connect:
                    info = "连接服务器";
                    break;
                case LoadEnum.LoginWar:
                    info = "登录战斗";
                    break;
                case LoadEnum.SendLoginInfo:
                    info = "发送登录信息";
                    break;
                case LoadEnum.SetLookRole:
                    info = "设置观战角色";
                    break;
                case LoadEnum.ConnectGameServer:
                    info = "连接游戏服务器";
                    break;
                case LoadEnum.DisconnectGameServer:
                    info = "断开游戏服务器";
                    break;
                default:
                    info = state.ToString();
                    break;
            }
            return info;
        }
    }
}