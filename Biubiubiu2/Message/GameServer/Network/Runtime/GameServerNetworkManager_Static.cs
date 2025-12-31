using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEditor;

namespace Sofunny.BiuBiuBiu2.GameServerMessage {

    public partial class GameServerNetworkManager {
        protected static GameServerNetworkManager instance = null;
        public const string GAMESERVER_CHANNEL = "GameServer";

        public static GameServerNetworkManager Instance {
            get {
                if (instance != null) {
                    return instance;
                }
#if UNITY_EDITOR
                if (EditorApplication.isPlaying == false) {
                    return null;
                }
#endif
                instance = new GameObject(typeof(GameServerNetworkManager).Name).AddComponent<GameServerNetworkManager>();
                if (Application.isPlaying) {
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        public static void Dispose() {
            if (instance == null) {
                return;
            }
            Debug.Log("GameServer Dispose");
            instance.CloseAll();
            GameObject.Destroy(instance.gameObject);
            instance = null;
        }

#if UNITY_EDITOR
        static GameServerNetworkManager() {
            // 监听运行模式的变化
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state) {
            // 检查当前状态
            instance?.Close(GAMESERVER_CHANNEL);
            DestroyImmediate(instance);
        }
#endif
    }

}