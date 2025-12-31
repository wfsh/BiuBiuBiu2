using System;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GamePlay {
    public class GameEngine : MonoBehaviour {
        private GameWorldManager gameWorldManager;
        void Start() {
            gameWorldManager = new GameWorldManager();
            gameWorldManager.Init();
            MsgRegister.Register<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
        }

        void OnDestroy() {
            if (Application.isEditor) {
                return;
            }
            Clear();
        }

        void Update() {
            gameWorldManager?.Update(Time.deltaTime);
        }

        private void OnGameEngineCloseCallBack(M_Game.GameEngineClose ent) {
            Clear();
        }

        private void Clear() {
            MsgRegister.Unregister<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
            if (gameWorldManager != null) {
                gameWorldManager.Clear();
                gameWorldManager = null;
            } 
        }
    }
}