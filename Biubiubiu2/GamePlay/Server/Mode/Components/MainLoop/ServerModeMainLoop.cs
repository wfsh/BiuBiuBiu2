using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeMainLoop : ComponentBase {
        private int maxTeamNum = 0;
        public List<SE_Mode.PlayModeCharacterData> characterDataList;
        public Dictionary<int, List<SE_Mode.PlayModeCharacterData>> teamDataDic;
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        private int currentRoundCount = 0;
        private int aiPlayerId = -100;
        private float downTime = 0.0f;
        private ushort prevSyncDownTime = 0;
        private Dictionary<int, long> gpoIdPlayerIdDic = new Dictionary<int, long>();
        private List<int> winTeamList = new List<int>();

        protected override void OnAwake() {
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLoginCallBack, 1);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Register<SM_Mode.CheckCharacterLogin>(OnCheckCharacterLogin);
            MsgRegister.Register<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Register<SM_Mode.AddAICharacter>(OnAddAICharacterCallBack);
            MsgRegister.Register<SM_Mode.AddScore>(OnAddScoreCallBack);
            MsgRegister.Register<SM_Mode.GetTeamId>(OnGetTeamIdCallBack);
            MsgRegister.Register<SM_Mode.GetKillCount>(OnGetKillCountCallBack);
            MsgRegister.Register<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
            MsgRegister.Register<SM_Mode.GetCharacterList>(OnSMGetCharacterListCallBack);
            MsgRegister.Register<SM_ShortcutTool.Event_GameOver>(OnShortcutToolGameOverCallBack);
            MsgRegister.Register<SM_ShortcutTool.Event_TeamScore>(OnShortcutToolTeamScoreCallBack);
            MsgRegister.Register<SM_Character.GetCharacterDataByPlayerId>(OnGetCharacterDataByPlayerIdCallBack);
            MsgRegister.Register<SM_Character.GetCharacterDataByGpoId>(OnGetCharacterDataByGpoIdCallBack);
            mySystem.Register<SE_Mode.Event_GetCharacterList>(OnGetCharacterListCallBack);
            mySystem.Register<SE_Mode.Event_GetGpoIdToPlayerIdDic>(OnGetGpoIdToPlayerIdDicCallBack);
            mySystem.Register<SE_Mode.Event_GetWinTeamList>(OnGetWinTeamListCallBack);
            mySystem.Register<SE_Mode.Event_ServiceQuitGame>(OnServiceQuitGameCallBack);
            mySystem.Register<SE_Mode.Event_OverGameMode>(OnOverGameModeCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<SM_Mode.CheckCharacterLogin>(OnCheckCharacterLogin);
            MsgRegister.Unregister<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Unregister<SM_Mode.AddAICharacter>(OnAddAICharacterCallBack);
            MsgRegister.Unregister<SM_Mode.AddScore>(OnAddScoreCallBack);
            MsgRegister.Unregister<SM_Mode.GetTeamId>(OnGetTeamIdCallBack);
            MsgRegister.Unregister<SM_Mode.GetKillCount>(OnGetKillCountCallBack);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLoginCallBack);
            MsgRegister.Unregister<SM_Mode.GetCharacterList>(OnSMGetCharacterListCallBack);
            MsgRegister.Unregister<SM_ShortcutTool.Event_GameOver>(OnShortcutToolGameOverCallBack);
            MsgRegister.Unregister<SM_ShortcutTool.Event_TeamScore>(OnShortcutToolTeamScoreCallBack);
            MsgRegister.Unregister<SM_Character.GetCharacterDataByPlayerId>(OnGetCharacterDataByPlayerIdCallBack);
            MsgRegister.Unregister<SM_Character.GetCharacterDataByGpoId>(OnGetCharacterDataByGpoIdCallBack);
            mySystem.Unregister<SE_Mode.Event_GetCharacterList>(OnGetCharacterListCallBack);
            mySystem.Unregister<SE_Mode.Event_ServiceQuitGame>(OnServiceQuitGameCallBack);
            mySystem.Unregister<SE_Mode.Event_GetGpoIdToPlayerIdDic>(OnGetGpoIdToPlayerIdDicCallBack);
            mySystem.Unregister<SE_Mode.Event_GetWinTeamList>(OnGetWinTeamListCallBack);
            mySystem.Unregister<SE_Mode.Event_OverGameMode>(OnOverGameModeCallBack);
            ClearCharacterData();
        }

        private void OnUpdate(float deltaTime) {
            RPCDownTime();
            switch (gameState) {
                case ModeData.GameStateEnum.Wait:
                    OnWait();
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    OnWaitStartDownTime();
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    OnWaitRoundStart();
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    OnRoundStart();
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                    OnWaitNextRound();
                    break;
                case ModeData.GameStateEnum.WaitModeOver:
                    OnWaitModeOver();
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    OnWaitSaveReport();
                    break;
                case ModeData.GameStateEnum.SaveReport:
                    OnWaitQuitAPP();
                    break;
                case ModeData.GameStateEnum.QuitApp:
                    OnDownTimeQuitAPP();
                    break;
            }
        }

        private void SetGameState(ModeData.GameStateEnum gameState) {
            Debug.Log("SetGameState: " + gameState);
            PerfAnalyzerAgent.SetTag(gameState.ToString());
            this.gameState = gameState;
            mySystem.Dispatcher(new SE_Mode.Event_GameState {
                GameState = gameState
            });
            MsgRegister.Dispatcher(new SM_Mode.SetGameState {
                GameState = gameState
            });
            ModeData.SetGameState(gameState);
            Rpc(new Proto_Mode.Rpc_GameState {
                gameState = (byte)gameState,
            });
        }

        /// <summary>
        /// 注意这块逻辑从 ServerGrpc 过来，会跑在子线程
        /// </summary>
        /// <param name="ent"></param>
        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            if (ModeData.ModeId == default || gameState != ModeData.GameStateEnum.None || teamDataDic != null) {
                return;
            }

            var teamInfos = ent.TeamInfos;

            Debug.Log("OnStartModeCallBack: " + teamInfos.Count);
            maxTeamNum = Mathf.FloorToInt(ModeData.PlayData.MaxRoleNum / ModeData.PlayData.MaxRoleNumPerTeam);
            characterDataList = new List<SE_Mode.PlayModeCharacterData>(ModeData.PlayData.MaxRoleNum);
            if (IsTestMode()) {
                InitTestTeamData();
            } else {
                InitTeamData(teamInfos);
            }
            SetGameState(ModeData.GameStateEnum.Wait);
            Debug.Log("模式启动时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Debug.Log("模式启动 【" + ModeData.ModeId + "】 - 最大队伍数量" + maxTeamNum + " - 最大人数:" +
                      ModeData.PlayData.MaxRoleNum + " - 每队最大人数:" + ModeData.PlayData.MaxRoleNumPerTeam);
        }

