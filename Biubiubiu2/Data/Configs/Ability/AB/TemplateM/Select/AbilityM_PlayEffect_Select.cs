using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_PlayEffect {
        public const string AbilityTypeID = AbilityConfig.AB_PlayEffect;
        public const ushort ConfigId = AbilityConfig.PlayEffect;
        private static Dictionary<byte, AbilityM_PlayEffect> idCache = new Dictionary<byte, AbilityM_PlayEffect>();
        private static Dictionary<string, AbilityM_PlayEffect> signCache = new Dictionary<string, AbilityM_PlayEffect>();
        private static List<AbilityM_PlayEffect> loadingCallbacks = new List<AbilityM_PlayEffect>();
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
        /// 格式：ID_主键值组合 = ID（如AbilityM_PlayEffect.ID_1）
        /// </summary>
        public const byte ID_Bomb = 1;
        public const byte ID_Particlecannon = 2;
        public const byte ID_MissileExplosive = 3;
        public const byte ID_AttackHit = 4;
        public const byte ID_PlungerEndAttack = 5;
        public const byte ID_Dead = 6;
        public const byte ID_RexKingBellowAttack = 7;
        public const byte ID_SwordTigerBellowAttack = 8;
        public const byte ID_UAVFire = 9;
        public const byte ID_RexKingFire = 10;
        public const byte ID_GrenadeGunExplosive = 11;
        public const byte ID_AncientDragonDelayBlastFireBall = 12;
        public const byte ID_AirBillowHitGround = 13;
        public const byte ID_SurgeSpawn = 15;
        public const byte ID_PlayDragonAOEEffect01 = 16;
        public const byte ID_PlayDragonAOEEffect02 = 17;
        public const byte ID_AuroraDragonFireBallStart = 18;
        public const byte ID_AuroraDragonDelayBlastFireBall = 19;
        public const byte ID_AuroraDragonDelayBlastExplosion = 20;
        public const byte ID_AncientDragonDelayBlastExplosion = 21;
        public const byte ID_AuroraDragonAppear = 22;
        public const byte ID_AncientDragonAppear = 23;
        public const byte ID_AuroraDragonFireBallBoom = 24;
        public const byte ID_GoldJokerRocketBombStartEffect = 26;
        public const byte ID_GoldJokerRocketBombEffect = 28;
        public const byte ID_GoldJokerDollBombDownStartEffect = 29;
        public const byte ID_GoldJokerDollBombDownEffect = 30;
        public const byte ID_JokerUAVSpadesFire = 32;
        public const byte ID_JokerUAVHeartsStart = 33;
        public const byte ID_AceJokerLeaveEffect = 34;
        public const byte ID_GoldJokerLeaveEffect = 35;
        public const byte ID_AceJokerFloatingGunStart = 36;
        public const byte ID_GoldJokerFloatingGunStart = 37;
        public const byte ID_AceJokerFloatingGunFire = 38;
        public const byte ID_AceJokerCardTrickStart = 39;
        public const byte ID_GoldJokerCardTrickStart = 40;
        public const byte ID_AceJokerSurpriseBombStart = 42;
        public const byte ID_GoldJokerSurpriseBomb = 43;
        public const byte ID_AceJokerDollBombDownStartEffect = 44;
        public const byte ID_AceJokerDollBombDownEffect = 45;
        public const byte ID_AceJokerRocketBombStartEffect = 46;
        public const byte ID_AceJokerRocketBombEffect = 48;
        public const byte ID_GoldJokerFloatingGunFire = 49;
        public const byte ID_JokerDroneDiamondsFireStart = 14;
        public const byte ID_JokerDroneClubsFireStart = 25;
        public const byte ID_JokerDroneHeartsFireStart = 27;
        public const byte ID_JokerDroneSpadesFireStart = 31;
        public const byte ID_BossAceJokerAddHpBuff = 41;
        public const byte ID_Char_XCC_Fuhuo = 47;

        /// <summary>
        /// ID查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_PlayEffect.ID_1）后调用Select()
        /// </summary>
        [NonSerialized]
        public byte K_ID = 0;
        /// <summary>
        /// Sign 查询主键（自动生成，优先于其他主键）
        /// 用法：赋值ID常量（如AbilityM_PlayEffect.Sign_XXX）后调用Select()
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
            Debug.Log("已清除 AbilityM_PlayEffect 实例缓存");
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
            AbilityM_PlayEffect target = null;
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
                Debug.LogError($"AbilityM_PlayEffect 查询未找到：K_Sign={this.K_Sign} 或 K_ID={this.K_ID} 或 K_RowSign={this.K_RowSign}");
            }
            callBack?.Invoke();
            callBack = null;
        }

        /// <summary>
        /// 多K字段匹配逻辑（遍历实例缓存找目标）
        /// </summary>
        private static AbilityM_PlayEffect FindByMultiKeys(string K_Sign) {
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
            AssetCsvManager.GetAbilityABAllCsvRows("AbilityM_PlayEffect", allRows => {
                if (allRows == null || allRows.Count == 0) {
                    Debug.LogError($"全量加载 CSV 失败：AbilityM_PlayEffect");
                    onLoadComplete?.Invoke();
                    return;
                }
                // 清空旧缓存，解析新数据
                idCache.Clear();
                signCache.Clear();
                foreach (var row in allRows) {
                    var idStr = AssetCsvManager.GetCsvRowValue(row, "ID");
                    if (!byte.TryParse(idStr, out byte rowId)) {
                        Debug.LogError($"CSV 行 ID 格式错误：AbilityM_PlayEffect → {idStr}");
                        continue;
                    }
                    if (idCache.ContainsKey(rowId)) {
                        Debug.LogError($"CSV AbilityM_PlayEffect 行 ID重复：{rowId}（已跳过）");
                        continue;
                    }
                    var rowSign = AssetCsvManager.GetCsvRowValue(row, "RowSign");
                    var instance = new AbilityM_PlayEffect();
                    instance.K_ID = rowId; // 赋值唯一ID
                    instance.K_RowSign = rowSign; // 赋值唯一Sign
                    instance.M_AudioSign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "M_AudioSign"));
                    instance.M_EffectSign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "M_EffectSign"));
                    instance.M_LifeTime = AssetCsvManager.ParseValue<float>(AssetCsvManager.GetCsvRowValue(row, "M_LifeTime"));
                    instance.M_Sign = AssetCsvManager.ParseValue<string>(AssetCsvManager.GetCsvRowValue(row, "K_M_Sign"));
                    instance.K_Sign = instance.M_Sign;
                    instance.IsSetCsvData = true;
                    idCache[rowId] = instance;
                    signCache[rowSign] = instance;
                }
                IsCsvLoaded = true;
                #if UNITY_EDITOR
                Debug.Log($"CSV全量加载完成：AbilityM_PlayEffect → 缓存 {idCache.Count}个实例  （耗时 {Time.realtimeSinceStartup - time:F2}秒）");
                #endif
                onLoadComplete?.Invoke();
            });
        }

        /// <summary>
        /// 从缓存实例拷贝数据到当前对象（纯内存操作，无文本转换）
        /// </summary>
        private void CopyInstanceData(AbilityM_PlayEffect source) {
            // 拷贝主键字段（K_XXX）
            this.K_Sign = source.K_Sign;

            // 拷贝业务字段（M_XXX）
            this.M_AudioSign = source.M_AudioSign;
            this.M_EffectSign = source.M_EffectSign;
            this.M_LifeTime = source.M_LifeTime;
            this.M_Sign = source.M_Sign;
        }

        /// <summary>
        /// 按主键创建实例（自动赋值K_主键字段）
        /// </summary>
        public static AbilityM_PlayEffect CreateForKey(string k_Sign) {
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
        public static AbilityM_PlayEffect CreateForID(byte id) {
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
        public static AbilityM_PlayEffect CreateForSign(string sign) {
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
        private static AbilityM_PlayEffect CreateSO() {
#if UNITY_EDITOR
           var url = $"{AssetURL.AbilitySO}SO/AB/AbilityM_PlayEffect.asset";
           var so = AssetDatabase.LoadAssetAtPath<AbilityM_PlayEffect>(url);
           if (so != null && so.IsEditorUseSO) {
              so.IsSetCsvData = true;
              return so;
           }
#endif
           return new AbilityM_PlayEffect();
        }
    }
}
