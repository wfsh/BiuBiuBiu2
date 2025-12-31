using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Skill {
        public struct Event_AddSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public SE_Mode.PlayModeCharacterWeapon SkillData;
        }
        public struct Event_AddSkillEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddSkillEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public SE_Mode.PlayModeCharacterWeapon SkillData;
        }
        public struct Event_RemoveAllSkills : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveAllSkills>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        public struct Event_SetSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public SE_Mode.PlayModeCharacterWeapon SkillData;
        }
        public struct Event_GetUseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetUseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }
        public struct Event_UseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UseGPOId;
            public SkillData.Data SkillData;
            public SE_Mode.PlayModeCharacterWeapon WeaponData;
        }

        public struct Event_UseSkillSummonDriver : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseSkillSummonDriver>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UseGPOId;
            public int SummerDriverGPOId;
        }

        // 请求结束技能
        public struct Event_RequestSkillOver : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RequestSkillOver>();

            public int GetID() => _id;
            // 下面写你的参数
            public int SkillId;
        }

        public struct Event_SkillOver : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SkillOver>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO UseGPO;
            public int SkillID;
            public SkillData.SkillTypeEnum SkillType;
        }
        public struct Event_SetSkillPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSkillPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public SkillData.GetSkillPointType GetSkillPointType;
            public int Value;
        }

        public struct Event_ReduceSkillPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ReduceSkillPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public int SkillId;
            public int Value;
        }

        public struct Event_SetSkillInProgressForAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSkillInProgressForAbility>();

            public int GetID() => _id;
            // 下面写你的参数
            public int AbilityId;
            public bool isSkillInProgress;
        }
    }
}