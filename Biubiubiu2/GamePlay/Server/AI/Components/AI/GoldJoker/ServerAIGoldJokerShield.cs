using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerShield : ComponentBase {
        private AIBehaviourData.FightStateEnum fightState;
        private GPOData.AttributeData attributeData;
        private Vector3 fightRangeCenter;
        private IAbilitySystem ab;
        private List<IGPO> gpoList;
        private bool isOpen;
        private bool isOpening;
        private byte configId;
        private byte upHpConfigId;
        private float upHpInterval;
        private float distance;
        private float timer;
        private float checkTimer;
        private bool isComeBack;
        private int upHp;
        private bool isSetFightRangeCenter = false;

        protected override void OnAwake() {
            base.OnAwake();
            Register<SE_AI.Event_SetShieldParam>(OnSetShieldParam);
            Register<SE_Behaviour.Event_ComeBack>(OnComeBack);
            Register<SE_AI.Event_ChangeAIState>(OnChangeMonsterState);
            Register<SE_AI_FightBoss.Event_CreateFightRangeData>(OnSetFightRangeDataCallBack);
            Register<SE_AI_FightBoss.Event_CheckGPOInFightRange>(OnCheckGPOInFightRangeCallBack);
        }
     
        protected override void OnStart() {
            base.OnStart();
            attributeData = ((S_AI_Base)mySystem).AttributeData;
            AddUpdate(OnUpdate);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
        }

        private void OnSetFightRangeDataCallBack(ISystemMsg body, SE_AI_FightBoss.Event_CreateFightRangeData ent) {
            fightRangeCenter = ent.fightRangeCenterPoint;
            distance = ent.fightRangeRadius;
            isSetFightRangeCenter = true;
        }

        private void OnChangeMonsterState(ISystemMsg body, SE_AI.Event_ChangeAIState ent) {
            fightState = ent.State;
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            Unregister<SE_AI.Event_ChangeAIState>(OnChangeMonsterState);
            Unregister<SE_AI.Event_SetShieldParam>(OnSetShieldParam);
            Unregister<SE_Behaviour.Event_ComeBack>(OnComeBack);
            Unregister<SE_AI_FightBoss.Event_CheckGPOInFightRange>(OnCheckGPOInFightRangeCallBack);
            Unregister<SE_AI_FightBoss.Event_CreateFightRangeData>(OnSetFightRangeDataCallBack);
        }

        private void OnSetShieldParam(ISystemMsg body, SE_AI.Event_SetShieldParam ent) {
            configId = (byte)ent.ConfigId;
            upHpConfigId = (byte)ent.UpHpConfigId;
            upHp = ent.UpHp;
            upHpInterval = ent.UpHpInterval;
            distance = ent.Distance;
        }

        private void OnComeBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            isComeBack = ent.IsTrue;
        }

        private void OnUpdate(float delta) {
            if (configId == 0) {
                return;
            }
            checkTimer -= delta;
            if (checkTimer > 0) {
                return;
            }
            checkTimer += 0.2f;
            UpdateShieldState();
            UpdateSwitchShield();
        }

        private void UpdateShieldState() {
            if (isComeBack) {
                isOpen = true;
                return;
            }
            if (fightState == AIBehaviourData.FightStateEnum.Fight) {
                isOpen = false;
                return;
            }
            foreach (var gpo in gpoList) {
                if (gpo.IsDead() || gpo.IsClear()) {
                    continue;
                }
                if (gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }
                var checkPoint = isSetFightRangeCenter == false ? iEntity.GetPoint() : fightRangeCenter;
                var dis = Vector3.Distance(gpo.GetPoint(), checkPoint);
                if (dis < distance) {
                    isOpen = false;
                    return;
                }
            }
            isOpen = true;
        }

        private void OnCheckGPOInFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_CheckGPOInFightRange ent) {
            ent.CallBack?.Invoke(isOpen == false);
        }
        private void UpdateSwitchShield() {
            if (isOpen && isOpening == false && ab == null) {
                isOpening = true;
                timer = 0;
                Dispatcher(new SE_AI.Event_AIGoldJokerShieldStateChange() {
                    isOpen = true
                });
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_GoldJokerFollowEffect.CreateForID(configId),
                    InData = new AbilityIn_GoldJokerFollowEffect() {
                        In_BodyPart = GPOData.PartEnum.RootBody, In_LifeTime = 3600
                    },
                    OR_CallBack = temp => {
                        isOpening = false;
                        if (isOpen == false) {
                            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility() {
                                AbilityId = temp.GetAbilityId()
                            });
                        } else {
                            ab = temp;
                        }
                    }
                });
                
            } else if (!isOpen && ab != null) {
                Dispatcher(new SE_AI.Event_AIGoldJokerShieldStateChange() {
                    isOpen = false
                });
                MsgRegister.Dispatcher(new SM_Ability.RemoveAbility() {
                    AbilityId = ab.GetAbilityId()
                });
                ab = null;
            }
            if (isOpen && attributeData.nowHp < attributeData.maxHp) {
                if (timer <= 0) {
                    timer += upHpInterval;
                    Dispatcher(new SE_GPO.Event_UpHP() {
                        UpHp = upHp
                    });
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_PlayEffect.CreateForID(upHpConfigId),
                        InData = new AbilityIn_PlayEffect() {
                            In_StartPoint = iEntity.GetPoint(),
                            In_StartRota = Quaternion.identity,
                            In_LifeTime = upHpInterval
                        }
                    });
                } else {
                    timer -= 0.1f;
                }
            }
        }
    }
}