using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Util;
using System.Threading.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Asset {
    public class PrefabPoolManager {
        private static bool useInstantiateAsync = true;
        public static PrefabPoolManager Instance;
        private Dictionary<string, PoolData> prefabPool = new Dictionary<string, PoolData>();
        private GameObject poolRoot;
        private Transform poolRootTransform;
        private float maxInactiveTime = 60f;
        private float cleanupInterval = 1f;
        private int InitIndex = 0;

        private struct PerfabData {
            public GameObject gameObj;
            public float playTime;
        }

        private class PoolData {
            public List<PerfabData> perfabDatas;
            public int count;
            public float InactiveTime;
        }

        public static void Init() {
            if (Instance != null) {
                Dispose();
            }
            Instance = new PrefabPoolManager();
            Instance.OnInit();
        }

        public static void Update(float deltaTime) {
            if (Instance == null) {
                return;
            }
            Instance.OnUpdate(deltaTime);
        }

        public static void Dispose() {
            if (Instance != null) {
                Instance.OnDispose();
                Instance = null;
            }
        }
        
        public static void SetUseInstantiateAsync(bool use) {
            useInstantiateAsync = use;
        }

        public static void OnGetPrefab(string assetUrl, Transform parent, Action<GameObject> callBack) {
            try {
                Instance?.GetPrefab(assetUrl, parent, callBack);
            } catch (Exception e) {
                Debug.LogError("[PrefabPoolManager] Error getting prefab AssetUrl: " + assetUrl + " E:" + e);
            }
        }

        /// <summary>
        /// 设置预制存储销毁时间， -1 为不销毁
        /// </summary>
        /// <param name="assetUrl"></param>
        /// <param name="time"></param>
        public static void OnSetInactiveTime(string assetUrl, float time) {
            Instance?.SetInactiveTime(assetUrl, time);
        }

        public static void OnReturnPrefab(string assetUrl, GameObject obj) {
            try {
                Instance?.ReturnPrefab(assetUrl, obj);
            } catch (Exception e) {
                Debug.LogError("[PrefabPoolManager] Error returning prefab AssetUrl: " + assetUrl + " E:" + e);
            }
        }

        private void OnInit() {
            Debug.Log("[PrefabPoolManager] OnInit");
            InitIndex++;
            Instance = this;
            poolRoot = new GameObject("[PrefabPoolManager]");
            GameObject.DontDestroyOnLoad(poolRoot);
            poolRootTransform = poolRoot.transform;
        }

        private void OnDispose() {
            Debug.Log("[PrefabPoolManager] OnDispose");
            Instance = null;
            ClearPrefabCache();
            GameObject.Destroy(poolRoot);
            poolRootTransform = null;
            poolRoot = null;
        }

        private void OnUpdate(float delta) {
            if (cleanupInterval > 0) {
                cleanupInterval -= delta;
                return;
            }
            cleanupInterval = 2f;
            var time = Time.realtimeSinceStartup;
            foreach (var pair in prefabPool) {
                var poolData = prefabPool[pair.Key];
                if (poolData.InactiveTime > 0) {
                    for (int i = poolData.perfabDatas.Count - 1; i >= 0; i--) {
                        var perfabData = poolData.perfabDatas[i];
                        if (time - perfabData.playTime > poolData.InactiveTime) {
                            GameObject.Destroy(perfabData.gameObj);
                            poolData.perfabDatas.RemoveAt(i);
                            poolData.count--;
                        }
                    }
                }
            }
        }

        private void ClearPrefabCache() {
            foreach (var pair in prefabPool) {
                var poolData = prefabPool[pair.Key];
                if (poolData.count != poolData.perfabDatas.Count) {
                    Debug.LogWarning(
                        $"预制没有全部回收 '{pair.Key}' count {poolData.count} != poolCount {poolData.perfabDatas.Count}");
                }
                for (int i = 0; i < poolData.perfabDatas.Count; i++) {
                    GameObject.Destroy(poolData.perfabDatas[i].gameObj);
                }
                poolData.perfabDatas.Clear();
            }
            prefabPool.Clear();
        }

        // 加载特效并从池中获取
        public void GetPrefab(string assetUrl, Transform parent, Action<GameObject> callBack) {
            if (!prefabPool.TryGetValue(assetUrl, out var poolData)) {
                poolData = new PoolData {
                    perfabDatas = new List<PerfabData>(), count = 0, InactiveTime = maxInactiveTime,
                };
                prefabPool.Add(assetUrl, poolData);
            }
            if (poolData.perfabDatas.Count > 0) {
                var perfabData = poolData.perfabDatas[0];
                poolData.perfabDatas.RemoveAt(0);
                ReturnGameObj(perfabData.gameObj, parent, callBack);
            } else {
                var index = InitIndex;

                // 使用异步 lambda 封装资源加载 + 异步实例化逻辑
                AssetManager.LoadPerfabForFullUrl(assetUrl, async obj => {
                    if (index != InitIndex) return;
                    try {
                        if (obj != null) {
                            var instance = await InstantiateAsyncWithTask(obj, parent);
                            poolData.count++;
                            ReturnGameObj(instance, parent, callBack);
                        } else {
                            Debug.LogWarning($"Failed to load prefab: {assetUrl}");
                            callBack?.Invoke(null);
                        }
                    } catch (Exception e) {
                        Debug.LogError(
                            $"[PrefabPoolManager] Error during async instantiation: {assetUrl}, Exception: {e}");
                        callBack?.Invoke(null);
                    }
                });
            }
        }

        public async Task<GameObject> InstantiateAsyncWithTask(GameObject prefab, Transform parent) {
            if (useInstantiateAsync == false) {
                var go = GameObject.Instantiate(prefab);
                if (go != null && parent != null) {
                    go.transform.SetParent(parent, false);
                }
                return go;
            } else {
                var request = GameObject.InstantiateAsync(prefab); 
                while (!request.isDone) {
                    await Task.Yield();
                }
                var go = request.Result[0];
                if (go != null && parent != null) {
                    go.transform.SetParent(parent, false);
                }
                return go;
            }
        }

        public void SetInactiveTime(string assetUrl, float time) {
            PoolData poolData;
            if (prefabPool.TryGetValue(assetUrl, out poolData)) {
                poolData.InactiveTime = time;
            }
        }

        private void ReturnGameObj(GameObject gameObj, Transform parent, Action<GameObject> callBack) {
            var obj = gameObj;
            obj.SetActive(true);
            if (parent != null) {
                obj.transform.SetParent(parent);
            } else {
                obj.transform.SetParent(poolRootTransform);
            }
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            callBack?.Invoke(obj);
        }

        // 回收特效到池
        public void ReturnPrefab(string assetUrl, GameObject obj) {
            if (string.IsNullOrEmpty(assetUrl)) {
                Debug.LogError("ReturnPrefab assetUrl is null");
                return;
            }
            if (obj == null) {
                return;
            }
            PoolData poolData;
            if (prefabPool.TryGetValue(assetUrl, out poolData) == false || poolRootTransform == null) {
                Debug.LogWarning($"Prefab '{assetUrl}' 没有在内存池登记.");
                GameObject.Destroy(obj);
                return;
            }
            obj.SetActive(false);
            obj.transform.SetParent(poolRootTransform);
            poolData.perfabDatas.Add(new PerfabData {
                gameObj = obj, playTime = Time.realtimeSinceStartup
            });
        }
    }
}