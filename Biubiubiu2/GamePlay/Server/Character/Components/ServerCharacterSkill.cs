using System;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterSkill : ServerGPOSkill {
        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Skill.Cmd_UseSkill.ID, CmdUseSkillCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Skill.Cmd_UseSkill.ID, CmdUseSkillCallBack);
            RpcAllSkill();
        }

        private void RpcAllSkill() {
            for (int i = 0; i < skillList.Count; i++) {
                var data = skillList[i];
                TargetRpc(networkBase, new Proto_Skill.TargetRpc_Skill {
                    skillId = data.ModeData.ID, skillPoint = data.CurrectSkillPoint, skillCD = data.CurrectSkillCD,
                });
            }
        }

        private void CmdUseSkillCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Skill.Cmd_UseSkill)cmdData;
            UseSkill(data.skillId);
        }

        protected override void OnSetSkill(UseSkillData skillData) {
            base.OnSetSkill(skillData);
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_Skill {
                skillId = skillData.ModeData.ID,
                skillPoint = skillData.CurrectSkillPoint,
                skillCD = skillData.CurrectSkillCD,
            });
        }

        protected override void OnRemoveAllSkills() {
            base.OnRemoveAllSkills();
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_RemoveAllSkills());
        }

        protected override void OnUseSkill(UseSkillData skillData) {
            base.OnUseSkill(skillData);
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_StartUseSkill {
                skillId = skillData.ModeData.ID,
            });
        }

        protected override void OnSkillOver(UseSkillData skillData) {
            base.OnSkillOver(skillData);
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_UseSkillEnd {
                skillId = skillData.ModeData.ID,
            });
        }

        protected override void OnUpdateSkillPoint(UseSkillData skillData) {
            base.OnUpdateSkillPoint(skillData);
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_UpdateSkillPoint() {
                skillId = skillData.ModeData.ID,
                skillPoint = skillData.CurrectSkillPoint,
            });
        }

        protected override void OnSetSkillInProgressForAbility(ISystemMsg body, SE_Skill.Event_SetSkillInProgressForAbility ent) {
            base.OnSetSkillInProgressForAbility(body, ent);
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_SetSkillInProgress {
                isInProgress = ent.isSkillInProgress
            });
        }

        protected override void OnUseSkillFailed(UseSkillFailed failed) {
            TargetRpc(networkBase, new Proto_Skill.TargetRpc_UseSkillFailed {
                failedReason = (byte)failed
            });
        }
    }
}