using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.Data {
    public class SceneData {
        public enum ElementEnum {
            None = 0,
            Ability_UpHp = 1,
            AbilityEffect_SpeedRatio = 2,
        }
        
        public const int ShipwreckBay_Test = 10000;
        public const int Id_OldCity = 10001;
        public const int Id_BarnFight = 10002;
        public const int Id_Street = 10003;
        public const int Id_Village = 10004;
        public const int Id_Square = 10005;
        public const int Id_Deck = 10006;
        public const int Scene_Test = 99999; // 测试场景使用
        public const int GoldDash_SOGoldDash_01_Remnant_01 = 73;
        public const int GoldDash_SOGoldDash_01_Remnant_01A = 106;
        public const int GoldDash_SOGoldDash_01_Remnant_01B = 108;
        public const int GoldDash_SOGoldDash_02_Forest_02 = 74;
        public const int GoldDash_SOGoldDash_02_Forest_02A = 107;
        public const int GoldDash_SOGoldDash_02_Forest_02B = 109;
        public const int GoldDash_SOGoldDash_03_JokerBase_03A = 119;
        public const int GoldDash_SOGoldDash_03_JokerBase_03B = 120;
        public const int GoldDash_SOGoldDash_03_JokerBase_03 = 121;
        public const int BeastCamp_Tree = 1003;
        public const int BeastCamp_Fire = 1004;
        public const int BeastCamp_Water = 1005;
        public const int BeastCamp_Sand = 1006;
        public const int GoldDash_SOGoldDash_00_Tutorial_00 = 122;
        public const int FastGoldDash_SORemnant_01 = 1010;
        public const int FastGoldDash_SOForest_02 = 1011;
        public const int FastGoldDash_SOJokerBase_03 = 1012;
        
        private static SceneConfig testSceneConfig;
        public static SceneConfig TestSceneConfig {
            get {
                return testSceneConfig;
            }
        }

        public class Data {
            public int ID;
            public string StageSign;
            public string ElementConfig;
        }

        public static List<Data> datas = new List<Data>() {
            new Data() {
                ID = ShipwreckBay_Test, StageSign = "ShipwreckBay", ElementConfig = "ShipwreckBay_Test",
            },
            new Data() {
                ID = Id_OldCity, StageSign = "LevelTest_02", ElementConfig = "LevelTest_02",
            },
            new Data() {
                ID = Id_BarnFight, StageSign = "LevelTest_03", ElementConfig = "LevelTest_03",
            },
            new Data() {
                ID = Id_Street, StageSign = "Map1V1_01_Street", ElementConfig = "Map1V1_01_Street",
            },
            new Data() {
                ID = Id_Village, StageSign = "Map1V1_02_Village", ElementConfig = "Map1V1_02_Village",
            },
            new Data() {
                ID = Id_Square, StageSign = "MapTDM_04_Square", ElementConfig = "MapTDM_04_Square",
            },
            new Data() {
                ID = Id_Deck, StageSign = "MapTDM_05_Deck", ElementConfig = "MapTDM_05_Deck",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_01_Remnant_01, StageSign = StageData.GoldDash_01_Remnant, ElementConfig = "GoldDash_SOGoldDash_01_Remnant_01",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_01_Remnant_01A, StageSign = StageData.GoldDash_01_Remnant, ElementConfig = "GoldDash_SOGoldDash_01_Remnant_01A",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_01_Remnant_01B, StageSign = StageData.GoldDash_01_Remnant, ElementConfig = "GoldDash_SOGoldDash_01_Remnant_01B",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_02_Forest_02, StageSign = StageData.GoldDash_02_Remnant, ElementConfig = "GoldDash_SOGoldDash_02_Forest_02",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_02_Forest_02A, StageSign = StageData.GoldDash_02_Remnant, ElementConfig = "GoldDash_SOGoldDash_02_Forest_02A",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_02_Forest_02B, StageSign = StageData.GoldDash_02_Remnant, ElementConfig = "GoldDash_SOGoldDash_02_Forest_02B",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_02_Forest_02B, StageSign = StageData.GoldDash_02_Remnant, ElementConfig = "GoldDash_SOGoldDash_02_Forest_02B",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_02_Forest_02B, StageSign = StageData.GoldDash_02_Remnant, ElementConfig = "GoldDash_SOGoldDash_02_Forest_02B",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_03_JokerBase_03A, StageSign = StageData.GoldDash_03_Remnant, ElementConfig = "GoldDash_SOGoldDash_03_JokerBase_03A",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_03_JokerBase_03B, StageSign = StageData.GoldDash_03_Remnant, ElementConfig = "GoldDash_SOGoldDash_03_JokerBase_03B",
            },
            new Data() {
                ID = GoldDash_SOGoldDash_03_JokerBase_03, StageSign = StageData.GoldDash_03_Remnant, ElementConfig = "GoldDash_SOGoldDash_03_JokerBase_03",
            },
            new Data() {
                ID = BeastCamp_Tree, StageSign = StageData.BeastCamp_Tree, ElementConfig = "BeastCamp_Tree",
            },
            new Data() {
                ID = BeastCamp_Fire, StageSign = StageData.BeastCamp_Fire, ElementConfig = "BeastCamp_Fire",
            },
            new Data() {
                ID = BeastCamp_Water, StageSign = StageData.BeastCamp_Water, ElementConfig = "BeastCamp_Water",
            },
            new Data() {
                ID = BeastCamp_Sand, StageSign = StageData.BeastCamp_Sand, ElementConfig = "BeastCamp_Sand",
            },
            new Data() {
            	ID = GoldDash_SOGoldDash_00_Tutorial_00, StageSign = StageData.GoldDash_00_Tutorial, ElementConfig = "GoldDash_SOGoldDash_00_Tutorial_00",	
            },
            new Data() {
                ID = FastGoldDash_SORemnant_01, StageSign = StageData.FastGoldDash_SORemnant_01, ElementConfig = "FastGoldDash_SOGoldDash_01_Remnant_01",
            },
            new Data() {
                ID = FastGoldDash_SOForest_02, StageSign = StageData.FastGoldDash_SOForest_02, ElementConfig = "FastGoldDash_SOGoldDash_02_Forest_02",
            },
            new Data() {
                ID = FastGoldDash_SOJokerBase_03, StageSign = StageData.FastGoldDash_SOJokerBase_03, ElementConfig = "FastGoldDash_SOGoldDash_03_JokerBase_03",
            },
        };

        public static void SetSceneTestData(string stageSign, string elementConfig, SceneConfig sceneConfig) {
            datas.Add(new Data {
                ID = Scene_Test, StageSign = stageSign, ElementConfig = elementConfig
            });
            testSceneConfig = sceneConfig;
        }

        public static Data Get(int id) {
            foreach (var data in datas) {
                if (data.ID == id) {
                    return data;
                }
            }
            return null;
        }
        
        public static bool HasSceneData(int id) {
            if (id <= 0) {
                return false;
            }
            foreach (var data in datas) {
                if (data.ID == id) {
                    return true;
                }
            }
            return false;
        }

        public static bool StartSceneTest(out string sceneConfigPath) {
            sceneConfigPath = "";
#if !UNITY_EDITOR
            return false;
#endif
            var testVar = PlayerPrefs.GetString("SceneEditorTest");
            if (string.IsNullOrEmpty(testVar)) {
                return false;
            }
            sceneConfigPath = Path.GetFileNameWithoutExtension(testVar);
            return true;
        }
    }
}