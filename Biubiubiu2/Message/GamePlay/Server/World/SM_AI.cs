using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_AI {
        public struct Event_AddAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AddAI>();

            public int GetID() => _id;
            // 下面写你的参数
            // 必填参数
            public string AISign; // 必填
            public Vector3 StartPoint;  // 必填
            // 可选参数
            public string OR_AISkinSign;
            public Quaternion OR_StartRota;
            public int OR_TeamId; // 如果不填 就是默认的怪物阵营
            public int OR_GpoId; // 特殊情况需要指定 GPO ID
            public GPOData.GPOType OR_GpoType; // 默认是普通怪物 可以不填
            public IGPOInData OR_InData; // 部分怪物需要传入 In参数
            public Action<IAI> OR_CallBack;
        }
        
        public struct Event_AddMasterAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AddMasterAI>();

            public int GetID() => _id;
            // 下面写你的参数
            // 必填参数
            public string AISign;
            public IGPO MasterGPO;
            public Vector3 StartPoint;
            // 可选参数
            public string OR_AISkinSign;
            public GPOData.AttributeData OR_AIData; // 如果直接录入了属性 就不会使用  AISign
            public Quaternion OR_StartRota;
            public IGPOInData OR_InData;
            public Action<IAI> OR_CallBack;
        }

        public struct Event_RemoveAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_RemoveAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        
        public struct Event_AICreateDropBox : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AICreateDropBox>();

            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public int AIGpoMId;
            public AIData.AIQuality AIQuality;
            public int BoxId;
            public Vector3 Position;
            public List<IGPO> hurtHistoryGPOSet;
        }
        
        public struct Event_DisableAsAITarget : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_DisableAsAITarget>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO Gpo;
        }
        
        public struct Event_BossSwitchStage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_BossSwitchStage>();

            public int GetID() => _id;

            // 下面写你的参数
            public int CurStage;
        }
    }
}