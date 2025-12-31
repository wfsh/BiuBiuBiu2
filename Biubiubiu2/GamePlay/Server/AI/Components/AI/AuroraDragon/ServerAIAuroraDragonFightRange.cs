using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

public class ServerAIAuroraDragonFightRange : ComponentBase {
    private List<IGPO> gpoList;
    private Queue<IGPO> candidateGPOQueue;
    private List<IGPO> reverseGPOList;
    private Vector3 fightRangeCenterPoint;
    private float fightRangeRadius;
    private System.Random random;
    private bool playerKnockedDownStatusCache;
    private bool playerIsBeRatBuffNoNeedFindByBoss;
    private Action<bool> SetIsBeRatBuffNoNeedFindByBossCallBack;
    private float removeTimeAfterEnd;
    private Queue<IGPO> tempQueueToRebuild;
    private Vector3 startPoint;

    protected override void OnAwake() {
        base.OnAwake();
        gpoList = new List<IGPO>();
        candidateGPOQueue = new Queue<IGPO>();
        reverseGPOList = new List<IGPO>();
        random = new System.Random();
        tempQueueToRebuild = new Queue<IGPO>();
        Register<SE_AI_FightBoss.Event_CreateFightRange>(OnCreateFightRangeCallBack);
        Register<SE_AI_FightBoss.Event_GetOneTargetInFightRange>(OnGetOneTargetInFightRangeCallBack);
        Register<SE_AI_FightBoss.Event_GetAllTargetInFightRange>(OnGetAllTargetInRangeCallBack);
        Register<SE_AI_FightBoss.Event_GetAliveTargetCount>(OnGetAliveTargetCountCallBack);
        Register<SE_AI_FightBoss.Event_GetFightRangeData>(OnGetFightRangeCenterPointCallBack);
        Register<SE_AI_AuroraDragon.Event_GetBlockFireEndPoint>(OnGetBlockFireEndPoint);
        MsgRegister.Register<SM_AI.Event_DisableAsAITarget>(OnDisableAsMonsterTargetCallBack);
        SetIsBeRatBuffNoNeedFindByBossCallBack = (b => playerIsBeRatBuffNoNeedFindByBoss = b);
    }

