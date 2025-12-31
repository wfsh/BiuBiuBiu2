using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayWWiseAudio {
        public const string AbilityTypeID = AbilityConfig.AB_PlayWWiseAudio;
        public const ushort ConfigId = AbilityConfig.PlayWWiseAudio;
        private static Dictionary<byte, AbilityM_PlayWWiseAudio> idCache = new Dictionary<byte, AbilityM_PlayWWiseAudio>();
        private static Dictionary<string, AbilityM_PlayWWiseAudio> signCache = new Dictionary<string, AbilityM_PlayWWiseAudio>();
        private static List<AbilityM_PlayWWiseAudio> loadingCallbacks = new List<AbilityM_PlayWWiseAudio>();
        private static bool IsCsvLoaded = false;
        private static bool isLoading = false;
        private Action callBack;
        private bool IsSetCsvData = true;
        public override string GetTypeID() {
            return AbilityTypeID;
        }
        public override ushort GetConfigId() {
            return ConfigId;
        }
        public override byte GetRowID() {
            return K_ID;
        }
        public override string GetRowSign() {
            return K_RowSign;
        }

        /// <summary>
        /// ID查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_PlayWWiseAudio.ID_1）后调用Select()
        /// </summary>
        [NonSerialized]
        public byte K_ID = 0;
        /// <summary>
        /// Sign 查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_PlayWWiseAudio.Sign_XXX）后调用Select()
        /// </summary>
        [NonSerialized]
        public string K_RowSign = "";

        /// <summary>
        /// 清除当前类的实例缓存（资源更新时调用）
        /// </summary>
        public static void ClearCache() {
            idCache.Clear();
            signCache.Clear();
            loadingCallbacks.Clear();
            IsCsvLoaded = false;
            isLoading = false;
            Debug.Log("已清除 AbilityM_PlayWWiseAudio 实例缓存");
        }

        /// <summary>
        /// 无参查询CSV配置（异步）
        /// 优先级：K_ID > 普通主键（K_ID=0时用普通主键）
        /// 本代码为自动生成，请勿手动修改
        /// </summary>
        override public void Select(Action callBack) {
            // 1. 使用缓存的 CSV 不需要在赋值
            if (IsSetCsvData) {
                callBack.Invoke();
                return;
            }
            this.callBack = callBack;
            // 2. 缓存已加载：直接从缓存查询
            if (IsCsvLoaded) {
                DoSelectFromCache();
                return;
            }
            // 3. 加入等待队列，避免重复加载
            loadingCallbacks.Add(this);
            if (isLoading) {
                return;
            }
            // 4. 触发全量加载，加载完成后处理队列
            LoadCsvAndCacheAll(OnLoadComplete);
        }

        /// <summary>
        /// 加载完成统一回调（触发所有等待的查询回调）
        /// </summary>
        private static void OnLoadComplete() {
            foreach (var abilityM in loadingCallbacks) {
                abilityM.DoSelectFromCache();
            }
            loadingCallbacks.Clear();
            isLoading = false;
        }

        /// <summary>
        /// 核心：从实例缓存中查询（无任何文本转换）
        /// </summary>
        private void DoSelectFromCache() {
            AbilityM_PlayWWiseAudio target = null;
            // 1. 优先ID查询（性能最优，无遍历）
            if (this.K_ID != 0) {
                idCache.TryGetValue(this.K_ID, out target);
            } else if (this.K_RowSign != "") {
                signCache.TryGetValue(this.K_RowSign, out target);
            }
            // 2. 无普通主键，K_ID=0→查询失败
            else {
                Debug.LogError($"[AbilityM_PlayWWiseAudio] 无普通主键，必须赋值K_ID后调用Select()");
            }
            // 3. 拷贝数据（纯内存操作）
            if (target != null) {
                this.K_ID = target.K_ID;
                this.K_RowSign = target.K_RowSign;
                CopyInstanceData(target);
                this.IsSetCsvData = true;
            } else {
                Debug.LogError($"AbilityM_PlayWWiseAudio 查询未找到：K_ID={this.K_ID}");
            }
            callBack?.Invoke();
            callBack = null;
        }

        /// <summary>
        /// 全量加载CSV并解析所有实例到缓存（仅执行一次）
        /// </summary>
        private static void LoadCsvAndCacheAll(Action onLoadComplete) {
            isLoading = true;
            var time = Time.realtimeSinceStartup;
            AssetCsvManager.GetAbilityABAllCsvRows("AbilityM_PlayWWiseAudio", allRows => {
                if (allRows == null || allRows.Count == 0) {
                    Debug.LogError($"全量加载 CSV 失败：AbilityM_PlayWWiseAudio");
                    onLoadComplete?.Invoke();
                    return;
                }
                // 清空旧缓存，解析新数据
                idCache.Clear();
                signCache.Clear();
                foreach (var row in allRows) {
                    var idStr = AssetCsvManager.GetCsvRowValue(row, "ID");
                    if (!byte.TryParse(idStr, out byte rowId)) {
                        Debug.LogError($"CSV 行 ID 格式错误：AbilityM_PlayWWiseAudio → {idStr}");
                        continue;
                    }
                    if (idCache.ContainsKey(rowId)) {
                        Debug.LogError($"CSV AbilityM_PlayWWiseAudio 行 ID重复：{rowId}（已跳过）");
                        continue;
                    }
                    var rowSign = AssetCsvManager.GetCsvRowValue(row, "RowSign");
                    var instance = new AbilityM_PlayWWiseAudio();
                    instance.K_ID = rowId; // 赋值唯一ID
                    instance.K_RowSign = rowSign; // 赋值唯一Sign
                    instance.IsSetCsvData = true;
                    idCache[rowId] = instance;
                    signCache[rowSign] = instance;
                }
                IsCsvLoaded = true;
                #if UNITY_EDITOR
                Debug.Log($"CSV全量加载完成：AbilityM_PlayWWiseAudio → 缓存 {idCache.Count}个实例  （耗时 {Time.realtimeSinceStartup - time:F2}秒）");
                #endif
                onLoadComplete?.Invoke();
            });
        }

        /// <summary>
        /// 从缓存实例拷贝数据到当前对象（纯内存操作，无文本转换）
        /// </summary>
        private void CopyInstanceData(AbilityM_PlayWWiseAudio source) {
            // 拷贝业务字段（M_XXX）
        }

        /// <summary>
        /// 按ID创建实例（自动赋值K_ID）
        /// </summary>
        public static AbilityM_PlayWWiseAudio CreateForID(byte id) {
            return Create();
        }

        /// <summary>
        /// 按Sign创建实例（自动赋值K_Sign）
        /// </summary>
        public static AbilityM_PlayWWiseAudio CreateForSign(string sign) {
            return Create();
        }

        /// <summary>
        ///  CSV 表没有数据，直接使用
        /// </summary>
        public static AbilityM_PlayWWiseAudio Create() {
            // 缓存已加载时直接返回缓存实例
            if (IsCsvLoaded && idCache.TryGetValue(0, out var cachedInstance)) {
                return cachedInstance;
            }
           var newData = new AbilityM_PlayWWiseAudio();
            newData.K_ID = 0;
            idCache.Add(0, newData);
            IsCsvLoaded = true;
            return newData;
        }
        /// <summary>
        /// 编辑器环境勾选 IsEditorUseSO 时可以使用 SO 进行测试
        /// </summary>
        private static AbilityM_PlayWWiseAudio CreateSO() {
#if UNITY_EDITOR
           var url = $"{AssetURL.AbilitySO}SO/AB/AbilityM_PlayWWiseAudio.asset";
           var so = AssetDatabase.LoadAssetAtPath<AbilityM_PlayWWiseAudio>(url);
           if (so != null && so.IsEditorUseSO) {
              so.IsSetCsvData = true;
              return so;
           }
#endif
           return new AbilityM_PlayWWiseAudio();
        }
    }
}
