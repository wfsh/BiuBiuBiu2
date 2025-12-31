using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerTaskReporter : ComponentBase {
    //     private long roundStartSeconds;
    //     private Dictionary<int, long> gpoIdPlayerIdDic;
    //     private List<SE_Mode.PlayModeCharacterData> characterDataList;
    //     private bool isFirstKill;
    //
    //     protected override void OnAwake() {
    //         MsgRegister.Register<SM_Mode.SuperWeaponDestroyed>(OnSuperWeaponDestroyed);
    //         MsgRegister.Register<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
    //         MsgRegister.Register<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
    //         MsgRegister.Register<SM_Mode.Event_PlayerLastKill>(OnPlayerLastKill);
    //         Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
    //     }
    //
    //     protected override void OnClear() {
    //         MsgRegister.Unregister<SM_Mode.SuperWeaponDestroyed>(OnSuperWeaponDestroyed);
    //         MsgRegister.Unregister<SM_Mode.AddCharacter>(OnAddCharacterCallBack);
    //         MsgRegister.Unregister<SM_Mode.Event_ModeMessage>(OnModeMessageCallBack);
    //         MsgRegister.Unregister<SM_Mode.Event_PlayerLastKill>(OnPlayerLastKill);
    //         Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
    //     }
    //
    //     private void OnPlayerLastKill(SM_Mode.Event_PlayerLastKill ent) {
    //         //上报伤害
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         var mainPlayerData = ent.lastKillCharacter;
    //         var mainPlayerId =mainPlayerData.PlayerId; 
    //         var weaponIndex = mainPlayerData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == ent.AttackItemId);
    //         var killWeapon = mainPlayerData.WeaponList[weaponIndex];
    //
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = mainPlayerId,
    //             SysJsonTypeId = 11,
    //             TaskJson = json
    //         });
    //     }
    //     private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
    //         switch (e.GameState) {
    //             case ModeData.GameStateEnum.Wait:
    //             case ModeData.GameStateEnum.WaitStartDownTime:
    //                 break;
    //             case ModeData.GameStateEnum.WaitRoundStart:
    //             case ModeData.GameStateEnum.RoundStart:
    //                 RoundStart();
    //                 break;
    //             case ModeData.GameStateEnum.WaitNextRound:
    //             case ModeData.GameStateEnum.RoundEnd:
    //             case ModeData.GameStateEnum.WaitModeOver:
    //                 break;
    //             case ModeData.GameStateEnum.ModeOver:
    //                 ClearCharacterData();
    //                 break;
    //         }
    //     }
    //
    //     private void RoundStart() {
    //         roundStartSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    //     }
    //
    //     private void OnAddCharacterCallBack(SM_Mode.AddCharacter ent) {
    //         CheckGetCharacterList();
    //         var data = GetCharacterData(ent.PlayerId);
    //         var iGpo = data.CharacterGPO;
    //         iGpo.Register<SE_GPO.Event_ContinueKillNum>(OnSetContinueKillNum);
    //         iGpo.Register<SE_GPO.Event_TypeDamageCount>(OnTypeDamageCount);
    //     }
    //
    //     private void CheckGetCharacterList() {
    //         if (characterDataList != null) {
    //             return;
    //         }
    //
    //         Dispatcher(new SE_Mode.Event_GetCharacterList {
    //             CallBack = list => characterDataList = list
    //         });
    //         Dispatcher(new SE_Mode.Event_GetGpoIdToPlayerIdDic {
    //             CallBack = dic => gpoIdPlayerIdDic = dic
    //         });
    //     }
    //
    //     private void ClearCharacterData() {
    //         if (characterDataList == null) {
    //             return;
    //         }
    //
    //         foreach (var data in characterDataList) {
    //             var iGpo = data.CharacterGPO;
    //             if (iGpo != null) {
    //                 iGpo.Unregister<SE_GPO.Event_ContinueKillNum>(OnSetContinueKillNum);
    //                 iGpo.Unregister<SE_GPO.Event_TypeDamageCount>(OnTypeDamageCount);
    //             }
    //         }
    //
    //         characterDataList = null;
    //     }
    //
    //     private SE_Mode.PlayModeCharacterData GetCharacterData(long playerId) {
    //         for (int i = 0; i < characterDataList.Count; i++) {
    //             var data = characterDataList[i];
    //             if (data.PlayerId == playerId) {
    //                 return data;
    //             }
    //         }
    //
    //         return null;
    //     }
    //
    //     private void OnTypeDamageCount(ISystemMsg body, SE_GPO.Event_TypeDamageCount ent) {
    //         //上报伤害
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         var mainPlayerId = gpoIdPlayerIdDic[ent.AttackGPO.GetGpoID()];
    //         var mainPlayerData = GetCharacterData(mainPlayerId);
    //         var weaponIndex = mainPlayerData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == ent.AttackItemId);
    //         var killWeapon = mainPlayerData.WeaponList[weaponIndex];
    //
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         hashData.Add("damage_type", (int)ent.DamageType);
    //         hashData.Add("damage_value", ent.Damage);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = mainPlayerId,
    //             SysJsonTypeId = 2,
    //             TaskJson = json
    //         });
    //     }
    //
    //     private void OnSetContinueKillNum(ISystemMsg body, SE_GPO.Event_ContinueKillNum ent) {
    //         //上报连杀
    //         bool isReload = false;
    //         bool isKillTwo = false;
    //         int killWeaponItemId = -1;
    //         ent.GPO.Dispatcher(new SE_GPO.Event_GetContinueKillState() {
    //             CallBack = (isContinueKillReload, isContinueKillOneShotKillTwo,weaponId) => {
    //                 isReload = isContinueKillReload;
    //                 isKillTwo = isContinueKillOneShotKillTwo;
    //                 killWeaponItemId = weaponId;
    //             }
    //         });
    //         if (killWeaponItemId  < 0) {
    //             return;
    //         }
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         var mainPlayerId = gpoIdPlayerIdDic[ent.GPO.GetGpoID()];
    //         var mainPlayerData = GetCharacterData(mainPlayerId);
    //         var weaponIndex = mainPlayerData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == killWeaponItemId);
    //         var killWeapon = mainPlayerData.WeaponList[weaponIndex];
    //
    //         bool isHurt = false;
    //         ent.GPO.Dispatcher(new SE_GPO.Event_GetHPInfo {
    //             CallBack = (nowHp,maxHp) => {
    //                 if (nowHp < maxHp) {
    //                     isHurt = true;
    //                 }
    //             }
    //         });
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         hashData.Add("kill_streal_cnt", ent.ContinueKillNum);
    //         hashData.Add("is_behurt", isHurt ? 1 : 0);
    //         
    //         hashData.Add("is_refill_bullet", isReload ? 1 : 0);
    //         hashData.Add("is_oneshot", isKillTwo ? 1 : 0);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = mainPlayerId,
    //             SysJsonTypeId = 12,
    //             TaskJson = json
    //         });
    //     }
    //     private void OnModeMessageCallBack(SM_Mode.Event_ModeMessage ent) {
    //         if (gpoIdPlayerIdDic.ContainsKey(ent.mainGpoId) == false) {
    //             return;
    //         }
    //
    //         if (gpoIdPlayerIdDic.ContainsKey(ent.subGpoId) == false) {
    //             return;
    //         }
    //
    //         var mainPlayerId = gpoIdPlayerIdDic[ent.mainGpoId];
    //         var mainPlayerData = GetCharacterData(mainPlayerId);
    //         var subPlayerId = gpoIdPlayerIdDic[ent.subGpoId];
    //         var subPlayerData = GetCharacterData(subPlayerId);
    //         mainPlayerData.CharacterGPO.Dispatcher(new SE_GPO.Event_SetKillWeaponId() {
    //             KillWeaponId = ent.ItemId
    //         });
    //         ReportPlayerKill(ent.mainGpoId, mainPlayerData, ent.ItemId, subPlayerData);
    //         if (isFirstKill == false) {
    //             isFirstKill = true;
    //             ReportFirstKill(ent.mainGpoId, mainPlayerData, ent.ItemId, subPlayerData);
    //         }
    //     }
    //
    //     private void ReportFirstKill(int killGpoId, SE_Mode.PlayModeCharacterData killData, int killItemId, SE_Mode.PlayModeCharacterData beKillData) {
    //         if (killData.IsAI) {
    //             return;
    //         }
    //         var killGpo = killData.CharacterGPO;
    //         var behaviourType = BehaviorState.Stand;
    //         var beKillGpo = beKillData.CharacterGPO;
    //         var killPoint = killGpo.GetPoint();
    //         var beKillPoint = beKillGpo.GetPoint();
    //         var distance = Vector3.Distance(killPoint, beKillPoint);
    //         var weaponIndex = killData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == killItemId);
    //         var killWeapon = killData.WeaponList[weaponIndex];
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         int killTime = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - roundStartSeconds);
    //         hashData.Add("within_time", killTime);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         //发送事件上报任务
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = killData.PlayerId,
    //             SysJsonTypeId = 10,
    //             TaskJson = json
    //         });
    //     }
    //
    //     //上报击杀任务
    //     private void ReportPlayerKill(int killGpoId, SE_Mode.PlayModeCharacterData killData, int killItemId, SE_Mode.PlayModeCharacterData beKillData) {
    //         if (killData.IsAI) {
    //             return;
    //         }
    //
    //         var killGpo = killData.CharacterGPO;
    //         bool isJump = false;
    //         bool isSlide = false;
    //         bool isMove = false;
    //         CharacterData.JumpType jumpType = CharacterData.JumpType.None;
    //         killGpo.Dispatcher(new SE_GPO.Event_GetState() {
    //             CallBack = (jump, slide, move, type) => {
    //                 isJump = jump;
    //                 isSlide = slide;
    //                 isMove = move;
    //                 jumpType = type;
    //             }
    //         });
    //         var behaviourType = BehaviorState.Stand;
    //         if (isJump) {
    //             switch (jumpType) {
    //                 case  CharacterData.JumpType.Jump:
    //                     behaviourType = BehaviorState.Jump;
    //                     break;
    //                 case CharacterData.JumpType.AirJump:
    //                     behaviourType = BehaviorState.Secjump;
    //                     break;
    //             }
    //         }
    //
    //         if (isSlide) {
    //             behaviourType = BehaviorState.Slipper;
    //         }
    //         var beKillGpo = beKillData.CharacterGPO;
    //         var killPoint = killGpo.GetPoint();
    //         var beKillPoint = beKillGpo.GetPoint();
    //         var distance = Vector3.Distance(killPoint, beKillPoint);
    //         var weaponIndex = killData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == killItemId);
    //         var killWeapon = killData.WeaponList[weaponIndex];
    //         // var weaponItem = ItemData.GetData(killData.WeaponList[weaponIndex].WeaponItemId);
    //         // float fightTime = 0;
    //         // beKillGpo.Dispatcher(new SE_GPO.Event_GetGPOAttacDuration {
    //         //     AttackGPOId = killGpoId,
    //         //     CallBack = time => fightTime = time
    //         // });
    //         int allDamage = 0;
    //         beKillGpo.Dispatcher(new SE_GPO.Event_GetGPOShortTimeDamage() {
    //             AttackGPOId = killGpoId,
    //             CallBack = damage => allDamage = damage
    //         });
    //         bool isOneshot = allDamage > 900;
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         hashData.Add("behavior_state", (int) behaviourType);
    //         // hashData.Add("kill_target", isHurt ? 1 : 0);
    //         hashData.Add("kill_target_type", (int)KillTargetType.Person);//击杀类型
    //         hashData.Add("kill_range", Mathf.CeilToInt(distance));
    //         hashData.Add("is_oneshot", isOneshot ? 1 : 0);
    //         hashData.Add("is_move", isMove ? 1 : 0);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         //发送事件上报任务
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = killData.PlayerId,
    //             SysJsonTypeId = 4,
    //             TaskJson = json
    //         });
    //     }
    //     
    //     
    //
    //     //击杀超武
    //     private void OnSuperWeaponDestroyed(SM_Mode.SuperWeaponDestroyed ent) {
    //         if (gpoIdPlayerIdDic.ContainsKey(ent.killGpoId) == false) {
    //             return;
    //         }
    //
    //         var killData = GetCharacterData(gpoIdPlayerIdDic[ent.killGpoId]);
    //         var beKillData = GetCharacterData(gpoIdPlayerIdDic[ent.beKillMasterGpoId]);
    //         if (killData.IsAI) {
    //             return;
    //         }
    //
    //         var killGpo = killData.CharacterGPO;
    //         var beKillGpo = beKillData.CharacterGPO;
    //         var killPoint = killGpo.GetPoint();
    //         var beKillPoint = beKillGpo.GetPoint();
    //         var distance = Vector2.Distance(new Vector2(killPoint.x, killPoint.z), new Vector2(beKillPoint.x, killPoint.z));
    //         var killWeaponIndex = killData.WeaponList.FindIndex(weapon => weapon.WeaponItemId == ent.ItemId);
    //         var killWeapon = killData.WeaponList[killWeaponIndex];
    //         // var killWeaponItem = ItemData.GetData(killData.WeaponList[killWeaponIndex].WeaponItemId);
    //         var superWeapon = beKillData.WeaponList.Find(weapon => weapon.IsSuperWeapon);
    //         // var superWeaponItem = ItemData.GetData(superWeapon.WeaponItemId);
    //         
    //         bool isJump = false;
    //         bool isSlide = false;
    //         bool isMove = false;
    //         CharacterData.JumpType jumpType = CharacterData.JumpType.None;
    //         killGpo.Dispatcher(new SE_GPO.Event_GetState() {
    //             CallBack = (jump, slide, move, type) => {
    //                 isJump = jump;
    //                 isSlide = slide;
    //                 isMove = move;
    //                 jumpType = type;
    //             }
    //         });
    //         var behaviourType = BehaviorState.Stand;
    //         if (isJump) {
    //             switch (jumpType) {
    //                 case  CharacterData.JumpType.Jump:
    //                     behaviourType = BehaviorState.Jump;
    //                     break;
    //                 case CharacterData.JumpType.AirJump:
    //                     behaviourType = BehaviorState.Secjump;
    //                     break;
    //             }
    //         }
    //
    //         if (isSlide) {
    //             behaviourType = BehaviorState.Slipper;
    //         }
    //         var hashData = new Hashtable();
    //         hashData.Add("game_mode", ModeData.ModeId);
    //         hashData.Add("game_map", ModeData.SceneId);
    //         hashData.Add("game_match", ModeData.MatchId);
    //         hashData.Add("weapon", killWeapon.WeaponItemId);
    //         hashData.Add("weapon_skin", killWeapon.WeaponSkinItemId);
    //         hashData.Add("behavior_state", (int)behaviourType);
    //         hashData.Add("kill_target", superWeapon.WeaponItemId);
    //         hashData.Add("kill_target_type", (int)KillTargetType.SuperWeapon);//击杀类型
    //         hashData.Add("kill_range", Mathf.CeilToInt(distance));
    //         hashData.Add("is_oneshot", 0);
    //         hashData.Add("is_move", isMove ? 1 : 0);
    //         var json = MiniJSON.jsonEncode(hashData);
    //         //发送事件上报任务
    //         MsgRegister.Dispatcher(new SM_GPO.ReportTask() {
    //             PlayerId = killData.PlayerId,
    //             SysJsonTypeId = 4,
    //             TaskJson = json
    //         });
    //     }
    }
}