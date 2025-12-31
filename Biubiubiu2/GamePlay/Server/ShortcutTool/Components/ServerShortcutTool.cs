using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerShortcutTool : ComponentBase {
        public const string KEY_GameOver = "GameOver";
        public const string KEY_TeamScore = "TeamScore";
        public const string KEY_AIMove = "AIMove";
        public const string KEY_AIFire = "AIFire";
        public const string KEY_CharacterLockBlood = "CharacterLockBlood";
        public const string KEY_EquipChange = "EquipChange";
        public const string KEY_ActivateSuperWeapon = "ActivateSuperWeapon";
        
        private float testTime = 0f;

        protected override void OnAwake() {
            MsgRegister.Register<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Register<SM_Mode.AddCharacterEnd>(OnAddCharacterEndCallBack);
            mySystem.Register<SE_ShortcutTool.InMessage>(OnInMessageCallBack);
        }
        
        protected override void OnStart() {
            // AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Unregister<SM_Mode.AddCharacterEnd>(OnAddCharacterEndCallBack);
            mySystem.Unregister<SE_ShortcutTool.InMessage>(OnInMessageCallBack);
            // RemoveUpdate(OnUpdate);
        }

        // private bool isTrue = false;
        // private void OnUpdate(float deltaTime) {
        //     if (testTime > 0f) {
        //         testTime -= deltaTime;
        //         if (testTime <= 0f) {
        //             isTrue = !isTrue;
        //             Debug.Log("测试时间：" + isTrue);
        //             var data = new Hashtable();
        //             data.Add("PlayerId", -1);
        //             ActivateSuperWeapon(data);
        //             testTime = 10;
        //         }
        //     }
        // }
        
        private void OnSetGameStateCallBack(SM_Mode.SetGameState msg) {
            if (msg.GameState == ModeData.GameStateEnum.Wait) {
                testTime = 5f;
                SendCreateData();
            } else if (msg.GameState == ModeData.GameStateEnum.ModeOver) {
                mySystem.Dispatcher(new SE_ShortcutTool.CloseWebSocket());
            }
        }

        private void SendCreateData() {
            var data = new Hashtable();
            if (PlayerData.UID != "") {
                data.Add("WarId", $"{PlayerData.UID}_{TimeUtil.GetSecond()}");
            } else {
                data.Add("WarId", $"{WarData.WarId}_{TimeUtil.GetSecond()}");
            }
            data.Add("ServerName", GameData.GetServerName());
            var openMessage = MiniJSON.jsonEncode(data);
            mySystem.Dispatcher(new SE_ShortcutTool.CreateWebSocket {
                OpenMessage = openMessage,
            });
        }
        
        private void OnAddCharacterEndCallBack(SM_Mode.AddCharacterEnd msg) {
            var list = RenderOutCharacterList();
            var data = new Hashtable();
            data.Add("CharacterList", list);
            var message = MiniJSON.jsonEncode(data);
            mySystem.Dispatcher(new SE_ShortcutTool.OutMessage {
                Message = message,
            });
        }

        private ArrayList RenderOutCharacterList() {
            var outList = new ArrayList();
            var list = GetCharacterList();
            foreach (var character in list) {
                if (character.CharacterGPO == null) {
                    continue;
                }
                var characterData = new Hashtable();
                characterData["PlayerId"] = character.PlayerId;
                characterData["TeamId"] = character.TeamId;
                characterData["NickName"] = character.NickName;
                characterData["IsAI"] = character.IsAI;
                characterData["AILevel"] = character.AILevel;
                var weaponList = new ArrayList();
                foreach (var weapon in character.WeaponList) {
                    var weaponData = new Hashtable();
                    weaponData["WeaponId"] = weapon.WeaponItemId;
                    weaponData["IsSuperWeapon"] = weapon.IsSuperWeapon;
                    weaponData["Index"] = weapon.Index;
                    weaponList.Add(weaponData);
                }
                characterData["WeaponList"] = weaponList;
                outList.Add(characterData);
            }

            return outList;
        }

        // 获取所有玩家列表含 AI
        private List<SE_Mode.PlayModeCharacterData> GetCharacterList() {
            var characterList = new List<SE_Mode.PlayModeCharacterData>();
            MsgRegister.Dispatcher(new SM_Mode.GetCharacterList {
                CallBack = (list) => {
                    characterList = list;
                }
            });
            return characterList;
        }

        private void OnInMessageCallBack(ISystemMsg body, SE_ShortcutTool.InMessage msg) {
            var jsonData = MiniJSON.jsonDecode(msg.Message) as Hashtable;
            var key = jsonData["Key"].ToString();
            var dataMsg = jsonData["Data"].ToString();
            var data = MiniJSON.jsonDecode(dataMsg) as Hashtable;
            switch (key) {
                case KEY_GameOver:
                    GameOver(data);
                    break;
                case KEY_TeamScore:
                    TeamScore(data);
                    break;
                case KEY_AIMove:
                    AIMove(data);
                    break;
                case KEY_AIFire:
                    AIFire(data);
                    break;
                case KEY_CharacterLockBlood:
                    CharacterLockBlood(data);
                    break;
                case KEY_EquipChange:
                    EquipChange(data);
                    break;
                case KEY_ActivateSuperWeapon:
                    ActivateSuperWeapon(data);
                    break;
            }
        }

        // 游戏结束
        private void GameOver(Hashtable data) {
            MsgRegister.Dispatcher(new SM_ShortcutTool.Event_GameOver());
        }

        // 队伍得分
        private void TeamScore(Hashtable data) {
            var teamId = int.Parse(data["TeamId"].ToString());
            var teamScore = int.Parse(data["Score"].ToString());
            MsgRegister.Dispatcher(new SM_ShortcutTool.Event_TeamScore {
                TeamId = teamId,
                TeamScore = teamScore,
            });
        }

        // AI 移动
        private void AIMove(Hashtable data) {
            var isMove = bool.Parse(data["IsMove"].ToString());
            WarData.TestIsAIMove = isMove;
        }

        // AI 移动
        private void AIFire(Hashtable data) {
            var isFire = bool.Parse(data["IsFire"].ToString());
            WarData.TestIsAIFire = isFire;
            MsgRegister.Dispatcher(new SM_ShortcutTool.Event_MonsterFire ());
        }

        // 锁血
        private void CharacterLockBlood(Hashtable data) {
            var gpoId = GetGPOId(data);
            if (gpoId == 0) {
                return;
            }
            var isLocked = bool.Parse(data["IsLock"].ToString());
            MsgRegister.Dispatcher(new SM_ShortcutTool.Event_CharacterLockBlood {
                GpoId = gpoId,
                IsLocked = isLocked,
            });
        }

        // 装备变更
        private void EquipChange(Hashtable data) {
            var gpoId = GetGPOId(data);
            if (gpoId == 0) {
                return;
            }
            var weaponId = int.Parse(data["WeaponId"].ToString());
            var isSuperWeapon = bool.Parse(data["IsSuperWeapon"].ToString());
            var index = int.Parse(data["Index"].ToString());
            ChangeCharacterWeaponData(gpoId, index, weaponId, isSuperWeapon);
            if (isSuperWeapon) {
                MsgRegister.Dispatcher(new SM_ShortcutTool.Event_EquipSkillChange {
                    GpoId = gpoId,
                    WeaponItemId = weaponId,
                });
            } else {
                MsgRegister.Dispatcher(new SM_ShortcutTool.Event_EquipWeaponChange {
                    GpoId = gpoId,
                    Index = index,
                    WeaponItemId = weaponId,
                });
            }
        }

        private void ChangeCharacterWeaponData(int gpoId, int index, int weaponId, bool isSuperWeapon) {
            var list = GetCharacterList();
            for (int i = 0; i < list.Count; i++) {
                var character = list[i];
                if (gpoId >= 0) {
                    if (character.CharacterGPO == null || character.CharacterGPO.GetGpoID() != gpoId) {
                        continue;
                    }
                } else {
                    if (gpoId == -2 && iGPO.GetGPOType() != GPOData.GPOType.Role) {
                        continue;
                    }
                }
                var isChangeData = false;
                for (int j = 0; j < character.WeaponList.Count; j++) {
                    var weapon = character.WeaponList[j];
                    if (weapon.Index == index) {
                        weapon.WeaponItemId = weaponId;
                        isChangeData = true;
                        break;
                    }
                }
                if (isChangeData == false) {
                    var weaponData = new SE_Mode.PlayModeCharacterWeapon {
                        WeaponItemId = weaponId, IsSuperWeapon = isSuperWeapon, Index = index,
                    };
                    character.WeaponList.Add(weaponData);
                }
            }
        }

        // 激活超级武器
        private void ActivateSuperWeapon(Hashtable data) {
            var gpoId = GetGPOId(data);
            if (gpoId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_ShortcutTool.Event_ActivateSuperWeapon {
                GpoId = gpoId,
            });
        }

        private int GetGPOId(Hashtable data) {
            var playerId = long.Parse(data["PlayerId"].ToString());
            var gpoId = 0;
            if (playerId < 0) {
                gpoId = (int)playerId;
            } else {
                var gpo = GetGpoForPlayerId(playerId);
                if (gpo == null) {
                    Debug.LogError($"没找到 PlayerId：{playerId} 对应的 GPO");
                    return 0;
                }

                gpoId = gpo.GetGpoID();
            }

            return gpoId;
        }

        private IGPO GetGpoForPlayerId(long playerId) {
            IGPO gpo = null;
            var list = GetCharacterList();
            foreach (var character in list) {
                if (character.PlayerId == playerId) {
                    gpo = character.CharacterGPO;
                    break;
                }
            }
            return gpo;
        }
    }
}