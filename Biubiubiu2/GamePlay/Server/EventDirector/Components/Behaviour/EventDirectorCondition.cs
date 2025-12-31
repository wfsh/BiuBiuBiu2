using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EventDirectorCondition : EventDirectorBase, IEventDirectorCondition {
        protected List<IGPO> gpoList = new List<IGPO>();
        private EventDirectorData.SubjectType subjectType = EventDirectorData.SubjectType.None;
        protected EventDirectorData.CompareType compareType = EventDirectorData.CompareType.Equal;
        protected EventDirectorData.ConditionType conditionType = EventDirectorData.ConditionType.None;
        private int subjectId = -1;
        private IGPO triggerGPO = null;
        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            for (int i = 0; i < gpoList.Count; i++) {
                OnSetGpo(gpoList[i]);
            }
        }

        protected override void OnStartBase() {
            base.OnStartBase();
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            triggerGPO = null;
            MsgRegister.Unregister<SM_GPO.AddGPOEnd>(OnAddGpoEnd);
            MsgRegister.Unregister<SM_GPO.RemoveGPOEnd>(OnRemoveGpoEnd);
            for (int i = 0; i < gpoList.Count; i++) {
                OnRemoveGpo(gpoList[i]);
            }
            gpoList = null;
        }
        
        protected virtual void OnSetGpo(IGPO gpo) {
        }
        
        protected virtual void OnRemoveGpo(IGPO gpo) {
        }
        
        public void SetConditionData(EventDirectorData.ConditionType conditionType, EventDirectorData.CompareType compareType) {
            this.compareType = compareType;
            this.conditionType = conditionType;
        }
        
        public void SetID(int id, int sType) {
            this.subjectType = (EventDirectorData.SubjectType)sType;
            this.subjectId = id;
            var isList = true;
            switch (this.subjectType) {
                case EventDirectorData.SubjectType.World:
                    GetWorldGPO();
                    break;
                case EventDirectorData.SubjectType.PlayerTeam:
                    GetTeamList(id);
                    break;
                case EventDirectorData.SubjectType.PlayerGroup:
                case EventDirectorData.SubjectType.AIGroup:
                    GetGPOTypeList((GPOData.GPOType)id);
                    break;
                case EventDirectorData.SubjectType.PlayerAndPlayerAIGroup:
                    GetRoleAndAIList();
                    break;
                case EventDirectorData.SubjectType.GPOMIdGroup:
                    GetGPOMList(id);
                    break;
                case EventDirectorData.SubjectType.PlayerAndPlayerAI:
                case EventDirectorData.SubjectType.Player:
                case EventDirectorData.SubjectType.AI:
                case EventDirectorData.SubjectType.GPOMId:
                    GetGPO(id);
                    isList = false;
                    break;
            }
            if (isList) {
                MsgRegister.Register<SM_GPO.AddGPOEnd>(OnAddGpoEnd);
                MsgRegister.Register<SM_GPO.RemoveGPOEnd>(OnRemoveGpoEnd);
            }
        }
        public void GetGPO(int gpoId) {
            MsgRegister.Dispatcher(new SM_GPO.GetGPO {
                GpoId = gpoId,
                CallBack = iGpo => {
                    gpoList.Add(iGpo);
                }
            });
        }
        public void GetGPOTypeList(GPOData.GPOType gpoType) {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForGpoType() {
                GpoType = gpoType,
                CallBack = list => {
                    gpoList = list;
                }
            });
        }
        public void GetRoleAndAIList() {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForRoleAndAI() {
                CallBack = list => {
                    gpoList = list;
                }
            });
        }
        public void GetTeamList(int teamId) {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForTeamId {
                TeamId = teamId,
                CallBack = list => {
                    gpoList = list;
                }
            });
        }
        
        public void GetWorldGPO() {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = list => {
                    gpoList = list;
                }
            });
        }
        
        public void GetGPOMList(int gpoMId) {
            gpoList.Clear();
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = list => {
                    for (int i = 0; i < list.Count; i++) {
                        var gpo = list[i];
                        if (gpo.GetGpoMID() == gpoMId) {
                            gpoList.Add(gpo);
                        }
                    }
                }
            });
        }
        
        private void OnAddGpoEnd(SM_GPO.AddGPOEnd msg) {
            var isSet = UpdateList(msg.IGpo);
            if (isSet) {
                OnSetGpo(msg.IGpo);
            }
        }

        private void OnRemoveGpoEnd(SM_GPO.RemoveGPOEnd msg) {
            var isRemove = UpdateList(msg.IGpo);
            if (isRemove) {
                OnRemoveGpo(msg.IGpo);
            }
        }

        private bool UpdateList(IGPO gpo) {
            var isTrue = false;
            switch (this.subjectType) {
                case EventDirectorData.SubjectType.World:
                    isTrue = true;
                    break;
                case EventDirectorData.SubjectType.PlayerTeam:
                    if (this.subjectId == gpo.GetTeamID()) {
                        isTrue = true;
                        GetTeamList(this.subjectId);
                    }
                    break;
                case EventDirectorData.SubjectType.PlayerGroup:
                case EventDirectorData.SubjectType.AIGroup:
                    var gpoType = (GPOData.GPOType)this.subjectId;
                    if (gpoType == gpo.GetGPOType()) {
                        isTrue = true;
                        GetGPOTypeList(gpoType);
                    }
                    break;
                case EventDirectorData.SubjectType.GPOMIdGroup:
                    if (this.subjectId == gpo.GetGpoMID()) {
                        isTrue = true;
                        GetTeamList(this.subjectId);
                    }
                    break;
                case EventDirectorData.SubjectType.PlayerAndPlayerAIGroup:
                    if (gpo.GetGPOType() == GPOData.GPOType.Role || gpo.GetGPOType() == GPOData.GPOType.RoleAI) {
                        isTrue = true;
                        GetRoleAndAIList();
                    }
                    break;
            }
            return isTrue;
        }

        public List<IGPO> GetGPOList() {
            return gpoList;
        }

        virtual public bool CheckCondition() {
            return false;
        }
        
        // 返回最后触发的GPO
        public IGPO LastTriggerGPO() {
            return triggerGPO;
        }
        
        protected void SetTriggerGPO(IGPO gpo) {
            this.triggerGPO = gpo;
        }
    }
}
