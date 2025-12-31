using System;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeReporter : ComponentBase {
        private long roundStartSeconds;
        private List<int> winTeamList;
        private Dictionary<int, long> gpoIdPlayerIdDic;
        private List<SE_Mode.PlayModeCharacterData> characterDataList;
        private readonly Dictionary<int, long> superWeaponSpawnDic = new Dictionary<int, long>();

        protected override void OnAwake() {
            MsgRegister.Register<SM_Mode.SuperWeaponDestroyed>(OnSuperWeaponDestroyed);
            MsgRegister.Register<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Register<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
            Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.SuperWeaponDestroyed>(OnSuperWeaponDestroyed);
            MsgRegister.Unregister<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
            MsgRegister.Unregister<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
            Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
        }

        private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
            switch (e.GameState) {
                case ModeData.GameStateEnum.Wait:
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                case ModeData.GameStateEnum.RoundStart:
                    RoundStart();
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                case ModeData.GameStateEnum.RoundEnd:
                case ModeData.GameStateEnum.WaitModeOver:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    ReportWarEnd();
                    ClearCharacterData();
                    break;
            }
        }

        private void RoundStart() {
            roundStartSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private void OnAddCharacterCallBack(SM_Mode.AddCharacter ent) {
            CheckGetCharacterList();
            var data = GetCharacterData(ent.PlayerId);
            var iGpo = data.CharacterGPO;
            iGpo.Register<SE_Skill.Event_UseSkill>(OnUseSkill);
            iGpo.Register<SE_Skill.Event_UseSkillSummonDriver>(OnUseSkillSummerDriver);
            iGpo.Register<SE_GPO.SetUseWeapon>(OnSetUseWeapon);
            iGpo.Register<SE_GPO.Event_Jump>(OnJump);
            iGpo.Register<SE_GPO.Event_Slide>(OnSlide);
            iGpo.Register<SE_GPO.Event_MoveDistance>(OnMoveDistance);
            ReportEnterWar(data);
        }

        private void CheckGetCharacterList() {
            if (characterDataList != null) {
                return;
            }
            Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = list => characterDataList = list
            });
            Dispatcher(new SE_Mode.Event_GetGpoIdToPlayerIdDic {
                CallBack = dic => gpoIdPlayerIdDic = dic
            });
        }

        private void ClearCharacterData() {
            if (characterDataList == null) {
                return;
            }
            foreach (var data in characterDataList) {
                var iGpo = data.CharacterGPO;
                if (iGpo != null) {
                    iGpo.Unregister<SE_Skill.Event_UseSkill>(OnUseSkill);
                    iGpo.Unregister<SE_Skill.Event_UseSkillSummonDriver>(OnUseSkillSummerDriver);
                    iGpo.Unregister<SE_GPO.SetUseWeapon>(OnSetUseWeapon);
                    iGpo.Unregister<SE_GPO.Event_Jump>(OnJump);
                    iGpo.Unregister<SE_GPO.Event_Slide>(OnSlide);
                    iGpo.Unregister<SE_GPO.Event_MoveDistance>(OnMoveDistance);
                }
            }
            characterDataList = null;
        }

        private void OnJump(ISystemMsg body, SE_GPO.Event_Jump ent) {
            var characterData = GetCharacterData(gpoIdPlayerIdDic[ent.GPOId]);
            characterData.JumpNum++;
            if (ent.JumpType == CharacterData.JumpType.AirJump) {
                characterData.AirJumpNum++;
            }
        }

        private void OnSlide(ISystemMsg body, SE_GPO.Event_Slide ent) {
            if (ent.IsSlide) {
                var characterData = GetCharacterData(gpoIdPlayerIdDic[ent.GPOId]);
                characterData.SlideNum++;
            }
        }

        private void OnMoveDistance(ISystemMsg body, SE_GPO.Event_MoveDistance ent) {
            var characterData = GetCharacterData(gpoIdPlayerIdDic[ent.GPOId]);
            characterData.MoveDistance += ent.moveDistance;
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

        private void OnModeMessageCallBack(SM_Mode.Event_ModeMessage ent) {
            if (gpoIdPlayerIdDic.ContainsKey(ent.mainGpoId) == false) {
                return;
            }
            if (gpoIdPlayerIdDic.ContainsKey(ent.subGpoId) == false) {
                return;
            }
            var mainPlayerId = gpoIdPlayerIdDic[ent.mainGpoId];
            var mainPlayerData = GetCharacterData(mainPlayerId);
            var subPlayerId = gpoIdPlayerIdDic[ent.subGpoId];
            var subPlayerData = GetCharacterData(subPlayerId);
            ReportPlayerKill(ent.mainGpoId, mainPlayerData, ent.ItemId, subPlayerData);
        }

        private string Format(float value) {
            return value.ToString("f2");
        }

        /// -------------------------------------------------------------------------------------------------
        ///                                         上报数据埋点
        /// -------------------------------------------------------------------------------------------------
        private void ReportEnterWar(SE_Mode.PlayModeCharacterData data) {
            int maxEquipLevel = ModeData.WeaponSource == ModeData.WeaponSourceEnum.Room ? data.WeaponList.Max(weapon => weapon.Level) : WeaponQualitySet.Quality_Red;
            var sameTeamRealPlayerNum =
                characterDataList.Count(character => character.TeamId == data.TeamId && !character.IsAI);
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportEnterWar {
                Pid = data.PlayerId,
                WarId = WarData.WarId,
                MatchId = ModeData.MatchId,
                Gmod = ModeData.ModeId,
                GMap = ModeData.SceneId,
                MapSide = data.TeamId,
                MaxEquipLevel = maxEquipLevel,
                RealPlayerNum = sameTeamRealPlayerNum
            });
        }

        private void ReportPlayerKill(int killGpoId, SE_Mode.PlayModeCharacterData killData, int killItemId, SE_Mode.PlayModeCharacterData beKillData) {
            if (killData.IsAI) {
                return;
            }
            var killGpo = killData.CharacterGPO;
            var beKillGpo = beKillData.CharacterGPO;
            var killPoint = killGpo.GetPoint();
            var beKillPoint = beKillGpo.GetPoint();
            var distance = Vector2.Distance(new Vector2(killPoint.x, killPoint.z), new Vector2(beKillPoint.x, beKillPoint.z));
            var weaponIndex = killData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == killItemId);
            var weaponItem = ItemData.GetData(killData.WeaponList[weaponIndex].WeaponItemId);
            float fightTime = 0;
            beKillGpo.Dispatcher(new SE_GPO.Event_GetGPOAttacDuration {
                AttackGPOId = killGpoId,
                CallBack = time => fightTime = time
            });
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportPlayerBeKill {
                Pid = killData.PlayerId,
                WarId = WarData.WarId,
                Gmod = ModeData.ModeId,
                GMap = ModeData.SceneId,
                GradeScore = killData.RankScore,
                HideScore = killData.HiddenScore,
                FightTime = Format(fightTime),
                FightTimeF = fightTime,
                KillLocationZ = Format(killPoint.z),
                KillLocationZF = killPoint.z,
                KillLocationX = Format(killPoint.x),
                KillLocationXF = killPoint.x,
                BeKillLocationZ = Format(beKillPoint.z),
                BeKillLocationZF = beKillPoint.z,
                BeKillLocationX = Format(beKillPoint.x),
                BeKillLocationXF = beKillPoint.x,
                KilledDistance = Format(distance),
                KilledDistanceF = distance,
                BeKillPid = beKillData.IsAI ? 0 : beKillData.PlayerId,
                MultiKillNum = killData.SameTypeWeaponContinueScore,
                DeadType = WeaponData.Get(weaponItem.Id).WeaponType.ToString(),
                DeadDetail = weaponItem.Sign,
                DeadDetailLevel = killData.WeaponList[weaponIndex].Level,
            });
        }

        private void OnUseSkill(ISystemMsg body, SE_Skill.Event_UseSkill ent) {
            var playerId = gpoIdPlayerIdDic[ent.UseGPOId];
            var data = GetCharacterData(playerId);
            // 切换武器类型需要重置
            data.SameTypeWeaponContinueScore = 0;
            if (data.IsAI) {
                return;
            }
            var superWeaponId = data.WeaponList.Find(weapon => weapon.IsSuperWeapon).WeaponItemId;
            data.SuperWeaponUseTime++;
            var skillData = SkillData.GetDataForItemId(superWeaponId);
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportPlayerUseSuperWeapon {
                Pid = data.PlayerId,
                WarId = WarData.WarId,
                Gmod = ModeData.ModeId,
                HideScore = data.HiddenScore,
                GradeScore = data.RankScore,
                SuperWeaponType = skillData.Sign,
            });
        }

        private void OnUseSkillSummerDriver(ISystemMsg body, SE_Skill.Event_UseSkillSummonDriver ent) {
            var playerId = gpoIdPlayerIdDic[ent.UseGPOId];
            var data = GetCharacterData(playerId);
            if (data.IsAI) {
                return;
            }
            superWeaponSpawnDic[ent.SummerDriverGPOId] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private void OnSuperWeaponDestroyed(SM_Mode.SuperWeaponDestroyed ent) {
            if (gpoIdPlayerIdDic.ContainsKey(ent.killGpoId) == false) {
                return;
            }
            var killData = GetCharacterData(gpoIdPlayerIdDic[ent.killGpoId]);
            var beKillData = GetCharacterData(gpoIdPlayerIdDic[ent.beKillMasterGpoId]);
            if (beKillData.IsAI) {
                return;
            }
            var killGpo = killData.CharacterGPO;
            var beKillGpo = beKillData.CharacterGPO;
            var killPoint = killGpo.GetPoint();
            var beKillPoint = beKillGpo.GetPoint();
            var distance = Vector2.Distance(new Vector2(killPoint.x, killPoint.z), new Vector2(beKillPoint.x, killPoint.z));
            var killWeaponIndex = killData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == ent.ItemId);
            var killWeaponItem = ItemData.GetData(killData.WeaponList[killWeaponIndex].WeaponItemId);
            var superWeapon = beKillData.WeaponList.Find(weapon => weapon.IsSuperWeapon);
            var superWeaponItem = ItemData.GetData(superWeapon.WeaponItemId);
            var duration = 0f;
            if (superWeaponSpawnDic.TryGetValue(ent.beKillGpoId, out var spawnTick)) {
                duration = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - spawnTick) / 1000f;
                superWeaponSpawnDic.Remove(ent.beKillGpoId);
            }
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportSuperWeaponDestroy {
                Pid = beKillData.PlayerId,
                WarId = WarData.WarId,
                Gmod = ModeData.ModeId,
                HideScore = beKillData.HiddenScore,
                GradeScore = beKillData.RankScore,
                SuperWeaponType = superWeaponItem.Sign,
                SuperWeaponDuration = Format(duration),
                SuperWeaponDurationF = duration,
                KillLocationZ = Format(killPoint.z),
                KillLocationZF = killPoint.z,
                KillLocationX = Format(killPoint.x),
                KillLocationXF = killPoint.x,
                BeKillLocationZ = Format(beKillPoint.z),
                BeKillLocationZF = beKillPoint.z,
                BeKillLocationX = Format(beKillPoint.x),
                BeKillLocationXF = beKillPoint.x,
                KilledDistance = Format(distance),
                KilledDistanceF = distance,
                BePid = killData.IsAI ? 0 : killData.PlayerId,
                DeadType = WeaponData.Get(killWeaponItem.Id).WeaponType.ToString(),
                DeadDetail = killWeaponItem.Sign,
                DeadDetailLevel = killData.WeaponList[killWeaponIndex].Level,
            });

        }

        private void OnSetUseWeapon(ISystemMsg body, SE_GPO.SetUseWeapon ent) {
            var characterData = GetCharacterData(gpoIdPlayerIdDic[ent.UseGPOId]);
            if (characterData.IsAI) {
                return;
            }
            characterData.SwitchWeaponNum += 1;
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportGunChange {
                Pid = characterData.PlayerId,
                WarId = WarData.WarId,
            });
        }

        private void ReportWarEnd() {
            if (characterDataList == null) {
                return;
            }
            try {
                CheckGetCharacterList();
                Dispatcher(new SE_Mode.Event_GetWinTeamList {
                    CallBack = list => winTeamList = list
                });
                var warDuration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - roundStartSeconds;
                var teamScoreDic = GetTeamScoreDict();
                foreach (var characterData in characterDataList) {
                    if (characterData.CharacterGPO == null || characterData.IsAI) {
                        continue;
                    }
                    // 战斗结果
                    var battleResult = GetTeamBattleResult(characterData.TeamId);
                    // 武器数据, 1v1 每回合会销毁武器，需要额外获取
                    var weaponList = ModeData.PlayMode == ModeData.ModeEnum.Mode1V1
                        ? GetWeaponResult(characterData.PlayerId, out int totalFireCount, out int totalHitCount)
                        : GetWeaponResult(characterData.WeaponList, out totalFireCount, out totalHitCount);
                    // 战利品
                    var awardDic = GetRewardInfoResult(characterData.CharacterGPO);
                    SendReportWarEnd(characterData, weaponList, awardDic, teamScoreDic, warDuration, battleResult, totalFireCount, totalHitCount);
                }
            } catch (Exception e) {
                Debug.LogError($"[ReportWarEnd] 异常，message : {e.Message}, stackTrace {e.StackTrace}");
            }
        }

        private Dictionary<int, int> GetTeamScoreDict() {
            var teamScoreDic = new Dictionary<int, int>();
            foreach (var data in characterDataList) {
                teamScoreDic.TryAdd(data.TeamId, 0);
                teamScoreDic[data.TeamId] += data.AllScore;
            }
            return teamScoreDic;
        }

        private int GetTeamBattleResult(int teamId) {
            int battleResult;
            if (winTeamList.Count == 0) {
                battleResult = 0;
            } else if (winTeamList.Contains(teamId)) {
                battleResult = 1;
            } else {
                battleResult = -1;
            }
            return battleResult;
        }

        // 随机武器结算
        private List<SM_FunnyDB.WeaponResultInfo> GetWeaponResult(long playerId, out int totalFireCount, out int totalHitCount) {
            totalFireCount = 0;
            totalHitCount = 0;
            List<SM_FunnyDB.WeaponResultInfo> result = null;
            Dispatcher(new SE_Mode.Event_GetWeaponResult {
                PlayerId = playerId,
                CallBack = list => result = list
            });
            foreach (var weaponResultInfo in result) {
                totalFireCount += weaponResultInfo.FireBullets;
                totalHitCount += weaponResultInfo.HitBullets;
            }
            return result;
        }

        // 房间武器结算
        private List<SM_FunnyDB.WeaponResultInfo> GetWeaponResult(List<SE_Mode.PlayModeCharacterWeapon> WeaponList, out int totalFireCount, out int totalHitCount) {
            totalHitCount = 0;
            totalFireCount = 0;
            var result = new List<SM_FunnyDB.WeaponResultInfo>();
            foreach (var weapon in WeaponList) {
                var weaponItem = ItemData.GetData(weapon.WeaponItemId);
                if (weapon.IsSuperWeapon) {
                    continue;
                }
                int fireCount = 0, hitCount = 0;
                if (weapon.iWeapon != null) {
                    weapon.iWeapon.Dispatcher(new SE_Weapon.Event_GetGunFireRecord {
                        CallBack = (count, hit) => {
                            fireCount = count;
                            hitCount = hit;
                        }
                    });
                } else {
                    Debug.LogError($"[GetWeaponResult] weapon.iWeapon is null, weaponItemId: {weapon.WeaponItemId}");
                }
                totalFireCount += fireCount;
                totalHitCount += hitCount;
                result.Add(new SM_FunnyDB.WeaponResultInfo {
                    Weapon = weaponItem.Sign,
                    WeaponLevel = weapon.Level,
                    FireBullets = fireCount,
                    HitBullets = hitCount
                });
            }
            return result;
        }

        private Dictionary<int, SM_FunnyDB.RewardInfo> GetRewardInfoResult(IGPO CharacterGPO) {
            var awardDic = new Dictionary<int, SM_FunnyDB.RewardInfo>();
            var dropList = new List<int>();
            if (CharacterGPO != null) {
                CharacterGPO.Dispatcher(new SE_Item.Event_GetDropItemList {
                    CallBack = drops => {
                        dropList = drops;
                    }
                });
            }
            foreach (int dropItemId in dropList) {
                if (!awardDic.TryGetValue(dropItemId, out var award)) {
                    award = new SM_FunnyDB.RewardInfo {
                        ItemId = dropItemId
                    };
                }
                award.ItemNum++;
                awardDic[dropItemId] = award;
            }
            return awardDic;
        }

        private void SendReportWarEnd(SE_Mode.PlayModeCharacterData characterData,
            List<SM_FunnyDB.WeaponResultInfo> weaponList,
            Dictionary<int, SM_FunnyDB.RewardInfo> awardDic,
            Dictionary<int, int> teamScoreDic,
            long warDuration,
            int battleResult,
            int totalFireCount,
            int totalHitCount) {
            MsgRegister.Dispatcher(new SM_FunnyDB.ReportWarEnd {
                Pid = characterData.PlayerId,
                WarId = WarData.WarId,
                MatchId = ModeData.MatchId,
                Gmod = ModeData.ModeId,
                GMap = ModeData.SceneId,
                HideScore = characterData.HiddenScore,
                GradeScore = characterData.RankScore,
                WarStartTime = roundStartSeconds,
                WarDuration = warDuration,
                BattleResult = battleResult,
                MapSide = characterData.TeamId,
                BattleScore = $"{teamScoreDic[characterData.TeamId]}:{teamScoreDic.First(kv => kv.Key != characterData.TeamId).Value}",
                FireGuns = weaponList,
                DamageNum = characterData.HurtValue,
                DeadNum = characterData.DeadCount,
                KillNum = characterData.KillCount,
                RealKillNum = characterData.RealKillCount,
                TotalBullets = totalFireCount,
                HitBullets = totalHitCount,
                MoveDistance = Format(characterData.MoveDistance),
                MoveDistanceF = characterData.MoveDistance,
                SlideNum = characterData.SlideNum,
                JumpNum = characterData.JumpNum,
                Award = awardDic.Values.ToList()
            });
        }
    }
}