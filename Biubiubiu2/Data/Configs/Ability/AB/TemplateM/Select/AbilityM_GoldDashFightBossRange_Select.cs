using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_GoldDashFightBossRange {
        public const string AbilityTypeID = AbilityConfig.AB_GoldDashFightBossRange;
        public const ushort ConfigId = AbilityConfig.GoldDashFightBossRange;
        private static Dictionary<byte, AbilityM_GoldDashFightBossRange> idCache = new Dictionary<byte, AbilityM_GoldDashFightBossRange>();
        private static Dictionary<string, AbilityM_GoldDashFightBossRange> signCache = new Dictionary<string, AbilityM_GoldDashFightBossRange>();
        private static List<AbilityM_GoldDashFightBossRange> loadingCallbacks = new List<AbilityM_GoldDashFightBossRange>();
        private static bool IsCsvLoaded = false;
        private static bool isLoading = false;
        private Action callBack;
        private bool IsSetCsvData = false;
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
        /// CSV ID常量映射（自动生成，对应CSV的ID列）
        /// 格式：ID_主键值组合 = ID（如AbilityM_GoldDashFightBossRange.ID_1）
        /// </summary>
        public const byte ID_FightRange = 1;
        public const byte ID_AuroraDragonFightRange = 2;
        public const byte ID_AncientDragonFightRange = 3;
        public const byte ID_AceJokerFightRange = 4;
        public const byte ID_GoldJokerFightRange = 5;

        /// <summary>
        /// ID查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_GoldDashFightBossRange.ID_1）后调用Select()
        /// </summary>
        [NonSerialized]
        public byte K_ID = 0;
        /// <summary>
        /// Sign 查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_GoldDashFightBossRange.Sign_XXX）后调用Select()
        /// </summary>
        [NonSerialized]
        public string K_RowSign = "";

        /// <summary>
        /// 普通主键字段（自动生成，K_ID=0  K_RoleSign =""时生效）
        /// 需在调用Select()前赋值
        /// </summary>
        [NonSerialized]
        public string K_Sign;

        /// <summary>
        /// 清除当前类的实例缓存（资源更新时调用）
        /// </summary>
        public static void ClearCache() {
            idCache.Clear();
            signCache.Clear();
            loadingCallbacks.Clear();
            IsCsvLoaded = false;
            isLoading = false;
            Debug.Log("已清除 AbilityM_GoldDashFightBossRange 实例缓存");
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
            AbilityM_GoldDashFightBossRange target = null;
            // 1. 优先ID查询（性能最优，无遍历）
            if (this.K_ID != 0) {
                idCache.TryGetValue(this.K_ID, out target);
            } else if (this.K_RowSign != "") {
                signCache.TryGetValue(this.K_RowSign, out target);
            }
            else {
                target = FindByMultiKeys(this.K_Sign);
            }
            // 3. 拷贝数据（纯内存操作）
            if (target != null) {
                this.K_ID = target.K_ID;
                this.K_RowSign = target.K_RowSign;
                CopyInstanceData(target);
                this.IsSetCsvData = true;
            } else {
                Debug.LogError($"AbilityM_GoldDashFightBossRange 查询未找到：K_Sign={this.K_Sign} 或 K_ID={this.K_ID} 或 K_RowSign={this.K_RowSign}");
            }
            callBack?.Invoke();
            callBack = null;
        }

        /// <summary>
        /// 多K字段匹配逻辑（遍历实例缓存找目标）
        /// </summary>
        private static AbilityM_GoldDashFightBossRange FindByMultiKeys(string K_Sign) {
            foreach (var instance in idCache.Values) {
                bool isMatch = true;
                if (K_Sign != instance.K_Sign) {
                    isMatch = false;
                }
                if (isMatch) {
                    return instance;
                }
            }
            return null;
        }

        /// <summary>
        /// 全量加载CSV并解析所有实例到缓存（仅执行一次）
        /// </summary>
        private static void LoadCsvAndCacheAll(Action onLoadComplete) {
            isLoading = true;
            var time = Time.realtimeSinceStartup;
            AssetCsvManager.GetAbilityABAllCsvRows("AbilityM_GoldDashFightBossRange", allRows => {
                if (allRows == null || allRows.Count == 0) {
                    Debug.LogError($"全量加载 CSV 失败：AbilityM_GoldDashFightBossRange");
                    onLoadComplete?.Invoke();
                    return;
                }
                // 清空旧缓存，解析新数据
                idCache.Clear();
                signCache.Clear();
                foreach (var row in allRows) {
                    var idStr = AssetCsvManager.GetCsvRowValue(row, "ID");
                    if (!byte.TryParse(idStr, out byte rowId)) {
                        Debug.LogError($"CSV 行 ID 格式错误：AbilityM_GoldDashFightBossRange → {idStr}");
                        continue;
                    }
                    if (idCache.ContainsKey(rowId)) {
                        Debug.LogError($"CSV AbilityM_GoldDashFightBossRange 行 ID重复：{rowId}（已跳过）");
                        continue;
                    }
                    var rowSign = AssetCsvManager.GetCsvRowValue(row, "RowSign");
                    var instance = new AbilityM_GoldDashFightBossRange();
                    instance.K_ID = rowId; // 赋值唯一ID
                    instance.K_RowSign = rowSign; // 赋值唯一Sign
                    instance.M_EffectSign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "M_EffectSign"));
                    instance.M_FactorValue = AssetCsvManager.ParseValue<Single[]>(AssetCsvManager.GetCsvRowValue(row, "M_FactorValue"));
                    instance.M_FightBGMSign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "M_FightBGMSign"));
                    instance.M_FightRangeCenterForwardOffset = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_FightRangeCenterForwardOffset"));
                    instance.M_FightRangeHeight = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_FightRangeHeight"));
                    instance.M_FightRangeRadius = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_FightRangeRadius"));
                    instance.M_IsCheckBossFailForGpoEmpty = AssetCsvManager.ParseValue<bool>(AssetCsvManager.GetCsvRowValue(row, "M_IsCheckBossFailForGpoEmpty"));
                    instance.M_RemoveTimeAfterDead = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_RemoveTimeAfterDead"));
                    instance.M_RemoveTimeNoTarget = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_RemoveTimeNoTarget"));
                    instance.M_RoleAddHpFactorValue = AssetCsvManager.ParseValue<Single[]>(AssetCsvManager.GetCsvRowValue(row, "M_RoleAddHpFactorValue"));
                    instance.M_SceneSign = AssetCsvManager.ParseValue<String[]>(AssetCsvManager.GetCsvRowValue(row, "M_SceneSign"));
                    instance.M_ServerEffectSign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "M_ServerEffectSign"));
                    instance.M_Sign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "K_M_Sign"));
                    instance.M_TriggerInfinitePackBullet = AssetCsvManager.ParseValue<bool>(AssetCsvManager.GetCsvRowValue(row, "M_TriggerInfinitePackBullet"));
                    instance.K_Sign = instance.M_Sign;
                    instance.IsSetCsvData = true;
                    idCache[rowId] = instance;
                    signCache[rowSign] = instance;
                }
                IsCsvLoaded = true;
                #if UNITY_EDITOR
                Debug.Log($"CSV全量加载完成：AbilityM_GoldDashFightBossRange → 缓存 {idCache.Count}个实例  （耗时 {Time.realtimeSinceStartup - time:F2}秒）");
                #endif
                onLoadComplete?.Invoke();
            });
        }

        /// <summary>
        /// 从缓存实例拷贝数据到当前对象（纯内存操作，无文本转换）
        /// </summary>
        private void CopyInstanceData(AbilityM_GoldDashFightBossRange source) {
            // 拷贝主键字段（K_XXX）
            this.K_Sign = source.K_Sign;

            // 拷贝业务字段（M_XXX）
            this.M_EffectSign = source.M_EffectSign;
            this.M_FactorValue = source.M_FactorValue;
            this.M_FightBGMSign = source.M_FightBGMSign;
            this.M_FightRangeCenterForwardOffset = source.M_FightRangeCenterForwardOffset;
            this.M_FightRangeHeight = source.M_FightRangeHeight;
            this.M_FightRangeRadius = source.M_FightRangeRadius;
            this.M_IsCheckBossFailForGpoEmpty = source.M_IsCheckBossFailForGpoEmpty;
            this.M_RemoveTimeAfterDead = source.M_RemoveTimeAfterDead;
            this.M_RemoveTimeNoTarget = source.M_RemoveTimeNoTarget;
            this.M_RoleAddHpFactorValue = source.M_RoleAddHpFactorValue;
            this.M_SceneSign = source.M_SceneSign;
            this.M_ServerEffectSign = source.M_ServerEffectSign;
            this.M_Sign = source.M_Sign;
            this.M_TriggerInfinitePackBullet = source.M_TriggerInfinitePackBullet;
        }

        /// <summary>
        /// 按主键创建实例（自动赋值K_主键字段）
        /// </summary>
        public static AbilityM_GoldDashFightBossRange CreateForKey(string k_Sign) {
            // 缓存已加载时直接查询并返回缓存实例
            if (IsCsvLoaded) {
                var cachedInstance = FindByMultiKeys(k_Sign);
                if (cachedInstance != null) {
                    return cachedInstance;
                }
            }
            var newData = CreateSO();
            // 赋值K_主键字段
            newData.K_Sign = k_Sign;
            return newData;
        }

        /// <summary>
        /// 按ID创建实例（自动赋值K_ID）
        /// </summary>
        public static AbilityM_GoldDashFightBossRange CreateForID(byte id) {
            // 缓存已加载时直接返回缓存实例
            if (IsCsvLoaded && idCache.TryGetValue(id, out var cachedInstance)) {
                return cachedInstance;
            }
            var newData = CreateSO();
            newData.K_ID = id;
            return newData;
        }

        /// <summary>
        /// 按Sign创建实例（自动赋值K_RowSign）
        /// </summary>
        public static AbilityM_GoldDashFightBossRange CreateForSign(string sign) {
            // 缓存已加载时直接返回缓存实例
            if (IsCsvLoaded && signCache.TryGetValue(sign, out var cachedInstance)) {
                return cachedInstance;
            }
            var newData = CreateSO();
            newData.K_RowSign = sign;
            return newData;
        }

        /// <summary>
        /// 编辑器环境勾选 IsEditorUseSO 时可以使用 SO 进行测试
        /// </summary>
        private static AbilityM_GoldDashFightBossRange CreateSO() {
#if UNITY_EDITOR
           var url = $"{AssetURL.AbilitySO}SO/AB/AbilityM_GoldDashFightBossRange.asset";
           var so = AssetDatabase.LoadAssetAtPath<AbilityM_GoldDashFightBossRange>(url);
           if (so != null && so.IsEditorUseSO) {
              so.IsSetCsvData = true;
              return so;
           }
#endif
           return new AbilityM_GoldDashFightBossRange();
        }
    }
}
