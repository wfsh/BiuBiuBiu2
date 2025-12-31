using System;
using System.Collections.Generic;
using System.IO;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerServiceStateCheck : ComponentBase {
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        private List<SE_Mode.PlayModeCharacterData> characterList;
        private float checkLocalFile = 0f;
        private float checkWaitTime = 60f;
        private float checkOfflineTime = 0f;
        private float checkOfflineDelay = 0f;
        private bool isQuit = false;

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            characterList = null;
            mySystem.Unregister<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
        }

        private void OnUpdate(float deltaTime) {
            if (isQuit) {
                return;
            }
            if (gameState != ModeData.GameStateEnum.None) {
                if (gameState == ModeData.GameStateEnum.Wait) {
                    CheckWaitTime();
                } else if (gameState == ModeData.GameStateEnum.RoundStart) {
                    CheckAllCharacterOffLine();
                }
            } else {
                CheckLocalFile();
            }
        }

        private void OnSetGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState ent) {
            gameState = ent.GameState;
            switch (gameState) {
                case ModeData.GameStateEnum.RoundStart:
                    RoundStart();
                    break;
            }
        }
        
        private void RoundStart() {
            mySystem.Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = OnGetCharacterListCallBack,
            });
        }
        
        private void OnGetCharacterListCallBack(List<SE_Mode.PlayModeCharacterData> characterList) {
            this.characterList = characterList;
        }

        private void CheckAllCharacterOffLine() {
            if (checkOfflineDelay > 0) {
                checkOfflineDelay -= Time.deltaTime;
                return;
            }
            checkOfflineDelay = 1f;
            var isAllOffLine = true;
            foreach (var data in characterList) {
                var iGpo = data.CharacterGPO;
                if (iGpo != null && iGpo.GetNetwork() != null && iGpo.GetNetwork().IsOnline()) {
                    isAllOffLine = false;
                    break;
                }
            }
            if (isAllOffLine) {
                checkOfflineTime++;
                if (checkOfflineTime > 60f) {// 倒计时超过 60f 退出
                    Debug.LogError("所有玩家均掉线，退出游戏");
                    QuitGame();
                }
            } else {
                checkOfflineTime = 0;
            }
        }
        
        /// <summary>
        ///  检查等待时间是否超时 60f 超时退出
        /// </summary>
        private void CheckWaitTime() {
            if (checkWaitTime > 0) {
                checkWaitTime -= Time.deltaTime;
                return;
            }
            checkWaitTime = 60f;
            Debug.LogError("玩家超过 60 秒没有登录，退出游戏");
            QuitGame();
        }

        // 读取本地文件
        private void CheckLocalFile() {
            if (checkLocalFile > 0) {
                checkLocalFile -= Time.deltaTime;
                return;
            }
            checkLocalFile = 1f;
            var flagStop = IsExistsFlagStop();
            if (flagStop) {
                Debug.Log("存在 flag_stop 文件，退出游戏");
                QuitGame();
            }
        }
        
        private void QuitGame() {
            isQuit = true;
            mySystem.Dispatcher(new SE_Mode.Event_ServiceQuitGame());
        }

        public bool IsExistsFlagStop() {
            return File.Exists("./flag_stop");
        }
    }
}