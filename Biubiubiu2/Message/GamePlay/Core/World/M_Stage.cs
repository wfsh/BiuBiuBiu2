using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sofunny.BiuBiuBiu2.CoreMessage {
    public class M_Stage {
        public struct LoadScene : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadScene>();

            public int GetID() => _id;
            // 下面写你的参数
            public string Sign;
            public LoadSceneMode LoadMode;
            public bool IsActive;
            public Action LoadEndCallBack;
        }

        public struct SetGamePlayWorldLayer : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetGamePlayWorldLayer>();

            public int GetID() => _id;
            // 下面写你的参数
            public Transform transform;
            public StageData.GameWorldLayerType layer;
        }
        
        public struct StartLoadStage : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartLoadStage>();

            public int GetID() => _id;
            // 下面写你的参数
            public StageData.StageType stageType;
        }
        
        public struct TaskLoadStart : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<TaskLoadStart>();

            public int GetID() => _id;
            // 下面写你的参数
            public StageData.LoadEnum loadState;
        }
        
        public struct TaskLoadEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<TaskLoadEnd>();

            public int GetID() => _id;
            // 下面写你的参数
            public StageData.LoadEnum loadState;
        }
    }
}