using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class AbsGameWorld {
        private List<IGameWorld> managers = new List<IGameWorld>();
        public void Init() {
            OnInit();
        }

        virtual protected void OnInit() {
        }

        public void Clear() {
            OnClear();
            RemoveManager();
        }
        
        public void Update(float deltaTime) {
            for (int i = 0; i < managers.Count; i++) {
                var manager = managers[i];
                if (manager == null) {
                    continue;
                }
                manager.Update(deltaTime);
            }
        }

        virtual protected void OnClear() {
        }

        public void AddManager<T>() where T : IGameWorld, new() {
            IGameWorld manager = new T();
            managers.Add(manager);
            manager.Awake();
        }

        public void RemoveManager() {
            foreach (var manager in managers) {
                try {
                    manager.Clear();
                } catch (Exception e) {
                    Debug.LogError($"[{manager.GetType().Name}] Clear() 异常: {e}");
                }
            }
            managers.Clear();
        }
    }

    public class CoreGameWorld : AbsGameWorld {
        protected override void OnInit() {
            base.OnInit();
            AddManager<GameWorldLayerManager>();
            AddManager<NetworkConnectManager>();
        }
    }
}