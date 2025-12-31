using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientModeMainLoop : ComponentBase {
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.Wait;
        private Proto_Mode.TargetRpc_VSModeWarEnd vsModeWarEndData;
        private List<int> winTeamList = new List<int>();
        private List<int> drawItemIds = new List<int>();
        private float deltaQuitGame = 0f;
        private IGPO lockGPO = null;
        private Dictionary<int, int> teamToWinCount = new Dictionary<int, int>();

        private Dictionary<long, CM_Mode.PlayModeCharacterData> characterDataDic =
            new Dictionary<long, CM_Mode.PlayModeCharacterData>();

        protected override void OnAwake() {
            MsgRegister.Register<CM_Mode.GetWarEndData>(OnGetWarEndDataCallBack);
            MsgRegister.Register<CM_Mode.GetPlayModeCharacterData>(OnGetPlayModeCharacterDataCallBack);
            MsgRegister.Register<CM_Mode.GetTeamsWinCount>(OnGetTeamsWinCount);
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            PerfAnalyzerAgent.SetReportSign(WarData.WarId);
            SentrySDKAgent.Instance.SetIPPort(NetworkData.Config.IP, NetworkData.Config.Port);
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Mode.Rpc_GameState.ID, OnRpcGameStateCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_GameState.ID, OnTargetRpcGameStateCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_GameDownTime.ID, OnRpoGameDownTimeCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_PlayCharacterData.ID, OnRpcPlayCharacterDataCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_PlayCharacterData.ID, OnTargetRpcPlayCharacterDataCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_Score.ID, OnRpcScoreCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_KillNum.ID, OnRpcKillNumCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_ModeMessage.ID, OnRpcModeMessageCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_VSModeWarEnd.ID, OnVSModeWarEndCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_HurtValue.ID, OnTargetRpcHurtValueCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_VSModeWarEndWinTeamList.ID, OnVSModeWarEndWinTeamListCallBack);
            AddProtoCallBack(Proto_Mode.Rpc_TeamWin.ID, OnRpcWinCountCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_TeamsWinCount.ID, OnTargetRpcWinCountCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            lockGPO = null;
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOGPOCallBack);
            MsgRegister.Unregister<CM_Mode.GetPlayModeCharacterData>(OnGetPlayModeCharacterDataCallBack);
            MsgRegister.Unregister<CM_Mode.GetTeamsWinCount>(OnGetTeamsWinCount);
            MsgRegister.Unregister<CM_Mode.GetWarEndData>(OnGetWarEndDataCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_GameState.ID, OnRpcGameStateCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_GameState.ID, OnTargetRpcGameStateCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_GameDownTime.ID, OnRpoGameDownTimeCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_PlayCharacterData.ID, OnRpcPlayCharacterDataCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_PlayCharacterData.ID, OnTargetRpcPlayCharacterDataCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_Score.ID, OnRpcScoreCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_KillNum.ID, OnRpcKillNumCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_ModeMessage.ID, OnRpcModeMessageCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_VSModeWarEnd.ID, OnVSModeWarEndCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_HurtValue.ID, OnTargetRpcHurtValueCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_VSModeWarEndWinTeamList.ID, OnVSModeWarEndWinTeamListCallBack);
            RemoveProtoCallBack(Proto_Mode.Rpc_TeamWin.ID, OnRpcWinCountCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_TeamsWinCount.ID, OnTargetRpcWinCountCallBack);
        }

        private void OnAddLocalGPOGPOCallBack(CM_GPO.AddLocalGPO ent) {
            if (ent.LocalGPO == null) {
                Debug.LogError("Look igpo 不能是 null");
                return;
            }
            lockGPO = ent.LocalGPO;
            UpdateRegister.AddInvoke(UpdateLocalCharacterData, 0f);
        }
        
        
        private void OnRpcPlayCharacterDataCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_PlayCharacterData)cmdData;
            AddPlayerData(data.playerId, data.nickName, data.gpoID, data.teamID, data.score, data.continueScore, data.hurtValue,
                data.KillCount, data.avatarURL);
        }

        private void OnTargetRpcHurtValueCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_HurtValue)cmdData;
            CM_Mode.PlayModeCharacterData playerData = null;
            if (characterDataDic.TryGetValue(data.playerId, out playerData)) {
                playerData.HurtValue = data.hurtValue;
                MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterHurt {
                    CharacterData = playerData
                });
            }
        }

        private void OnTargetRpcPlayCharacterDataCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_PlayCharacterData)cmdData;
            AddPlayerData(data.playerId, data.NickName, data.gpoID, data.teamID, data.score, data.continueScore, data.hurtValue,
                data.KillCount, data.avatarURL);
        }

        private void AddPlayerData(long playerId, string nickName, int gpoId, int teamId, int score, int continueScore, int hurtValue,
            int killCount, string avatarURL) {
            if (characterDataDic.ContainsKey(playerId)) {
                return;
            }
            var playerData = new CM_Mode.PlayModeCharacterData {
                PlayerId = playerId,
                TeamId = teamId,
                Score = score,
                ContinueScore = continueScore,
                GPOId = gpoId,
                HurtValue = hurtValue,
                KillCount = killCount,
                AvatarURL = avatarURL,
                NickName = nickName,
            };
            characterDataDic.Add(playerId, playerData);
            MsgRegister.Dispatcher(new CM_Mode.UpdatePlayCharacterData {
                CharacterDataDic = characterDataDic
            });
            UpdateLocalCharacterData();
        }

        private void UpdateLocalCharacterData() {
            if (lockGPO == null) {
                return;
            }
            var playerData = GetPlayModeCharacterData(lockGPO.GetGpoID());
            if (playerData == null) {
                return;
            }
            MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterScore {
                CharacterData = playerData
            });
            MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterHurt {
                CharacterData = playerData
            });
            MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterKillNum {
                CheckFirstKill = false,
                CharacterData = playerData
            });
        }
        private void OnRpcGameStateCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_GameState)cmdData;
            SetGameState((ModeData.GameStateEnum)data.gameState);
        }

        private void OnTargetRpcGameStateCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_GameState)cmdData;
            SetGameState((ModeData.GameStateEnum)data.gameState);
        }

        private void OnRpoGameDownTimeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_GameDownTime)cmdData;
            MsgRegister.Dispatcher(new CM_Mode.SetDownTime {
                DownTime = data.downTime
            });
        }

        private void OnRpcScoreCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_Score)cmdData;
            CM_Mode.PlayModeCharacterData playerData = null;
            if (characterDataDic.TryGetValue(data.playerId, out playerData)) {
                playerData.Score = data.score;
                playerData.ContinueScore = data.continueScore;
                MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterScore {
                    CharacterData = playerData
                });
            }
        }

        private void OnRpcKillNumCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_KillNum)cmdData;
            CM_Mode.PlayModeCharacterData playerData = null;
            if (characterDataDic.TryGetValue(data.playerId, out playerData)) {
                playerData.KillCount = data.killCount;
                MsgRegister.Dispatcher(new CM_Mode.UpdateCharacterKillNum {
                    CheckFirstKill = true,
                    CharacterData = playerData
                });
            }
        }

        private void OnRpcModeMessageCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_ModeMessage)cmdData;
            MsgRegister.Dispatcher(new CM_Mode.ModeMessage {
                MainText = data.mainText,
                SubText = data.subText,
                UseId = data.itemId,
                MainTeamId = data.teamId,
                MessageState = (ModeData.MessageEnum)data.messageState
            });
        }

        private void OnVSModeWarEndCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            vsModeWarEndData = (Proto_Mode.TargetRpc_VSModeWarEnd)cmdData;
            drawItemIds = vsModeWarEndData.dropitemList;
            var list = vsModeWarEndData.battlePlayerDatas;
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                CM_Mode.PlayModeCharacterData playerData = null;
                if (characterDataDic.TryGetValue(data.playerId, out playerData)) {
                    playerData.KillCount = data.killCount;
                    playerData.AllScore = data.AllScore;
                    playerData.DeadNum = data.deadNum;
                    playerData.HurtValue = data.hurtValue;
                }
            }
            PerfAnalyzerAgent.SetTag("掉落物品数量:" + drawItemIds.Count);
            PerfAnalyzerAgent.LoginScene("WarEnd");
            SentrySDKAgent.Instance.SetScene("WarEnd");
            MsgRegister.Dispatcher(new CM_Mode.ShowWarEndUI());
        }

        private void OnVSModeWarEndWinTeamListCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_VSModeWarEndWinTeamList)cmdData;
            winTeamList = data.teamList;

            var gpoId = 0;
            CM_Mode.PlayModeCharacterData playerData = null;
            if (characterDataDic.TryGetValue(data.triggerPlayerId, out playerData)) {
                gpoId = playerData.GPOId;
            }
            MsgRegister.Dispatcher(new CM_Mode.WinTeamList {
                TeamList = data.teamList,
                TriggerEndGpoId = gpoId,
            });
        }

        private void OnRpcWinCountCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.Rpc_TeamWin)cmdData;
            // 更新队伍分数，平局不加分
            if (data.teamList.Count == 1) {
                var winTeamId = data.teamList[0];
                if (teamToWinCount.TryGetValue(winTeamId, out var winCount)) {
                    teamToWinCount[winTeamId] = winCount + 1;
                }
            }
            MsgRegister.Dispatcher(new CM_Mode.TeamWin {
                teamList = data.teamList
            });
        }

        private void OnTargetRpcWinCountCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_TeamsWinCount)cmdData;
            for (var i = 0; i < data.teamIDs.Count; i++) {
                teamToWinCount[data.teamIDs[i]] = data.winCounts[i];
            }
            MsgRegister.Dispatcher(new CM_Mode.SetTeamsWinCount {
                roundCount = data.roundCount,
                teamToWinCount = teamToWinCount
            });
        }

        private void OnGetWarEndDataCallBack(CM_Mode.GetWarEndData ent) {
            ent.CallBack(vsModeWarEndData.dropitemList, characterDataDic.Values.ToList(), winTeamList);
        }

        private void OnGetPlayModeCharacterDataCallBack(CM_Mode.GetPlayModeCharacterData ent) {
            var gpoId = ent.GpoId;
            ent.CallBack(GetPlayModeCharacterData(gpoId));
        }

        private void OnGetTeamsWinCount(CM_Mode.GetTeamsWinCount ent) {
            ent.callBack(teamToWinCount);
        }

        private CM_Mode.PlayModeCharacterData GetPlayModeCharacterData(int gpoId) {
            foreach (var value in characterDataDic.Values) {
                if (value.GPOId == gpoId) {
                    return value;
                }
            }
            return null;
        }
        
        private void OnUpdate(float deltaTime) {
            switch (gameState) {
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    DeltaModeOver();
                    break;
            }
        }

        private void SetGameState(ModeData.GameStateEnum gameState) {
            this.gameState = gameState;
            if (NetworkData.IsStartServer == false) {
                PerfAnalyzerAgent.SetTag(gameState.ToString());
            }
            switch (gameState) {
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    break;
                case ModeData.GameStateEnum.WaitModeOver:
                    deltaQuitGame = 10f;
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    ModeOver();
                    break;
            }
            ModeData.SetGameState(gameState);
            MsgRegister.Dispatcher(new CM_Mode.SetGameState {
                GameState = gameState
            });
        }

        private void DeltaModeOver() {
            if (deltaQuitGame > 0) {
                deltaQuitGame -= Time.deltaTime;
                if (deltaQuitGame <= 0) {
                    ModeOver();
                }
            }
        }

        private void ModeOver() {
            MsgRegister.Dispatcher(new CM_Mode.SaveData());
        }
    }
}