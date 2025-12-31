using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class EventDirectorInstance {
        public class ActionInfo {
            public EventDirectorData.ActionData Data;
            public int UseLimitCount;
        }
        public int EventId;
        private EventDirectorSubject subject;
        private EventDirectorData.SubjectType subjectType;
        private EventDirectorData.Data data;
        private List<EventDirectorTime> timeList = new List<EventDirectorTime>();
        private Dictionary<int, List<EventDirectorAction>> actionDict = new Dictionary<int, List<EventDirectorAction>>();
        private Dictionary<int, List<EventDirectorCondition>> conditionDict = new Dictionary<int, List<EventDirectorCondition>>();
        private Dictionary<int, List<ActionInfo>> canUseActionDict = new Dictionary<int, List<ActionInfo>>();
        private Dictionary<int, List<EventDirectorCondition>> quitConditionDict = new Dictionary<int, List<EventDirectorCondition>>();
        private bool isInTime = false;
        
        public void Init (EventDirectorData.Data data, int eventId) {
            EventId = eventId;
            this.data = data;
            CheckCreateTime();
        }
        
        public void Clear () {
            EventId = 0;
            ClearSubject();
            ClearCondition();
            ClearAllAction();
            timeList.Clear();
        }
        
        public void Update(float deltaTime) {
            if (EventId <= 0 || data.Actions.Length <= 0) {
                return;
            }
            CheckTime();
            if (isInTime) {
                CheckSubject();
                CheckCondition();
                CheckQuitCondition(); // 检查退出条件
            }
        }
        private void CheckCreateTime() {
            timeList.Clear();
            if (data.TimeLogicType == EventDirectorData.LogicType.Random) {
                var index = Random.Range(0, this.data.Times.Length);
                var data = this.data.Times[index];
                CreateTime(data);
            } else {
                for (int i = 0; i < this.data.Times.Length; i++) {
                    var data = this.data.Times[i];
                    if (data.Type == EventDirectorData.TimeType.AnyTime) {
                        continue;
                    }
                    CreateTime(data);
                }
            }
        }

        private void CreateTime(EventDirectorData.TimeData data) {
            var time = GetTime(data);
            if (time != null) {
                time.SetTimeData(data.Type, data.StartValue, data.EndValue);
                time.SetData(data.Value);
                time.Awake();
                timeList.Add(time);
            }
        }
        
        private void CheckTime() {
            var isTrue = true;
            if (timeList.Count > 0) {
                if (data.TimeLogicType == EventDirectorData.LogicType.Or) {
                    isTrue = false;
                    foreach (var time in timeList) {
                        if (time.CheckInTime()) {
                            isTrue = true;
                            break;
                        }
                    }
                } else {
                    foreach (var time in timeList) {
                        if (!time.CheckInTime()) {
                            isTrue = false;
                            break;
                        }
                    }
                }
            }
            if (isInTime == isTrue) {
                return;
            }
            isInTime = isTrue;
            if (isTrue) {
                subjectType = data.Subject.Type;
                if (NoCreateSubjectType() == false) {
                    subject = GetSubject(data.Subject);
                    if (subject != null) {
                        subject.SetSubjectData(data.Subject.Type);
                        subject.SetData(data.Subject.Value);
                        subject.Awake();
                    }
                }
            } else {
                ClearSubject();
                ClearCondition();
            }
        }

        private void CheckSubject() {
            if (isInTime == false) {
                return;
            }
            switch (subjectType) {
                case EventDirectorData.SubjectType.World:
                case EventDirectorData.SubjectType.PlayerAndPlayerAIGroup:
                    CheckCreateCondition(-1);
                    break;
                case EventDirectorData.SubjectType.AIGroup:
                    CheckCreateCondition((int)GPOData.GPOType.AI);
                    break;
                case EventDirectorData.SubjectType.PlayerGroup:
                    CheckCreateCondition((int)GPOData.GPOType.Role);
                    break;
                case EventDirectorData.SubjectType.GPOMIdGroup:
                    CheckCreateCondition(int.Parse(data.Subject.Value));
                    break;
                default:
                    if (subject == null) {
                        Debug.LogError("[EventDirector] Subject 不能是空的, EventId=" + EventId + ", SubjectType=" + subjectType);
                        return;
                    }
                    var list = subject.GetSubjectIds();
                    if (list == null || list.Count <= 0) {
                        return;
                    }
                    foreach (var gpo in list) {
                        CheckCreateCondition(gpo);
                    }
                    break;
            }
        }

        private void ClearSubject() {
            subjectType = EventDirectorData.SubjectType.None;
            if (subject == null) {
                return;
            }
            subject.Clear();
            subject = null;
        }

        private void CheckCreateCondition(int subjectId) {
            var count = GetCanUseActionListCount(subjectId);
            if (count <= 0) {
                return;
            }
            if (conditionDict.ContainsKey(subjectId) == false) {
                var list = new List<EventDirectorCondition>();
                if (data.EnterConditions != null && data.EnterConditions.Length > 0) {
                    if (data.EnterConditionLogicType == EventDirectorData.LogicType.Random) {
                        var index = Random.Range(0, data.EnterConditions.Length);
                        var condData = data.EnterConditions[index];
                        var condition = CreateCondition(condData, subjectId);
                        list.Add(condition);
                    } else {
                        foreach (var condData in data.EnterConditions) {
                            var condition = CreateCondition(condData, subjectId);
                            list.Add(condition);
                        }
                    }
                }
                conditionDict[subjectId] = list;
            }
        }

        private EventDirectorCondition CreateCondition(EventDirectorData.ConditionData condData, int id) {
            var condition = GetCondition(condData);
            if (condition != null) {
                condition.SetID(id, (int)subjectType);
                condition.SetConditionData(condData.Type, condData.CompareType);
                condition.SetData(condData.Value);
                condition.Awake();
            }
            return condition;
        }
        
        private void ClearCondition() {
            foreach (var data in conditionDict) {
                var list = data.Value;
                foreach (var condition in list) {
                    condition?.Clear();
                }
            }
            conditionDict.Clear();
        }
        
        private void CheckCondition() {
            var keys = new List<int>(conditionDict.Keys);
            for (int i = 0; i < keys.Count; i++) {
                var subjectID = keys[i];
                var list = conditionDict[subjectID];
                var isTrue = true;
                List<IGPO> gpoList = null;
                IGPO triggerGPO = null;
                if (data.EnterConditionLogicType == EventDirectorData.LogicType.Or) {
                    isTrue = false;
                    foreach (var condition in list) {
                        if (condition == null) {
                            continue;
                        }
                        gpoList = condition.GetGPOList();
                        if (condition.CheckCondition()) {
                            triggerGPO = condition.LastTriggerGPO();
                            isTrue = true;
                            break;
                        }
                    }
                } else {
                    foreach (var condition in list) {
                        if (condition == null) {
                            continue;
                        }
                        gpoList = condition.GetGPOList();
                        if (condition.CheckCondition() == false) {
                            isTrue = false;
                            break;
                        }
                        triggerGPO = condition.LastTriggerGPO();
                    }
                }
                if (isTrue) {
                    CheckCreateAction(subjectID, gpoList, triggerGPO);
                    ResetCondition(subjectID);
                }
            }
        }
        
        private void CheckCreateAction(int subjectId, List<IGPO> gpoList, IGPO triggerGPO) {
            var count = GetCanUseActionListCount(subjectId);
            if (count <= 0) {
                return;
            }
            var list = GetCanUseActionList(subjectId);
            List<EventDirectorAction> actionList;
            if (actionDict.TryGetValue(subjectId, out actionList) == false) {
                actionList = new List<EventDirectorAction>();
                actionDict[subjectId] = actionList;
            }
            if (data.IsActionRandom) {
                var index = Random.Range(0, list.Count);
                var info = list[index];
                DownActionLimit(info);
                actionList.Add(CreateAction(info.Data, gpoList, subjectId, triggerGPO));
            } else {
                for (int i = list.Count - 1; i >= 0; i--) {
                    var info = list[i];
                    if (info.UseLimitCount == 0) {
                        continue;
                    }
                    DownActionLimit(info);
                    actionList.Add(CreateAction(info.Data, gpoList, subjectId, triggerGPO));
                }
            }
            CreateQuitCondition(subjectId);
        }
        
        private void DownActionLimit (ActionInfo info) {
            if (info.UseLimitCount > 0) {
                info.UseLimitCount--;
            }
        }
        
        private void ResetCondition(int subjectID) {
            List<EventDirectorCondition> conditionList;
            if (conditionDict.TryGetValue(subjectID, out conditionList)) {
                foreach (var condition in conditionList) {
                    condition.Clear();
                }
                conditionDict.Remove(subjectID);
            }
        }
        
        private List<ActionInfo> GetActionList(int subjectID) {
            List<ActionInfo> canUseActionList;
            if (canUseActionDict.TryGetValue(subjectID, out canUseActionList) == false) {
                canUseActionList = new List<ActionInfo>();
                foreach (var actionData in data.Actions) {
                    var info = new ActionInfo();
                    info.Data = actionData;
                    info.UseLimitCount = actionData.UseLimitCount;
                    canUseActionList.Add(info);
                }
                canUseActionDict[subjectID] = canUseActionList;
            }
            return canUseActionList;
        }
        
        private int GetCanUseActionListCount(int subjectID) {
            var actionList = GetActionList(subjectID);
            var count = 0;
            foreach (var info in actionList) {
                if (info.UseLimitCount != 0) {
                    count++;
                }
            }
            return count;
        }
        
        private List<ActionInfo> GetCanUseActionList(int subjectID) {
            var actionList = GetActionList(subjectID);
            var canUseActionList = new List<ActionInfo>();
            foreach (var info in actionList) {
                if (info.UseLimitCount != 0) {
                    canUseActionList.Add(info);
                }
            }
            return canUseActionList;
        }
        
        private EventDirectorAction CreateAction( EventDirectorData.ActionData actionData, List<IGPO> gpoList, int subjectId, IGPO triggerGPO) {
            var action = GetAction(actionData);
            if (action != null) {
                action.SetGpoList(gpoList);
                action.SetSubject((int)subjectType, subjectId);
                action.SetActionData(data, triggerGPO, actionData.Type, actionData.WaitTime);
                action.SetData(actionData.Value);
                action.SetOnQuitAction(OnQuitAction);
                action.Awake();
            }
            return action;
        }
        
        private void ClearAllAction () {
            foreach (var data in actionDict) {
                var list = data.Value;
                foreach (var action in list) {
                    action?.Clear();
                }
            }
            actionDict.Clear();
            canUseActionDict.Clear();
        }

        private void OnQuitAction(EventDirectorAction action) {
            Debug.LogError("退出事件");
            action.Clear();
            if (actionDict.ContainsKey(action.GetSubjectID()) == false) {
                return;
            }
            var list = actionDict[action.GetSubjectID()];
            list.Remove(action);
        }
        
        private void CreateQuitCondition(int subjectId) {
            if (data.QuitConditions == null || data.QuitConditions.Length <= 0) {
                return;
            }
            if (quitConditionDict.ContainsKey(subjectId) == false) {
                var list = new List<EventDirectorCondition>();
                if (data.QuitConditionLogicType == EventDirectorData.LogicType.Random) {
                    var index = Random.Range(0, data.QuitConditions.Length);
                    var condData = data.QuitConditions[index];
                    var condition = CreateCondition(condData, subjectId);
                    list.Add(condition);
                } else {
                    foreach (var condData in data.QuitConditions) {
                        var condition = CreateCondition(condData, subjectId);
                        list.Add(condition);
                    }
                }
                quitConditionDict[subjectId] = list;
            }
        }
        
        private void CheckQuitCondition() {
            var keys = new List<int>(quitConditionDict.Keys);
            for (int i = 0; i < keys.Count; i++) {
                var subjectID = keys[i];
                var list = quitConditionDict[subjectID];
                var isTrue = true;
                List<IGPO> gpoList = null;
                if (data.QuitConditionLogicType == EventDirectorData.LogicType.Or) {
                    isTrue = false;
                    foreach (var condition in list) {
                        if (condition == null) {
                            continue;
                        }
                        if (condition.CheckCondition()) {
                            isTrue = true;
                            break;
                        }
                    }
                } else {
                    foreach (var condition in list) {
                        if (condition == null) {
                            continue;
                        }
                        if (condition.CheckCondition() == false) {
                            isTrue = false;
                            break;
                        }
                    }
                }
                if (isTrue) {
                    QuitAllAction(subjectID);
                    ResetQuitCondition(subjectID);
                }
            }
        }
        
        private void QuitAllAction(int subjectID) {
            List<EventDirectorAction> actionList;
            if (actionDict.TryGetValue(subjectID, out actionList)) {
                for (int i = actionList.Count - 1; i >= 0; i--) {
                    var action = actionList[i];
                    action?.QuitAction();
                }
                actionList.Clear();
            }
        }
        
        private void ResetQuitCondition(int subjectID) {
            List<EventDirectorCondition> conditionList;
            if (quitConditionDict.TryGetValue(subjectID, out conditionList)) {
                foreach (var condition in conditionList) {
                    condition?.Clear();
                }
                quitConditionDict.Remove(subjectID);
            }
        }
    }
}
