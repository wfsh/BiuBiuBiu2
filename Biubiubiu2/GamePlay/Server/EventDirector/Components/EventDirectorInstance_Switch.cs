using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class EventDirectorInstance {
        private bool NoCreateSubjectType() {
            return subjectType == EventDirectorData.SubjectType.World ||
                   subjectType == EventDirectorData.SubjectType.PlayerAndPlayerAIGroup ||
                   subjectType == EventDirectorData.SubjectType.PlayerGroup ||
                   subjectType == EventDirectorData.SubjectType.AIGroup ||
                   subjectType == EventDirectorData.SubjectType.GPOMIdGroup ;
        }
        
        public EventDirectorTime GetTime(EventDirectorData.TimeData data) {
            EventDirectorTime condition = null;
            switch (data.Type) {
                case EventDirectorData.TimeType.GameStartTime:
                    condition = new Time_GameStartTime();
                    break;
                default:
                    Debug.LogError($"EventDirectorInstance.GetTime: Unknown Condition type: {data.Type}");
                    break;
            }
            return condition;
        }
        
        public EventDirectorSubject GetSubject(EventDirectorData.SubjectData data) {
            EventDirectorSubject subject = null;
            switch (data.Type) {
                case EventDirectorData.SubjectType.Player:
                    subject = new Subject_Player();
                    break;
                case EventDirectorData.SubjectType.PlayerTeam:
                    subject = new Subject_PlayerTeam();
                    break;
                case EventDirectorData.SubjectType.AI:
                    subject = new Subject_AI();
                    break;
                case EventDirectorData.SubjectType.PlayerAndPlayerAI:
                    subject = new Subject_PlayerAndAI();
                    break;
                case EventDirectorData.SubjectType.GPOMId:
                    subject = new Subject_GpoMID();
                    break;
                default:
                    Debug.LogError($"EventDirectorInstance.GetSubject: Unknown Subject type: {data.Type}");
                    break;
            }
            return subject;
        }
        
        public EventDirectorCondition GetCondition(EventDirectorData.ConditionData data) {
            EventDirectorCondition condition = null;
            switch (data.Type) {
                case EventDirectorData.ConditionType.HasItem:
                    condition = new Condition_HasItem();
                    break;
                case EventDirectorData.ConditionType.SlideCount:
                    condition = new Condition_SlideCount();
                    break;
                case EventDirectorData.ConditionType.FireCount:
                    condition = new Condition_FireCount();
                    break;
                case EventDirectorData.ConditionType.HasBoss:
                    condition = new Condition_HasBoss();
                    break;
                case EventDirectorData.ConditionType.WaitTime:
                    condition = new Condition_WaitTime();
                    break;
                case EventDirectorData.ConditionType.GPOMDead:
                    condition = new Condition_GPOMDead();
                    break;
                case EventDirectorData.ConditionType.GPOMTypeDead:
                    condition = new Condition_GPOMTypeDead();
                    break;
                case EventDirectorData.ConditionType.KillMTypeAI:
                    condition = new Condition_KillMTypeAI();
                    break;
                case EventDirectorData.ConditionType.KillAI:
                    condition = new Condition_KillAI();
                    break;
                case EventDirectorData.ConditionType.KillRangePointAI:
                    condition = new Condition_KillRangePointAI();
                    break;
                case EventDirectorData.ConditionType.PlayerEnterArea:
                    condition = new Condition_PlayerEnterArea();
                    break;
                default:
                    Debug.LogError($"EventDirectorInstance.GetCondition: Unknown Condition type: {data.Type}");
                    break;
            }
            return condition;
        }
        
        public EventDirectorAction GetAction(EventDirectorData.ActionData data) {
            EventDirectorAction action = null;
            switch (data.Type) {
                case EventDirectorData.ActionType.SpawnAI:
                    action = new Action_SpawnAI();
                    break;
                case EventDirectorData.ActionType.UpHpPlayer:
                    action = new Action_UpHpPlayer();
                    break;
                case EventDirectorData.ActionType.SpawnTriggerGPOTeamAI:
                    action = new Action_SpawnTriggerGPOTeamAI();
                    break;
                case EventDirectorData.ActionType.SpawnSummerJokerStone:
                    action = new Action_SpawnSummerJokerStone();
                    break;
                default:
                    Debug.LogError($"EventDirectorInstance.GetAction: Unknown Action type: {data.Type}");
                    break;
            }
            return action;
        }
    }
}
