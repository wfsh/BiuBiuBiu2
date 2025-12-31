using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class GameData {     
        public enum BuildModeEnum {
            None = 0,
            Dev = 1,
            Test = 2,
            Release = 3,
            DevProfiler = 4,
        }
        public const string GAME_VERSION = "1.26"; // 基础游戏版本号
        public static int BuildIndex = 0; // 该版本下的第几次打包
        public static long AsserVersion = 0; // 资源版本号，含 AB 和 DLL
        private static string showGameVersion;
        private static string serverName;
        public static BuildModeEnum BuildMode = BuildModeEnum.None; // 0: None 1:开发 2:测试 3:先行，4：开发-Profiler
        private static long networkVersion = 0; // 服务器版本号
        
#if UNITY_EDITOR
        public static float PerfRate = 1f; // 性能采样率
#elif BUILD_SERVER
        public static float PerfRate = 0.3f; 
#elif UNITY_WEBGL
        public static float PerfRate = 0f;
#else
        public static float PerfRate = 0.2f; 
#endif

        private static string remoteClientVersion;
        
        public static string ShowGameVersion {
            get {
                return showGameVersion;
            }
        }
        public static long NetworkVersion {
            get {
                return networkVersion;
            }
        }
        public static void Init() {
            var configTxt = Resources.Load<TextAsset>("Txt/Config").text;
            var data = (Hashtable)MiniJSON.jsonDecode(configTxt);
            BuildIndex = int.Parse((string)data["BuildCount"]);
            BuildMode = (BuildModeEnum)int.Parse(data["buildMode"].ToString());
            serverName = (string)data["serverName"];
            networkVersion = long.Parse((string)data["mirrorVersion"]);
            showGameVersion = $"{GAME_VERSION}.{BuildIndex}";
            Debug.Log($"BuildIndex:{BuildIndex} BuildMode:{BuildMode} GameVersion:{showGameVersion} NetworkVersion:{networkVersion}");
        }
        
        public static string GetBuildName() {
            return BuildMode.ToString();
        }
        
        public static string GetServerName() {
            return serverName;
        }
        
        public static string GetBuildPackageName() {
            if (serverName.Contains("正式") || serverName.Contains("体验") || serverName.Contains("审核")) {
                return serverName;
            }
            return "开发服";
        }

        public static bool IsRelease() {
#if RELEASE
        return true;
#endif
            return BuildMode == BuildModeEnum.Release;
        }

        public static void SetRemoteClientVersion(string remoteVersion) {
            remoteClientVersion = remoteVersion;
        }
        
        public static bool IsLocalClientVersionNewer() {
            if (remoteClientVersion == ShowGameVersion) {
                return true;
            }

            if (string.IsNullOrEmpty(remoteClientVersion)) {
                return true;
            }

            var localVersion = GAME_VERSION;
            var localBuild = BuildIndex;
            var parts = remoteClientVersion.Split('.');
            if (parts.Length < 3) {
                return true;
            }

            var clientGameVersion = $"{parts[0]}.{parts[1]}";
            if (!int.TryParse(parts[2], out var clientBuildCount)) {
                return true;
            }

            var localVer = new Version(localVersion + ".0");
            var serverVer = new Version(clientGameVersion + ".0");

            int comparison = serverVer.CompareTo(localVer);
            if (comparison > 0) {
                return false;
            }

            if (comparison < 0) {
                return true;
            }

            return clientBuildCount <= localBuild;
        }
    }
}