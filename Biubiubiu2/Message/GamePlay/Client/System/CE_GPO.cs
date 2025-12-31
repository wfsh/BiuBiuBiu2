using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_GPO {

        public struct Event_StartRemoveGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartRemoveGPO>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_MoveDir : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveDir>();
            public int GetID() => _id;

            // 下面写你的参数
            public float MoveH;
            public float MoveV;
        }

        public struct Event_HPChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HPChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int NowHp;
        }
        public struct Event_MaxHPChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MaxHPChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int MaxHp;
        }
        public struct Event_LevelChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_LevelChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Level;
        }
        public struct Event_GetLevel : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetLevel>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }

        public struct Para_GetHpData {
            public int Hp;
            public int MaxHp;
        }
        public struct Event_GetHPData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHPData>();

            public int GetID() => _id;
            // 下面写你的参数 (Hp, MaxHp)
            public Action<Para_GetHpData> CallBack;
        }

        public struct Event_GetFollowAIList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetFollowAIList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<GPOData.AttributeData>> CallBack;
        }

        public struct Event_GetFollowAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetFollowAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<GPOData.AttributeData> CallBack;
        }

        public struct Event_ChangeFollowAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ChangeFollowAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.AttributeData AIData;
        }

        public struct Event_ADDFollowAIData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ADDFollowAIData>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.AttributeData AIData;
        }

        public struct Event_UpdateFollowList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateFollowList>();

            public int GetID() => _id;
            // 下面写你的参数
            public List<GPOData.AttributeData> FollowList;
        }

        public struct Event_TakeOutAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeOutAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        public struct Event_TakeBackAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeBackAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        public struct Event_UseAISkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseAISkill>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_GetAttribute : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAttribute>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<GPOData.AttributeData> CallBack;
        }

        public struct Event_DriveGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DriveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO PlayerDriveGPO;
        }

        public struct Event_PlayerDriveGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayerDriveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO DriveGPO;
        }

        public struct Event_GetDriveGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDriveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }

        public struct Event_SetTankGeneratePoints : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetTankGeneratePoints>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3[] Points;
            public int Count;
        }

        public struct Event_DeviceSkillCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DeviceSkillCD>();

            public int GetID() => _id;
            // 下面写你的参数
            public float MaxCD;
            public float NowCD;
        }

        public struct Event_GetDeviceSkillCD : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDeviceSkillCD>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float> CallBack;
        }

        public struct Event_OnInputDeviceFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnInputDeviceFire>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDown;
        }

        public struct Event_OnDeviceFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnDeviceFire>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3[] Points;
        }

        public struct Event_PlayAnimSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAnimSign>();

            public int GetID() => _id;
            // 下面写你的参数
            public string AnimSign;
        }

        public struct Event_DriverPointRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DriverPointRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
        }
        public struct Event_IsGodMode : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsGodMode>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct ServerSetPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<ServerSetPoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
        }

        public struct Event_SetIsDead : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetIsDead>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsDead;
            public IGPO DeadGPO;
        }

        public struct Event_HasUseSuperWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HasUseSuperWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_SetTransformPointRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetTransformPointRota>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
        }

        public struct GetUseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<IWeapon> CallBack;
        }
    }
}