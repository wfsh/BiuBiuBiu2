using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GamePlay {
    public interface IGameWorldManager {
        void Init();
        void Clear();
        void Update(float deltaTime);
    }

    public class GameWorldManager {
        private List<GameWorld> gameWorlds = new List<GameWorld>();
        public void Init() {
            var gameWorld = new GameWorld();
            gameWorld.Init();
            gameWorlds.Add(gameWorld);
        }

        public void Clear() {
            for (int i = 0; i < gameWorlds.Count; i++) {
                var gameWorld = gameWorlds[i];
                try {
                    gameWorld.Clear();
                } catch (Exception e) {
                    Debug.LogError($"[{gameWorld.GetType().Name}] Clear() 异常: {e}");
                }
            }
            gameWorlds.Clear();
        }
        
        public void Update(float deltaTime) {
            for (int i = 0; i < gameWorlds.Count; i++) {
                var gameWorld = gameWorlds[i];
                if (gameWorld == null) {
                    continue;
                }
                gameWorld.Update(deltaTime);
            }
        }
    }
}
