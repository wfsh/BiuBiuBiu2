using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSkillCallAI : ServerNetworkComponentBase {
        private int summerSkillId;
        /// <summary>
        /// 载具默认只有一只， List 是写给召唤怪物看的
        /// </summary>
        private List<IGPO> summerDriverGPOList = new List<IGPO>();

        protected override void OnAwake() {
            Register<SE_Skill.Event_UseSkill>(OnUseSkill);
            Register<SE_Skill.Event_SkillOver>(OnSkillOver);
        }

        protected override void OnClear() {
            SkillOver();
            Unregister<SE_Skill.Event_UseSkill>(OnUseSkill);
            Unregister<SE_AI.Event_SummonFollowAIEnd>(OnSummonFollowMonsterEnd);
        }

        private void OnUseSkill(ISystemMsg body, SE_Skill.Event_UseSkill ent) {
            if (ent.SkillData.SkillType != SkillData.SkillTypeEnum.CallMonster) {
                return;
            }
            summerSkillId = ent.SkillData.ID;
            Register<SE_AI.Event_SummonFollowAIEnd>(OnSummonFollowMonsterEnd);
            var gunData = WeaponData.Get(ent.WeaponData.WeaponItemId);
            var weaponSkinSign = ent.WeaponData.WeaponSkinItemId == 0 ? string.Empty : ItemData.GetData(ent.WeaponData.WeaponSkinItemId).ResSign;
            var atk = gunData.ATK  + ent.WeaponData.RandAttack;
            var hp = gunData.Hp  + ent.WeaponData.RandHp;
            var attackRange = gunData.AttackRange  + ent.WeaponData.RandAttackRange;
            for (int i = 0; i < ent.SkillData.SummonNum; i++) {
                iGPO.Dispatcher(new SE_Character.Event_AddMasterAI() {
                    MonsterSign = ent.SkillData.Sign, MonsterSkinSign = weaponSkinSign, ATK = atk, HP = hp, AttackRange = attackRange
                });
            }
            iGPO.Dispatcher(new SE_Skill.Event_SetSkillInProgressForAbility {
                isSkillInProgress = true
            });
        }

        private void OnSkillOver(ISystemMsg body, SE_Skill.Event_SkillOver ent) {
            if (ent.SkillType != SkillData.SkillTypeEnum.CallMonster) {
                return;
            }
            SkillOver();
            iGPO.Dispatcher(new SE_Character.Event_RemoveMasterAI() {
                MonsterSign = SkillData.GetData(ent.SkillID).Sign
            });
            iGPO.Dispatcher(new SE_Skill.Event_SetSkillInProgressForAbility {
                isSkillInProgress = false
            });
        }
        
        private void SkillOver() {
            iGPO.Unregister<SE_AI.Event_SummonFollowAIEnd>(OnSummonFollowMonsterEnd);
            ClearSummerDriver();
        }

        private void OnSummonFollowMonsterEnd(ISystemMsg body, SE_AI.Event_SummonFollowAIEnd ent) {
            if (ent.Iai == null) {
                return;
            }
            var summerDriverGPO = ent.Iai.GetGPO();
            summerDriverGPO.Register<SE_GPO.Event_SetIsDead>(OnSummerMonsterSetIsDeadCallBack);
            summerDriverGPOList.Add(summerDriverGPO);
        }

        private void ClearSummerDriver() {
            for (int i = 0; i < summerDriverGPOList.Count; i++) {
                var gpo = summerDriverGPOList[i];
                gpo.Unregister<SE_GPO.Event_SetIsDead>(OnSummerMonsterSetIsDeadCallBack);
                gpo.Dispatcher(new SE_GPO.Event_OnSetDead());
            }
            summerDriverGPOList.Clear();
        }

        private void OnSummerMonsterSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                for (int i = 0; i < summerDriverGPOList.Count; i++) {
                    var gpo = summerDriverGPOList[i];
                    if (gpo.GetGpoID() == ent.DeadGpo.GetGpoID()) {
                        summerDriverGPOList.RemoveAt(i);
                        break;
                    }
                }
                if (summerDriverGPOList.Count <= 0) {
                    Dispatcher(new SE_Skill.Event_RequestSkillOver {
                        SkillId = summerSkillId
                    });
                    summerSkillId = 0;
                }
            }
        }
    }
}