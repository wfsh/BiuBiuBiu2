using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_GPOSpawner {
        public struct Event_WaveEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_WaveEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            
            public int WaveIndex;
            public int MaxWaveIndex;
            public bool IsWin;
        }
        public struct Event_IntoNextWave : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IntoNextWave>();

            public int GetID() => _id;
            // 下面写你的参数
            public int NextWaveIndex;
            public SpawnAIWaveGroup NextWaveGroup;
        }
        public struct Event_IntoWaveEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IntoWaveEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public int WaveIndex;
            public SpawnAIWaveGroup WaveGroup;
        }
        
        public struct Event_AddSpawnerGPOEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddSpawnerGPOEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO SpawnerGPO;
        }
    }
}