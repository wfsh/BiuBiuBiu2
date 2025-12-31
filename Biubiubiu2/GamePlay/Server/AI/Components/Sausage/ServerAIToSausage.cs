using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIToSausage : ComponentBase {
        private Dictionary<int, float> firstTimeGetHurtByPlayerMap;
        private List<IGPO> hurtHistoryGPOSet;
        private float lastTimeGetHurtByPlayer;

        protected override void OnAwake() {
            base.OnAwake();
            firstTimeGetHurtByPlayerMap = new Dictionary<int, float>();
            hurtHistoryGPOSet = new List<IGPO>();
            Register<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            firstTimeGetHurtByPlayerMap.Clear();
            firstTimeGetHurtByPlayerMap = null;
            hurtHistoryGPOSet.Clear();
            hurtHistoryGPOSet = null;
            Unregister<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
        }

        private void AfterDownHPCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            if (ent.AttackGPO.GetGPOType() == GPOData.GPOType.Role ||
                ent.AttackGPO.GetGPOType() == GPOData.GPOType.AI ||
                ent.AttackGPO.GetGPOType() == GPOData.GPOType.RoleAI) {
                int attackerGPOId = ent.AttackGPO.GetGpoID();
                if (!firstTimeGetHurtByPlayerMap.TryGetValue(attackerGPOId, out _)) {
                    firstTimeGetHurtByPlayerMap[attackerGPOId] = Time.realtimeSinceStartup;
                }
                lastTimeGetHurtByPlayer = Time.realtimeSinceStartup;
                hurtHistoryGPOSet.Add(ent.AttackGPO);
                if (iGPO.IsDead()) {
                    long playerId = 0;
                    if (ent.AttackGPO.GetGPOType() == GPOData.GPOType.Role) {
                        ent.AttackGPO.Dispatcher(new SE_Character.GetPlayerId() {
                            CallBack = id => {
                                playerId = id;
                            }
                        });
                        float firstTime = firstTimeGetHurtByPlayerMap[attackerGPOId];
                        var attackSourceType = SM_Sausage.MonsterKilledByRole.AttackSourceType.Undefined;
                        switch (ent.AttackItemId) {
                            case -1:
                                attackSourceType = SM_Sausage.MonsterKilledByRole.AttackSourceType.Melee;
                                break;
                            case 0:
                                attackSourceType = SM_Sausage.MonsterKilledByRole.AttackSourceType.Ability;
                                break;
                            default:
                                attackSourceType = SM_Sausage.MonsterKilledByRole.AttackSourceType.Gun;
                                break;
                        }
                        var gpoData = GpoSet.GetGpoById(iGPO.GetGpoMID());
                        MsgRegister.Dispatcher(new SM_Sausage.MonsterKilledByRole() {
                            AttackPlayerId = playerId,
                            Iai = (IAI)mySystem,
                            MonsterId = iGPO.GetGpoMID(),
                            MonsterName = gpoData.Name,
                            TimeUsedToDead = lastTimeGetHurtByPlayer - firstTime,
                            MonsterPos = iGPO.GetPoint(),
                            AttackType = attackSourceType,
                            AIQuality = (AIData.AIQuality)iGPO.GetMData().GetQuality(),
                            hurtHistoryGPOSet = hurtHistoryGPOSet,
                            AttackItemId = ent.AttackItemId,
                        });
                    }
                    hurtHistoryGPOSet.Clear();
                }
            }
        }
    }
}