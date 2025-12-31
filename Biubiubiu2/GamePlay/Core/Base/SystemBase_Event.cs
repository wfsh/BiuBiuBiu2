using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.Pool;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public partial class SystemBase {
        public struct Body {
            public int Priority;
            public Delegate handler;
            public bool isRemove;
        }
        private Dictionary<int, List<Body>> handlers = DictionaryPool<int, List<Body>>.Get();

        /// <summary>
        ///  优先级 priority 越高越先执行，默认 0
        /// </summary>
        public void Register<T>(Action<ISystemMsg, T> handler, int priority) where T : GamePlayEvent.ISystemEvent {
            var id = GamePlayEvent.GetSystemEventID<T>();
            RegisterDelegate(id, handler, priority);
        }

        public void Register<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent {
            var id = GamePlayEvent.GetSystemEventID<T>();
            RegisterDelegate(id, handler, 0);
        }

        public void Unregister<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent {
            var id = GamePlayEvent.GetSystemEventID<T>();
            UnRegisterDelegate(id, handler);
        }

        public void Dispatcher<T>(T ent) where T : GamePlayEvent.ISystemEvent {
            if (handlers.TryGetValue(ent.GetID(), out List<Body> lists)) {
                for (int i = 0; i < lists.Count; i++) {
                    var body = lists[i];
                    if (body.isRemove) {
                        lists.RemoveAt(i);
                        i--;
                    } else {
                        var time = Time.realtimeSinceStartup;
                        try {
                            ((Action<ISystemMsg, T>)body.handler).Invoke(this, ent);
                        } catch (Exception e) {
                            var methodInfo = body.handler.Method?.Name;
                            var targetInfo = body.handler.Target.GetType().ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                            var handlerSign = $"{targetInfo}.{methodInfo}";
                            Debug.LogError($"SystemBase Dispatcher {handlerSign} E: {e}");
                        } finally {
                            var deltaTime = Time.realtimeSinceStartup - time;
                            if (deltaTime > 1.0f) { // 单次 1000 ms
                                var methodInfo = body.handler.Method?.Name;
                                var targetInfo = body.handler.Target.GetType().ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                                var handlerSign = $"{targetInfo}.{methodInfo}";
                                var kcp = NetworkData.Config.IsKCP ? "KCP" : "WEB";
                                PerfAnalyzerAgent.SetLog($"{kcp} SystemBase: {handlerSign} 耗时过长 {deltaTime}");
                                MsgRegister.Dispatcher(new M_Game.HandlerErrorUseTime {
                                    UseTime = deltaTime,
                                });
#if !BUILD_SERVER
                                Debug.LogError($"{kcp} SystemBase: {handlerSign} 耗时过长 {deltaTime}");
#endif
                            }
                        }
                    }
                }
            }
        }

        public void Dispatcher<T>(T ent, float deltaTime) where T : GamePlayEvent.ISystemEvent {
           UpdateRegister.AddInvoke(delegate {
               Dispatcher(ent);
           }, deltaTime);
        }

        private void RegisterDelegate(int id, Delegate handler, int priority) {
            if (handler == null) {
                Debug.LogError($"RegisterDelegate handler is null {id}");
                return;
            }
            List<Body> actionList;
            if (handlers.TryGetValue(id, out actionList) == false) {
                actionList = ListPool<Body>.Get();
                handlers.Add(id, actionList);
            }
            var body = new Body();
            body.handler = handler;
            body.Priority = priority;
            if (actionList.Count <= 0) {
                actionList.Add(body);
            } else {
                var index = actionList.Count - 1;
                for (int i = index; i >= 0; i--) {
                    var saveBody = actionList[i];
                    if (saveBody.isRemove) {
                        continue;
                    }
                    if (saveBody.Priority >= priority) {
                        actionList.Insert(i+1, body);
                        return;
                    }
                }
                actionList.Insert(0, body);
            }
        }

        public void UnRegisterDelegate(int id, Delegate handler) {
            List<Body> actionList;
            if (handlers.TryGetValue(id, out actionList)) {
                for (int i = 0; i < actionList.Count; i++) {
                    var body = actionList[i];
                    if (body.isRemove == false && handler == body.handler) {
                        body.isRemove = true;
                        actionList[i] = body;
                        break;
                    }
                }
            }
        }

        private void ClearHandles() {
            foreach (var kv in handlers) {
                ListPool<Body>.Release(kv.Value);
            }
            DictionaryPool<int, List<Body>>.Release(handlers);
        }
    }
}