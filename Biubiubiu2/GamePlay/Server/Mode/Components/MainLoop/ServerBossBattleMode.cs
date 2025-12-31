using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerBossBattleMode : ComponentBase {
        private PlayerSpawnPoint bossPoint;
        private PlayerSpawnPoint characterPoint;
        private float range = 5f;

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Register<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Register<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
        }

        protected override void OnStart() {
        }
        
        protected override void OnClear() {
            mySystem.Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Unregister<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
        }

        private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
            switch (e.GameState) {
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    WaitRoundStart();
                    RoundStart();
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    break;
            }
        }
        

        private void WaitRoundStart() {
            Debug.Log("等待游戏开始");
            mySystem.Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = list => {
                    foreach (var data in list) {
                        var iGpo = data.CharacterGPO;
                        if (iGpo != null) {
                            AddLoginCharacterDefaultItem(iGpo);
                        }
                    }
                },
            });
        }
        

        private void RoundStart() {
            MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                AISign = GPOM_RexKingSet.Sign_RexKing,
                StartPoint = bossPoint.Point,
                OR_GpoType = GPOData.GPOType.AI
            });
        }
        
        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 1,
                CallBack = GetStartPoint1s
            });
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 2,
                CallBack = GetStartPoint2s
            });
        }

        private void GetStartPoint1s(List<PlayerSpawnPoint> pointList) {
            characterPoint = pointList[0];
        }

        private void GetStartPoint2s(List<PlayerSpawnPoint> pointList) {
            bossPoint = pointList[0];
        }

        private void OnAddCharacterCallBack(ISystemMsg body, SE_Mode.Event_AddCharacterFinish ent) { // 队友
        }

        private void OnGetStartPointCallBack(SM_Mode.GetStartPoint ent) {
            ent.CallBack(GetStartPointRange(), characterPoint.Rota);
        }
        
        private Vector3 GetStartPointRange() {
            var point = characterPoint.Point;
            var x = Random.Range(-range, range);
            var z = Random.Range(-range, range);
            return new Vector3(point.x + x, point.y, point.z + z);
        }
        
        private void AddLoginCharacterDefaultItem(IGPO characterGpo) {
            var range = Random.Range(0f, 1f);
            if (characterGpo.GetGPOType() == GPOData.GPOType.Role) {
                if (range > 0.7f) {
                    AddOwnItem(characterGpo, "Bat_28", 1);
                } else if (range > 0.4f) {
                    AddOwnItem(characterGpo, "RPG", 1);
                } else {
                    AddOwnItem(characterGpo, "FocusGun", 1);
                }
            } else {
                if (range > 0.4f) {
                    AddOwnItem(characterGpo, "FocusGun", 1);
                } else {
                    AddOwnItem(characterGpo, "RPG", 1);
                }
            }
            AddOwnItem(characterGpo,"FirstAidKit", 2);
            AddOwnItem(characterGpo, "Aerocraft_1", 1);
        }

        private void AddOwnItem(IGPO characterGpo, string itemSign, ushort itemNum, bool isProtect = false) {
            var itemData = ItemData.GetData(itemSign);
            characterGpo.Dispatcher(new SE_Item.Event_AddPickItem {
                ItemId = itemData.Id, ItemNum = itemNum, IsProtect = isProtect, IsQuickUse = true,
            });
        }
    }
}