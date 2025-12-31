using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Scene {
        public struct GetPlayerPointList : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetPlayerPointList>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<PlayerSpawnPoint>> CallBack;
            public int TeamIndex;
        }

        public struct GetMonsterPointData : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetMonsterPointData>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<AISpawnPoint>> CallBack;
        }

        public struct SceneSerialized : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SceneSerialized>();

            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct GetRoomAreas : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetRoomAreas>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<RoomArea>> CallBack;
        }
        
        public struct GetRangeScenePoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetRangeScenePoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Point;
            public int MaxRange; // 0 为无上限
            public int MinRange; // 0 为无下限
            public List<int> PointTypes; // 0 为全部
            public Action<List<ScenePoint>> CallBack;
        }
    }
}