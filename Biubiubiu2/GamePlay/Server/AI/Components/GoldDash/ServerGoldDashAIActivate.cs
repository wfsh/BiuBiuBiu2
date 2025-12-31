using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /*
     * 受到刺激后变为“警戒”或“战斗”状态
     * 记录敌人来源点和 GPO
     * 检测怪物是否卡住（被 NavMesh 卡住原地）并自动恢复
     * 支持客户端表现（触发头顶表情/特效）
     */
    public class ServerGoldDashAIActivate : ServerNetworkComponentBase {
        private const float STUCK_CHECK_INTERVAL = 2;
        private const string ALERT_EFFECT_SIGN = "Expression_GoldDash_01";
        private const string FIGHT_EFFECT_SIGN = "Expression_GoldDash_01";
        private S_AI_Base aiSystem;
        private Vector3 alertStartPoint; // 进入警戒状态时的起始点
        private NavMeshAgent navMeshAgent;
        private float checkInterval; // 检测间隔
        private bool isInAlertStatus;
        private bool isInFightStatus;
        private MonsterBehavior goldDashBehavior;
        protected Vector3 latestStimulusPoint;
        protected IGPO latestStimulusSourceGPO;
        private IGPO lastMaxHateTargetGpo;
        private long lastMaxHateTargetPlayerId;

        protected override void OnAwake() {
            base.OnAwake();
            aiSystem = (S_AI_Base)mySystem;
            Register<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            Register<SE_AI.Event_TriggerFightStatus>(EnterFightStatus);
            Register<SE_AI.Event_GetActivateStatus>(GetActivateStatusCallBack);
            Register<SE_AI.Event_GetAlertStartPoint>(OnGetAlertStartPointCallBack);
            Register<SE_Behaviour.Event_ChainStimulus>(OnChainStimulusCallBack);
            Register<SE_Behaviour.Event_GetLatestStimulusState>(OnGetStimulusStateCallBack);
            MsgRegister.Register<SM_Weapon.Event_Fire>(OnFireCallBack);
            goldDashBehavior = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
        }


        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            navMeshAgent = GetNavMeshAgent();
            alertStartPoint = iEntity.GetPoint();
        }

        protected override void OnClear() {
            base.OnClear();
            Unregister<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            Unregister<SE_AI.Event_TriggerFightStatus>(EnterFightStatus);
            Unregister<SE_AI.Event_GetActivateStatus>(GetActivateStatusCallBack);
            Unregister<SE_AI.Event_GetAlertStartPoint>(OnGetAlertStartPointCallBack);
            Unregister<SE_Behaviour.Event_ChainStimulus>(OnChainStimulusCallBack);
            Unregister<SE_Behaviour.Event_GetLatestStimulusState>(OnGetStimulusStateCallBack);
            MsgRegister.Unregister<SM_Weapon.Event_Fire>(OnFireCallBack);
        }


        private void OnGetAlertStartPointCallBack(ISystemMsg body, SE_AI.Event_GetAlertStartPoint ent) {
            ent.CallBack?.Invoke(alertStartPoint);
        }

        private void OnGetStimulusStateCallBack(ISystemMsg body, SE_Behaviour.Event_GetLatestStimulusState ent) {
            ent.CallBack?.Invoke(latestStimulusSourceGPO, latestStimulusPoint);
        }

        private void OnFireCallBack(SM_Weapon.Event_Fire ent) {
            if (ent.FireGpo.GetTeamID() == iGPO.GetTeamID()) {
                return;
            }

            float sqrtDis = Vector3.SqrMagnitude(ent.FireGpo.GetPoint() - iGPO.GetPoint());
            if (sqrtDis <= Mathf.Pow(goldDashBehavior.CatchFireRange, 2)) {
                latestStimulusPoint = ent.FireGpo.GetPoint();
                latestStimulusSourceGPO = ent.FireGpo;
                Dispatcher(new SE_AI.Event_CaughtFireEvent() {
                    FireGPO = ent.FireGpo,
                });
            }
        }

        private void OnChainStimulusCallBack(ISystemMsg body, SE_Behaviour.Event_ChainStimulus ent) {
            if (ent.CasterGPO == null) {
                return;
            }
            latestStimulusPoint = ent.CasterGPO.GetPoint();
            latestStimulusSourceGPO = ent.CasterGPO;
        }

        private void EnterAlertStatus(ISystemMsg body, SE_AI.Event_TriggerAlertStatus ent) {
            if (ent.isEnabled) {
                Rpc(new Proto_AI.Rpc_Effect() {
                    effectSign = GetAlertEffectSign(),
                    lifeTime = 2,
                    offsetParentPart = GPOData.PartEnum.Head,
                    offset = Vector3.zero,
                });
                alertStartPoint = iGPO.GetPoint();
            }

            isInAlertStatus = ent.isEnabled;
        }

        private void EnterFightStatus(ISystemMsg body, SE_AI.Event_TriggerFightStatus ent) {
            if (ent.isEnabled) {
                Rpc(new Proto_AI.Rpc_Effect() {
                    effectSign = GetFightEffectSign(),
                    lifeTime = 2,
                    offsetParentPart = GPOData.PartEnum.Head,
                    offset = Vector3.zero,
                });

                PlayLockAudio();
            }

            isInFightStatus = ent.isEnabled;
            MsgRegister.Dispatcher(new SM_Sausage.MonsterTriggerFightStatus() {
                Iai = aiSystem,
                IsTrigger = ent.isEnabled,
            });
        }

        private void PlayLockAudio() {
            lastMaxHateTargetGpo = null;
            lastMaxHateTargetPlayerId = 0;
            
            Dispatcher(new SE_Behaviour.Event_GetMaxHateTarget() {
                CallBack = OnGetMaxHateTargetGPO
            });

            if (lastMaxHateTargetGpo == null) {
                return;
            }
            
            lastMaxHateTargetGpo.Dispatcher(new SE_Character.GetPlayerId() {
                CallBack = OnGetMaxHateTargetPlayerId
            });
            
            if (lastMaxHateTargetPlayerId != 0 && lastMaxHateTargetGpo != null) {
                lastMaxHateTargetGpo.Dispatcher(new SE_Character.PlayAudio() {
                    PlayerId = lastMaxHateTargetPlayerId,
                    WwiseId = WwiseAudioSet.Id_GoldDashAiFightHint,
                });
            }
        }

        private void OnGetMaxHateTargetPlayerId(long playerId) {
            lastMaxHateTargetPlayerId = playerId;
        }

        private void OnGetMaxHateTargetGPO(IGPO maxHateTargetGpo) {
            lastMaxHateTargetGpo = maxHateTargetGpo;
        }

        private void GetActivateStatusCallBack(ISystemMsg body, SE_AI.Event_GetActivateStatus ent) {
            ent.CallBack?.Invoke(isInAlertStatus, isInFightStatus);
        }
        private void ResetNavMeshAgent() {
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
        }

        private NavMeshAgent GetNavMeshAgent() {
            var entity = (EntityBase)iEntity;
            return entity.GetComponent<NavMeshAgent>();
        }
        
        protected virtual string GetFightEffectSign() {
            var resultSign = FIGHT_EFFECT_SIGN;
            if (iGPO != null) {
                var quality = (AIData.AIQuality)iGPO.GetMData().GetQuality();
                switch (quality) {
                    case AIData.AIQuality.Senior:
                        resultSign += "_Senior";
                        break;
                    case AIData.AIQuality.Elite:
                        resultSign +=  "_Elite";
                        break;
                }
            }
            
            return resultSign;
        }

        private string GetAlertEffectSign() {
            var resultSign = ALERT_EFFECT_SIGN;
            if (iGPO != null) {
                var quality = (AIData.AIQuality)iGPO.GetMData().GetQuality();
                switch (quality) {
                    case AIData.AIQuality.Senior:
                        resultSign += "_Senior";
                        break;
                    case AIData.AIQuality.Elite:
                        resultSign +=  "_Elite";
                        break;
                }
            }

            return resultSign;
        }
    }
}