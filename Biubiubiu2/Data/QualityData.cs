using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Sofunny.BiuBiuBiu2.Data {
    public class QualityData {
        public const int DEVICE_LOW = 1; // 低端设备
        public const int DEVICE_MIDDLE = 2; // 中端设备
        public const int DEVICE_HIGHT = 3; // 高端设备
        public const int DEVICE_ULTRA = 4; // 旗舰设备

        // URP 渲染管线配置文件名称
        private const string URP_Default_Lobby = "Default_Lobby";
        private const string URP_Default_Low = "Default_Low";
        private const string URP_Default_Mid = "Default_Mid";
        private const string URP_Default_Hight = "Default_Hight";

        public const string QUALITY_TYPE_CACHE_KEY = "QualityType";
        public const string TARGET_FPS_CACHE_KEY = "TargetFps";

        public enum QualityType : int {
            Low = 2, // 低画质
            Medium = 3, // 中画质
            High = 4, // 高画质
        }

        public enum FpsType : int {
            Low = 2, // 30
            Medium = 3, // 45
            High = 4, // 60
            High120 = 5, // 120
        }

        // 提供一个 class 用于设置对应画质下需要调整的参数
        // 1、目标分辨率， 2、 分辨率比例， 3、 弹道数量  4、 音频数量  5、 是否显示阴影
        public class QualityInfo {
            public int TargetResolution; // 目标分辨率
            public float ResolutionScale; // 分辨率比例
            public int SelfBulletCount; // 弹道数量
            public int OtherBulletCount; // 弹道数量
            public int BloodSplatterCount; // 喷血数量
            public int SelfPlayAnimEffectProbability; // 播放自己动画特效的概率
            public int OtherPlayAnimEffectProbability; // 播放他人动画特效的概率
            public int AudioCount; // 音频数量
            public int SameEffectCount; // 想通特效数量
            public string GraphicSign; // 渲染管线配置文件名称
            public int GlobalMipmapLimit; // 全局纹理 Mipmap 限制
            public int UnreliableMessageLimit; // 非可靠消息限制
            public float AnimEffectResetLimitTime; // 动画特效重置限制时间
        }

        // 高画质对应的画质参数
        private static Dictionary<QualityType, QualityInfo> hightQualityInfoDict =
            new Dictionary<QualityType, QualityInfo>() {
                {
                    QualityType.High, new QualityInfo() {
                        TargetResolution = 1080,
                        ResolutionScale = 0.8f,
                        SelfBulletCount = 10,
                        OtherBulletCount = 8,
                        SelfPlayAnimEffectProbability = 100,
                        OtherPlayAnimEffectProbability = 100,
                        BloodSplatterCount = 6,
                        AudioCount = 6,
                        SameEffectCount = 6,
                        GraphicSign = URP_Default_Hight,
                        GlobalMipmapLimit = 0,
                        UnreliableMessageLimit = 50,
                        AnimEffectResetLimitTime = 0.05f,
                    }
                }, {
                    QualityType.Medium, new QualityInfo() {
                        TargetResolution = 960,
                        ResolutionScale = 0.7f,
                        SelfBulletCount = 8,
                        OtherBulletCount = 5,
                        SelfPlayAnimEffectProbability = 80,
                        OtherPlayAnimEffectProbability = 50,
                        BloodSplatterCount = 4,
                        AudioCount = 4,
                        SameEffectCount = 4,
                        GraphicSign = URP_Default_Mid,
                        GlobalMipmapLimit = 0,
                        UnreliableMessageLimit = 45,
                        AnimEffectResetLimitTime = 0.1f,
                    }
                }, {
                    QualityType.Low, new QualityInfo() {
                        TargetResolution = 960,
                        ResolutionScale = 0.6f,
                        SelfBulletCount = 6,
                        OtherBulletCount = 4,
                        SelfPlayAnimEffectProbability = 50,
                        OtherPlayAnimEffectProbability = 0,
                        BloodSplatterCount = 3,
                        AudioCount = 3,
                        SameEffectCount = 3,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 1,
                        UnreliableMessageLimit = 40,
                        AnimEffectResetLimitTime = 0.2f,
                    }
                }
            };

        // 中画质对应的画质参数
        private static Dictionary<QualityType, QualityInfo> midQualityInfoDict =
            new Dictionary<QualityType, QualityInfo>() {
                {
                    QualityType.High, new QualityInfo() {
                        TargetResolution = 960,
                        ResolutionScale = 0.7f,
                        SelfBulletCount = 8,
                        OtherBulletCount = 5,
                        SelfPlayAnimEffectProbability = 80,
                        OtherPlayAnimEffectProbability = 50,
                        BloodSplatterCount = 4,
                        AudioCount = 4,
                        SameEffectCount = 4,
                        GraphicSign = URP_Default_Mid,
                        GlobalMipmapLimit = 0,
                        UnreliableMessageLimit = 35,
                        AnimEffectResetLimitTime = 0.1f,
                    }
                }, {
                    QualityType.Medium, new QualityInfo() {
                        TargetResolution = 720,
                        ResolutionScale = 0.6f,
                        SelfBulletCount = 6,
                        OtherBulletCount = 4,
                        SelfPlayAnimEffectProbability = 50,
                        OtherPlayAnimEffectProbability = 20,
                        BloodSplatterCount = 3,
                        AudioCount = 3,
                        SameEffectCount = 3,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 0,
                        UnreliableMessageLimit = 30,
                        AnimEffectResetLimitTime = 0.2f,
                    }
                }, {
                    QualityType.Low, new QualityInfo() {
                        TargetResolution = 720,
                        ResolutionScale = 0.5f,
                        SelfBulletCount = 5,
                        OtherBulletCount = 3,
                        SelfPlayAnimEffectProbability = 20,
                        OtherPlayAnimEffectProbability = 0,
                        BloodSplatterCount = 2,
                        AudioCount = 2,
                        SameEffectCount = 2,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 1,
                        UnreliableMessageLimit = 25,
                        AnimEffectResetLimitTime = 0.3f,
                    }
                },
            };

        // 低画质对应的画质参数
        private static Dictionary<QualityType, QualityInfo> lowQualityInfoDict =
            new Dictionary<QualityType, QualityInfo>() {
                {
                    QualityType.High, new QualityInfo() {
                        TargetResolution = 720,
                        ResolutionScale = 0.7f,
                        SelfBulletCount = 6,
                        OtherBulletCount = 4,
                        SelfPlayAnimEffectProbability = 50,
                        OtherPlayAnimEffectProbability = 20,
                        BloodSplatterCount = 3,
                        AudioCount = 3,
                        SameEffectCount = 3,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 0,
                        UnreliableMessageLimit = 30,
                        AnimEffectResetLimitTime = 0.2f,
                    }
                }, {
                    QualityType.Medium, new QualityInfo() {
                        TargetResolution = 720,
                        ResolutionScale = 0.6f,
                        SelfBulletCount = 5,
                        OtherBulletCount = 3,
                        SelfPlayAnimEffectProbability = 30,
                        OtherPlayAnimEffectProbability = 0,
                        BloodSplatterCount = 2,
                        AudioCount = 2,
                        SameEffectCount = 2,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 1,
                        UnreliableMessageLimit = 25,
                        AnimEffectResetLimitTime = 0.3f,
                    }
                }, {
                    QualityType.Low, new QualityInfo() {
                        TargetResolution = 640,
                        ResolutionScale = 0.5f,
                        SelfBulletCount = 4,
                        OtherBulletCount = 2,
                        SelfPlayAnimEffectProbability = 0,
                        OtherPlayAnimEffectProbability = 0,
                        BloodSplatterCount = 1,
                        AudioCount = 1,
                        SameEffectCount = 1,
                        GraphicSign = URP_Default_Low,
                        GlobalMipmapLimit = 1,
                        UnreliableMessageLimit = 20,
                        AnimEffectResetLimitTime = 0.4f,
                    }
                },
            };

        private static int deviceLevel = 2; // 1: 低端 2: 中端 3: 高端

        public static int DeviceLevel => deviceLevel;
        private static QualityInfo deviceInfo = null;
        public static QualityInfo DeviceInfo => deviceInfo;
        private static string graphicSign = "";
        private static QualityType qualityType = QualityType.High;
        private static FpsType gameFPS = FpsType.High; // 游戏帧率 (战场内)
        public static FpsType GameFPS => gameFPS;

        private static Dictionary<string, RenderPipelineAsset> renderPipelineAssetDict =
            new Dictionary<string, RenderPipelineAsset>();

        private static bool isInit = false;
        
        public static QualityType GetQualityType() {
            return qualityType;
        }
        
        public static FpsType GetFpsType() {
            return gameFPS;
        }

        public static void Init(int level) {
            if (isInit == true) {
                return;
            } 
            isInit = true;
            qualityType = level >= DEVICE_HIGHT ? QualityType.High : QualityType.Medium; // 默认中画质, 高端设备默认高画质
            renderPipelineAssetDict.Add(URP_Default_Lobby, QualitySettings.renderPipeline);
            var qType = (QualityType)PlayerPrefs.GetInt(QUALITY_TYPE_CACHE_KEY, (int)qualityType);
            var fpsType = (FpsType)PlayerPrefs.GetInt(TARGET_FPS_CACHE_KEY, (int)gameFPS);
            SetDeviceLevel(level);
            SetQuality(qType);
            SetTargetFPS(fpsType);
        }

        public static void SetDeviceLevel(int level) {
            Debug.Log("[Quality]SetDeviceLevel: " + level);
            deviceLevel = level;
        }

        public static void SetTargetFPS(FpsType fpsType) {
            gameFPS = fpsType;
            ApplyTargetFPS(StageData.NowStageType);
            PlayerPrefs.SetInt(TARGET_FPS_CACHE_KEY, (int)fpsType);
        }
        
        public static void ApplyTargetFPS(StageData.StageType stageType) {
            return;
            var fps = 60;
            if (stageType == StageData.StageType.Room) {
                fps = 30;
            } else {
                switch (gameFPS) {
                    case FpsType.Low:
                        fps = 30;
                        break;
                    case FpsType.Medium:
                        fps = 45;
                        break;
                    case FpsType.High:
                        fps = 60;
                        break;
                    case FpsType.High120:
                        fps = 120;
                        break;
                }
            }
            Debug.Log("[Quality]SetTargetFPS: " + fps + " gameFPS:" + gameFPS);
            Application.targetFrameRate = fps;
        }

        public static void SetQuality(QualityType qType) {
            if (deviceInfo != null && qualityType == qType) {
                return;
            }
            Debug.Log("[Quality]SetQuality: " + qType + " DeviceLevel: " + deviceLevel);
            qualityType = qType;
            PerfAnalyzerAgent.SetQuality(GetQualitySign(qualityType));
            try {
                switch (deviceLevel) {
                    case DEVICE_ULTRA:
                    case DEVICE_HIGHT:
                        deviceInfo = hightQualityInfoDict[qType];
                        break;
                    case DEVICE_MIDDLE:
                        deviceInfo = midQualityInfoDict[qType];
                        break;
                    case DEVICE_LOW:
                        deviceInfo = lowQualityInfoDict[qType];
                        break;
                }
            } catch (Exception e) {
                Debug.LogError("Error SetQuality qType: " + qType + " deviceLevel: " + deviceLevel + " E:" + e);
                qualityType = QualityType.Medium;
                deviceInfo = hightQualityInfoDict[qualityType];
            }
            ApplyURPGraphic(StageData.NowStageType, null); // 设置 URP 渲染管线
            ApplyGlobalMipmapLimit(); // 设置全局纹理 Mipmap 限制
            ApplyResolution(); // 设置分辨率
            SetMaxAudioPerAsset(); // 设置音频池每种的最大音频数量
            Debug.Log("[Quality]SelfBulletCount: " + deviceInfo.SelfBulletCount);
            Debug.Log("[Quality]OtherBulletCount: " + deviceInfo.OtherBulletCount);
            Debug.Log("[Quality]GetBloodSplatterCount: " + deviceInfo.BloodSplatterCount);
            PlayerPrefs.SetInt(QUALITY_TYPE_CACHE_KEY, (int)qualityType);
        }

        public static void ApplyURPGraphic(StageData.StageType stageType, Action CallBack) {
            var showGraphicSign = "";
            if (stageType == StageData.StageType.Room) {
                showGraphicSign = "Default_Lobby";
            } else {
                showGraphicSign = deviceInfo.GraphicSign;
            }
            if (graphicSign == showGraphicSign) {
                CallBack?.Invoke();
                return;
            }
            Debug.Log("[Quality]ApplyURPGraphic: " + showGraphicSign);
            if (renderPipelineAssetDict.TryGetValue(showGraphicSign, out var asset) && asset != null) {
                SetRenderPipelineAsset(asset);
                graphicSign = showGraphicSign;
                CallBack?.Invoke();
            } else {
                AssetManager.LoadURPConfigs(showGraphicSign, asset => {
                    SetRenderPipelineAsset(asset);
                    renderPipelineAssetDict.Add(showGraphicSign, asset);
                    graphicSign = showGraphicSign;
                    CallBack?.Invoke();
                });
            }
        }

        private static void SetRenderPipelineAsset(RenderPipelineAsset asset) {
            if (asset == null) {
                Debug.LogError("RenderPipelineAsset is null");
                return;
            }
            Debug.Log("[Quality]SetRenderPipelineAsset: " + asset);
            GraphicsSettings.renderPipelineAsset = asset;
            QualitySettings.renderPipeline = GraphicsSettings.renderPipelineAsset;
        }

        private static void ApplyGlobalMipmapLimit() {
            if (deviceInfo.GlobalMipmapLimit == QualitySettings.globalTextureMipmapLimit) {
                return;
            }
            Debug.Log("[Quality]ApplyGlobalMipmapLimit: " + deviceInfo.GlobalMipmapLimit);
            QualitySettings.globalTextureMipmapLimit = deviceInfo.GlobalMipmapLimit;
        }

        private static void ApplyResolution() {
            Debug.Log("[Quality]ApplyResolution: " + deviceInfo.ResolutionScale + " " + deviceInfo.TargetResolution);
#if UNITY_WEBGL
            WXSDKAgent.Instance.InitDevicePixel(deviceInfo.ResolutionScale, deviceInfo.TargetResolution);
#elif UNITY_IOS || UNITY_ANDROID
            Screen.SetResolution((int)(StageData.SceneWidth * deviceInfo.ResolutionScale), (int)(StageData.SceneHeight * deviceInfo.ResolutionScale), true);
#endif
        }

        public static void SetMaxAudioPerAsset() {
            Debug.Log("[Quality]SetMaxAudioPerAsset: " + deviceInfo.AudioCount);
            AudioPoolManager.SetMaxAudioPerAsset(deviceInfo.AudioCount);
        }

        public static int GetSelfBulletCount() {
            return deviceInfo.SelfBulletCount;
        }

        public static int GetOtherBulletCount() {
            return deviceInfo.OtherBulletCount;
        }

        public static int GetBloodSplatterCount() {
            return deviceInfo.BloodSplatterCount;
        }

        public static int GetUnreliableMessageLimit() {
            return deviceInfo.UnreliableMessageLimit;
        }
        
        public static float GetAnimEffectResetLimitTime() {
            return deviceInfo.AnimEffectResetLimitTime;
        }

        public static string GetQualitySign(QualityType qType) {
            switch (qType) {
                case QualityType.High:
                    return "精致";
                case QualityType.Medium:
                    return "均衡";
            }
            return "流畅";
        }
    }
}