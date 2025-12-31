using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerFightRange : ComponentBase {        
        private List<IGPO> gpoList;
        private Queue<IGPO> candidateGPOQueue;
        private Queue<IGPO> tempQueueToRebuild;
        private List<IGPO> reverseGPOList;
        private Vector3 fightRangeCenterPoint;
        private float fightRangeRadius;
        private System.Random random;
        private bool playerKnockedDownStatusCache;
        private float removeTimeAfterEnd;
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
            MsgRegister.Register<SM_AI.Event_DisableAsAITarget>(OnDisableAsMonsterTargetCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            startPoint = iGPO.GetPoint();
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_GoldDashFightBossRange.CreateForID(AbilityM_GoldDashFightBossRange.ID_GoldJokerFightRange),
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
            Dispatcher(new SE_AI.Event_PlayBossInMusic());
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList() {
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
            ent.CallBack?.Invoke(fightRangeCenterPoint, fightRangeRadius, removeTimeAfterEnd, true);
        }

        private IGPO GetOneAliveTarget() {
            if (!candidateGPOQueue.TryPeek(out IGPO headGPO) || headGPO.IsDead() || CheckPlayerBeKnockDown(headGPO)) {
                ShuffleCandidate();
            }

            while (candidateGPOQueue.TryPeek(out IGPO candGPO) && (candGPO.IsDead() || CheckPlayerBeKnockDown(candGPO))) {
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

        private void ShuffleCandidate() {
            int count = reverseGPOList.Count;
            for (int i = count - 1; i >= 0; i--) {
                int j = random.Next(i + 1);
                (reverseGPOList[i], reverseGPOList[j]) = (reverseGPOList[j], reverseGPOList[i]);
            }

            for (int i = 0; i < count; i++) {
                IGPO reverseGPO = reverseGPOList[i];
                if (!(reverseGPO.IsDead() || CheckPlayerBeKnockDown(reverseGPO))) {
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
                if (gpo.GetGPOType() == GPOData.GPOType.Role) {
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
        }

        private void SetKnockedDownStatusCache(bool value) {
            playerKnockedDownStatusCache = value;
        }
    }
}