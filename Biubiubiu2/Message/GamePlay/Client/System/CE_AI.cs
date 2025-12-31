using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_AI {
        public struct Event_GetArmedCustomData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetArmedCustomData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool, MonsterArmedCustom> CallBack; // <isInit, data>
        }

        public struct RemoveAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<RemoveAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }
        public struct Event_IsSkillType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsSkillType>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsSkillType;
        }
        public struct Event_GetSkillType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetSkillType>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
        public struct Event_MoveUp : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveUp>();
            public int GetID() => _id;
            public bool IsTrue;
        }

        public struct Event_MoveDown : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveDown>();
            public int GetID() => _id;
            public bool IsTrue;
        }
        public struct Event_MoveDir : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveDir>();
            public int GetID() => _id;

            // 下面写你的参数
            public float MoveX;
            public float MoveZ;
        }

        public struct Event_SetPlayAnimSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetPlayAnimSign>();
            public int GetID() => _id;

            // 下面写你的参数
            public string AnimSign;
        }

        public struct Event_SetPlayClipId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetPlayClipId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ClipId;
        }

        public struct Event_GetDriveing : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDriveing>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_EnabledDriveMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_EnabledDriveMove>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_DriverPointRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DriverPointRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Point;
            public Quaternion Rota;
        }

        public struct Event_SyncTankUpperBodyRota : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SyncTankUpperBodyRota>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Rota;
        }
        public struct Event_GiantDaDaGetFightRangeData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaGetFightRangeData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3, float> CallBack; // < centerPoint, radius>
        }

        public struct Event_GiantDaDaStartSwitchPerform : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaStartSwitchPerform>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_GiantDaDaSwitchStage : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaSwitchStage>();

            public int GetID() => _id;
            // 下面写你的参数
            public int CurStage;
        }
        public struct Event_SetSummonAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetSummonAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO SummonAIGPO;
        }
        public struct Event_GetMasterGpoID : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMasterGpoID>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }
        public struct Event_GetMaterGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetMaterGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }
        public struct Event_GetIsBoss : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsBoss>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
    }
}