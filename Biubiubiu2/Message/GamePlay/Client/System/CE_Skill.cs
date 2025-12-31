using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Skill {
        public struct Event_SetUseSkillData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetUseSkillData>();

            public int GetID() => _id;
            // 下面写你的参数
            public SkillData.UseSkillData UseSkillData;
        }

        public struct Event_GetUseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetUseSkill>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<SkillData.UseSkillData> CallBack;
        }
        
        public struct Event_UseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public SkillData.UseSkillData UseSkillData;
        }
        
        public struct Event_UpdateSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateSkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public SkillData.UseSkillData UseSkillData;
        }

        public struct Event_RemoveAllSkills : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveAllSkills>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_CancelUseSkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CancelUseSkill>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_SetSuperWeaponTip : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSuperWeaponTip>();

            public int GetID() => _id;
            // 下面写你的参数
            public string Tip;
        }

        public struct Event_SetSkillInProgress : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSkillInProgress>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool isInProgress;
        }
    }
}