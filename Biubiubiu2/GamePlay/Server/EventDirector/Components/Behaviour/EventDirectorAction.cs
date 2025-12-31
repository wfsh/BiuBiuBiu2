using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EventDirectorAction : EventDirectorBase, IEventDirectorAction {
        protected List<IGPO> gpoList;
        protected EventDirectorData.SubjectType subjectType = EventDirectorData.SubjectType.None;
        protected EventDirectorData.ActionType actionType = EventDirectorData.ActionType.None;
        private Action<EventDirectorAction> onQuitAction;
        private EventDirectorData.Data eventData;
        protected IGPO TriggerGPO;
        protected int subjectID = 0;
        private float waitTime = 0f;
        private bool isQuit = false;
        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            isQuit = false;
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            gpoList = null;
            onQuitAction = null;
        }

        protected override void OnStartBase() {
            base.OnStartBase();
            if (waitTime <= 0) {
                EnterAction();
            } else {
                MsgRegister.Dispatcher(new SM_EventDirector.Event_WaitAction() {
                    ID = ID,
                    EventData = eventData,
                    MData = mData,
                    WaitTime = waitTime,
                    ActionType = (int)actionType,
                    GpoList = gpoList,
                });
                UpdateRegister.AddInvoke(EnterAction, waitTime);
            }
        }
        
        private void EnterAction() {
            MsgRegister.Dispatcher(new SM_EventDirector.Event_EnterAction {
                ID = ID,
                EventData = eventData,
                MData = mData,
                ActionType = (int)actionType,
                GpoList = gpoList,
            });
            OnEnterAction();
        }
        
        public void QuitAction() {
            if (isQuit) {
                return;
            }
            isQuit = true;
            OnQuitAction();
            MsgRegister.Dispatcher(new SM_EventDirector.Event_QuitAction {
                ID = ID,
                EventData = eventData,
                MData = mData,
                ActionType = (int)actionType,
                GpoList = gpoList,
            });
            onQuitAction?.Invoke(this);
        }
        
        virtual protected void OnEnterAction() {
        }
        
        virtual protected void OnQuitAction() {
        }

        
        public void SetGpoList(List<IGPO> gpoList) {
            this.gpoList = gpoList;
        }
        
        public void SetSubject(int subjectType, int subjectID) {
            this.subjectType = (EventDirectorData.SubjectType)subjectType;
            this.subjectID = subjectID;
        }
        
        public void SetActionData(EventDirectorData.Data data, IGPO triggerGPO, EventDirectorData.ActionType actionType, float time) {
            this.actionType = actionType;
            this.TriggerGPO = triggerGPO;
            this.eventData = data;
            waitTime = time;
        }

        public int GetSubjectID() {
            return subjectID;
        }

        public int GetSubjectType() {
            return (int)subjectType;
        }

        public List<IGPO> GetGPOList() {
            return gpoList;
        }

        public void SetOnQuitAction(Action<EventDirectorAction> action) {
            onQuitAction = action;
        }
    }
}
