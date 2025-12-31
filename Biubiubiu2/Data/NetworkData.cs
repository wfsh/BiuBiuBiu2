using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class NetworkData {
        public const string Spawn_WorldNetwork = "WorldNetwork";
        public static int ConnIndex = 0;
        private static int serverPlatform = -1;

        public enum SpawnConnType : byte {
            None = 0,
            Role = 1,
            Ability = 2,
            AI = 3,
        }

        public static class Channels {
            public const int Reliable = 0; // ordered
            public const int Unreliable = 1; // unordered
        }
        
        public static bool IsStartServer {
            get {
                if (WarReportData.IsStartWarReport()) {
                    // 战报模式下，不启动网络连接，不启动服务端
                    return false;
                }
                if (Config != null) {
                    return Config.IsStartServer;
                }
                return IsServerPlatform();
            }
        }
        
        public static bool IsStartClient {
            get {
                if (Config != null) {
                    return Config.IsStartClient;
                }
                return IsServerPlatform() == false;
            }
        }

        public class ConfigData {
            public bool IsStartServer;
            public bool IsStartClient;
            public string ProxyAddr;
            public string IP;
            public ushort Port;
            public bool IsKCP = false;
            public bool IsSetKcpConfig = false; // 是否设置 KCP 配置
            public uint Interval = 10; // KCP 发送间隔
            public int FastResend = 2; // KCP 快速重传次数
            public uint SendWindowSize = 4096; // KCP 发送窗口大小
            public uint ReceiveWindowSize = 4096; // KCP 接收窗口大小
            public int RecvBufferSize  = 1024 * 1027 * 7; // KCP 最大重传间隔
            public int SendBufferSize  = 1024 * 1027 * 7; // KCP 最大重传间隔
            public uint MaxRetransmit = 10; // KCP 最大重传次数
            
            // 自定义网络配置
            public Hashtable NetConfig;
        }

        public static ConfigData Config = null;

        public static bool IsServerPlatform() {
#if BUILD_SERVER
            return true;
#endif
            if (serverPlatform != -1) {
                return serverPlatform == 1;
            }
            if (PlayerPrefs.HasKey("ServerPlatform") == false) {
                return false;
            }
            serverPlatform = PlayerPrefs.GetInt("ServerPlatform");
            return serverPlatform == 1;
        }

        public static void InitConfig(string proxyAddr, string ip, ushort port, Hashtable netConfig = null) {
            Config = new ConfigData();
            ConnIndex = 10000;
            if (IsServerPlatform()) {
                Config.IsStartServer = true;
                Config.IsStartClient = false;
            } else {
                Config.IsStartServer = false;
                Config.IsStartClient = true;
            }
            Config.ProxyAddr = proxyAddr;
            Config.IP = ip;
            Config.Port = port;
            Config.NetConfig = netConfig;
            if (netConfig != null) {
                Config.IsSetKcpConfig = true;
                Config.IsKCP = GetIntFromConfig("nt", 1) == 1;
                Config.Interval = GetUintFromConfig("i", Config.Interval);
                Config.FastResend = GetIntFromConfig("fr", Config.FastResend);
                Config.SendWindowSize = GetUintFromConfig("sws", Config.SendWindowSize);
                Config.ReceiveWindowSize = GetUintFromConfig("rws", Config.ReceiveWindowSize);
                Config.RecvBufferSize = GetIntFromConfig("rbs", Config.RecvBufferSize);
                Config.SendBufferSize = GetIntFromConfig("sbs", Config.SendBufferSize);
                Config.MaxRetransmit = GetUintFromConfig("mr", Config.MaxRetransmit);
                var useInstantiateAsync = false;
                if (IsServerPlatform() == false) {
                    useInstantiateAsync = GetIntFromConfig("ias", 1) == 1;
                }
                WarData.SetUseInstantiateAsync(useInstantiateAsync);
                PrefabPoolManager.SetUseInstantiateAsync(useInstantiateAsync);
                AudioPoolManager.SetUseInstantiateAsync(useInstantiateAsync);
            }
            Debug.Log("Init Network Config: " + Config.ProxyAddr + " " + Config.IP + ":" + Config.Port);
            // SentrySDKAgent.Instance.SetIPPort(ip, port);
        }
        
        public static void InitConfig(string ip, ushort port, bool isClient, bool isServer) {
            Config = new ConfigData();
            ConnIndex = 10000;
            Config.IsStartServer = isClient;
            Config.IsStartClient = isServer;
            Config.IP = ip;
            Config.Port = port;
            var useInstantiateAsync = true;
#if SERVER_LOGIC
            useInstantiateAsync = false;
#endif
            WarData.SetUseInstantiateAsync(useInstantiateAsync);
            PrefabPoolManager.SetUseInstantiateAsync(useInstantiateAsync);
            AudioPoolManager.SetUseInstantiateAsync(useInstantiateAsync);
            Debug.Log("Init Network Config:" + Config.IP + ":" + Config.Port + " useInstantiateAsync: " + useInstantiateAsync);
        }
        
        
        private static uint GetUintFromConfig(string key, uint defaultValue) {
            var value = defaultValue;
            if (Config != null && Config.NetConfig != null && Config.NetConfig.ContainsKey(key)) {
                var str = Config.NetConfig[key].ToString();
                if (uint.TryParse(str, out var parsedValue) == false) {
                    value = defaultValue;
                    Debug.Log("[NetworkConfig Warning] key: " + key + " str:" + str + " value failed, using default: " + defaultValue);
                } else {
                    value = parsedValue;
                    Debug.Log("[NetworkConfig] key: " + key + " value: " + value);
                }
            } else {
                Debug.Log("[NetworkConfig Warning]NetworkData NetConfig is null or key not found: " + key);
            }
            return value;
        }

        private static int GetIntFromConfig(string key, int defaultValue) {
            var value = defaultValue;
            if (Config != null && Config.NetConfig != null && Config.NetConfig.ContainsKey(key)) {
                var str = Config.NetConfig[key].ToString();
                if (int.TryParse(str, out var parsedValue) == false) {
                    value = defaultValue;
                    Debug.Log("[NetworkConfig Warning] key: " + key + " str:" + str +" value failed, using default: " + defaultValue);
                } else {
                    value = parsedValue;
                    Debug.Log("[NetworkConfig] key: " + key + " value: " + value);
                }
            } else {
                Debug.Log("[NetworkConfig Warning]NetworkData NetConfig is null or key not found: " + key);
            }
            return value;
        }

        public static int GetNetworkSyncDistance() {
            switch (ModeData.PlayMode) {
                case ModeData.ModeEnum.SausageGoldDash:
                    return 150;
                case ModeData.ModeEnum.SausageBeastCamp:
                    return 50;
            }
            return 200;
        }

        public static int GetBehaviourSyncDistance() {
            switch (ModeData.PlayMode) {
                case ModeData.ModeEnum.SausageGoldDash:
                    return 100;
                case ModeData.ModeEnum.SausageBeastCamp:
                    return 50;
            }
            return GetNetworkSyncDistance();
        }
    }
}