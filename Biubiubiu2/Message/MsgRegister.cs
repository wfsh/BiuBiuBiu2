using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public partial class MsgRegister : IDisposable {
        private List<Wrapper> removes;
        private Dictionary<int, List<Wrapper>> handlers;
        private float checkTime = 0;

        public MsgRegister() {
            isDispose = false;
            removes = new List<Wrapper>();
            handlers = new Dictionary<int, List<Wrapper>>();
            maxHandlerUseTime = 0f;
        }

        public void OnUpdate(float deltaTime) {
            if (checkTime >= 0f) {
                checkTime -= deltaTime;
            } else {
                checkTime = 1f;
                OnCheckRemoves();
            }
        }

        public void Dispose() {
            isDispose = true;
            removes.Clear();
            handlers.Clear();
            removes = null;
            handlers = null;
        }

        public void RegisterMessage<T>(Action<T> handler, int priority) where T : GamePlayEvent.IWorldEvent {
            var id = GamePlayEvent.GetWorldEventID<T>();
            RegisterDelegate(id, handler, priority);
        }

        public void UnregisterMessage<T>(Action<T> handler) where T : GamePlayEvent.IWorldEvent {
            if (handler == null) {
                return;
            }
            var id = GamePlayEvent.GetWorldEventID<T>();
            if (handlers.TryGetValue(id, out List<Wrapper> wps)) {
                int index = SearchWrapperIndex(wps, handler);
                if (index >= 0) {
                    var wp = wps[index];
                    wp.SetRemove();
                    wps[index] = wp;
                    removes.Add(wp);
                }
            }
        }

        public void DispatcherMessage<T>(T ent) where T : GamePlayEvent.IWorldEvent {
            if (handlers.TryGetValue(ent.GetID(), out List<Wrapper> wps)) {
                for (int i = 0; i < wps.Count; i++) {
                    var wp = wps[i];
                    if (wp.IsRemove()) {
                        wps.RemoveAt(i);
                        i--;
                    } else {
                        try {
                            wp.Invoke(ent);
                        } catch (Exception e) {
                            var id = ent.ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                            var target = wp.handler.Target.GetType().ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                            Debug.LogError($"MsgRegister Dispatcher {id}: {target} {e}");
                        }
                    }
                }
            }
        }

        private void RegisterDelegate(int id, Delegate handler, int priority) {
            if (handler == null) {
                return;
            }
            List<Wrapper> wps;
            if (!handlers.TryGetValue(id, out wps)) {
                wps = new List<Wrapper>();
                handlers.Add(id, wps);
            }
#if UNITY_EDITOR
            if (SearchWrapperIndex(wps, handler) != -1) {
                Debug.LogError("RegisterDelegate: 重复注册:" + id);
            }
#endif
            var body = new Wrapper(id, handler, priority);
            if (wps.Count <= 0) {
                wps.Add(body);
            } else {
                var index = wps.Count - 1;
                for (int i = index; i >= 0; i--) {
                    var saveBody = wps[i];
                    if (saveBody.IsRemove()) {
                        continue;
                    }
                    if (saveBody.Priority >= priority) {
                        wps.Insert(i+1, body);
                        return;
                    }
                }
                wps.Insert(0, body);
            }
        }

        private int SearchWrapperIndex(List<Wrapper> list, Delegate handler) {
            int index = -1;
            int length = list.Count;
            for (int i = 0; i < length; ++i) {
                var wp = list[i];
                if (wp.IsRemove()) {
                    continue;
                }
                if (wp.handler == handler) {
                    index = i;
                    break;
                }
            }
            return index;
        }
        
        private int GetRemoveHandlerIndex(List<Wrapper> list, Delegate handler) {
            int index = -1;
            int length = list.Count;
            for (int i = 0; i < length; ++i) {
                var wp = list[i];
                if (wp.IsRemove() && wp.handler == handler) {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void OnCheckRemoves() {
            if (isDispose) {
                return;
            }
            int length = removes.Count;
            if (length == 0) {
                return;
            }
            for (int i = 0; i < length; ++i) {
                var wp = removes[i];
                if (handlers.TryGetValue(wp.id, out List<Wrapper> wps)) {
                    int index = GetRemoveHandlerIndex(wps, wp.handler);
                    if (index >= 0) {
                        wps.RemoveAt(index);
                    }
                    if (wps.Count == 0) {
                        handlers.Remove(wp.id);
                    }
                }
            }
            removes.Clear();
        }

        private struct Wrapper {
            public int id;
            public int Priority;

            private bool isRemove;
            public Delegate handler;

            public Wrapper(int id, Delegate handler, int priority) {
                this.id = id;
                this.Priority = priority;
                this.isRemove = false;
                this.handler = handler;
            }
            
            public void SetRemove() {
                isRemove = true;
            }
            
            public bool IsRemove() {
                return isRemove || handler == null;
            }

            public void Invoke<T>(T data) {
                if (!isRemove) {
                    var time = Time.realtimeSinceStartup;
                    ((Action<T>)handler).Invoke(data);
                    var deltaTime = Time.realtimeSinceStartup - time;
                    if (deltaTime > 1.0f) { // 单次 1000 ms
                        var kcp = NetworkData.Config.IsKCP ? "KCP" : "WEB";
                        var methodInfo = handler.Method?.Name;
                        var targetInfo = handler.Target.GetType().ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                        var handlerSign = $"{targetInfo}.{methodInfo}";
                        PerfAnalyzerAgent.SetLog($"{kcp} MsgRegister: {handlerSign} 耗时过长 {deltaTime}");
                        maxHandlerUseTime = Mathf.Max(maxHandlerUseTime, deltaTime);
#if !BUILD_SERVER
                        Debug.LogError($"{kcp} MsgRegister: {handlerSign} 耗时过长 {deltaTime}");
#endif
                    }
                }
            }
        }
    }
}