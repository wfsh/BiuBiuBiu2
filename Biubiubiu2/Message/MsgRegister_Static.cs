using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public partial class MsgRegister {
        private static MsgRegister msgRegister;
        private static bool isDispose = true;
        private static float maxHandlerUseTime = 0.0f;
        public static float MaxHandlerUseTime {
            get {
                return maxHandlerUseTime;
            }
        }
        
        public static MsgRegister Init() {
            msgRegister = new MsgRegister();
            return msgRegister;
        }
        
        /// <summary>
        ///  优先级 priority 越高越先执行，默认 0
        /// </summary>
        public static void Register<T>(Action<T> handler, int priority) where T : GamePlayEvent.IWorldEvent {
            if (isDispose) {
                return;
            }
            msgRegister.RegisterMessage(handler, priority);
        }

        public static void Register<T>(Action<T> handler) where T : GamePlayEvent.IWorldEvent {
            if (isDispose) {
                return;
            }
            msgRegister.RegisterMessage(handler, 0);
        }

        public static void Unregister<T>(Action<T> handler) where T : GamePlayEvent.IWorldEvent {
            if (isDispose) {
                return;
            }
            msgRegister.UnregisterMessage(handler);
        }

        public static void Dispatcher<T>(T ent) where T : GamePlayEvent.IWorldEvent {
            if (isDispose) {
                return;
            }
            msgRegister.DispatcherMessage(ent);
        }
    }
}