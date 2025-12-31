using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterGPOSkill : ComponentBase {
        protected SkillData.UseSkillData useSkillData;
        protected List<SkillData.UseSkillData> skillList = new List<SkillData.UseSkillData>(6);

        protected enum UseSkillFailed {
            InRoom
        }

        protected override void OnAwake() {
            mySystem.Register<CE_Skill.Event_GetUseSkill>(OnGetUseSkillWeapon);
        }
        
        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Skill.TargetRpc_Skill.ID, OnSkillCallBack);
            AddProtoCallBack(Proto_Skill.TargetRpc_UpdateSkillPoint.ID, OnUpdateSkillPointCallBack);
            AddProtoCallBack(Proto_Skill.TargetRpc_StartUseSkill.ID, OnStartUseSkillCallBack);
            AddProtoCallBack(Proto_Skill.TargetRpc_UseSkillEnd.ID, OnUseSkillEndCallBack);
            AddProtoCallBack(Proto_Skill.TargetRpc_RemoveAllSkills.ID, OnRemoveAllSkillsCallBack);
            AddProtoCallBack(Proto_Skill.TargetRpc_SetSkillInProgress.ID, OnSetSkillInProgress);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_Skill.ID, OnSkillCallBack);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_UpdateSkillPoint.ID, OnUpdateSkillPointCallBack);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_StartUseSkill.ID, OnStartUseSkillCallBack);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_UseSkillEnd.ID, OnUseSkillEndCallBack);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_RemoveAllSkills.ID, OnRemoveAllSkillsCallBack);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_SetSkillInProgress.ID, OnSetSkillInProgress);
            mySystem.Unregister<CE_Skill.Event_GetUseSkill>(OnGetUseSkillWeapon);
        }

        private void OnUpdate(float deltaTime) {
            DownSkillCD(deltaTime);
        }
        
        private void OnGetUseSkillWeapon(ISystemMsg body, CE_Skill.Event_GetUseSkill evn) {
            evn.CallBack(useSkillData);
        }

        private void OnSkillCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_Skill.TargetRpc_Skill)docData;
            var skillData = SkillData.GetData(rpcData.skillId);
            if (skillData == null) {
                return;
            }
            var skill = new SkillData.UseSkillData {
                ModeData = skillData,
                CurrectSkillCD = rpcData.skillCD,
                CurrectSkillDuration = 0f,
                CurrectSkillPoint = rpcData.skillPoint,
            };
            skillList.Add(skill);
            if (useSkillData == null) {
                useSkillData = skill;
                mySystem.Dispatcher(new CE_Skill.Event_SetUseSkillData {
                    UseSkillData = skill
                });
            }
        }

        private void OnUpdateSkillPointCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Skill.TargetRpc_UpdateSkillPoint)docData;
            var data = GetSkillData(cmdData.skillId);
            data.CurrectSkillPoint = cmdData.skillPoint;
            mySystem.Dispatcher(new CE_Skill.Event_UpdateSkill {
                UseSkillData = data
            });
        }

        private void OnStartUseSkillCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Skill.TargetRpc_StartUseSkill)docData;
            var data = GetSkillData(cmdData.skillId);
            data.CurrectSkillCD = 0f;
            data.CurrectSkillDuration = data.ModeData.SkillDuration;
            mySystem.Dispatcher(new CE_Skill.Event_UpdateSkill {
                UseSkillData = data
            });
        }

        private void OnUseSkillEndCallBack(INetwork network, IProto_Doc docData) {
            var cmdData = (Proto_Skill.TargetRpc_UseSkillEnd)docData;
            var data = GetSkillData(cmdData.skillId);
            data.CurrectSkillCD = data.ModeData.SkillCD;
            data.CurrectSkillDuration = 0f;
            mySystem.Dispatcher(new CE_Skill.Event_UpdateSkill {
                UseSkillData = data
            });
        }

        private void OnRemoveAllSkillsCallBack(INetwork network, IProto_Doc docData) {
            skillList.Clear();
            useSkillData = null;
            mySystem.Dispatcher(new CE_Skill.Event_RemoveAllSkills());
        }

        private void OnSetSkillInProgress(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_Skill.TargetRpc_SetSkillInProgress)docData;
            mySystem.Dispatcher(new CE_Skill.Event_SetSkillInProgress {
                isInProgress = rpcData.isInProgress
            });
        }

        public void DownSkillCD(float deltaTime) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.CurrectSkillCD > 0) {
                    skillData.CurrectSkillCD -= deltaTime;
                }
                if (skillData.CurrectSkillDuration > 0) {
                    skillData.CurrectSkillDuration -= deltaTime;
                }
            }
        }
        
        private SkillData.UseSkillData GetSkillData(int skillId) {
            for (int i = 0; i < skillList.Count; i++) {
                if (skillList[i].ModeData.ID == skillId) {
                    return skillList[i];
                }
            }
            return null;
        }
    }
}