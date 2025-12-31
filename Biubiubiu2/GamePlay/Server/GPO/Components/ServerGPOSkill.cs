using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSkill : ServerNetworkComponentBase {
        
        public class UseSkillData {
            public SkillData.Data ModeData;
            public SE_Mode.PlayModeCharacterWeapon WeaponData;
            public float CurrectSkillCD; // 当前技能 CD
            public float CurrectSkillDuration; // 当前技能持续时间
            public int CurrectSkillPoint; // 当前技能点数
            public bool IsSkillInProgressForAbility; // 是否正在使用技能 (由技能本身驱动)
            
            public bool IsSkillInProgress {
                get {
                    return IsSkillInProgressForAbility || CurrectSkillDuration > 0;
                }
            }
        }

        protected enum UseSkillFailed {
            InRoom
        }

        protected List<UseSkillData> skillList = new List<UseSkillData>(6);
        private float delayGetModePointTime = 1f;
        private IWeapon useWeapon;
        private int unEquipWeaponId;

        protected override void OnAwake() {
            MsgRegister.Register<SM_ShortcutTool.Event_EquipSkillChange>(OnShortcutEquipSkillChange);
            MsgRegister.Register<SM_ShortcutTool.Event_ActivateSuperWeapon>(OnActivateSuperWeapon);
            Register<SE_Skill.Event_AddSkill>(OnAddSkillCallBack);
            Register<SE_Skill.Event_RemoveAllSkills>(OnRemoveAllSkills);
            Register<SE_Skill.Event_SetSkillPoint>(OnSetSkillPointCallBack);
            Register<SE_Skill.Event_ReduceSkillPoint>(OnReduceSkillPoint);
            Register<SE_Skill.Event_GetUseSkill>(OnGetUseSkillCallBack);
            Register<SE_Skill.Event_RequestSkillOver>(OnRequestSkillOver);
            Register<SE_Skill.Event_SetSkillInProgressForAbility>(OnSetSkillInProgressForAbility);
            Register<SE_GMEditor.Event_StartActiveSkill>(OnStartActiveSkill);
            Register<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            // SetSkill(SkillData.Skill_SummerDriverTank);
        }

        // private int clearIndex = 0;
        protected override void OnClear() {
            base.OnClear();
            // clearIndex++;
            // Debug.Log("clearIndex:" + clearIndex + " -- " + isClear);
            MsgRegister.Unregister<SM_ShortcutTool.Event_EquipSkillChange>(OnShortcutEquipSkillChange);
            MsgRegister.Unregister<SM_ShortcutTool.Event_ActivateSuperWeapon>(OnActivateSuperWeapon);
            Unregister<SE_Skill.Event_AddSkill>(OnAddSkillCallBack);
            Unregister<SE_Skill.Event_RemoveAllSkills>(OnRemoveAllSkills);
            Unregister<SE_Skill.Event_SetSkillPoint>(OnSetSkillPointCallBack);
            Unregister<SE_Skill.Event_ReduceSkillPoint>(OnReduceSkillPoint);
            Unregister<SE_Skill.Event_GetUseSkill>(OnGetUseSkillCallBack);
            Unregister<SE_Skill.Event_RequestSkillOver>(OnRequestSkillOver);
            Unregister<SE_GMEditor.Event_StartActiveSkill>(OnStartActiveSkill);
            Unregister<SE_Skill.Event_SetSkillInProgressForAbility>(OnSetSkillInProgressForAbility);
            Unregister<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            RemoveProtoCallBack(Proto_Skill.Cmd_CancelUseSkill.ID, OnCancelUseSkill);
            RemoveUpdate(OnUpdate);
        }
        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Skill.Cmd_CancelUseSkill.ID, OnCancelUseSkill);
        }
        private void OnCancelUseSkill(INetwork network, IProto_Doc proto) {

            var cmd = (Proto_Skill.Cmd_CancelUseSkill)proto;
            Dispatcher(new SE_Skill.Event_RequestSkillOver {
                SkillId = cmd.skillId,
            });
        }
        private void OnUseWeaponCallBack(ISystemMsg body, SE_GPO.UseWeapon ent) {
            useWeapon = ent.Weapon;
        }

        private void OnUpdate(float deltaTime) {
            DownSkillCD(deltaTime);
            TimeGetModePoint();
        }

        protected void UseSkill(int skillId) {
            if (IsHaveSkill(skillId) == false) {
                Debug.LogError($"没有该技能： {SkillData.GetData(skillId).SkillType}  ID：{skillId}");
                return;
            }
            var skillData = GetSkillData(skillId);
            if (CheckUseSkill(skillData) == false) {
                return;
            }
            skillData.CurrectSkillDuration = skillData.ModeData.SkillDuration;
            PerfAnalyzerAgent.SetTag($"超级武器 - {skillData.ModeData.Sign}");
            if (skillData.ModeData.SkillType != SkillData.SkillTypeEnum.HoldOn) {
                DownSkillPoint(skillData, skillData.ModeData.SkillActivePoint);
            }

            if (skillData.ModeData.SkillType != SkillData.SkillTypeEnum.CallMonster) {
                CancelUseWeapon();
            }
            
            OnUseSkill(skillData);
        }

        virtual protected void OnUseSkill(UseSkillData skillData) {
            Dispatcher(new SE_Skill.Event_UseSkill {
                UseGPOId = iGPO.GetGpoID(),
                SkillData = skillData.ModeData,
                WeaponData = skillData.WeaponData
            });
        }

        private bool CheckUseSkill(UseSkillData skillData) {
            if (skillData.CurrectSkillDuration > 0) {
#if UNITY_EDITOR
                Debug.LogError("技能持续中");
#endif
                return false;
            }
            if (skillData.CurrectSkillCD > 0) {
#if UNITY_EDITOR
                Debug.LogError("技能 CD 中");
#endif
                return false;
            }
            if (skillData.CurrectSkillPoint < skillData.ModeData.SkillActivePoint) {
#if UNITY_EDITOR
                Debug.LogError("技能点数不足");
#endif
                return false;
            }
            if (SkillData.IsDisableUseInRoom(skillData.ModeData.ID)) {
                bool canUse = IsInRoom() == false && IsCanUseSkill(skillData);

                if (!canUse) {
                    OnUseSkillFailed(UseSkillFailed.InRoom);
                }
                return canUse;
            } else {
                bool canUse = IsCanUseSkill(skillData);
                if (!canUse) {
                    OnUseSkillFailed(UseSkillFailed.InRoom);
                }
                return canUse;
            }

            return true;
        }
        private bool IsCanUseSkill(UseSkillData skillData) {
            var skillType = skillData.ModeData.SkillType;
            var canUse = false;
            if (skillType == SkillData.SkillTypeEnum.CallMonster ||  skillType == SkillData.SkillTypeEnum.SummonDriver) {
                var area = skillData.ModeData.NeedArea;
                Dispatcher(new SE_GPO.Event_IsCanUseCallAISkill() {
                    Width = area.width,
                    Height = area.height,
                    CallBack = isCallMonster => canUse = isCallMonster
                });
                
                return canUse;
            }

            return true;
        }
        private bool IsInRoom() {
            bool isInRoom = false;
            Dispatcher(new SE_GPO.Event_GetIsInRoom {
                CallBack = inRoom => isInRoom = inRoom
            });
            return isInRoom;
        }
        
        public void DownSkillCD(float deltaTime) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.CurrectSkillCD > 0) {
                    skillData.CurrectSkillCD -= deltaTime;
                }
                if (skillData.CurrectSkillDuration > 0) {
                    skillData.CurrectSkillDuration -= deltaTime;
                    if (skillData.CurrectSkillDuration <= 0) {
                        SkillOver(skillData);
                    }
                }
            }
        }

        // 技能持续时间结束
        public void SkillOver(UseSkillData skillData) {
            skillData.CurrectSkillDuration = 0f;
            skillData.CurrectSkillCD = skillData.ModeData.SkillCD;
            // 结束技能
            mySystem.Dispatcher(new SE_Skill.Event_SkillOver {
                SkillID = skillData.ModeData.ID,
                UseGPO = iGPO,
                SkillType = skillData.ModeData.SkillType
            });
            // 重新穿上武器
            if (unEquipWeaponId != 0) {
                iGPO.Dispatcher(new SE_GPO.Event_OnEquipPackWeapon {
                    WeaponId = unEquipWeaponId
                });
                unEquipWeaponId = 0;
            }
            OnSkillOver(skillData);
        }

        virtual protected void OnSkillOver(UseSkillData skillData) { }
        private void OnShortcutEquipSkillChange(SM_ShortcutTool.Event_EquipSkillChange ent) {
            if (ent.GpoId >= 0) {
                if (ent.GpoId != GpoID) {
                    return;
                }
            } else {
                if (ent.GpoId == -2 && iGPO.GetGPOType() != GPOData.GPOType.Role) {
                    return;
                }
            }
            RemoveAllSkills();
            var weaponData = new SE_Mode.PlayModeCharacterWeapon();
            weaponData.WeaponItemId = ent.WeaponItemId;
            weaponData.IsSuperWeapon = true;
            AddSkillForWeaponData(weaponData);
        }
        private void OnAddSkillCallBack(ISystemMsg body, SE_Skill.Event_AddSkill ent) {
            AddSkillForWeaponData(ent.SkillData);
        }

        private void AddSkillForWeaponData(SE_Mode.PlayModeCharacterWeapon weaponData) {
            var skillData = SkillData.GetDataForItemId(weaponData.WeaponItemId);
            if (skillData == null) {
                return;
            }
            SetSkill(skillData, weaponData);
            Dispatcher(new SE_GPO.Event_SetCheckInRoomEnable {
                IsEnable = SkillData.IsDisableUseInRoom(skillData.ID)
            });
            if (ModeData.PlayMode == ModeData.ModeEnum.ModeExplore) {
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, 1);
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, 2);
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, 3);
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, 4);
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, 5);
            }
        }
         
        private void OnStartActiveSkill(ISystemMsg body, SE_GMEditor.Event_StartActiveSkill ent) {
            ActiveSkill();
        }
         
        private void OnActivateSuperWeapon(SM_ShortcutTool.Event_ActivateSuperWeapon ent) {
            if (ent.GpoId >= 0) {
                if (ent.GpoId != GpoID) {
                    return;
                }
            } else {
                if (ent.GpoId == -2 && iGPO.GetGPOType() != GPOData.GPOType.Role) {
                    return;
                }
            }
            ActiveSkill();
        }

        private void ActiveSkill() {
            var skillData = skillList[0];
            if (skillData.IsSkillInProgress) {
                SkillOver(skillData);
            }
            skillData.CurrectSkillCD = 0f;
            for (int i = 0; i < skillData.ModeData.SkillActivePoint; i++) {
                AddPoint(SkillData.GetSkillPointType.ContinueGetModePoint, i + 1);
            }
        }
        
        private void OnRemoveAllSkills(ISystemMsg body, SE_Skill.Event_RemoveAllSkills ent) {
            RemoveAllSkills();
        }

        private void RemoveAllSkills() {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.IsSkillInProgress) {
                    SkillOver(skillData);
                }
            }
            skillList.Clear();
            Dispatcher(new SE_GPO.Event_SetCheckInRoomEnable {
                IsEnable = false
            });
            OnRemoveAllSkills();
        }

        private void OnGetUseSkillCallBack(ISystemMsg body, SE_Skill.Event_GetUseSkill ent) {
            if (skillList.Count == 0) {
                ent.CallBack(0);
                return;
            }
            ent.CallBack(skillList[0].ModeData.ID);
        }

        private void OnRequestSkillOver(ISystemMsg body, SE_Skill.Event_RequestSkillOver ent) {
            SkillOver(GetSkillData(ent.SkillId));
        }

        protected virtual void OnSetSkillInProgressForAbility(ISystemMsg body, SE_Skill.Event_SetSkillInProgressForAbility ent) {
            // 当前只有 1 个技能，所以可以这么写
            var skillData = skillList[0];
            skillData.IsSkillInProgressForAbility = ent.isSkillInProgress;
        }

        private void SetSkill(SkillData.Data skillData, SE_Mode.PlayModeCharacterWeapon weapon) {
            if (IsHaveSkill(skillData.ID)) {
                Debug.LogError($"已经有该技能了 {skillData.SkillType}");
                return;
            }
            var data = new UseSkillData {
                ModeData = skillData,
                WeaponData = weapon,
                CurrectSkillCD = 0f,
                CurrectSkillDuration = 0f,
                CurrectSkillPoint = 0,
            };
            skillList.Add(data);
            OnSetSkill(data);
            mySystem.Dispatcher(new SE_Skill.Event_AddSkillEnd {
                SkillData = weapon
            });
        }

        virtual protected void OnSetSkill(UseSkillData skillData) {
        }

        protected virtual void OnRemoveAllSkills() { }

        protected virtual void OnUseSkillFailed(UseSkillFailed failed) { }

        private void OnSetSkillPointCallBack(ISystemMsg body, SE_Skill.Event_SetSkillPoint ent) {
            AddPoint(ent.GetSkillPointType, ent.Value);
        }

        private void OnReduceSkillPoint(ISystemMsg body, SE_Skill.Event_ReduceSkillPoint ent) {
            var skillData = GetSkillData(ent.SkillId);
            DownSkillPoint(skillData, ent.Value);
        }

        private void TimeGetModePoint() {
            if (delayGetModePointTime > 0) {
                delayGetModePointTime -= Time.deltaTime;
                return;
            }
            delayGetModePointTime = 1f;
            AddPoint(SkillData.GetSkillPointType.TimeGetModePoint, 1);
        }

        private void AddPoint(SkillData.GetSkillPointType type, int value) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.IsSkillInProgress) {
                    continue;
                }
                if (skillData.ModeData.GetSkillPointType == type) {
                    switch (type) {
                        case SkillData.GetSkillPointType.GetModePoint:
                            AddSkillPoint(skillData, skillData.ModeData.GetSkillPoint);
                            break;
                        case SkillData.GetSkillPointType.ContinueGetModePoint:
                            SetContinueGetPoint(skillData, value);
                            break;
                        case SkillData.GetSkillPointType.TimeGetModePoint:
                            AddSkillPoint(skillData, skillData.ModeData.GetSkillPoint);
                            break;
                    }
                }
            }
        }

        private void SetContinueGetPoint(UseSkillData skillData, int value) {
            if (value == 0 || value < skillData.CurrectSkillPoint) {
                if (skillData.CurrectSkillPoint < skillData.ModeData.SkillActivePoint) {
                    skillData.CurrectSkillPoint = value;
                }
            } else {
                if (skillData.CurrectSkillPoint >= skillData.ModeData.MaxSaveSkillPoint) {
                    skillData.CurrectSkillPoint = skillData.ModeData.MaxSaveSkillPoint;
                } else {
                    skillData.CurrectSkillPoint += skillData.ModeData.GetSkillPoint;
                }
            }
            UpdateSkillPoint(skillData);
        }

        private void AddSkillPoint(UseSkillData skillData, int value) {
            skillData.CurrectSkillPoint += value;
            if (skillData.CurrectSkillPoint >= skillData.ModeData.MaxSaveSkillPoint) {
                skillData.CurrectSkillPoint = skillData.ModeData.MaxSaveSkillPoint;
            }
            UpdateSkillPoint(skillData);
        }

        private void UpdateSkillPoint(UseSkillData skillData) {
            OnUpdateSkillPoint(skillData);
            if (iGPO.GetGPOType() == GPOData.GPOType.RoleAI && !iGPO.IsDead()) {
                if (skillData.CurrectSkillDuration <= 0
                    && skillData.CurrectSkillCD <= 0
                    && skillData.CurrectSkillPoint >= skillData.ModeData.SkillActivePoint) {
                    UseSkill(skillData.ModeData.ID);
                }
            }
        }

        private void DownSkillPoint(UseSkillData skillData, int value) {
            skillData.CurrectSkillPoint -= value;
            skillData.CurrectSkillPoint = Mathf.Max(skillData.CurrectSkillPoint, 0);
            UpdateSkillPoint(skillData);
        }

        virtual protected void OnUpdateSkillPoint(UseSkillData skillData) {
        }

        // 是否已经有该技能
        private bool IsHaveSkill(int skillID) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.ModeData.ID == skillID) {
                    return true;
                }
            }
            return false;
        }

        private UseSkillData GetSkillData(int skillID) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.ModeData.ID == skillID) {
                    return skillData;
                }
            }
            Debug.LogError("没有该技能 " + skillID);
            return null;
        }
        private UseSkillData GetSkillData(string skillSign) {
            for (int i = 0; i < skillList.Count; i++) {
                var skillData = skillList[i];
                if (skillData.ModeData.Sign == skillSign) {
                    return skillData;
                }
            }
            Debug.LogError("没有该技能 " + skillSign);
            return null;
        }

        private void CancelUseWeapon() {
            unEquipWeaponId = useWeapon?.GetWeaponId() ?? 0;
            // 卸下当前在使用的武器
            iGPO.Dispatcher(new SE_GPO.SetCanceUseWeapon() {
                WeaponId = 0
            });
        }
    }
}