using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Scene {
        public class ElementCreateData {
            public int CreateID; // 生成 ID
            public int GatheringCount; // 剩余可采集次数
            public float NextResetTime; // 下次次数重置时间
            public Vector3 Point; // 采集点
            public SceneData.ElementEnum Element; // 采集物品
            public ElementSpawnPoint ModeData;
        }
        public struct SendElementSpawnPointList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SendElementSpawnPointList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ElementSpawnPoint> ElementSpawnPoints;
        }
        public struct SendElementCreateList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SendElementCreateList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ElementCreateData> ElementCreateDataList;
        }
        public struct PlayAbilityForElementType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAbilityForElementType>();
            public int GetID() => _id;

            // 下面写你的参数
            public SceneData.ElementEnum ElementType; // 采集物品
            public IGPO PickGPO;
        }
        public struct CanPlayAbilityForElementType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CanPlayAbilityForElementType>();
            public int GetID() => _id;

            // 下面写你的参数
            public SceneData.ElementEnum ElementType; // 采集物品
            public IGPO PickGPO;
            public Action<bool> CallBack;
        }
        public struct SendScenePoints : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SendScenePoints>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ScenePoint> ScenePoints;
        }
    }
}