// ----------------------------------------  生成测试数据 Start ----------------------------------------------------------
        private void InitTestTeamData() {
            var needAddAIList = new List<SE_Mode.PlayModeCharacterData>();
            teamDataDic = new Dictionary<int, List<SE_Mode.PlayModeCharacterData>>(maxTeamNum);
            for (int i = 1; i <= maxTeamNum; i++) {
                var teamId = i;
                var list = new List<SE_Mode.PlayModeCharacterData>(ModeData.PlayData.MaxRoleNumPerTeam);
                teamDataDic.Add(teamId, list);
                for (int j = 0; j < ModeData.PlayData.MaxRoleNumPerTeam; j++) {
                    var data = new SE_Mode.PlayModeCharacterData();
                    data.PlayerId = aiPlayerId--;
                    data.TeamId = teamId;
                    data.IsAI = true;
                    data.AILevel = 1;
                    data.NickName = $"AI_{aiPlayerId}";
                    data.Avatar = "";
                    data.WeaponList = GetTestWeaponList();
                    data.AiConfig = new AiConfig {
                        JumpMaxIntervalTime = 4,
                        JumpMinIntervalTime = 8,
                        SlideMinIntervalTime = 6,
                        SlideMaxIntervalTime = 10,
                        WeaponHitRate = Random.Range(20, 40),
                        SuperWeaponHitRate = Random.Range(50, 70),
                    };
                    characterDataList.Add(data);
                    needAddAIList.Add(data);
                    list.Add(data);
                }
            }
            mySystem.Dispatcher(new SE_Mode.Event_OnSetCreateAIList {
                AIList = needAddAIList
            });
        }

        private List<SE_Mode.PlayModeCharacterWeapon> GetTestWeaponList() {
            var weaponList = new List<SE_Mode.PlayModeCharacterWeapon>();
            if (ModeData.WeaponSource == ModeData.WeaponSourceEnum.Room) {
                var weapon1 = WarData.TestWeaponId1 > 0 ? WarData.TestWeaponId1 : ItemSet.Id_Ak12;
                var weapon2 = WarData.TestWeaponId2 > 0 ? WarData.TestWeaponId2 : ItemSet.Id_FlashGun;
                var superWeapon = WarData.TestSuperWeaponId > 0 ? WarData.TestSuperWeaponId : ItemSet.Id_MachineGun;
                if (SceneData.TestSceneConfig) {
                    weapon1 = SceneData.TestSceneConfig.Weapon1Id;
                    weapon2 = SceneData.TestSceneConfig.Weapon2Id;
                    superWeapon = SceneData.TestSceneConfig.SuperWeaponId;
                }
                var weapon1Skin = WarData.TestWeaponSkinItemId1 > 0 ? WarData.TestWeaponSkinItemId1 : WeaponData.GetRandomWeaponSkinItemId(weapon1);
                var weapon2Skin = WarData.TestWeaponSkinItemId2 > 0 ? WarData.TestWeaponSkinItemId2 : WeaponData.GetRandomWeaponSkinItemId(weapon2);
                var superWeaponSkin = WarData.TestSuperWeaponSkinItemId > 0 ? WarData.TestSuperWeaponSkinItemId : WeaponData.GetRandomWeaponSkinItemId(superWeapon);
                for (int i = 0; i < 2; i++) {
                    var characterWeapon = new SE_Mode.PlayModeCharacterWeapon();
                    characterWeapon.WeaponSkinItemId = i == 0 ? weapon1Skin : weapon2Skin;
                    characterWeapon.WeaponItemId = i == 0 ? weapon1 : weapon2;
                    characterWeapon.Index = i + 1;
                    characterWeapon.RandAttack = 0;
                    characterWeapon.RandMagazineNum = 0;
                    characterWeapon.RandIntervalTime = 0;
                    characterWeapon.RandFireRange = 0;
                    characterWeapon.RandReloadTime = 0;
                    characterWeapon.RandFireOverHotTime = 0;
                    characterWeapon.RandAttackRange = 0f;
                    characterWeapon.RandBulletSpeed = 0;
                    characterWeapon.RandAddEffectFireDistanceRate = Random.Range(5f, 20f) * 0.01f;
                    characterWeapon.RandHp = 0;
                    characterWeapon.IsSuperWeapon = false;
                    characterWeapon.Level = WeaponSet.GetWeaponByItemId(characterWeapon.WeaponItemId).Quality;
                    if (characterWeapon.WeaponItemId == ItemSet.Id_Gatling) {
                        characterWeapon.RandFireOverHotTime = Random.Range(1f, 5f);
                    }
                    weaponList.Add(characterWeapon);
                }
                weaponList.Add(new SE_Mode.PlayModeCharacterWeapon {
                    WeaponItemId = superWeapon,
                    WeaponSkinItemId = superWeaponSkin,
                    Index = 3,
                    RandAttack = 0,
                    RandMagazineNum = 0,
                    RandIntervalTime = 0,
                    RandFireRange = 0,
                    RandReloadTime = 0,
                    RandBulletSpeed = 0,
                    RandAttackRange = 0,
                    RandAddEffectFireDistanceRate = 0,
                    RandHp = 0,
                    IsSuperWeapon = true,
                });
            }
            return weaponList;
        }

