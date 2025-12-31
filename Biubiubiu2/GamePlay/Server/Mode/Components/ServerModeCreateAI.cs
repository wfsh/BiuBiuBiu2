using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeCreateAI : ComponentBase {
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        public List<SE_Mode.PlayModeCharacterData> needCreateAIList = new List<SE_Mode.PlayModeCharacterData>();
        private float waitAddAITime = 0.0f;
        private float waitAddAIDeltaTime = 0.0f;
        private bool isCloseWaitAutoAddAI = false;
        private float delayUpdateAITime = 0;

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_OnSetCreateAIList>(OnSetCreateAIListCallBack);
            mySystem.Register<SE_Mode.Event_OnSetCreateAI>(OnSetCreateAICallBack);
            mySystem.Register<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
            mySystem.Register<SE_Mode.Event_OnCloseWaitAutoAddAI>(OnCloseWaitAutoAddAICallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Mode.Event_OnSetCreateAIList>(OnSetCreateAIListCallBack);
            mySystem.Unregister<SE_Mode.Event_OnSetCreateAI>(OnSetCreateAICallBack);
            mySystem.Unregister<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
            mySystem.Unregister<SE_Mode.Event_OnCloseWaitAutoAddAI>(OnCloseWaitAutoAddAICallBack);
        }

        private void OnUpdate(float deltaTime) {
            if (isCloseWaitAutoAddAI) {
                return;
            }
            switch (gameState) {
                case ModeData.GameStateEnum.WaitStartDownTime:
                case ModeData.GameStateEnum.WaitRoundStart:
                case ModeData.GameStateEnum.RoundStart:
                    UpdateAddAI();
                    break;
            }
        }
        
        private void OnCloseWaitAutoAddAICallBack(ISystemMsg body, SE_Mode.Event_OnCloseWaitAutoAddAI ent) {
            isCloseWaitAutoAddAI = true;
        }

        private void OnSetCreateAIListCallBack(ISystemMsg body, SE_Mode.Event_OnSetCreateAIList ent) {
            needCreateAIList = ent.AIList;
            if (needCreateAIList.Count == 0) {
                waitAddAITime = 0f;
                return;
            }
            delayUpdateAITime = ModeData.PlayData.StartModeDownTime * 0.2f;
            var lostTime = ModeData.PlayData.StartModeDownTime - delayUpdateAITime;
            waitAddAIDeltaTime = 0f;
            waitAddAITime = (lostTime * 0.8f) / (float)needCreateAIList.Count;
        }

        private void OnSetCreateAICallBack(ISystemMsg body, SE_Mode.Event_OnSetCreateAI ent) {
            AddAI(ent.Data);
        }

        private void OnSetGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState ent) {
            gameState = ent.GameState;
        }

        private void UpdateAddAI() {
            if (delayUpdateAITime > 0f) {
                delayUpdateAITime -= Time.deltaTime;
                return;
            } 
            if (waitAddAITime == 0f || needCreateAIList.Count == 0) {
                return;
            }
            waitAddAIDeltaTime += Time.deltaTime;
            if (waitAddAIDeltaTime >= waitAddAITime) {
                AddAIForWaitStart();
                waitAddAIDeltaTime = 0;
            }
        }

        private void AddAIForWaitStart() {
            if (needCreateAIList.Count == 0) {
                return;
            }
            var index = Random.Range(0, needCreateAIList.Count);
            var aiData = needCreateAIList[index];
            needCreateAIList.RemoveAt(index);
            if (aiData.IsAI == false) {
                return;
            }
            AddAI(aiData);
        }

        private void AddAI(SE_Mode.PlayModeCharacterData data) {
            var teamId = 0;
            mySystem.Dispatcher(new SE_Mode.Event_GetCharacterData {
                PlayerId = data.PlayerId,
                CallBack = characterData => {
                    teamId = characterData.TeamId;
                }
            });
            var gpoData = GpoSet.GetGpoById(data.GPOMId);
            MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                AISign = gpoData.Sign,
                OR_AISkinSign = data.AssetSign,
                OR_GpoType = GPOData.GPOType.RoleAI,
                OR_TeamId = teamId,
                OR_InData = data.InData,
                OR_CallBack = monster => {
                    AddAICallBack(monster, data);
                },
            });
        }

        private void AddAICallBack(IAI iai, SE_Mode.PlayModeCharacterData data) {
            var monsterSystem = (S_AI_Base)iai;
            var gpo = monsterSystem.GetGPO();
            gpo.Dispatcher(new SE_AI.Event_DisabledDeadToRemove());
            MsgRegister.Dispatcher(new SM_Mode.AddAICharacter {
                CharacterGPO = gpo,
                PlayerId = data.PlayerId,
            });
            MsgRegister.Dispatcher(new SM_Mode.GetStartPoint {
                CharacterGPO = gpo,
                CallBack = (point, rota) => {
                    monsterSystem.SetStartPoint(point);
                    monsterSystem.SetStartRota(rota);
                }
            });
        }
    }
}