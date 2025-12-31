using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_GPO {
        public struct AddGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO IGpo;
        }
        public struct AddGPOEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddGPOEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO IGpo;
        }
        public struct RemoveGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }
        public struct RemoveGPOEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveGPOEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO IGpo;
        }
        public struct GetGPOList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOList>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
        }
        public struct GetGPOListForTeamId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOListForTeamId>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
            public int TeamId;
        }
        
        public struct GetGPOListForGpoType : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOListForGpoType>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
            public GPOData.GPOType GpoType;
        }
        
        public struct GetGPOListForRoleAndAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOListForRoleAndAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<List<IGPO>> CallBack;
        }

        public struct GetGPO : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public Action<IGPO> CallBack;
        }
        public struct GetCharacterGPOByPlayerId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCharacterGPOByPlayerId>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public Action<IGPO> CallBack;
        }
        public struct GetAICharacterGPOByAiId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetAICharacterGPOByAiId>();

            public int GetID() => _id;
            // 下面写你的参数
            public int AiId;
            public Action<IGPO> CallBack;
        }
        public struct GetGPOForCharacterNetwork : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGPOForCharacterNetwork>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ConnId;
            public Action<IGPO> CallBack;
        }
        public struct Event_AfterDownHP : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AfterDownHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
            public IGPO TargetGPO;
            public float ActualDownHp;
            public bool IsHead;
            public int AttackItemId;
        }

        public struct ReportTask : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ReportTask>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public int SysJsonTypeId;
            public string TaskJson;
        }
        public struct GpoDeadEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GpoDeadEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO DeadGpo;
        }
    }
}