// ----------------------------------------  生成测试数据 END ----------------------------------------------------------
        public void InitTeamData(List<TeamInfo> teamInfos) {
            teamDataDic = new Dictionary<int, List<SE_Mode.PlayModeCharacterData>>();
            characterDataList = new List<SE_Mode.PlayModeCharacterData>();
            var needAddAIList = new List<SE_Mode.PlayModeCharacterData>();
            foreach (var teamInfo in teamInfos) {
                ProcessTeamInfo(teamInfo, needAddAIList);
            }
            if (mySystem == null) {
                Debug.LogError("MySystem is null");
                return;
            }
            mySystem.Dispatcher(new SE_Mode.Event_OnSetCreateAIList {
                AIList = needAddAIList
            });
        }

        private void ProcessTeamInfo(TeamInfo teamInfo, List<SE_Mode.PlayModeCharacterData> needAddAIList) {
            if (teamInfo == null || teamInfo.Players == null) {
                Debug.LogError("TeamInfo or Players is null");
                return;
            }
            if (teamDataDic.TryGetValue(teamInfo.TeamId, out var list) == false) {
                list = new List<SE_Mode.PlayModeCharacterData>();
                teamDataDic.Add(teamInfo.TeamId, list);
            }
            foreach (var playerInfo in teamInfo.Players) {
                var playerData = ProcessPlayerInfo(playerInfo, teamInfo.TeamId);
                if (playerData == null) continue;
                characterDataList.Add(playerData);
                list.Add(playerData);
                if (playerInfo.IsAi) {
                    needAddAIList.Add(playerData);
                }
            }
        }

        private SE_Mode.PlayModeCharacterData ProcessPlayerInfo(PlayerWarInfo playerInfo, int teamId) {
            if (playerInfo == null || playerInfo.Weapons == null) {
                Debug.LogError("PlayerInfo or Weapons is null");
                return null;
            }
            var characterWeapons = new List<SE_Mode.PlayModeCharacterWeapon>();
            if (ModeData.WeaponSource == ModeData.WeaponSourceEnum.Room) {
                var info = " Name:" + playerInfo.NickName + " weaponCount:" + playerInfo.Weapons.Count;
                foreach (var weaponInfo in playerInfo.Weapons) {
                    var wInfo = " ItemId:" + weaponInfo.ItemId;
                    if (weaponInfo.ItemId <= 0) {
                        continue;
                    }
                    var iteam = ItemData.GetData(weaponInfo.ItemId);
                    var isSuperWeapon = iteam.ItemTypeId == ItemTypeSet.Id_SuperWeapon;
                    var characterWeapon = new SE_Mode.PlayModeCharacterWeapon();
                    characterWeapon.WeaponSkinItemId = weaponInfo.SkinItemId;
                    characterWeapon.WeaponItemId = weaponInfo.ItemId;
                    characterWeapon.IsSuperWeapon = isSuperWeapon;
                    characterWeapon.Level = WeaponSet.GetWeaponByItemId(iteam.Id).Quality +
                                            Mathf.Max(weaponInfo.Attrs.Count - 1, 0);
                    characterWeapons.Add(characterWeapon);
                    for (int i = 0; i < weaponInfo.Attrs.Count; i++) {
                        var attr = weaponInfo.Attrs[i];
                        wInfo += "[ AttrId:" + attr.AttrId + " AttrValue:" + attr.AttrValue + "]";
                        switch (attr.AttrId) {
                            case WeaponAttributeSet.Id_FireRange:
                                characterWeapon.RandFireRange += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_Attack:
                                characterWeapon.RandAttack += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_BulletSpeed:
                                characterWeapon.RandBulletSpeed += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_IntervalTime:
                                characterWeapon.RandIntervalTime += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_EffectFireDistance:
                                characterWeapon.RandAddEffectFireDistanceRate += attr.AttrValue * 0.01f;
                                break;
                            case WeaponAttributeSet.Id_MagazineNum:
                                characterWeapon.RandMagazineNum += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_ReloadTime:
                                characterWeapon.RandReloadTime += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_Hp:
                                characterWeapon.RandHp += attr.AttrValue;
                                break;
                            case WeaponAttributeSet.Id_BombRange:
                                characterWeapon.RandAttackRange += attr.AttrValue * 0.01f;
                                break;
                            case WeaponAttributeSet.Id_FireOverHotTime:
                                characterWeapon.RandFireOverHotTime += attr.AttrValue * 0.001f;
                                break;
                            case WeaponAttributeSet.Id_FireDistance:
                                characterWeapon.RandFireDistance += attr.AttrValue * 0.01f;
                                break;
                            case WeaponAttributeSet.Id_MissileRange:
                                characterWeapon.RandMissileRange += attr.AttrValue * 0.01f;
                                break;
                            case WeaponAttributeSet.Id_MissileDuration:
                                characterWeapon.RandMissileDuration += attr.AttrValue * 0.001f;
                                break;
                            case WeaponAttributeSet.Id_DiffusionReduction:
                                characterWeapon.RandDiffusionReduction += attr.AttrValue * 0.1f;
                                break;
                        }
                    }
                    info += wInfo;
                }
                Debug.Log(info);
            }
            return new SE_Mode.PlayModeCharacterData {
                PlayerId = playerInfo.PlayerId,
                TeamId = teamId,
                IsAI = playerInfo.IsAi,
                AILevel = playerInfo.AiLevel,
                NickName = playerInfo.NickName,
                Avatar = playerInfo.Avatar,
                WeaponList = characterWeapons,
                AiConfig = playerInfo.AiConfig,
                WinLootBoxItemId = playerInfo.LootBoxItemId,
                GroupId = playerInfo.GroupId,
                RankScore = playerInfo.RankScore,
                HiddenScore = playerInfo.HiddenScore,
                originMatchId = playerInfo.OriginMatchId,
            };
        }

        private void ClearCharacterData() {
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.CharacterGPO == null) {
                    continue;
                }
                data.CharacterGPO?.Unregister<SE_GPO.Event_SetIsDead>(PlayerCharacterDead);
                data.CharacterGPO?.Unregister<SE_GPO.Event_SetHurtTOGpo>(PlayerHurtValue);
                data.CharacterGPO?.Unregister<SE_Skill.Event_SkillOver>(SkillOverValue);
            }
            characterDataList.Clear();
            teamDataDic.Clear();
        }

        private void OnModeMessageCallBack(SM_Mode.Event_ModeMessage ent) {
            if (gpoIdPlayerIdDic.ContainsKey(ent.mainGpoId) == false) {
                Debug.LogError($"OnModeMessageCallBack Error: mainGpoId not found  {ent.mainGpoId}");
                return;
            }
            var mainPlayerId = gpoIdPlayerIdDic[ent.mainGpoId];
            var mainPlayerData = GetCharacterData(mainPlayerId);
            mainPlayerData.KillCount++;
            SE_Mode.PlayModeCharacterData subPlayerData = null;
            if (ent.subGpoId > 0) {
                var subPlayerId = gpoIdPlayerIdDic[ent.subGpoId];
                subPlayerData = GetCharacterData(subPlayerId);
                if (!subPlayerData.IsAI) {
                    mainPlayerData.RealKillCount++;
                }
            }
            var mainText = "";
            var subText = "";
            switch (ent.MessageState) {
                case ModeData.MessageEnum.WeaponKillRole:
                    mainText = mainPlayerData.NickName;
                    subText = subPlayerData.NickName;
                    break;
                case ModeData.MessageEnum.SuperWeaponKillRole:
                    mainText = mainPlayerData.NickName;
                    subText = subPlayerData.NickName;
                    break;
            }
            Rpc(new Proto_Mode.Rpc_KillNum {
                killCount = (byte)mainPlayerData.KillCount, playerId = mainPlayerId,
            });
            Rpc(new Proto_Mode.Rpc_ModeMessage {
                mainText = mainText,
                subText = subText,
                messageState = (byte)ent.MessageState,
                itemId = (ushort)ent.ItemId,
                teamId = (byte)mainPlayerData.TeamId,
            });
        }

        private bool IsTestMode() {
#if UNITY_EDITOR
            return true;
#endif
            if (NetworkData.Config.IsStartClient || Application.isEditor) {
                return true;
            }
            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         模式登录状态检查
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// 添加 玩家 角色 （玩家登录触发）
        /// </summary>
        /// <param name="ent"></param>
        private void OnCheckCharacterLogin(SM_Mode.CheckCharacterLogin ent) {
            if (ent.WarId != WarData.WarId) {
                Debug.Log($"[Error] 登录 WarId 错误:  {ent.WarId} != {WarData.WarId}");
                ent.CallBack(ModeData.ModeLoginState.WarIdError);
                return;
            }
            if (ent.NetworkVersion != GameData.NetworkVersion) {
                Debug.Log($"[Error] 登录版本号错误: {ent.NetworkVersion} != {GameData.NetworkVersion}");
                ent.CallBack(ModeData.ModeLoginState.NetworkVersionError);
                return;
            }
            if (ent.ModeId != ModeData.ModeId) {
                Debug.Log($"[Error] 登录模式错误:  {ent.ModeId} != {ModeData.ModeId}");
                ent.CallBack(ModeData.ModeLoginState.ModeError);
                return;
            }
            if (gameState is ModeData.GameStateEnum.WaitModeOver or ModeData.GameStateEnum.ModeOver
                or ModeData.GameStateEnum.QuitApp) {
                Debug.Log($"[Error] 登录失败，战场已结束 {gameState}");
                ent.CallBack(ModeData.ModeLoginState.WarEnd);
                return;
            }
            var data = GetCharacterData(ent.PlayerId);
            if (data == null) {
                if (IsTestMode()) {
                    // 玩家登录后，自动替换尚未生成 GPO 的 AI 角色
                    var notCreateGPOAI = GetNotCreateGPOAI();
                    if (notCreateGPOAI == null) {
                        Debug.LogError("AI 数据不足，无法替换");
                        return;
                    }
                    notCreateGPOAI.IsAI = false;
                    notCreateGPOAI.PlayerId = ent.PlayerId;
                } else {
                    Debug.LogError($"CheckCharacterLogin Error: PlayerId {ent.PlayerId} not found");
                    ent.CallBack(ModeData.ModeLoginState.ModeCharacterListNull);
                    return;
                }
            }
            Debug.Log("CheckCharacterLogin Success: PlayerId " + ent.PlayerId);
            ent.CallBack(ModeData.ModeLoginState.Success);
        }

        private SE_Mode.PlayModeCharacterData GetNotCreateGPOAI() {
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.CharacterGPO == null && data.IsAI) {
                    return data;
                }
            }
            return null;
        }

        /// <summary>
        /// 玩家上传角色信息，创建 GPO 后触发
        /// </summary>
        /// <param name="ent"></param>
        private void OnAddCharacterCallBack(SM_Mode.AddCharacter ent) {
            Debug.Log($"OnAddCharacterCallBack  PlayerId {ent.PlayerId}");
            var data = GetCharacterData(ent.PlayerId);
            if (data == null) {
                Debug.LogError($"AddGPOCallBack Error: PlayerId {ent.PlayerId} not found");
                return;
            }
            if (data.CharacterGPO != null) {
                Debug.LogError($"AddGPOCallBack Error: PlayerId {ent.PlayerId} GPO already exists");
                return;
            }
            Debug.Log($"SetCharacterGPO  PlayerId {ent.PlayerId}");
            ent.CharacterGPO.Register<Event_SystemBase.SetNetwork>(OnSetNetworkCallBack);
            SetCharacterGPO(data, ent.CharacterGPO, ent.NickName);
        }
        
        private void OnGetCharacterDataByPlayerIdCallBack(SM_Character.GetCharacterDataByPlayerId ent) {
            var data = GetCharacterData(ent.PlayerId);
            if (data != null) {
                ent.CallBack.Invoke(data);
            } else {
                Debug.LogError($"get character data error no playerId {ent.PlayerId} data!");
            }
        }
        
        private void OnGetCharacterDataByGpoIdCallBack(SM_Character.GetCharacterDataByGpoId ent) {
            var data = GetCharacterData(ent.GpoId);
            if (data != null) {
                ent.CallBack.Invoke(data);
            } else {
                Debug.LogError($"get character data error no GpoId {ent.GpoId} data!");
            }
        }

        private void OnCharacterLoginCallBack(SM_Character.CharacterLogin ent) {
            if (gpoIdPlayerIdDic.ContainsKey(ent.GpoID) == false) {
                return;
            }
            TargetRpcGameDataForGPO(ent.GpoID, ent.INetwork);
        }
        
        private void TargetRpcGameDataForGPO(int gpoId, INetwork targetNetwork) {
            if (gpoIdPlayerIdDic.ContainsKey(gpoId) == false) {
                return;
            }
            TargetRpcGameState(targetNetwork);
            TargetRpcAllTeamData(targetNetwork);
            TargetRpcAllCharacterData(targetNetwork);
        }

        /// <summary>
        /// 添加 AI 角色
        /// </summary>
        /// <param name="ent"></param>
        private void OnAddAICharacterCallBack(SM_Mode.AddAICharacter ent) {
            var data = GetCharacterData(ent.PlayerId);
            if (data == null) {
                return;
            }
            SetCharacterGPO(data, ent.CharacterGPO, data.NickName);
            ent.CallBack(data.TeamId);
        }

        /// <summary>
        ///  刚登录的玩家下发所有当前角色数据
        /// </summary>
        /// <param name="targetNetwork"></param>
        private void TargetRpcAllCharacterData(INetwork targetNetwork) {
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                TargetRpc(targetNetwork,
                    new Proto_Mode.TargetRpc_PlayCharacterData {
                        gpoID = data.CharacterGPO == null ? 0 : data.CharacterGPO.GetGpoID(),
                        playerId = data.PlayerId,
                        teamID = data.TeamId,
                        score = data.Score,
                        NickName = data.NickName,
                        continueScore = data.ContinueScore,
                        hurtValue = data.HurtValue,
                        KillCount = (byte)data.KillCount,
                        avatarURL = data.Avatar,
                    });
            }
        }

        private void TargetRpcAllTeamData(INetwork targetNetwork) {
            // 同步队伍信息
            var targetRpc = new Proto_Mode.TargetRpc_TeamsWinCount {
                roundCount = currentRoundCount,
                teamIDs = new List<int>(teamDataDic.Count),
                winCounts = new List<byte>(teamDataDic.Count),
            };
            foreach (var kv in teamDataDic) {
                if (kv.Value.Count == 0) {
                    continue;
                }
                targetRpc.teamIDs.Add(kv.Key);
                targetRpc.winCounts.Add((byte)kv.Value[0].WinCount);
            }
            TargetRpc(targetNetwork, targetRpc);
        }

        private void RpcWinList(List<int> winTeamList) {
            Rpc(new Proto_Mode.Rpc_TeamWin {
                teamList = winTeamList,
            });
        }

        private void SetCharacterGPO(SE_Mode.PlayModeCharacterData data, IGPO igpo, string nickName) {
            data.CharacterGPO = igpo;
            data.NickName = nickName;
            mySystem.Dispatcher(new SE_Mode.Event_AddCharacterFinish {
                Data = data
            });
            igpo.Register<SE_GPO.Event_SetIsDead>(PlayerCharacterDead);
            igpo.Register<SE_GPO.Event_SetHurtTOGpo>(PlayerHurtValue);
            igpo.Register<SE_Skill.Event_SkillOver>(SkillOverValue);
            Rpc(new Proto_Mode.Rpc_PlayCharacterData {
                gpoID = data.CharacterGPO.GetGpoID(),
                playerId = data.PlayerId,
                nickName = data.NickName,
                teamID = data.TeamId,
                score = data.Score,
                continueScore = data.ContinueScore,
                hurtValue = data.HurtValue,
                KillCount = (byte)data.KillCount,
                avatarURL = data.Avatar,
            });
            gpoIdPlayerIdDic.Add(igpo.GetGpoID(), data.PlayerId);
            if (data.CharacterGPO.GetGPOType() == GPOData.GPOType.Role) {
                var network = data.CharacterGPO.GetNetwork();
                TargetRpcGameDataForGPO(igpo.GetGpoID(), network);
            }
            if (data.IsAI) {
                igpo.Dispatcher(new SE_GPO.Event_SetAIConfig {
                    Config = data.AiConfig,
                });
            }
            MsgRegister.Dispatcher(new SM_Mode.AddCharacterEnd {
                PlayerId = data.PlayerId,
                CharacterGPO = data.CharacterGPO,
            });
        }

        private void SkillOverValue(ISystemMsg body, SE_Skill.Event_SkillOver ent) {
            var playerId = gpoIdPlayerIdDic[ent.UseGPO.GetGpoID()];
            var data = GetCharacterData(playerId);
            data.SameTypeWeaponContinueScore = 0;
        }

        private void PlayerCharacterDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            var playerId = gpoIdPlayerIdDic[ent.DeadGpo.GetGpoID()];
            var data = GetCharacterData(playerId);
            if (data.IsAI == false && ent.IsDead) {
                if (data.ContinueScore > 1) {
                    ent.DeadGpo.Dispatcher(new SE_GPO.Event_ContinueKillNum() {
                        ContinueKillNum = data.ContinueScore,
                        GPO = ent.DeadGpo
                    });
                }
            }
            
            if (ent.IsDead) {
                data.DeadCount++;
            }
            data.ContinueScore = 0; // 死亡重置连杀次数
            data.SameTypeWeaponContinueScore = 0;
            data.CharacterGPO.Dispatcher(new SE_Skill.Event_SetSkillPoint {
                GetSkillPointType = SkillData.GetSkillPointType.ContinueGetModePoint, Value = data.ContinueScore,
            });
        }

        private void PlayerHurtValue(ISystemMsg body, SE_GPO.Event_SetHurtTOGpo ent) {
            var playerId = gpoIdPlayerIdDic[ent.AttackGPO.GetGpoID()];
            var data = GetCharacterData(playerId);
            data.HurtValue += ent.HurtValue;
            if (data.IsAI == false) {
                TargetRpc(data.CharacterGPO.GetNetwork(), new Proto_Mode.TargetRpc_HurtValue {
                    playerId = data.PlayerId, hurtValue = data.HurtValue,
                });
            }
        }

        /// <summary>
        /// 监听是否进行了网络重连
        /// </summary>
        private void OnSetNetworkCallBack(ISystemMsg body, Event_SystemBase.SetNetwork ent) {
            TargetRpcGameState(ent.network);
        }

        private void TargetRpcGameState(INetwork network) {
            TargetRpc(network, new Proto_Mode.TargetRpc_GameState {
                gameState = (byte)gameState,
            });
        }

        private void OnGetTeamIdCallBack(SM_Mode.GetTeamId ent) {
            var data = GetCharacterData(ent.PlayerId);
            if (data == null) {
                Debug.LogError($"GetTeamIdCallBack Error: PlayerId {ent.PlayerId} not found");
                return;
            }
            ent.CallBack(data.TeamId);
        }

        private void OnGetKillCountCallBack(SM_Mode.GetKillCount ent) {
            if (!gpoIdPlayerIdDic.TryGetValue(ent.GpoId, out var playerId)) {
                Debug.LogError($"OnGetKillCountCallBack Error: GpoId {ent.GpoId} not found");
                return;
            }
            var data = GetCharacterData(playerId);
            if (data == null) {
                Debug.LogError($"OnGetKillCountCallBack Error: PlayerId {playerId} not found");
                return;
            }
            ent.CallBack(data.KillCount);
        }
        private void OnSMGetCharacterListCallBack(SM_Mode.GetCharacterList list) {
            list.CallBack(characterDataList);
        }

        private void OnGetCharacterListCallBack(ISystemMsg body, SE_Mode.Event_GetCharacterList list) {
            list.CallBack(characterDataList);
        }
        
        private void OnGetGpoIdToPlayerIdDicCallBack(ISystemMsg body, SE_Mode.Event_GetGpoIdToPlayerIdDic ent) {
            ent.CallBack(gpoIdPlayerIdDic);
        }

        private void OnGetWinTeamListCallBack(ISystemMsg body, SE_Mode.Event_GetWinTeamList ent) {
            ent.CallBack(winTeamList);
        }

        private void OnOverGameModeCallBack(ISystemMsg body, SE_Mode.Event_OverGameMode ent) {
            switch (gameState) {
                case ModeData.GameStateEnum.WaitModeOver:
                case ModeData.GameStateEnum.ModeOver:
                case ModeData.GameStateEnum.QuitApp:
                    return;
            }
            WaitModeOver(ent.WinTeamList, ent.TriggerData);
        }

        private SE_Mode.PlayModeCharacterData GetCharacterData(long playerId) {
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.PlayerId == playerId) {
                    return data;
                }
            }
            return null;
        }

        /// <summary>
        /// 回合开始后才会有数据
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private List<SE_Mode.PlayModeCharacterData> GetTeamList(int teamId) {
            List<SE_Mode.PlayModeCharacterData> list;
            if (teamDataDic.TryGetValue(teamId, out list) == false) {
                Debug.LogError("GetTeamList Error: TeamId " + teamId + " not found");
            }
            return list;
        }

        private int GetTeamNum(int teamId) {
            List<SE_Mode.PlayModeCharacterData> list;
            if (teamDataDic.TryGetValue(teamId, out list) == false) {
                return 0;
            }
            return list.Count;
        }

        private bool HasTeam(int teamId) {
            return teamDataDic.ContainsKey(teamId);
        }

        /// <summary>
        /// 同步各个阶段的倒计时 (1 秒同步一次)
        /// </summary>
        private void RPCDownTime() {
            if (gameState == ModeData.GameStateEnum.None) {
                return;
            }
            var time = (ushort)Mathf.FloorToInt(downTime);
            if (prevSyncDownTime == time) {
                return;
            }
            prevSyncDownTime = time;
            Rpc(new Proto_Mode.Rpc_GameDownTime {
                downTime = time
            });
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         等待游戏开始
        /// -------------------------------------------------------------------------------------------------

        // 等待达成最小开启游戏的队伍数量，期间玩家可以通过邀请或服务端自动补人方式达成最小开启游戏的队伍数量
        private void OnWait() {
            var teamNum = CheckHasLoginPlayerTeam();
            if (teamNum >= ModeData.PlayData.MinStartModeTeamNum) {
                SetGameState(ModeData.GameStateEnum.WaitStartDownTime);
                downTime = ModeData.PlayData.StartModeDownTime;
            }
        }

        // 倒计时结束才开始游戏
        private void OnWaitStartDownTime() {
            if (downTime <= 0) {
                WaitRoundStart();
            } else {
                downTime -= Time.deltaTime;
            }
        }

        // 检查有多少队伍目前有玩家进入
        private int CheckHasLoginPlayer() {
            var playerNum = 0;
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.CharacterGPO != null && data.IsAI == false) {
                    playerNum++;
                }
            }
            return playerNum;
        }

        // 检查进入玩家的队伍有多少
        private int CheckHasLoginPlayerTeam() {
            var teamNum = 0;
            foreach (var teams in teamDataDic.Values) {
                for (int i = 0; i < teams.Count; i++) {
                    var data = teams[i];
                    if (data.CharacterGPO != null && data.IsAI == false) {
                        teamNum++;
                    }
                }
            }
            return teamNum;
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         进入回合开始的等待状态
        /// -------------------------------------------------------------------------------------------------
        private void WaitRoundStart() {
            SetGameState(ModeData.GameStateEnum.WaitRoundStart);
            downTime = ModeData.PlayData.StartRoundDownTime;
        }

        // 开始回合等待倒计时
        private void OnWaitRoundStart() {
            downTime -= Time.deltaTime;
            if (downTime <= 0) {
                RoundStart();
            }
        }

        private void RoundStart() {
            currentRoundCount++;
            SetGameState(ModeData.GameStateEnum.RoundStart);
            downTime = ModeData.PlayData.RoundTime;
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         回合开始
        /// -------------------------------------------------------------------------------------------------
        private void OnRoundStart() {
            UpdateRoundTime();
        }

        /// <summary>
        /// 根据回合时间更新回合状态
        /// </summary>
        private void UpdateRoundTime() {
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                downTime = -1;
                RoundEnd(GetMaxScoreTeam(), null);
            }
        }

        /// <summary>
        /// 获取积分
        /// </summary>
        /// <param name="ent"></param>
        private void OnAddScoreCallBack(SM_Mode.AddScore ent) {
            if (gameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            if (gpoIdPlayerIdDic.ContainsKey(ent.GpoId) == false) {
                return;
            }
            var addScore = GetScoreForChannel(ent.Channel);
            AddScore(ent.GpoId, addScore, ent.AttackItemId);
        }

        private void AddScore(int gpoId, int addScore, int attackItemId) {
            var playerId = gpoIdPlayerIdDic[gpoId];
            var data = GetCharacterData(playerId);
            if (data == null || addScore == 0) {
                return;
            }
            data.Score += addScore;
            if (data.CharacterGPO.IsDead() == false) {
                data.ContinueScore += addScore;
                data.SameTypeWeaponContinueScore += addScore;
                data.CharacterGPO.Dispatcher(new SE_GPO.Event_CheckOneShotKillTwoTime { });
            }
            mySystem.Dispatcher(new SE_Mode.Event_SendCharacterScore {
                Data = data, Score = data.Score,
            });
            data.CharacterGPO.Dispatcher(new SE_Skill.Event_SetSkillPoint {
                GetSkillPointType = SkillData.GetSkillPointType.GetModePoint, Value = addScore,
            });
            
            data.CharacterGPO.Dispatcher(new SE_Skill.Event_SetSkillPoint {
                GetSkillPointType = SkillData.GetSkillPointType.ContinueGetModePoint, Value = data.ContinueScore,
            });
            Rpc(new Proto_Mode.Rpc_Score {
                playerId = data.PlayerId, score = data.Score, continueScore = data.ContinueScore,
            });
            switch (ModeData.PlayData.RoundWinState) {
                case ModeData.RoundWinStateEnum.PersonalScoreTop:
                    CheckRoundPersonalScore(data);
                    break;
                
                case ModeData.RoundWinStateEnum.TeamScoreTop:
                    CheckRoundTeamScore(data, attackItemId);
                    break;
            }
        }

        private int GetScoreForChannel(ModeData.GetScoreChannelEnum channel) {
            for (int i = 0; i < ModeData.PlayData.ScoreChannelDatas.Length; i++) {
                var data = ModeData.PlayData.ScoreChannelDatas[i];
                if (data.Channel == channel) {
                    return data.Score;
                }
            }
            return 0;
        }

        private void CheckRoundTeamScore(SE_Mode.PlayModeCharacterData getScoreCharacter,int attackItemId) {
            var countScore = 0;
            var teamList = GetTeamList(getScoreCharacter.TeamId);
            for (int i = 0; i < teamList.Count; i++) {
                var data = teamList[i];
                countScore += data.Score;
            }
            if (ModeData.PlayData.WinScore > 0 && countScore >= ModeData.PlayData.WinScore) {
                //上报最后一击
                GrpcLastKill(getScoreCharacter,attackItemId);
                RoundEnd(GetTeamList(getScoreCharacter.TeamId), getScoreCharacter);
            }
        }

        private void CheckRoundPersonalScore( SE_Mode.PlayModeCharacterData getScoreCharacter) {
            if (ModeData.PlayData.WinScore > 0 && getScoreCharacter.Score >= ModeData.PlayData.WinScore) {
                RoundEnd(GetTeamList(getScoreCharacter.TeamId), getScoreCharacter);
            }
        }

        /// <summary>
        /// 根据回合结算条件获得获胜的队伍 (当前回合)
        /// </summary>
        /// <returns></returns>
        private List<SE_Mode.PlayModeCharacterData> GetMaxScoreTeam() {
            if (ModeData.PlayData.RoundWinState == ModeData.RoundWinStateEnum.PersonalScoreTop) {
                return GetMaxPersonalScoreList();
            }
            return GetMaxTeamScoreList();
        }

        // 获取当前队伍总分最高的队伍
        private List<SE_Mode.PlayModeCharacterData> GetMaxTeamScoreList() {
            var teamScoreDic = new Dictionary<int, List<SE_Mode.PlayModeCharacterData>>();
            var maxScoreTeamId = 0;
            var maxScore = 0;
            foreach (var teamId in teamDataDic.Keys) {
                var teamList = teamDataDic[teamId];
                var teamScore = 0;
                for (int i = 0; i < teamList.Count; i++) {
                    var data = teamList[i];
                    teamScore += data.Score;
                }
                List<SE_Mode.PlayModeCharacterData> list;
                if (teamScoreDic.TryGetValue(teamScore, out list) == false) {
                    list = new List<SE_Mode.PlayModeCharacterData>();
                    teamScoreDic.Add(teamScore, list);
                }
                list.AddRange(teamList);
            }
            // 根据 key 从大到小排序
            var keys = new List<int>(teamScoreDic.Keys);
            keys.Sort((a, b) => b.CompareTo(a));
            // 获取最大分数
            var maxScoreList = teamScoreDic[keys[0]];
            return maxScoreList;
        }

        // 获取当前个人总分最高的个人
        private List<SE_Mode.PlayModeCharacterData> GetMaxPersonalScoreList() {
            var teamScoreDic = new Dictionary<int, List<SE_Mode.PlayModeCharacterData>>();
            var maxScoreTeamId = 0;
            var maxScore = 0;
            foreach (var teamId in teamDataDic.Keys) {
                var teamList = teamDataDic[teamId];
                for (int i = 0; i < teamList.Count; i++) {
                    var data = teamList[i];
                    List<SE_Mode.PlayModeCharacterData> list;
                    if (teamScoreDic.TryGetValue(data.Score, out list) == false) {
                        list = new List<SE_Mode.PlayModeCharacterData>();
                        teamScoreDic.Add(data.Score, list);
                    }
                    list.Add(data);
                }
            }
            // 根据 key 从大到小排序
            var keys = new List<int>(teamScoreDic.Keys);
            keys.Sort((a, b) => b.CompareTo(a));
            // 获取最大分数
            var maxScoreList = teamScoreDic[keys[0]];
            return maxScoreList;
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         回合结束
        /// -------------------------------------------------------------------------------------------------
        // 回合结束
        private void RoundEnd(List<SE_Mode.PlayModeCharacterData> roundWinList, SE_Mode.PlayModeCharacterData triggerData) {
            SetGameState(ModeData.GameStateEnum.RoundEnd);
            RoundSettlement();
            var teamList = GetTeamList(roundWinList);
            var maxWinCount = AddWinCount(teamList, roundWinList);
            RpcWinList(teamList);
            if (maxWinCount < ModeData.PlayData.ModeWinRoundCount &&
                currentRoundCount < ModeData.PlayData.MaxRoundCount) {
                WaitNextRound();
            } else {
                string winInfo;
                if (triggerData != null) {
                    winInfo = "最终触发结束的玩家:" + triggerData.NickName + "\n";
                } else {
                    winInfo = "由时间触发结束" + "\n";
                }
                // 本回合获胜不一定是最终胜利
                var winTeamList = CheckWinTeamList();
                for (int i = 0; i < winTeamList.Count; i++) {
                    winInfo += "胜利队伍:" + winTeamList[i] + "\n";
                }
                Debug.Log(winInfo);
                WaitModeOver(winTeamList, triggerData);
            }
        }

        // 回合结算
        private void RoundSettlement() {
            if (characterDataList == null) {
                return;
            }
            var info = "";
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                data.AllScore += data.Score;
                if (data.ContinueScore > 1) {
                    data.CharacterGPO.Dispatcher(new SE_GPO.Event_ContinueKillNum() {
                        ContinueKillNum = data.ContinueScore,
                        GPO = data.CharacterGPO
                    });
                }
                data.Score = 0;
                data.ContinueScore = 0;
                data.SameTypeWeaponContinueScore = 0;
                info += "TeamId" + data.TeamId + " - 玩家：" + data.NickName + " - 得分：" + data.AllScore + "\n";
            }
            Debug.Log("回合结算：" + info);
        }

        private List<int> GetTeamList(List<SE_Mode.PlayModeCharacterData> teamList) {
            List<int> result = new List<int>();
            foreach (var playData in teamList) {
                if (result.Contains(playData.TeamId)) {
                    continue;
                }
                result.Add(playData.TeamId);
            }
            return result;
        }

        // 增加胜利次数
        private int AddWinCount(List<int> winTeamList, List<SE_Mode.PlayModeCharacterData> winList) {
            var maxWinCount = -1;
            for (int i = 0; i < winList.Count; i++) {
                var data = winList[i];
                // 平局不加分
                if (winTeamList.Count == 1) {
                    data.WinCount++;
                }
                if (maxWinCount <= data.WinCount) {
                    maxWinCount = data.WinCount;
                }
            }
            return maxWinCount;
        }

        // 获取获胜次数最多的队伍 (总回合)
        private List<int> CheckWinTeamList() {
            var teamWinDic = new Dictionary<int, List<int>>();
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                List<int> teamList;
                if (teamWinDic.TryGetValue(data.WinCount, out teamList) == false) {
                    teamList = new List<int>();
                    teamWinDic.Add(data.WinCount, teamList);
                }
                if (teamList.Contains(data.TeamId) == false) {
                    teamList.Add(data.TeamId);
                }
            }
            // 根据 key 从大到小排序
            var keys = new List<int>(teamWinDic.Keys);
            keys.Sort((a, b) => b.CompareTo(a));
            return teamWinDic[keys[0]];
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         等待进入下个回合
        /// -------------------------------------------------------------------------------------------------

        // 等待进入下个回合
        private void WaitNextRound() {
            SetGameState(ModeData.GameStateEnum.WaitNextRound);
            downTime = ModeData.PlayData.WaitNextRoundTime;
        }

        private void OnWaitNextRound() {
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                downTime = -1;
                WaitRoundStart();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         等待模式结束
        /// -------------------------------------------------------------------------------------------------
        private void WaitModeOver(List<int> winTeamList, SE_Mode.PlayModeCharacterData triggerData) {
            this.winTeamList = winTeamList;
            Debug.Log("模式结束 - 胜利队伍数量：" + this.winTeamList.Count);
            SetGameState(ModeData.GameStateEnum.WaitModeOver);
            RpcWarEndTeamList(winTeamList, triggerData);
            GrpcWarEndTeamList(winTeamList);
            downTime = ModeData.PlayData.WaitModeOverTime;
        }

        private void GrpcLastKill(SE_Mode.PlayModeCharacterData getScoreCharacter,int attackItemId) {
            if (attackItemId < 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Mode.Event_PlayerLastKill() {
                lastKillCharacter = getScoreCharacter,
                AttackItemId = attackItemId
            });
        }

        private void RpcWarEndTeamList(List<int> winTeamList, SE_Mode.PlayModeCharacterData triggerData) {
            var triggerPlayerId = triggerData == null ? 0 : triggerData.PlayerId;
            var rpcList = new List<INetworkCharacter>(characterDataList.Count);
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.IsAI == false && data.CharacterGPO != null) {
                    rpcList.Add((INetworkCharacter)data.CharacterGPO.GetNetwork());
                }
            }
            TargetRpcList(rpcList, new Proto_Mode.TargetRpc_VSModeWarEndWinTeamList {
                teamList = winTeamList, triggerPlayerId = triggerPlayerId
            });
        }

        private void GrpcWarEndTeamList(List<int> winTeamList) {
            var winCharacterList = new List<SM_Mode.WarResultData>();
            var loseCharacterList = new List<SM_Mode.WarResultData>();
            int maxKillCount = -1;
            int maxHurtValue = -1;
            SE_Mode.PlayModeCharacterData mvpPlayer = default;
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data.IsAI == false) {
                    var dropItems = GetDropItems(data);
                    var carryItems = new RepeatedField<int>();
                    for (int j = 0; j < data.WeaponList.Count; j++) {
                        var weaponData = data.WeaponList[j];
                        carryItems.Add(weaponData.WeaponItemId);
                        if (weaponData.WeaponSkinItemId != 0) {
                            carryItems.Add(weaponData.WeaponSkinItemId);
                        }
                    }
                    CheckIsCharacterTeamUp(data);
                    if (winTeamList.Contains(data.TeamId)) {
                        if (winTeamList.Count == 1 && winTeamList.Contains(data.TeamId)
                                                   && ModeData.PlayMode != ModeData.ModeEnum.Mode1V1) {
                            if (data.WinLootBoxItemId > 0) {
                                dropItems.Insert(0, new CommonItem() {
                                    ItemId = data.WinLootBoxItemId, ItemNum = 1
                                });
                            }
                        }
                        winCharacterList.Add(new SM_Mode.WarResultData() {
                            playerId = data.PlayerId,
                            deathCount = data.DeadCount,
                            killCount = data.KillCount,
                            GroupId = data.GroupId,
                            items = dropItems,
                            isMvp = false,
                            originMatchId = data.originMatchId,
                            damage = data.HurtValue,
                            slideTimes = data.SlideNum,
                            switchWeaponTimes = data.SwitchWeaponNum,
                            jumpTimes = data.JumpNum,
                            airJumpTimes = data.AirJumpNum,
                            superWeaponUseTimes = data.SuperWeaponUseTime,
                            carryItems = carryItems,
                            isTeamUp = data.IsTeamUp,
                        });
                    } else {
                        loseCharacterList.Add(new SM_Mode.WarResultData() {
                            playerId = data.PlayerId,
                            deathCount = data.DeadCount,
                            killCount = data.KillCount,
                            GroupId = data.GroupId,
                            items = dropItems,
                            isMvp = false,
                            originMatchId = data.originMatchId,
                            damage = data.HurtValue,
                            slideTimes = data.SlideNum,
                            switchWeaponTimes = data.SwitchWeaponNum,
                            jumpTimes = data.JumpNum,
                            airJumpTimes = data.AirJumpNum,
                            superWeaponUseTimes = data.SuperWeaponUseTime,
                            carryItems = carryItems,
                            isTeamUp = data.IsTeamUp,
                        });
                    }
                }
                if (winTeamList.Contains(data.TeamId)) {
                    if (data.KillCount > maxKillCount ||
                        (data.KillCount == maxKillCount && data.HurtValue > maxHurtValue)) {
                        maxKillCount = data.KillCount;
                        maxHurtValue = data.HurtValue;
                        mvpPlayer = data;
                    }
                }
            }
            SetMvp(ref winCharacterList, mvpPlayer.PlayerId);
            MsgRegister.Dispatcher(new SM_Mode.Event_WarEndTeamPlayerList {
                WinTeamList = winCharacterList, LoseTeamList = loseCharacterList,
            });
        }

        private void CheckIsCharacterTeamUp(SE_Mode.PlayModeCharacterData data) {
            long playerGroupId = data.GroupId;
            for (int i = 0; i < characterDataList.Count; i++) {
                SE_Mode.PlayModeCharacterData characterData = characterDataList[i];
                if (characterData.IsAI == false && characterData.PlayerId != data.PlayerId && characterData.GroupId == playerGroupId) {
                    data.IsTeamUp = true;
                    return;
                }
            }
        }

        private void SetMvp(ref List<SM_Mode.WarResultData> winList, long pid) {
            for (var i = 0; i < winList.Count; i++) {
                var data = winList[i];
                if (pid == data.playerId) {
                    data.isMvp = true;
                    winList[i] = data;
                    break;
                }
            }
        }

        private RepeatedField<CommonItem> GetDropItems(SE_Mode.PlayModeCharacterData characterData) {
            var awardDic = new Dictionary<int, CommonItem>();
            var dropList = new List<int>();
            if (characterData.CharacterGPO != null) {
                characterData.CharacterGPO.Dispatcher(new SE_Item.Event_GetDropItemList {
                    CallBack = drops => {
                        dropList = drops;
                    }
                });
            }
            foreach (int dropItemId in dropList) {
                if (!awardDic.TryGetValue(dropItemId, out var award)) {
                    award = new CommonItem {
                        ItemId = dropItemId
                    };
                }
                award.ItemNum++;
                awardDic[dropItemId] = award;
            }
            return new RepeatedField<CommonItem>() {
                awardDic.Values.ToList()
            };
        }

        private void OnWaitModeOver() {
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                downTime = -1;
                ModeOver();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         模式结束
        /// -------------------------------------------------------------------------------------------------
        // 模式结束
        private void ModeOver() {
            Debug.Log("模式结束");
            PerfAnalyzerAgent.SetLog("忽略 RPC 子弹数量:" + SAB_BulletSystem.IgnoreRpcBulletCount);
            RenderWarEndData();
            // ReportWarEnd();
            ChangeModeOverState();
        }

        private void OnServiceQuitGameCallBack(ISystemMsg body, SE_Mode.Event_ServiceQuitGame ent) {
            RoundSettlement();
            ChangeModeOverState();
        }

        private void ChangeModeOverState() {
            SetGameState(ModeData.GameStateEnum.ModeOver);
            downTime = 2f;
        }

        // 整理结算数据下发
        private void RenderWarEndData() {
            var battlePlayerList = GetBattlePlayerList();
            var info = "";
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                if (data != null && data.IsAI == false && data.CharacterGPO != null) {
                    var dropList = new List<int>();
                    data.CharacterGPO.Dispatcher(new SE_Item.Event_GetDropItemList {
                        CallBack = drops => {
                            dropList = drops;
                        }
                    });
                    if (winTeamList.Count == 1 && winTeamList.Contains(data.TeamId)) {
                        if (data.WinLootBoxItemId > 0 && ModeData.PlayMode != ModeData.ModeEnum.Mode1V1) {
                            dropList.Insert(0, data.WinLootBoxItemId);
                        }
                        info += $"{data.NickName} - 掉落数量 {dropList.Count} - 胜利宝箱 {data.WinLootBoxItemId}, ";
                    } else {
                        info += $"{data.NickName} - 掉落数量 {dropList.Count}";
                    }
                    TargetRpc(data.CharacterGPO.GetNetwork(), new Proto_Mode.TargetRpc_VSModeWarEnd {
                        dropitemList = dropList, battlePlayerDatas = battlePlayerList
                    });
                }
            }
            Debug.Log(info);
        }

        private List<Proto_Mode.BattlePlayerData> GetBattlePlayerList() {
            var list = new List<Proto_Mode.BattlePlayerData>(characterDataList.Count);
            for (int i = 0; i < characterDataList.Count; i++) {
                var data = characterDataList[i];
                var playerData = new Proto_Mode.BattlePlayerData {
                    playerId = data.PlayerId,
                    killCount = data.KillCount,
                    AllScore = data.AllScore,
                    deadNum = data.DeadCount,
                    hurtValue = data.HurtValue,
                };
                list.Add(playerData);
            }
            return list;
        }

        private void OnWaitSaveReport() {
            if (downTime <= -1f) {
                return;
            }
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                SaveReport();
            }
        }
        /// -------------------------------------------------------------------------------------------------
        ///                                         保存报告
        /// -------------------------------------------------------------------------------------------------
        private void SaveReport() {
            downTime = 1f;
            Debug.Log("保存报告");
            SetGameState(ModeData.GameStateEnum.SaveReport);
#if BUILD_SERVER
            PerfAnalyzerAgent.SaveReport();
#endif
        }

        private void OnWaitQuitAPP() {
            if (downTime <= -1f) {
                return;
            }
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                SetGameState(ModeData.GameStateEnum.QuitApp);
                downTime = 3f;
            }
        }
        /// -------------------------------------------------------------------------------------------------
        ///                                         关闭进程
        /// -------------------------------------------------------------------------------------------------
        private void OnDownTimeQuitAPP() {
            if (downTime <= -1f) {
                return;
            }
            downTime -= Time.deltaTime;
            if (downTime <= 0f) {
                QuitAPP();
            }
        }

        // 模式结束
        private void QuitAPP() {
#if BUILD_SERVER
            Debug.Log("关闭游戏");
            Application.Quit();
#endif
        }
        /// -------------------------------------------------------------------------------------------------
        ///                                         测试工具
        /// -------------------------------------------------------------------------------------------------
        
        // 测试工具回合结束
        private void OnShortcutToolGameOverCallBack (SM_ShortcutTool.Event_GameOver ent) {
            if (gameState == ModeData.GameStateEnum.RoundEnd || gameState == ModeData.GameStateEnum.ModeOver || gameState == ModeData.GameStateEnum.QuitApp) {
                return;
            }
            if (gameState != ModeData.GameStateEnum.RoundStart) {
                RoundStart();
            }
            RoundEnd(GetMaxScoreTeam(), null);
        }
        
        // 测试工具直接调分
        private void OnShortcutToolTeamScoreCallBack(SM_ShortcutTool.Event_TeamScore ent) {
            if (gameState != ModeData.GameStateEnum.RoundStart) {
                Debug.LogError("测试加分 Error: GameState is not RoundStart");
                return;
            }
            var teamId = ent.TeamId;
            if (!HasTeam(teamId)) {
                Debug.LogError($"测试加分 Error: TeamId {teamId} not found");
                return;
            }
            var teamList = GetTeamList(teamId);
            for (int i = 0; i < teamList.Count; i++) {
                var data = teamList[i];
                if (data.CharacterGPO == null) {
                    continue;
                }
                AddScore(data.CharacterGPO.GetGpoID(), ent.TeamScore, -1);
                return;
            }
        }
    }
}