using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public class GamePlayEvent {
        private static Dictionary<System.Type, int> EventData = new Dictionary<System.Type, int>();
        public static int Index = 0;

        public interface ISystemEvent {
            int GetID();
        }

        public interface IWorldEvent {
            int GetID();
        }

        public static int ReadonlySystemEventID<T>() where T : ISystemEvent {
            return GetSystemEventID<T>();
        }

        public static int GetSystemEventID<T>() where T : ISystemEvent {
            var type = typeof(T);
            int id = 0;
            if (EventData.TryGetValue(type, out id) == false) {
                id = ++Index;
                EventData.Add(type, id);
            }
            return id;
        }

        public static int ReadonlyWorldEventID<T>() where T : IWorldEvent {
            return GetWorldEventID<T>();
        }

        public static int GetWorldEventID<T>() where T : IWorldEvent {
            var type = typeof(T);
            int id = 0;
            if (EventData.TryGetValue(type, out id) == false) {
                id = ++Index;
                EventData.Add(type, id);
            }
            return id;
        }

        public static int GetNetworkEventID<T>() where T : IProto_Doc {
            var type = typeof(T);
            int id = 0;
            if (EventData.TryGetValue(type, out id) == false) {
                id = ++Index;
                EventData.Add(type, id);
            }
            return id;
        }
    }
}