using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerVSMode : ComponentBase {
        private List<PlayerSpawnPoint> team1PointList;
        private List<PlayerSpawnPoint> team2PointList;
        private List<SE_Mode.PlayModeCharacterData> characterList;
        private Dictionary<long, Time> playerIdToOfflineTimer =  new Dictionary<long, Time>();    // 掉线倒计时
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        private const float PROTECT_DURATION = 3f;
        private const float OFFLINE_DURATION = 30f;

        private class Time {
            public float value;
        }

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Register<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Register<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLoginCallBack);
            AddUpdate(Update);
        }

        protected override void OnClear() {
            playerIdToOfflineTimer.Clear();
            RemoveUpdate(Update);
            mySystem.Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Unregister<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLoginCallBack);
        }

        private void Update(float dt) {
            if (IsModeOver()) {
                return;
            }
            foreach ((long playerId, var time) in playerIdToOfflineTimer) {
                time.value -= dt;
                if (time.value > 0) {
                    continue;
                }
                // 结束比赛
                OverGameMode(playerId);
                playerIdToOfflineTimer.Clear();
                return;
            }
        }

        private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
            gameState = e.GameState;
            switch (e.GameState) {
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    WaitRoundStart();
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    RoundStart();
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    break;
                case ModeData.GameStateEnum.WaitModeOver:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    break;
            }
        }

        private void OnAddCharacterCallBack(ISystemMsg body, SE_Mode.Event_AddCharacterFinish ent) {
        }

        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 1, CallBack = GetStartPoint1s
            });
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 2, CallBack = GetStartPoint2s
            });
        }

        private void OnGetStartPointCallBack(SM_Mode.GetStartPoint ent) {
            var startTran = GetStartData(ent.CharacterGPO);
            ent.CallBack(startTran.Point, startTran.Rota);
        }

        private void GetStartPoint1s(List<PlayerSpawnPoint> pointList) {
            team1PointList = pointList;
        }

        private void GetStartPoint2s(List<PlayerSpawnPoint> pointList) {
            team2PointList = pointList;
        }

        private void ResetCharacterState(IGPO iGpo) {
            var data = GetStartData(iGpo);
            iGpo.Dispatcher(new SE_Entity.SyncPointAndRota {
                Point = data.Point,
                Rota = data.Rota,
            });
            iGpo.Dispatcher(new SE_GPO.Event_ReLife {
                UpHp = 9999999,
            });
        }

        private PlayerSpawnPoint GetStartData(IGPO iGpo) {
            PlayerSpawnPoint data;
            if (iGpo.GetTeamID() == 1) {
                data = team1PointList[Random.Range(0, team1PointList.Count)];
            } else {
                data = team2PointList[Random.Range(0, team2PointList.Count)];
            }
            return data;
        }

        private void WaitRoundStart() {
            ReSetRoundData();
        }

        private void RoundStart() {
            Debug.Log("RoundStart 开始分配武器");
            mySystem.Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = OnGetCharacterListCallBack,
            });
        }

        private void OnGetCharacterListCallBack(List<SE_Mode.PlayModeCharacterData> characterList) {
            this.characterList = characterList;
            foreach (var data in characterList) {
                var iGpo = data.CharacterGPO;
                if (iGpo != null) {
                    iGpo.Register<SE_GPO.Event_SetIsDead>(OnCharacterDeadCallBack);
                    iGpo.Register<SE_Network.Event_SetIsOnline>(OnSetIsOnlineCallBack);
                    ResetCharacterState(iGpo);
                    AddWeapon(data);
                    // 无敌
                    iGpo.Dispatcher(new SE_GPO.Event_BecomeTargetProtectTime {
                        ProtectTime = PROTECT_DURATION,
                    });
                    // 检查是否掉线
                    CheckIsOffline(data);
                }
            }
        }

        private void OnCharacterDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            Debug.Log("角色死亡：" + ent.DeadGpo.GetGpoID());
            ClearBattleData(ent.DeadGpo);
            CheckLostTeam();
        }

        private void OnSetIsOnlineCallBack(ISystemMsg body, SE_Network.Event_SetIsOnline ent) {
            if (IsModeOver() || !TryGetPlayerId(ent.GpoId, out var playerId)) {
                return;
            }
            if (ent.IsOnline) {
                playerIdToOfflineTimer.Remove(playerId);
            } else {
                playerIdToOfflineTimer[playerId] = new Time {
                    value = OFFLINE_DURATION
                };
            }
            Rpc(new Proto_Mode.Rpc_OnlineChange {
                playerId = playerId,
                isOnline = ent.IsOnline,
                reconnectDuration = ent.IsOnline ? 0 : OFFLINE_DURATION
            });
        }

        private void OnCharacterLoginCallBack(SM_Character.CharacterLogin ent) {
            if (IsModeOver()) {
                return;
            }
            foreach ((long playerId, var time) in playerIdToOfflineTimer) {
                TargetRpc(ent.INetwork, new Proto_Mode.TargetRpc_OnlineChange {
                    playerId = playerId,
                    isOnline = false,
                    reconnectDuration = time.value
                });
            }
        }

        /// <summary>
        /// 检查存货的队伍数量
        /// </summary>
        private void CheckLostTeam() {
            int lostTeamId = -1;
            for (int i = 0; i < characterList.Count; i++) {
                var data = characterList[i];
                var checkIsDead = false;
                if (data.CharacterGPO.IsClear()) {
                    checkIsDead = true;
                } else {
                    data.CharacterGPO.Dispatcher(new SE_GPO.Event_GetIsDead {
                        CallBack = isDead => {
                            checkIsDead = isDead;
                        }
                    });
                }
                if (checkIsDead == false) {
                    if (lostTeamId == -1) {
                        lostTeamId = data.CharacterGPO.GetTeamID();
                    } else {
                        if (lostTeamId != data.CharacterGPO.GetTeamID()) {
                            return;
                        }
                    }
                }
            }
            if (lostTeamId == -1) {
                Debug.LogError("错误：所有队伍都已经死亡");
                lostTeamId = characterList[0].CharacterGPO.GetTeamID();
            }
            WinTeam(lostTeamId);
        }

        /// <summary>
        /// 胜利队伍加分
        /// </summary>
        private void WinTeam(int winTeamId) {
            Debug.Log("胜利队伍：" + winTeamId);
            for (int i = 0; i < characterList.Count; i++) {
                var data = characterList[i];
                if (data.CharacterGPO.GetTeamID() == winTeamId) {
                    // 其中一个成员进行加分就可以
                    MsgRegister.Dispatcher(new SM_Mode.AddScore {
                        Channel = ModeData.GetScoreChannelEnum.FinalSurvivalTeam, GpoId = data.CharacterGPO.GetGpoID(),
                    });
                    return;
                }
            }
        }

        private void AddWeapon(SE_Mode.PlayModeCharacterData data) {
            for (int i = 0; i < data.WeaponList.Count; i++) {
                var weapon = data.WeaponList[i];
                if (weapon.IsSuperWeapon == false) {
                    data.CharacterGPO.Dispatcher(new SE_Item.Event_AddWeaponForMode {
                        WeaponData = weapon
                    });
                } else {
                    data.CharacterGPO.Dispatcher(new SE_Skill.Event_AddSkill {
                        SkillData = weapon,
                    });
                }
            }
            AddOwnItem(data.CharacterGPO, ItemSet.Id_GunBullet, 999);
            AddOwnItem(data.CharacterGPO, ItemSet.Id_FocusGunBullet, 999);
        }

        private void ReSetRoundData() {
            if (characterList == null || characterList.Count <= 0) {
                return;
            }
            Debug.Log("回合准备重新开始，重置所有上回合数据：" + characterList.Count);
            foreach (var data in characterList) {
                var iGpo = data.CharacterGPO;
                Debug.Log("重生：" + iGpo.GetGpoID() + " 队伍：" + iGpo.GetTeamID() + " -- " + data.TeamId);
                if (iGpo.IsClear()) {
                    mySystem.Dispatcher(new SE_Mode.Event_OnSetCreateAI {
                        Data = data,
                    });
                } else {
                    iGpo.Unregister<SE_GPO.Event_SetIsDead>(OnCharacterDeadCallBack);
                    iGpo.Unregister<SE_Network.Event_SetIsOnline>(OnSetIsOnlineCallBack);
                }
                ClearBattleData(iGpo);
            }
        }

        private void ClearBattleData(IGPO iGpo) {
            iGpo.Dispatcher(new SE_Item.Event_DropUnprotectedItems {
                IsDrop = false,
            });
            iGpo.Dispatcher(new SE_Character.Event_DropAllFollowAI {
                IsDrop = false,
            });
            iGpo.Dispatcher(new SE_GPO.Event_RemoveAllWeaponPack());
            iGpo.Dispatcher(new SE_Skill.Event_RemoveAllSkills());
        }

        private void AddOwnItem(IGPO characterGpo, int itemId, ushort itemNum, bool isProtect = false) {
            characterGpo.Dispatcher(new SE_Item.Event_AddPickItem {
                ItemId = itemId, ItemNum = itemNum, IsProtect = isProtect, IsQuickUse = true,
            });
        }

        private void OverGameMode(long offlinePlayerId) {
            var winTeamList = new List<int>(characterList.Count / 2);
            var offlineCharacter = characterList.Find(player => player.PlayerId == offlinePlayerId);
            foreach (var player in characterList) {
                if (player.TeamId != offlineCharacter.TeamId) {
                    winTeamList.Add(player.TeamId);
                }
            }
            Dispatcher(new SE_Mode.Event_OverGameMode {
                WinTeamList = winTeamList,
            });
        }

        private bool IsModeOver() {
            return gameState switch { ModeData.GameStateEnum.WaitModeOver
                    or ModeData.GameStateEnum.ModeOver
                    or ModeData.GameStateEnum.QuitApp => true,
                _ => false
            };
        }

        private bool TryGetPlayerId(int gpoId, out long playerId) {
            playerId = 0;
            foreach (var player in characterList) {
                if (player.CharacterGPO != null && player.CharacterGPO.GetGpoID() == gpoId) {
                    playerId = player.PlayerId;
                    return true;
                }
            }
            return false;
        }

        private void CheckIsOffline(SE_Mode.PlayModeCharacterData characterData) {
            characterData.CharacterGPO.Dispatcher(new SE_Network.Event_GetIsOnline {
                CallBack = isOnline => {
                    if (isOnline || playerIdToOfflineTimer.ContainsKey(characterData.PlayerId)) {
                        return;
                    }
                    playerIdToOfflineTimer[characterData.PlayerId] = new Time {
                        value = OFFLINE_DURATION
                    };
                    Rpc(new Proto_Mode.Rpc_OnlineChange {
                        playerId = characterData.PlayerId,
                        isOnline = false,
                        reconnectDuration = OFFLINE_DURATION
                    });
                }
            });
        }
    }
}