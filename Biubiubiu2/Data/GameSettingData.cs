using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class GameSettingData {
        private static Dictionary<string, Dictionary<string, GameSettingDataBase>> data;
        private static Dictionary<string, GameSettingDataBase> basicData;
        private static Dictionary<string, GameSettingDataBase> soundData;
        private static Dictionary<string, GameSettingDataBase> graphicData;

        public const string Sensitivity = "Sensitivity";
        public const string GunVibrate = "GunVibrate";
        public const string SuperWeaponVibrate = "SuperWeaponVibrate";

        public const string BGMVolume = "BGMVolume";
        public const string LobbyVolume = "LobbyVolume";
        public const string WarVolume = "WarVolume";

        public const string GraphicQuality = "GraphicQuality";
        public const string GraphicFrame = "GraphicFrame";
        
        public class GameSettingDataBase {
            private object value;
            private bool isInit = false;
            private string sign;
            private object defaultValue;
            private System.Type valueType;
            private const string CACHE_KEY_FORMAT = "GameSettingKey_{0}";
            protected string cacheKey;
            private int version = 1;
            
            // Init
            public virtual void Init<T>(string sign, T defaultValue) {
                this.sign = sign;
                this.defaultValue = defaultValue;
                valueType = typeof(T);
                SetCacheKey();
                InitValue();
            }

            protected virtual void SetCacheKey() {
                cacheKey = string.Format(CACHE_KEY_FORMAT, sign);
            }

            private void InitValue() {
                var hasCache = PlayerPrefs.HasKey(cacheKey);
                if (hasCache) {
                    if (valueType == typeof(int)) {
                        value = PlayerPrefs.GetInt(cacheKey);
                    } else if (valueType == typeof(float)) {
                        value = PlayerPrefs.GetFloat(cacheKey);
                    } else if (valueType == typeof(string)) {
                        value = PlayerPrefs.GetString(cacheKey);
                    }
                } else {
                    value = defaultValue;
                }
            }
            
            // GetValue
            public virtual T GetValue<T>() {
                return (T)value;
            }
            
            // SetValue
            public virtual bool SetValue<T>(T value) {
                if (this.value.Equals(value)) {
                    return false;
                }

                this.value = value;
                UpdateCache(this.value);
                
                return true;
            }

            protected virtual void UpdateCache(object value) {
                if (valueType == typeof(int)) {
                    PlayerPrefs.SetInt(cacheKey, (int)value);
                } else if (valueType == typeof(float)) {
                    PlayerPrefs.SetFloat(cacheKey, (float)value);
                } else if (valueType == typeof(string)) {
                    PlayerPrefs.SetString(cacheKey, (string)value);
                }
            }
            
            // Reset
            public virtual bool Reset() {
                return SetValue(defaultValue);
            }
        }

        public class GraphicQualityData : GameSettingDataBase {
            protected override void SetCacheKey() {
                cacheKey = QualityData.QUALITY_TYPE_CACHE_KEY;
            }
            protected override void UpdateCache(object value) {
            }
        }
        
        public class GraphicFrameData : GameSettingDataBase {
            protected override void SetCacheKey() {
                cacheKey = QualityData.TARGET_FPS_CACHE_KEY;
            }
            protected override void UpdateCache(object value) {
            }
        }

        public static void Init() {
            InitData();
            InitFeatureData();
        }

        private static void InitData() {
            basicData = new Dictionary<string, GameSettingDataBase>();
            var sensitivityData = new GameSettingDataBase();
            sensitivityData.Init<int>(Sensitivity, 100);
            basicData.Add(Sensitivity, sensitivityData);
            var gunVibrateData = new GameSettingDataBase();
            gunVibrateData.Init<int>(GunVibrate, 1);
            basicData.Add(GunVibrate, gunVibrateData);
            var superWeaponVibrateData = new GameSettingDataBase();
            superWeaponVibrateData.Init<int>(SuperWeaponVibrate, 1);
            basicData.Add(SuperWeaponVibrate, superWeaponVibrateData);

            soundData = new Dictionary<string, GameSettingDataBase>();
            var bgmVolumeData = new GameSettingDataBase();
            bgmVolumeData.Init<int>(BGMVolume, 100);
            soundData.Add(BGMVolume, bgmVolumeData);
            var lobbyVolumeData = new GameSettingDataBase();
            lobbyVolumeData.Init<int>(LobbyVolume, 100);
            soundData.Add(LobbyVolume, lobbyVolumeData);
            var warVolumeData = new GameSettingDataBase();
            warVolumeData.Init<int>(WarVolume, 100);
            soundData.Add(WarVolume, warVolumeData);
            
            graphicData = new Dictionary<string, GameSettingDataBase>();
            var graphicQualityData = new GraphicQualityData();
            graphicQualityData.Init<int>(GraphicQuality, (int)QualityData.QualityType.High);
            graphicData.Add(GraphicQuality, graphicQualityData);
            var graphicFrameData = new GraphicFrameData();
            graphicFrameData.Init<int>(GraphicFrame, (int)QualityData.FpsType.High);
            graphicData.Add(GraphicFrame, graphicFrameData);
            
            data = new Dictionary<string, Dictionary<string, GameSettingDataBase>>();
            data.Add("Basic", basicData);
            data.Add("Sound", soundData);
            data.Add("Graphic", graphicData);
        }

        private static void InitFeatureData() {
            TouchUtil.SetTouchSensitivityPercent(GetValue<int>(Sensitivity));
            UpdateGraphicFrameData();
            UpdateGraphicQualityData();
        }

        public static void UpdateGraphicQualityData() {
            int graphicQuality = GetValue<int>(GraphicQuality);
            var qualityType = graphicQuality switch {
                (int)QualityData.QualityType.Low => QualityData.QualityType.Low,
                (int)QualityData.QualityType.Medium => QualityData.QualityType.Medium,
                (int)QualityData.QualityType.High => QualityData.QualityType.High,
                _ => QualityData.QualityType.High
            };
            QualityData.SetQuality(qualityType);
        }

        public static void UpdateGraphicFrameData() {
            int graphicFrame = GetValue<int>(GraphicFrame);
            var fpsType = graphicFrame switch {
                (int)QualityData.FpsType.Low => QualityData.FpsType.Low,
                (int)QualityData.FpsType.Medium => QualityData.FpsType.Medium,
                (int)QualityData.FpsType.High => QualityData.FpsType.High,
                (int)QualityData.FpsType.High120 => QualityData.FpsType.High120,
                _ => QualityData.FpsType.High
            };
            QualityData.SetTargetFPS(fpsType);
        }

        public static T GetValue<T>(string sign) {
            if (basicData.ContainsKey(sign)) {
                return basicData[sign].GetValue<T>();
            }
            if (soundData.ContainsKey(sign)) {
                return soundData[sign].GetValue<T>();
            }
            if (graphicData.ContainsKey(sign)) {
                return graphicData[sign].GetValue<T>();
            }
            
            return default(T);
        }

        public static bool SetValue<T>(string sign, T value) {
            if (basicData.ContainsKey(sign)) {
                return basicData[sign].SetValue(value);
            }
            if (soundData.ContainsKey(sign)) {
                return soundData[sign].SetValue(value);
            }
            if (graphicData.ContainsKey(sign)) {
                return graphicData[sign].SetValue(value);
            }

            return false;
        }

        public static List<string> Reset(string tabSign) {
            var settingDatas = data[tabSign];
            List<string> resetSignList = new List<string>();
            foreach (KeyValuePair<string, GameSettingDataBase> kvp in settingDatas) {
                if (kvp.Value.Reset()) {
                    resetSignList.Add(kvp.Key);
                }
            }

            return resetSignList;
        }

        public static Dictionary<string, object> GetAllSettingValues() {
            var allSettingValues = new Dictionary<string, object>();
            foreach (var settingDatas in data.Values) {
                foreach (var sign in settingDatas.Keys) {
                    allSettingValues.Add(sign, settingDatas[sign].GetValue<object>());
                }
            }

            return allSettingValues;
        }
    }
}