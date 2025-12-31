using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.CoreMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public interface IGameWorld {
        void Awake();
        void Clear();
        void Update(float deltaTime);
    }

    public class ManagerBase : IGameWorld {
        public interface ISystem {
            void Init(ManagerBase manager);
            void Awake();
            bool IsClearIng();
            void ClearImmediate();
            void Update(float deltaTime);
            void Start();
        }

        private List<ISystem> systemLists = new List<ISystem>();
        private List<ISystem> invokeSystemsStart = new List<ISystem>();
        private static Dictionary<string, int> teamDictionary = new Dictionary<string, int>();
        private static int tempTeamId = 1;
        private bool isClear = false;

        public void Awake() {
            teamDictionary = new Dictionary<string, int>();
            tempTeamId = 1;
            OnAwake();
            UpdateRegister.AddInvoke(OnStart, 0.0f);
        }

        virtual protected void OnAwake() {
        }

        virtual protected void OnStart() {
        }

        public void Clear() {
            invokeSystemsStart.Clear();
            isClear = true;
            OnClear();
            ClearAllSystem();
            teamDictionary.Clear();
        }

        public void Update(float deltaTime) {
            try {
                InvokeSystemStart();
                UpdateClearSystem();
                OnUpdate();
                UpdateSystem(deltaTime);
            } catch (Exception e) {
                Debug.LogError($"[{GetType().Name}] Update() 异常: {e}");
            } 
        }

        virtual protected void OnClear() {
        }

        public T AddSystem<T>(Action<T> callBack) where T : ISystem, new() {
            ISystem system = new T();
            systemLists.Add(system);
            try {
                system.Init(this);
                callBack?.Invoke((T)system);
                system.Awake();
                invokeSystemsStart.Add(system);
            } catch (Exception e) {
                Debug.LogError($"AddSystem {system} Error:" + e);
            }
            return (T)system;
        }
        private void InvokeSystemStart() {
            if (invokeSystemsStart.Count == 0) {
                return;
            }
            for (int i = 0; i < invokeSystemsStart.Count; i++) {
                var system = invokeSystemsStart[i];
                try {
                    system.Start();
                } catch (Exception e) {
                    Debug.LogError($"StartSystem {system} Error:" + e);
                }
            }
            invokeSystemsStart.Clear();
        }

        private void ClearAllSystem() {
            var count = systemLists.Count - 1;
            for (int i = count; i >= 0; i--) {
                var system = systemLists[i];
                if (system == null) {
                    continue;
                }
                try {
                    system.ClearImmediate();
                } catch (Exception e) {
                    Debug.LogError($"[{system.GetType().Name}] Clear() 异常: {e}");
                }
            }
            systemLists.Clear();
        }

        private void UpdateClearSystem() {
            var count = systemLists.Count - 1;
            for (int i = count; i >= 0; i--) {
                var system = systemLists[i];
                if (system.IsClearIng()) {
                    try {
                        system.ClearImmediate();
                    } catch (Exception e) {
                        Debug.LogError("ClearSystem Error:" + e);
                    } finally {
                        systemLists.RemoveAt(i);
                    }
                }
            }
        }
        
        private void UpdateSystem(float deltaTime) {
            var count = systemLists.Count - 1;
            for (int i = count; i >= 0; i--) {
                var system = systemLists[i];
                if (system.IsClearIng() == false) {
                    system.Update(deltaTime);
                }
            }
        }

        virtual protected void OnUpdate() {
        }

        protected int GetTeamId(string teamSign) {
            if (teamDictionary.TryGetValue(teamSign, out int teamId) == false) {
                teamId = tempTeamId++;
                teamDictionary.Add(teamSign, teamId);
            }
            return teamId;
        }
    }
}