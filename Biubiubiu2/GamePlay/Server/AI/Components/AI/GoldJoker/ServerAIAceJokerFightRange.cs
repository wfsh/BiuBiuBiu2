using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

public class ServerAIAceJokerFightRange : ComponentBase {
    private List<IGPO> gpoList;
    private float fightRangeRadius;
    private bool playerKnockedDownStatusCache;
    private Vector3 fightRangeCenterPoint;
    private float removeTimeAfterEnd;
    private float checkInterval = 1f;
    private List<IGPO> lastInRangeGpoList;
    private Vector3 startPoint;

    protected override void OnAwake() {
        base.OnAwake();
        gpoList = new List<IGPO>();
        lastInRangeGpoList = new List<IGPO>();
        Register<SE_AI_FightBoss.Event_GetAllTargetInFightRange>(OnGetAllTargetInRangeCallBack);
        Register<SE_AI_FightBoss.Event_GetAliveTargetCount>(OnGetAliveTargetCountCallBack);
        Register<SE_AI_FightBoss.Event_GetFightRangeData>(OnGetFightRangeCenterPointCallBack);
        Register<SE_AI_FightBoss.Event_CheckGPOInFightRange>(OnCheckGPOInFightRangeCallBack);
        Register<SE_AI_FightBoss.Event_CreateFightRange>(OnCreateFightRangeCallBack);
    }

    protected override void OnStart() {
        base.OnStart();
        startPoint = iGPO.GetPoint();
        AddUpdate(OnUpdate);
        InitightRange();
    }

    protected override void OnClear() {
        Unregister<SE_AI_FightBoss.Event_GetAllTargetInFightRange>(OnGetAllTargetInRangeCallBack);
        Unregister<SE_AI_FightBoss.Event_GetAliveTargetCount>(OnGetAliveTargetCountCallBack);
        Unregister<SE_AI_FightBoss.Event_GetFightRangeData>(OnGetFightRangeCenterPointCallBack);
        Unregister<SE_AI_FightBoss.Event_CheckGPOInFightRange>(OnCheckGPOInFightRangeCallBack);
        Unregister<SE_AI_FightBoss.Event_CreateFightRange>(OnCreateFightRangeCallBack);
        RemoveUpdate(OnUpdate);
        gpoList.Clear();
        gpoList = null;
        lastInRangeGpoList.Clear();
        lastInRangeGpoList = null;
        base.OnClear();
    }

    private void OnUpdate(float deltaTime) {
        UpdateFightGpoList();
    }

    private void UpdateFightGpoList() {
        if (fightRangeRadius <= 0f) {
            return;
        }
        checkInterval -= Time.deltaTime;
        if (checkInterval > 0f) {
            return;
        }
        checkInterval = 1f;
        MsgRegister.Dispatcher(new SM_GPO.GetGPOList() {
            CallBack = list => {
                SetGPOListInRange(list);
            }
        });
        var newInRangeGpoList = new List<IGPO>(gpoList);
        
        foreach (var gpo in newInRangeGpoList) {
            if (!lastInRangeGpoList.Contains(gpo)) {
                gpo.Dispatcher(new SE_AI_FightBoss.Event_ChangeFightRangeStage(){isInRange = true});
            }
        }
        
        foreach (var gpo in lastInRangeGpoList) {
            if (!newInRangeGpoList.Contains(gpo)) {
                gpo.Dispatcher(new SE_AI_FightBoss.Event_ChangeFightRangeStage(){isInRange = false});
            }
        }
        
        lastInRangeGpoList = newInRangeGpoList;
    }

    private void InitightRange() {
        MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
            FireGPO = iGPO,
            MData = AbilityM_GoldDashFightBossRange.CreateForID(AbilityM_GoldDashFightBossRange.ID_AceJokerFightRange),
            InData = new AbilityIn_GoldDashFightBossRange() {
                In_FightRangeCenterPoint = startPoint
            },
        });
    }

    private void OnGetAllTargetInRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_GetAllTargetInFightRange ent) {
        ent.CallBack?.Invoke(gpoList);
    }

    private void OnCreateFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_CreateFightRange ent) {
        fightRangeRadius = ent.FightRangeRadius;
        fightRangeCenterPoint = iGPO.GetPoint() + iGPO.GetForward() * ent.CenterOffset;
        removeTimeAfterEnd = ent.RemoveTimeAfterEnd;
        MsgRegister.Dispatcher(new SM_GPO.GetGPOList() {
            CallBack = list => {
                SetGPOListInRange(list);
            }
        });

        Dispatcher(new SE_AI_FightBoss.Event_CreateFightRangeData() {
            fightRangeCenterPoint = fightRangeCenterPoint,
            fightRangeRadius = fightRangeRadius,
            removeTimeAfterEnd = removeTimeAfterEnd,
        });
    }

    private void OnGetAliveTargetCountCallBack(ISystemMsg body, SE_AI_FightBoss.Event_GetAliveTargetCount ent) {
        int aliveTargetCount = 0;
        for (int i = 0; i < gpoList.Count; i++) {
            var gpo = gpoList[i];
            gpo.Dispatcher(new SE_Character.GetKnockDownStatus() {
                CallBack = SetKnockedDownStatusCache,
            });
            if (!playerKnockedDownStatusCache && !gpo.IsDead()) {
                aliveTargetCount++;
            }
            playerKnockedDownStatusCache = false;
        }
        ent.CallBack?.Invoke(aliveTargetCount);
    }

    private void OnGetFightRangeCenterPointCallBack(ISystemMsg body, SE_AI_FightBoss.Event_GetFightRangeData ent) {
        ent.CallBack?.Invoke(fightRangeCenterPoint, fightRangeRadius, removeTimeAfterEnd, false);
    }

    private void SetGPOListInRange(List<IGPO> globalGPOList) {
        gpoList.Clear();
        for (int i = 0, count = globalGPOList.Count; i < count; i++) {
            var gpo = globalGPOList[i];
            if (gpo.GetTeamID() != iGPO.GetTeamID()) {
                var gpoPos = gpo.GetPoint();
                if (gpoPos.y < -150f) {
                    continue; // 忽略过高或过低的 GPO
                }
                gpoPos.y = fightRangeCenterPoint.y;
                if (Vector3.Distance(gpoPos, fightRangeCenterPoint) < fightRangeRadius) {
                    gpoList.Add(gpo);
                }
            }
        }
        mySystem.Dispatcher(new SE_AI_FightBoss.Event_SetAllGpoInFightRange {
            GpoList = gpoList
        });
    }
    
    private void OnCheckGPOInFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_CheckGPOInFightRange ent) {
        ent.CallBack?.Invoke(CheckGpoInFightRange(ent.GPO));
    }

    private bool CheckGpoInFightRange(IGPO checkGpo) {
        for (int i = 0; i < gpoList.Count; i++) {
            if (gpoList[i].GetGpoID() == checkGpo.GetGpoID()) {
                return true;
            }
        }
        return false;
    }

    private void SetKnockedDownStatusCache(bool value) {
        playerKnockedDownStatusCache = value;
    }
}