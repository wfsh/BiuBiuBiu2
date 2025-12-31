using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_EventDirector {
        public struct Event_WaitAction : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_WaitAction>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ID;
            public EventDirectorData.Data EventData;
            public float WaitTime;
            public int ActionType;
            public IEventDirectorData MData;
            public List<IGPO> GpoList;
        }    
        
        public struct Event_QuitAction : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_QuitAction>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ID;
            public EventDirectorData.Data EventData;
            public int ActionType;
            public IEventDirectorData MData;
            public List<IGPO> GpoList;
        }   
        
        public struct Event_EnterAction : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_EnterAction>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ID;
            public EventDirectorData.Data EventData;
            public int ActionType;
            public IEventDirectorData MData;
            public List<IGPO> GpoList;
        }     
    }
}