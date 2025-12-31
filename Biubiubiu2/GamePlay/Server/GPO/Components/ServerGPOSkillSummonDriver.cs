using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSkillSummonDriver : ServerNetworkComponentBase {
        private int summonSkillId;
        /// <summary>
        /// 载具默认只有一只， List 是写给召唤怪物看的
        /// </summary>
        private List<IGPO> summonDriverGPOList = new List<IGPO>();

        protected override void OnAwake() {
            Register<SE_Skill.Event_UseSkill>(OnUseSkill);
            Register<SE_Skill.Event_SkillOver>(OnSkillOver);
        }

        protected override void OnClear() {
            SkillOver();
            Unregister<SE_Skill.Event_UseSkill>(OnUseSkill);
            Unregister<SE_Skill.Event_SkillOver>(OnSkillOver);
        }

        private void OnUseSkill(ISystemMsg body, SE_Skill.Event_UseSkill ent) {
            if (ent.SkillData.SkillType != SkillData.SkillTypeEnum.SummonDriver) {
                return;
            }
            summonSkillId = ent.SkillData.ID;
            iGPO.Register<SE_AI.Event_SummonFollowAIEnd>(OnSummonFollowMonsterEnd);
            var gunData = WeaponData.Get(ent.WeaponData.WeaponItemId);
            var weaponSkinSign = ent.WeaponData.WeaponSkinItemId == 0 ? string.Empty : ItemData.GetData(ent.WeaponData.WeaponSkinItemId).ResSign;
            var atk = gunData.ATK  + ent.WeaponData.RandAttack;
            var hp = gunData.Hp  + ent.WeaponData.RandHp;
            var attackRange = gunData.AttackRange  + ent.WeaponData.RandAttackRange;
            iGPO.Dispatcher(new SE_Character.Event_AddMasterAI() {
                MonsterSign = ent.SkillData.Sign, MonsterSkinSign = weaponSkinSign, ATK = atk, HP = hp, AttackRange = attackRange
            });
        }

        private void OnSkillOver(ISystemMsg body, SE_Skill.Event_SkillOver ent) {
            if (ent.SkillType != SkillData.SkillTypeEnum.SummonDriver) {
                return;
            }
            SkillOver();
            iGPO.Dispatcher(new SE_Character.Event_RemoveMasterAI() {
                MonsterSign = SkillData.GetData(ent.SkillID).Sign
            });
        }

        private void SkillOver() {
            iGPO.Unregister<SE_AI.Event_SummonFollowAIEnd>(OnSummonFollowMonsterEnd);
            ClearSummerDriver();
            ResetMasterPoint();
            ResetMasterState();
            summonSkillId = 0;
        }

        private void ResetMasterState() {
            iGPO.Dispatcher(new SE_GPO.Event_ReLife {
                UpHp = 9999999,
            });
        }

        private void ResetMasterPoint() {
            if (summonSkillId != SkillData.Skill_SummonDriverHelicopter) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Mode.GetStartPoint {
                CallBack = SetStartPoints,
                CharacterGPO = iGPO,
            });
        }
        
        private void SetStartPoints(Vector3 startPoint, Quaternion startRota) {
            iGPO.Dispatcher(new SE_Entity.SyncPointAndRota {
                Point = startPoint,
                Rota = startRota,
            });
        }

        private void OnSummonFollowMonsterEnd(ISystemMsg body, SE_AI.Event_SummonFollowAIEnd ent) {
            if (ent.Iai == null) {
                return;
            }
            ClearSummerDriver();
            var summerDriverGPO = ent.Iai.GetGPO();
            summerDriverGPO.Register<SE_GPO.Event_SetIsDead>(OnSummerMonsterSetIsDeadCallBack);
            summonDriverGPOList.Add(summerDriverGPO);
            Dispatcher(new SE_Skill.Event_UseSkillSummonDriver {
                UseGPOId = iGPO.GetGpoID(),
                SummerDriverGPOId = summerDriverGPO.GetGpoID()
            });
        }

        private void ClearSummerDriver() {
            for (int i = 0; i < summonDriverGPOList.Count; i++) {
                var gpo = summonDriverGPOList[i];
                gpo.Unregister<SE_GPO.Event_SetIsDead>(OnSummerMonsterSetIsDeadCallBack);
                gpo.Dispatcher(new SE_GPO.Event_OnSetDead());
            }
            summonDriverGPOList.Clear();
        }

        private void OnSummerMonsterSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                for (int i = 0; i < summonDriverGPOList.Count; i++) {
                    var gpo = summonDriverGPOList[i];
                    if (gpo.GetGpoID() == ent.DeadGpo.GetGpoID()) {
                        summonDriverGPOList.RemoveAt(i);
                        break;
                    }
                }
                if (summonDriverGPOList.Count <= 0) {
                    Dispatcher(new SE_Skill.Event_RequestSkillOver {
                        SkillId = summonSkillId
                    });
                    summonSkillId = 0;
                }
            }
        }
    }
}