    protected override void OnStart() {
        base.OnStart();
        startPoint = iGPO.GetPoint();
        MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
            FireGPO = iGPO,
            MData = AbilityM_GoldDashFightBossRange.CreateForID(AbilityM_GoldDashFightBossRange.ID_FightRange),
            InData = new AbilityIn_GoldDashFightBossRange() {
                In_FightRangeCenterPoint = startPoint
            },
        });
        
        
    }

    protected override void OnClear() {
        Unregister<SE_AI_FightBoss.Event_CreateFightRange>(OnCreateFightRangeCallBack);
        Unregister<SE_AI_FightBoss.Event_GetOneTargetInFightRange>(OnGetOneTargetInFightRangeCallBack);
        Unregister<SE_AI_FightBoss.Event_GetAllTargetInFightRange>(OnGetAllTargetInRangeCallBack);
        Unregister<SE_AI_FightBoss.Event_GetAliveTargetCount>(OnGetAliveTargetCountCallBack);
        Unregister<SE_AI_FightBoss.Event_GetFightRangeData>(OnGetFightRangeCenterPointCallBack);
        Unregister<SE_AI_AuroraDragon.Event_GetBlockFireEndPoint>(OnGetBlockFireEndPoint);
        MsgRegister.Unregister<SM_AI.Event_DisableAsAITarget>(OnDisableAsMonsterTargetCallBack);
        gpoList.Clear();
        gpoList = null;
        candidateGPOQueue.Clear();
        candidateGPOQueue = null;
        reverseGPOList.Clear();
        reverseGPOList = null;
        tempQueueToRebuild.Clear();
        tempQueueToRebuild = null;
        random = null;
        base.OnClear();
    }
    
    private void OnDisableAsMonsterTargetCallBack(SM_AI.Event_DisableAsAITarget ent) {
        tempQueueToRebuild.Clear();
        foreach (var gpo in candidateGPOQueue) {
            if (gpo.GetGpoID() == ent.Gpo.GetGpoID()) {
                continue;
            }

            tempQueueToRebuild.Enqueue(gpo);
        }

        candidateGPOQueue = new Queue<IGPO>(tempQueueToRebuild);

        tempQueueToRebuild.Clear();
        foreach (var gpo in reverseGPOList) {
            if (gpo.GetGpoID() == ent.Gpo.GetGpoID()) {
                continue;
            }

            tempQueueToRebuild.Enqueue(gpo);
        }

        reverseGPOList = new List<IGPO>(tempQueueToRebuild);

        tempQueueToRebuild.Clear();
        foreach (var gpo in gpoList) {
            if (gpo.GetGpoID() == ent.Gpo.GetGpoID()) {
                continue;
            }

            tempQueueToRebuild.Enqueue(gpo);
        }

        gpoList = new List<IGPO>(tempQueueToRebuild);
    }

    private void OnCreateFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_CreateFightRange ent) {
        MsgRegister.Dispatcher(new SM_GPO.GetGPOListForGpoType() {
            GpoType = GPOData.GPOType.Role,
            CallBack = list => {
                SetGPOListInRange(list, ent.FightRangeRadius, ent.CenterOffset);
                removeTimeAfterEnd = ent.RemoveTimeAfterEnd;
            }
        });
    }

    private void OnGetOneTargetInFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_GetOneTargetInFightRange ent) {
        ent.CallBack?.Invoke(GetOneAliveTarget());
    }

    private void OnGetAllTargetInRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_GetAllTargetInFightRange ent) {
        ent.CallBack?.Invoke(gpoList);
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

    private IGPO GetOneAliveTarget() {
        if (!candidateGPOQueue.TryPeek(out IGPO headGPO) || headGPO.IsDead() || CheckPlayerBeKnockDown(headGPO) || CheckPlayerIsBeRatBuffNoNeedFindByBoss(headGPO)) {
            ShuffleCandidate();
        }

        while (candidateGPOQueue.TryPeek(out IGPO candGPO) && (candGPO.IsDead() || CheckPlayerBeKnockDown(candGPO)|| CheckPlayerIsBeRatBuffNoNeedFindByBoss(candGPO))) {
            reverseGPOList.Add(candidateGPOQueue.Dequeue());
        }

        if (candidateGPOQueue.TryDequeue(out IGPO gpo)) {
            reverseGPOList.Add(gpo);
        }

        return gpo;
    }

    private bool CheckPlayerBeKnockDown(IGPO gpo) {
        gpo.Dispatcher(new SE_Character.GetKnockDownStatus() {
            CallBack = SetKnockedDownStatusCache,
        });

        return playerKnockedDownStatusCache;
    }
    
    private bool CheckPlayerIsBeRatBuffNoNeedFindByBoss(IGPO gpo) {
        gpo.Dispatcher(new SE_Character.GetIsBeRatBuffNoNeedFindByBoss() {
            CallBack = SetIsBeRatBuffNoNeedFindByBossCallBack,
        });

        return playerIsBeRatBuffNoNeedFindByBoss;
    }
    
    private void ShuffleCandidate() {
        int count = reverseGPOList.Count;
        for (int i = count - 1; i >= 0; i--) {
            int j = random.Next(i + 1);
            (reverseGPOList[i], reverseGPOList[j]) = (reverseGPOList[j], reverseGPOList[i]);
        }

        for (int i = 0; i < count; i++) {
            IGPO reverseGPO = reverseGPOList[i];
            if (!(reverseGPO.IsDead() || CheckPlayerBeKnockDown(reverseGPO)) || CheckPlayerIsBeRatBuffNoNeedFindByBoss(reverseGPO)) {
                candidateGPOQueue.Enqueue(reverseGPOList[i]);
            }
        }
    }

    private void SetGPOListInRange(List<IGPO> globalGPOList, float radius, float centerForwardOffset) {
        gpoList.Clear();
        candidateGPOQueue.Clear();
        reverseGPOList.Clear();

        fightRangeRadius = radius;
        fightRangeCenterPoint = iGPO.GetPoint() + iGPO.GetForward() * centerForwardOffset;
        for (int i = 0, count = globalGPOList.Count; i < count; i++) {
            IGPO gpo = globalGPOList[i];
            Vector3 gpoPos = gpo.GetPoint();
            if (gpoPos.y < -150f) {
                continue; // 忽略过高或过低的 GPO
            }
            gpoPos.y = fightRangeCenterPoint.y;
            if (Vector3.Distance(gpoPos, fightRangeCenterPoint) <= radius) {
                gpoList.Add(gpo);
                reverseGPOList.Add(gpo);
            }
        }
    }

    private void SetKnockedDownStatusCache(bool value) {
        playerKnockedDownStatusCache = value;
    }

    private void OnGetBlockFireEndPoint(ISystemMsg body, SE_AI_AuroraDragon.Event_GetBlockFireEndPoint ent) {
        ent.CallBack(GetBlockFireEndPoint(ent.FirePos, ent.FireDir, ent.AttackHeight, ent.AttackLength));
    }
    
    private static Vector3 GetBlockFireEndPoint(Vector3 firePos, Vector3 fireDir, float attackHeight, float attackLength) {
        var tempHitList = new RaycastHit[10];
        firePos.y += attackHeight;
        var endPos = firePos + fireDir * attackLength;
        int retMin = Physics.RaycastNonAlloc(firePos, fireDir, tempHitList, attackLength, LayerData.DefaultLayerMask | LayerData.ServerLayerMask);
        var minDistanceRevertHit = GetMinDistance(tempHitList, retMin);
        int retMax = Physics.RaycastNonAlloc(endPos, -fireDir, tempHitList, attackLength, LayerData.DefaultLayerMask | LayerData.ServerLayerMask);
        if (retMax < 1 && retMin < 1) {
            return endPos;
        }

        var maxDistanceRevertHit = GetMaxDistance(tempHitList, retMax);
        if (minDistanceRevertHit.collider != null && maxDistanceRevertHit.collider != null) {
            return (attackLength - maxDistanceRevertHit.distance) > minDistanceRevertHit.distance? minDistanceRevertHit.point : maxDistanceRevertHit.point;
        }

        if (maxDistanceRevertHit.collider != null) {
            return maxDistanceRevertHit.point;
        }

        if (minDistanceRevertHit.collider != null) {
            return minDistanceRevertHit.point;
        }

        return tempHitList[0].point;
    }
    
    private static RaycastHit GetMinDistance(RaycastHit[] raycastHits, int len) {
        RaycastHit outHit = new RaycastHit();
        if (len < 1) {
            return outHit;
        }

        float minDistance = 1000;
        for (var i = 0; i < len; i++) {
            var checkCastHit = raycastHits[i];
            if (checkCastHit.collider != null && !checkCastHit.collider.isTrigger) {
                var hitType = checkCastHit.collider.GetComponent<HitType>();
                if (hitType != null &&
                    (hitType.Part == GPOData.PartEnum.Body ||
                     hitType.Part == GPOData.PartEnum.Head)) {
                    continue;
                }
                if (checkCastHit.distance < minDistance) {
                    minDistance = checkCastHit.distance;
                    outHit = checkCastHit;
                }
            }
        }

        return outHit;
    }

    private static RaycastHit GetMaxDistance(RaycastHit[] raycastHits, int len) {
        RaycastHit outHit = new RaycastHit();
        if (len < 1) {
            return outHit;
        }

        float maxDistance = 0;
        for (var i = 0; i < len; i++) {
            var checkCastHit = raycastHits[i];
            if (checkCastHit.collider != null && !checkCastHit.collider.isTrigger) {
                var hitType = checkCastHit.collider.GetComponent<HitType>();
                if (hitType != null &&
                    (hitType.Part == GPOData.PartEnum.Body ||
                     hitType.Part == GPOData.PartEnum.Head)) {
                    continue;
                }
                if (checkCastHit.distance > maxDistance) {
                    maxDistance = checkCastHit.distance;
                    outHit = checkCastHit;
                }
            }
        }

        return outHit;
    }
}
