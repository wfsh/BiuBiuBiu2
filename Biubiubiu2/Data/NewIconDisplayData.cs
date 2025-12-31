using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class NewIconDisplayData {
        [Serializable]
        private class MapIdListWrapper {
            public List<int> ids = new List<int>();
        }

        public static string NEW_MAP_ID_LIST_KEY = "new_map_id_list";

        public static void SetKey(string pid) {
            NEW_MAP_ID_LIST_KEY = $"new_map_id_list_{pid}";
        }

        public static void AddNewMapIds(List<int> mapIds) {
            if (mapIds == null || mapIds.Count == 0) {
                return;
            }

            var wrapper = new MapIdListWrapper { ids = mapIds };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(NEW_MAP_ID_LIST_KEY, json);
        }

        public static void AddSingleMapId(int mapId) {
            var mapIds = GetNewMapIds();
            if (!mapIds.Contains(mapId)) {
                mapIds.Add(mapId);
                AddNewMapIds(mapIds);
            }
        }

        public static bool HasNewMapIds() {
            if (!PlayerPrefs.HasKey(NEW_MAP_ID_LIST_KEY)) {
                return false;
            }

            return true;
        }

        public static List<int> GetNewMapIds() {
            var result = new List<int>();
            if (!PlayerPrefs.HasKey(NEW_MAP_ID_LIST_KEY))
                return result;

            string json = PlayerPrefs.GetString(NEW_MAP_ID_LIST_KEY);
            try {
                var wrapper = JsonUtility.FromJson<MapIdListWrapper>(json);
                if (wrapper != null && wrapper.ids != null)
                    result = wrapper.ids;
            } catch (Exception e) {
                Debug.LogWarning("Failed to parse new map ID list: " + e.Message);
            }

            return result;
        }

        public static void RemoveNewMapIds() {
            PlayerPrefs.DeleteKey(NEW_MAP_ID_LIST_KEY);
        }

        public static void RemoveSingleMapId(int mapId) {
            var mapIds = GetNewMapIds();
            if (mapIds.Contains(mapId)) {
                mapIds.Remove(mapId);
                if (mapIds.Count > 0) {
                    AddNewMapIds(mapIds);
                } else {
                    RemoveNewMapIds();
                }
            }
        }
    }
}
