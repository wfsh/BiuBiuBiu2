using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_AuroraDragonFire {
        public const string AbilityTypeID = AbilityConfig.AB_AuroraDragonFire;
        public const ushort ConfigId = AbilityConfig.AuroraDragonFire;
        private static Dictionary<byte, AbilityM_AuroraDragonFire> idCache = new Dictionary<byte, AbilityM_AuroraDragonFire>();
        private static Dictionary<string, AbilityM_AuroraDragonFire> signCache = new Dictionary<string, AbilityM_AuroraDragonFire>();
        private static List<AbilityM_AuroraDragonFire> loadingCallbacks = new List<AbilityM_AuroraDragonFire>();
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
        /// 格式：ID_主键值组合 = ID（如AbilityM_AuroraDragonFire.ID_1）
        /// </summary>
        public const byte ID_1_Forest_02A = 1;
        public const byte ID_2_Forest_02A = 2;
        public const byte ID_1_Forest_02B = 3;
        public const byte ID_2_Forest_02B = 4;
        public const byte ID_1_Forest_02 = 5;
        public const byte ID_2_Forest_02 = 6;
        public const byte ID_1_Remnant_01A = 7;
        public const byte ID_2_Remnant_01A = 8;
        public const byte ID_1_Remnant_01B = 9;
        public const byte ID_2_Remnant_01B = 10;
        public const byte ID_1_Remnant_01 = 11;
        public const byte ID_2_Remnant_01 = 12;

        /// <summary>
        /// ID查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_AuroraDragonFire.ID_1）后调用Select()
        /// </summary>
        [NonSerialized]
        public byte K_ID = 0;
        /// <summary>
        /// Sign 查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_AuroraDragonFire.Sign_XXX）后调用Select()
        /// </summary>
        [NonSerialized]
        public string K_RowSign = "";

        /// <summary>
        /// 普通主键字段（自动生成，K_ID=0  K_RoleSign =""时生效）
        /// 需在调用Select()前赋值
        /// </summary>
        [NonSerialized]
        public byte K_BossType;
        [NonSerialized]
        public string K_SceneType;

        /// <summary>
        /// 清除当前类的实例缓存（资源更新时调用）
        /// </summary>
        public static void ClearCache() {
            idCache.Clear();
            signCache.Clear();
            loadingCallbacks.Clear();
            IsCsvLoaded = false;
            isLoading = false;
            Debug.Log("已清除 AbilityM_AuroraDragonFire 实例缓存");
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
            AbilityM_AuroraDragonFire target = null;
            // 1. 优先ID查询（性能最优，无遍历）
            if (this.K_ID != 0) {
                idCache.TryGetValue(this.K_ID, out target);
            } else if (this.K_RowSign != "") {
                signCache.TryGetValue(this.K_RowSign, out target);
            }
            else {
                target = FindByMultiKeys(this.K_BossType, this.K_SceneType);
            }
            // 3. 拷贝数据（纯内存操作）
            if (target != null) {
                this.K_ID = target.K_ID;
                this.K_RowSign = target.K_RowSign;
                CopyInstanceData(target);
                this.IsSetCsvData = true;
            } else {
                Debug.LogError($"AbilityM_AuroraDragonFire 查询未找到：K_BossType={this.K_BossType}, K_SceneType={this.K_SceneType} 或 K_ID={this.K_ID} 或 K_RowSign={this.K_RowSign}");
            }
            callBack?.Invoke();
            callBack = null;
        }

        /// <summary>
        /// 多K字段匹配逻辑（遍历实例缓存找目标）
        /// </summary>
        private static AbilityM_AuroraDragonFire FindByMultiKeys(byte K_BossType, string K_SceneType) {
            foreach (var instance in idCache.Values) {
                bool isMatch = true;
                if (K_BossType != instance.K_BossType) {
                    isMatch = false;
                }
                if (K_SceneType != instance.K_SceneType) {
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
            AssetCsvManager.GetAbilityABAllCsvRows("AbilityM_AuroraDragonFire", allRows => {
                if (allRows == null || allRows.Count == 0) {
                    Debug.LogError($"全量加载 CSV 失败：AbilityM_AuroraDragonFire");
                    onLoadComplete?.Invoke();
                    return;
                }
                // 清空旧缓存，解析新数据
                idCache.Clear();
                signCache.Clear();
                foreach (var row in allRows) {
                    var idStr = AssetCsvManager.GetCsvRowValue(row, "ID");
                    if (!byte.TryParse(idStr, out byte rowId)) {
                        Debug.LogError($"CSV 行 ID 格式错误：AbilityM_AuroraDragonFire → {idStr}");
                        continue;
                    }
                    if (idCache.ContainsKey(rowId)) {
                        Debug.LogError($"CSV AbilityM_AuroraDragonFire 行 ID重复：{rowId}（已跳过）");
                        continue;
                    }
                    var rowSign = AssetCsvManager.GetCsvRowValue(row, "RowSign");
                    var instance = new AbilityM_AuroraDragonFire();
                    instance.K_ID = rowId; // 赋值唯一ID
                    instance.K_RowSign = rowSign; // 赋值唯一Sign
                    instance.M_ATK = AssetCsvManager.ParseValue<ushort>(AssetCsvManager.GetCsvRowValue(row, "M_ATK"));
                    instance.M_AttackMoveSpeed = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_AttackMoveSpeed"));
                    instance.M_AttackRotationSpeed = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_AttackRotationSpeed"));
                    instance.M_AttackTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_AttackTime"));
                    instance.M_BossType = AssetCsvManager.ParseValue<byte>(AssetCsvManager.GetCsvRowValue(row, "K_M_BossType"));
                    instance.M_ChargeTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_ChargeTime"));
                    instance.M_CreateRayEffectTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_CreateRayEffectTime"));
                    instance.M_DamageFixLength = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_DamageFixLength"));
                    instance.M_DamageFixPos = AssetCsvManager.ParseValue<Vector3>(AssetCsvManager.GetCsvRowValue(row, "M_DamageFixPos"));
                    instance.M_DamageHeight = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_DamageHeight"));
                    instance.M_DamageInterval = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_DamageInterval"));
                    instance.M_DamageLength = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_DamageLength"));
                    instance.M_DamageRadius = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_DamageRadius"));
                    instance.M_EffectPos = AssetCsvManager.ParseValue<Vector3>(AssetCsvManager.GetCsvRowValue(row, "M_EffectPos"));
                    instance.M_EffectRota = AssetCsvManager.ParseValue<Vector3>(AssetCsvManager.GetCsvRowValue(row, "M_EffectRota"));
                    instance.M_EffectScale = AssetCsvManager.ParseValue<Vector3>(AssetCsvManager.GetCsvRowValue(row, "M_EffectScale"));
                    instance.M_EndRayEffectTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_EndRayEffectTime"));
                    instance.M_MoveRangeDis = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_MoveRangeDis"));
                    instance.M_SceneType = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "K_M_SceneType"));
                    instance.M_WaitTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_WaitTime"));
                    instance.K_BossType = instance.M_BossType;
                    instance.K_SceneType = instance.M_SceneType;
                    instance.IsSetCsvData = true;
                    idCache[rowId] = instance;
                    signCache[rowSign] = instance;
                }
                IsCsvLoaded = true;
                #if UNITY_EDITOR
                Debug.Log($"CSV全量加载完成：AbilityM_AuroraDragonFire → 缓存 {idCache.Count}个实例  （耗时 {Time.realtimeSinceStartup - time:F2}秒）");
                #endif
                onLoadComplete?.Invoke();
            });
        }

        /// <summary>
        /// 从缓存实例拷贝数据到当前对象（纯内存操作，无文本转换）
        /// </summary>
        private void CopyInstanceData(AbilityM_AuroraDragonFire source) {
            // 拷贝主键字段（K_XXX）
            this.K_BossType = source.K_BossType;
            this.K_SceneType = source.K_SceneType;

            // 拷贝业务字段（M_XXX）
            this.M_ATK = source.M_ATK;
            this.M_AttackMoveSpeed = source.M_AttackMoveSpeed;
            this.M_AttackRotationSpeed = source.M_AttackRotationSpeed;
            this.M_AttackTime = source.M_AttackTime;
            this.M_BossType = source.M_BossType;
            this.M_ChargeTime = source.M_ChargeTime;
            this.M_CreateRayEffectTime = source.M_CreateRayEffectTime;
            this.M_DamageFixLength = source.M_DamageFixLength;
            this.M_DamageFixPos = source.M_DamageFixPos;
            this.M_DamageHeight = source.M_DamageHeight;
            this.M_DamageInterval = source.M_DamageInterval;
            this.M_DamageLength = source.M_DamageLength;
            this.M_DamageRadius = source.M_DamageRadius;
            this.M_EffectPos = source.M_EffectPos;
            this.M_EffectRota = source.M_EffectRota;
            this.M_EffectScale = source.M_EffectScale;
            this.M_EndRayEffectTime = source.M_EndRayEffectTime;
            this.M_MoveRangeDis = source.M_MoveRangeDis;
            this.M_SceneType = source.M_SceneType;
            this.M_WaitTime = source.M_WaitTime;
        }

        /// <summary>
        /// 按主键创建实例（自动赋值K_主键字段）
        /// </summary>
        public static AbilityM_AuroraDragonFire CreateForKey(byte k_BossType, string k_SceneType) {
            // 缓存已加载时直接查询并返回缓存实例
            if (IsCsvLoaded) {
                var cachedInstance = FindByMultiKeys(k_BossType, k_SceneType);
                if (cachedInstance != null) {
                    return cachedInstance;
                }
            }
            var newData = CreateSO();
            // 赋值K_主键字段
            newData.K_BossType = k_BossType;
            newData.K_SceneType = k_SceneType;
            return newData;
        }

        /// <summary>
        /// 按ID创建实例（自动赋值K_ID）
        /// </summary>
        public static AbilityM_AuroraDragonFire CreateForID(byte id) {
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
        public static AbilityM_AuroraDragonFire CreateForSign(string sign) {
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
        private static AbilityM_AuroraDragonFire CreateSO() {
#if UNITY_EDITOR
           var url = $"{AssetURL.AbilitySO}SO/AB/AbilityM_AuroraDragonFire.asset";
           var so = AssetDatabase.LoadAssetAtPath<AbilityM_AuroraDragonFire>(url);
           if (so != null && so.IsEditorUseSO) {
              so.IsSetCsvData = true;
              return so;
           }
#endif
           return new AbilityM_AuroraDragonFire();
        }
    }
}
