using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOAIMaster : ServerNetworkComponentBase {
        private const int SUMMER_MAX_FOLLOW_COUNT = 6;
        protected List<IAI> followMonsterList = new List<IAI>();
        private float recoveryTime;

        protected override void OnAwake() {
            Register<SE_Behaviour.Event_HateLockTarget>(OnHateLockTargetCallBack);
            Register<SE_Behaviour.Event_HateAttackTarget>(OnHateAttackTargetCallBack);
            Register<SE_AI.Event_CatchAIState>(OnCatchMonsterCallBack);
            Register<SE_AI.Event_GetFollowList>(OnGetFollowListCallBack);
            Register<SE_Character.Event_DropAllFollowAI>(OnDropAllFollowAICallBack);
            Register<SE_Character.Event_AddMasterAI>(OnAddMasterAICallBack);
            Register<SE_Character.Event_RemoveMasterAI>(OnRemoveMasterAICallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            ClearFollowMonster();
            Unregister<SE_Behaviour.Event_HateLockTarget>(OnHateLockTargetCallBack);
            Unregister<SE_Behaviour.Event_HateAttackTarget>(OnHateAttackTargetCallBack);
            Unregister<SE_AI.Event_CatchAIState>(OnCatchMonsterCallBack);
            Unregister<SE_AI.Event_GetFollowList>(OnGetFollowListCallBack);
            Unregister<SE_Character.Event_DropAllFollowAI>(OnDropAllFollowAICallBack);
            Unregister<SE_Character.Event_AddMasterAI>(OnAddMasterAICallBack);
            Unregister<SE_Character.Event_RemoveMasterAI>(OnRemoveMasterAICallBack);
        }

        private void OnDropAllFollowAICallBack(ISystemMsg body, SE_Character.Event_DropAllFollowAI ent) {
            DiscardAllMonster(ent.IsDrop);
        }

        /// <summary>
        /// 直接获得宠物
        /// </summary>
        /// <param name="ent"></param>
        private void OnAddMasterAICallBack(ISystemMsg body, SE_Character.Event_AddMasterAI ent) {
            if (followMonsterList.Count < SUMMER_MAX_FOLLOW_COUNT) {
                var attributeData = new GPOData.AttributeData();
                attributeData.Sign = ent.MonsterSign;
                attributeData.SkinSign = ent.MonsterSkinSign;
                attributeData.maxHp = ent.HP;
                attributeData.nowHp = ent.HP;
                attributeData.ATK = ent.ATK;
                attributeData.AttackRange = ent.AttackRange;
                CatchMonster(attributeData);
            } else {
                Debug.Log("超过宠物可携带上限，无法创建");
            }
        }

        private void OnRemoveMasterAICallBack(ISystemMsg body, SE_Character.Event_RemoveMasterAI ent) {
            RemoveMonsterForSign(ent.MonsterSign, false);
        }

        /// <summary>
        /// 通过捕捉获得宠物
        /// </summary>
        /// <param name="ent"></param>
        private void OnCatchMonsterCallBack(ISystemMsg body, SE_AI.Event_CatchAIState ent) {
            if (ent.IsSuccess) {
                CatchMonster(ent.CatchAIData);
            }
        }

        private void CatchMonster(GPOData.AttributeData data) {
            if (followMonsterList.Count < SUMMER_MAX_FOLLOW_COUNT) {
                SummerMonster(data, GetMonsterPoint());
            } else {
                Debug.Log("超过宠物可携带上限，无法创建 2");
            }
        }
        
        private Vector3 GetMonsterPoint() {
            return iEntity.GetPoint();
        }

        /// <summary>
        /// 死亡后丢弃所有当局捕捉宠物
        /// </summary>
        private void DiscardAllMonster(bool isDrop) {
            var count = followMonsterList.Count - 1;
            for (int i = count; i >= 0; i--) {
                var follow = (S_AI_Base)followMonsterList[i];
                TackBackMonster(follow.GetGPO());
                var att = follow.AttributeData;
                followMonsterList.RemoveAt(i);
                OnDiscardMonster(att);
                if (isDrop) {
                    MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                        AISign = att.Sign,
                        StartPoint = iEntity.GetPoint(),
                        OR_GpoType = GPOData.GPOType.AI
                    });
                }
            }
            followMonsterList.Clear();
        }

        private void RemoveMonsterForSign(string monsterSign, bool isDrop) {
            var count = followMonsterList.Count - 1;
            for (int i = count; i >= 0; i--) {
                var follow = followMonsterList[i];
                var att = follow.GetAttributeData();
                if (follow != null && att.Sign == monsterSign) {
                    TackBackMonster(follow.GetGPO());
                    followMonsterList.RemoveAt(i);
                    OnDiscardMonster(att);
                    if (isDrop) {
                        MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                            AISign = att.Sign,
                            StartPoint = iEntity.GetPoint(),
                            OR_GpoType = GPOData.GPOType.AI
                        });
                    }
                }
            }
        }

        virtual protected void OnDiscardMonster(GPOData.AttributeData data) {
        }

        private void OnGetFollowListCallBack(ISystemMsg body, SE_AI.Event_GetFollowList ent) {
            ent.CallBack.Invoke(followMonsterList);
        }

        protected void SummerMonster(GPOData.AttributeData data, Vector3 point) {
            MsgRegister.Dispatcher(new SM_AI.Event_AddMasterAI {
                OR_CallBack = SetFollowMonster, MasterGPO = iGPO, OR_AIData = data, StartPoint = point
            });
        }

        protected GPOData.AttributeData GetMonsterData(int mpid) {
            for (int i = 0; i < followMonsterList.Count; i++) {
                var data = (S_AI_Base)followMonsterList[i];
                if (data.GpoId == mpid) {
                    return data.AttributeData;
                }
            }
            return null;
        }

        private void TackBackMonster(IGPO monsterGPO) {
            if (monsterGPO == null || monsterGPO.IsClear()) {
                return;
            }
            monsterGPO.Dispatcher(new SE_AI.Event_TakeBack());
            monsterGPO.Unregister<SE_GPO.Event_HPChange>(OnHPChangeCallBack);
            monsterGPO.Unregister<SE_GPO.Event_SetIsDead>(OnMonsterDeadCallBack);
        }

        private void SetFollowMonster(IAI iai) {
            iai.Register<SE_GPO.Event_HPChange>(OnHPChangeCallBack);
            iai.Register<SE_GPO.Event_SetIsDead>(OnMonsterDeadCallBack);
            mySystem.Dispatcher(new SE_AI.Event_SummonFollowAIEnd() {
                Iai = iai
            });
            OnSetFollowMonster(iai);
        }

        private void ClearFollowMonster() {
            for (int i = 0; i < followMonsterList.Count; i++) {
                var followMonster = followMonsterList[i];
                followMonster.Unregister<SE_GPO.Event_HPChange>(OnHPChangeCallBack);
                followMonster.Unregister<SE_GPO.Event_SetIsDead>(OnMonsterDeadCallBack);
            }
            followMonsterList.Clear();
            OnSetFollowMonster(null);
        }

        virtual protected void OnSetFollowMonster(IAI iai) {
        }

        private void OnHPChangeCallBack(ISystemMsg body, SE_GPO.Event_HPChange ent) {
            OnHPChange(ent.HP);
        }

        virtual protected void OnHPChange(int hp) {
        }

        private void OnMonsterDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            TackBackMonster(ent.DeadGpo);
        }

        private void OnHateLockTargetCallBack(ISystemMsg body, SE_Behaviour.Event_HateLockTarget ent) {
            for (int i = 0; i < followMonsterList.Count; i++) {
                var followMonster = followMonsterList[i];
                followMonster?.Dispatcher(ent);
            }
        }

        private void OnHateAttackTargetCallBack(ISystemMsg body, SE_Behaviour.Event_HateAttackTarget ent) {
            for (int i = 0; i < followMonsterList.Count; i++) {
                var followMonster = followMonsterList[i];
                followMonster?.Dispatcher(ent);
            }
        }
    }
}