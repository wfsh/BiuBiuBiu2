using System;
using Mirror;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class NetworkBase : NetworkBehaviour {
        protected bool isDestroy = false;
        public static int FrameInvokeCount = 0;
        public static float FrameInvokeTime = 0f;

        public struct SaveProtoStruct {
            public IProto_Doc Proto;
            public int ConnId;
            public float SaveTime;
        }
        
        public struct NewListenerData {
            public string ID;
            public int ConnId;
        }
        
        public struct Body {
            public Action<INetwork, IProto_Doc> action;
            public bool isRemove;
        }

        protected Dictionary<string, List<Body>> listeners = new Dictionary<string, List<Body>>();
        protected Dictionary<int, Dictionary<string, List<Body>>> connListeners = new Dictionary<int, Dictionary<string, List<Body>>>();

        private Dictionary<string, List<SaveProtoStruct>> saveListener =  new Dictionary<string, List<SaveProtoStruct>>();
        private List<NewListenerData> newProtoListenerList = new List<NewListenerData>();
        private float unreliableDelayTime = 0f;
        private EntityBase entity;
        private SystemBase mySystem;
        private StageData.GameWorldLayerType layer = StageData.GameWorldLayerType.None;
        private INetwork iNetwork;
        private INetworkSync iNetworkSync;
        private HashSet<int> observedNetwork = new HashSet<int>();
        private Action<INetwork> observerAddedCallback = null;
        private float lastObserverUpdateTime = 0f;
        
        private static byte networkIndex = 100; // 默认从 100 开始，避免跟其他网络协议冲突
        private byte networkId = 0;

        void Awake() {
            networkIndex++;
            networkId = networkIndex;
            iNetwork = GetComponent<INetwork>();
            iNetworkSync = GetComponent<INetworkSync>();
            iNetworkSync.SetNetwork(iNetwork);
            if (iNetwork == null) {
                Debug.LogError("NetworkBase 无法直接使用，需要通过 INetwork 扩展子类，参考 CharacterNetwork");
            }
        }

        void Start() {
            OnStart();
        }

        void Update() {
            UpdateSaveProto();
            UpdateObservers();
            OnUpdate();
        }

        public bool IsIgnoreSyncDistance() {
            return false;
        }

        virtual protected void OnUpdate() {
        }

        virtual protected void OnStart() {
        }

        void OnDestroy() {
            OnClear();
            iNetwork = null;
            iNetworkSync = null;
            isDestroy = true;
            listeners.Clear();
            saveListener.Clear();
            connListeners.Clear();
        }

        virtual protected void OnClear() {
        }

        public bool IsDestroy() {
            return isDestroy;
        }

        public void ConnectionToClientDisconnect() {
            if (isDestroy || connectionToClient == null) {
                return;
            }
            connectionToClient.Disconnect();
        }
        
        public byte GetNetworkId() {
            return networkId;
        }

        public int ConnId() {
            if (connectionToClient != null) {
                return connectionToClient.connectionId;
            }
            if (connectionToServer != null) {
                return connectionToServer.connectionId;
            }
            return -1;
        }

        public void Rpc(IRpc proto) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = proto, ConnID = -1, Channel = proto.GetChannel(), CallBack = OnRPCSerializeCallBack,
            });
        }

        public void Rpc(int connId, IRpc proto) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = proto,
                ConnID = connId,
                Channel = proto.GetChannel(),
                CallBack = OnRPCSerializeCallBack,
            });
        }

        public void TargetRpc(INetwork targeConn, ITargetRpc proto) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.TargetRPCSerialize {
                ProtoDoc = proto,
                ConnID = -1,
                TargetNetwork = targeConn,
                Channel = proto.GetChannel(),
                CallBack = OnTargetRPCSerializeCallBack,
            });
        }
        public void TargetRpcList(List<INetworkCharacter> targeConnList, IProto_Doc proto) {
            if (isDestroy || targeConnList.Count <= 0) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = proto,
                ConnID = -1,
                Channel = proto.GetChannel(),
                CallBack = (channel, connId, bytes) => {
                    var isFirst = true;
                    for (int i = 0; i < targeConnList.Count; i++) {
                        var targeConn = targeConnList[i];
                        if (targeConn.IsDestroy() == false && targeConn.IsOnline()) {
                            OnTargetRPCSerializeCallBack(channel, -1, bytes, targeConn, proto.GetID(), isFirst);
                            isFirst = false;
                        }
                    }
                }
            });
        }

        public void TargetRpc(INetwork targeConn, int connID, ITargetRpc proto) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.TargetRPCSerialize {
                ProtoDoc = proto,
                TargetNetwork = targeConn,
                ConnID = connID,
                Channel = proto.GetChannel(),
                CallBack = OnTargetRPCSerializeCallBack,
            });
        }

        public void TargetRpc(INetwork targeConn, int connID, IRpc proto) {
            if (isDestroy) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.TargetRPCSerialize {
                ProtoDoc = proto,
                TargetNetwork = targeConn,
                ConnID = connID,
                Channel = proto.GetChannel(),
                CallBack = OnTargetRPCSerializeCallBack,
            });
        }

        public void TargetRpcList(List<INetworkCharacter> targeConnList, int connID, IProto_Doc proto) {
            if (isDestroy || targeConnList.Count <= 0) {
                return;
            }
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = proto,
                ConnID = connID,
                Channel = proto.GetChannel(),
                CallBack = (channel, connId, bytes) => {
                    var isFirst = true;
                    for (int i = 0; i < targeConnList.Count; i++) {
                        var targeConn = targeConnList[i];
                        if (targeConn.IsDestroy() == false && targeConn.IsOnline()) {
                            OnTargetRPCSerializeCallBack(channel, connId, bytes, targeConn, proto.GetID(), isFirst);
                            isFirst = false;
                        }
                    }
                }
            });
        }

        private void OnRPCSerializeCallBack(int channel, int connId, byte[] bytes) {
            MsgRegister.Dispatcher(new M_WarReport.SetRpcReportData {
                NetworkId = networkId,
                TargetNetworkId = 0,
                Bytes = bytes,
                ConnID = connId,
                Channel = (byte)channel,
            });
            if (channel == NetworkData.Channels.Unreliable) {
                if (connId > 0) {
                    RpcBytesForSyncIdUnreliable(connId, bytes);
                } else {
                    RpcBytesUnreliable(bytes);
                }
            } else {
                if (connId > 0) {
                    RpcBytesForSyncId(connId, bytes);
                } else {
                    RpcBytes(bytes);
                }
            }
        }

        private void OnTargetRPCSerializeCallBack(int channel, int connId, byte[] bytes, INetwork targetConn, string protoId, bool isNew) {
            NetworkConnectionToClient connToClient = null;
            var isSaveData = isNew;
            if (protoId == "Proto_Network.TargetRpc_RemoveWorldNetwork" || protoId == "Proto_Network.TargetRpc_SyncSpawnData") {
                if (targetConn.IsIgnoreSyncDistance() == false) {
                    isSaveData = false;
                }
            }
            if (isSaveData) {
                MsgRegister.Dispatcher(new M_WarReport.SetTargetRpcReportData {
                    NetworkId = networkId,
                    Bytes = bytes,
                    TargetNetworkId = targetConn.GetNetworkId(),
                    ConnID = connId,
                    Channel = (byte)channel,
                });
            } 
            if (targetConn.IsIgnoreSyncDistance()) {
                return;
            }
            var target = (NetworkBase)targetConn;
            connToClient = target.connectionToClient;
            if (channel == NetworkData.Channels.Unreliable) {
                if (connId > 0) {
                    TargetRpcBytesForSyncIdUnreliable(connToClient, bytes, connId);
                } else {
                    TargetRpcBytesUnreliable(connToClient, bytes);
                }
            } else {
                if (connId > 0) {
                    TargetRpcBytesForSyncId(connToClient, bytes, connId);
                } else {
                    TargetRpcBytes(connToClient, bytes);
                }
            }
        }

        [ClientRpc]
        private void RpcBytes(byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, CallBack = OnRPCSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [ClientRpc(channel = Channels.Unreliable)]
        private void RpcBytesUnreliable(byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, CallBack = OnRPCUnSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [TargetRpc]
        private void TargetRpcBytes(NetworkConnection conn, byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, CallBack = OnRPCSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [TargetRpc(channel = Channels.Unreliable)]
        private void TargetRpcBytesUnreliable(NetworkConnection conn, byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, CallBack = OnRPCUnSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [ClientRpc]
        private void RpcBytesForSyncId(int connId, byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, ConnID = connId, CallBack = OnRPCSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [ClientRpc(channel = Channels.Unreliable)]
        private void RpcBytesForSyncIdUnreliable(int connId, byte[] bytes) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, ConnID = connId, CallBack = OnRPCUnSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [TargetRpc]
        private void TargetRpcBytesForSyncId(NetworkConnection conn, byte[] bytes, int connId) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, ConnID = connId, CallBack = OnRPCSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        [TargetRpc(channel = Channels.Unreliable)]
        private void TargetRpcBytesForSyncIdUnreliable(NetworkConnection conn, byte[] bytes, int connId) {
            if (isDestroy) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = bytes, ConnID = connId, CallBack = OnRPCUnSerializeCallBack,
            });
            var useTime = Time.realtimeSinceStartup - time;
            FrameInvokeTime += useTime;
        }

        private void OnRPCUnSerializeCallBack(int connId, IProto_Doc proto) {
            SendProtoData(proto, connId);
        }

        private void OnRPCSerializeCallBack(int connId, IProto_Doc proto) {
            SendProtoData(proto, connId);
        }
        

        private bool CheckUnreliableMessageLimit() {
            return FrameInvokeCount <= QualityData.GetUnreliableMessageLimit();
        }

        private void SendProtoData(IProto_Doc proto, int connId) {
            try {
                PerfAnalyzerAgent.BeginSample(proto.GetID());
                if (connId != 0) {
                    SendProtoCallBackConnId(connId, proto);
                } else {
                    SendProtoCallBack(proto);
                }
            } catch (Exception e) {
                Debug.LogError($"RPC 协议执行出错{proto.GetID()} SyncId: {connId} _[E]:{e}");
            } finally {
                PerfAnalyzerAgent.EndSample(proto.GetID());
            }
        }

        public void SetParent(StageData.GameWorldLayerType layer) {
            if (this.layer == layer) {
                return;
            }
            this.layer = layer;
            MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                layer = layer, transform = transform
            });
        }

        public void SetName(string name) {
            gameObject.name = name;
        }

        public string GetName() {
            return gameObject.name;
        }

        public void AddProtoCallBack(string id, Action<INetwork, IProto_Doc> action) {
            List<Body> list;
            if (listeners.TryGetValue(id, out list) == false) {
                list = new List<Body>();
                listeners.Add(id, list);
            }
            var body = new Body();
            body.action = action;
            body.isRemove = false;
            list.Add(body);
            newProtoListenerList.Add(new NewListenerData {
                ConnId = -1,
                ID = id
            });
        }

        public void RemoveProtoCallBack(string id, Action<INetwork, IProto_Doc> action) {
            List<Body> list;
            if (listeners.TryGetValue(id, out list)) {
                for (int i = 0; i < list.Count; i++) {
                    var body = list[i];
                    if (body.action == action) {
                        body.isRemove = true;
                        list[i] = body;
                        return;
                    }
                }
            }
        }

        public void AddProtoCallBack(int connId, string id, Action<INetwork, IProto_Doc> action) {
            Dictionary<string, List<Body>> worldData;
            if (connListeners.TryGetValue(connId, out worldData) == false) {
                worldData = new Dictionary<string, List<Body>>();
                connListeners.Add(connId, worldData);
            }
            List<Body> list;
            if (worldData.TryGetValue(id, out list) == false) {
                list = new List<Body>();
                worldData.Add(id, list);
            }
            var body = new Body();
            body.action = action;
            body.isRemove = false;
            list.Add(body);
            newProtoListenerList.Add(new NewListenerData {
                ConnId = connId,
                ID = id
            });
        }

        public void RemoveProtoCallBack(int worldId, string id, Action<INetwork, IProto_Doc> action) {
            Dictionary<string, List<Body>> worldData;
            if (connListeners.TryGetValue(worldId, out worldData)) {
                List<Body> list;
                if (worldData.TryGetValue(id, out list)) {
                    for (int i = 0; i < list.Count; i++) {
                        var body = list[i];
                        if (body.action == action) {
                            body.isRemove = true;
                            list[i] = body;
                            return;
                        }
                    }
                }
            }
        }

        public void SendProtoCallBack(IProto_Doc data, bool updateSave = true) {
            List<Body> list;
            var isSend = false;
            if (listeners.TryGetValue(data.GetID(), out list)) {
                if (list.Count > 0) {
                    if (updateSave) {
                        UpdateSaveProtoForProtoId(data.GetID(), -1);
                    }
                    isSend = true;
                    for (int i = 0; i < list.Count; i++) {
                        var body = list[i];
                        if (body.isRemove || body.action == null) {
                            list.RemoveAt(i);
                            i--;
                        } else {
                            body.action.Invoke(iNetwork, data);
                            FrameInvokeCount++;
                        }
                    }
                }
            }
            if (isSend == false && data.GetChannel() == NetworkData.Channels.Reliable) {
                SaveProto(data, -1);
            } 
        }

        public void SendProtoCallBackConnId(int connId, IProto_Doc data, bool updateSave = true) {
            var isSend = false;
            Dictionary<string, List<Body>> worldData;
            if (connListeners.TryGetValue(connId, out worldData)) {
                List<Body> list;
                if (worldData.TryGetValue(data.GetID(), out list)) {
                    if (list.Count > 0) {
                        if (updateSave) {
                            UpdateSaveProtoForProtoId(data.GetID(), connId);
                        }
                        isSend = true;
                        for (int i = 0; i < list.Count; i++) {
                            var body = list[i];
                            if (body.isRemove || body.action == null) {
                                list.RemoveAt(i);
                                i--;
                            } else {
                                body.action.Invoke(iNetwork, data);
                                FrameInvokeCount++;
                            }
                        }
                    }
                }
            }
            if (isSend == false && data.GetChannel() == NetworkData.Channels.Reliable) {
                SaveProto(data, connId);
            }
        }
        
        // 更新保存的协议数据
        private void UpdateSaveProtoForProtoId(string protoID, int connId) {
            List<SaveProtoStruct> saveList;
            if (saveListener.TryGetValue(protoID, out saveList) == false) {
                return;
            }
            var time = Time.realtimeSinceStartup;
            for (int i = 0; i < saveList.Count; i++) {
                var saveProto = saveList[i];
                if (saveProto.ConnId != connId) {
                    continue;
                }
                if (connId == -1) {
                    SendProtoCallBack(saveProto.Proto, false);
                } else {
                    SendProtoCallBackConnId(connId, saveProto.Proto, false);
                }
                saveList.RemoveAt(i);
                i--;
#if !RELEASE
                var lostTime = time - saveProto.SaveTime;
                if (lostTime > 5f) {
                    Debug.LogError($"ConnID:{connId}，Data:{saveProto.Proto.GetID()} 存储的临时数据超过 5 秒, 需要检查是否符合需求");
                }
#endif
            }
            if (saveList.Count <= 0) {
                saveListener.Remove(protoID);
            }
        }
        
        private void SaveProto(IProto_Doc data, int connId) {
            if (data is ICmd) {
                // 服务端数据不需要缓存。
                return;
            }
            List<SaveProtoStruct> saveList;
            if (saveListener.TryGetValue(data.GetID(), out saveList) == false) {
                saveList = new List<SaveProtoStruct>(10);
                saveListener.Add(data.GetID(), saveList);
            }
            if (data is IRpc) {
                if (saveList.Count >= 10) {
                    // 超过 10 条数据。将不在进行缓存，有可能数据本身就不需要监听
                    return;
                }
            } else {
#if UNITY_EDITOR
                if (saveList.Count >= 10) {
                    Debug.LogError($"ConnID:{connId}，Data:{data} 存储的临时数据超过 {saveList.Count} 条，需要检查是否正确");
                }
#endif
            }
            var protoData = new SaveProtoStruct();
            protoData.Proto = data;
            protoData.ConnId = connId;
            protoData.SaveTime = Time.realtimeSinceStartup;
            saveList.Add(protoData);
        }
        // 针对新增的协议监听，发送提前缓存的数据
        private void UpdateSaveProto() {
            var len = newProtoListenerList.Count;
            if (len <= 0) {
                return;
            }
            for (int i = 0; i < len; i++) {
                var data = newProtoListenerList[i];
                UpdateSaveProtoForProtoId(data.ID, data.ConnId);
            }
            newProtoListenerList.Clear();
        }
        public INetworkSync GetNetworkSync() {
            return iNetworkSync;
        }

        public bool IsOnline() {
            if (NetworkData.IsStartServer) {
                return this.isDestroy == false && this.connectionToClient != null;
            }
            return this.isDestroy == false;
        }
        
        public bool HasObservers(int connectionId) {
            foreach (var net in netIdentity.observers) {
                var key = net.Key;
                var value = net.Value;
                if (value == null || value.isReady == false) {
                    continue;
                }
                if (key == connectionId) {
                    return true;
                }
            }
            return false;
        }

        public void AddObserverNetwork(Action<INetwork> action) {
            if (observerAddedCallback == null) {
                observedNetwork.Clear();
                lastObserverUpdateTime = Time.realtimeSinceStartup;
            }
            observerAddedCallback += action;
        }

        public void RemoveObserverNetwork(Action<INetwork> action) {
            observerAddedCallback -= action;
        }
        
        /// <summary>
        /// 更新观察者列表
        /// </summary>
        private void UpdateObservers() {
            if (observerAddedCallback == null) {
                return;
            }
            if (Time.realtimeSinceStartup - lastObserverUpdateTime < 0.1f) {
                if (observedNetwork.Count == netIdentity.observers.Count) {
                    return;
                }
            }
            lastObserverUpdateTime = Time.realtimeSinceStartup;
            var list = new HashSet<int>();
            foreach (var net in netIdentity.observers) {
                var key = net.Key;
                var value = net.Value;
                if (value == null || value.isReady == false) {
                    continue;
                }
                list.Add(key);
                // 根据缓存的 tempObserverList 和 key 做对比，传出新增的观察者
                if (observedNetwork.Contains(key)) {
                    continue;
                }
                var gameObject = value.identity.gameObject;
                var toNet = gameObject.GetComponent<INetwork>();
                if (toNet is INetworkCharacter) {
                    var characterNet = (INetworkCharacter)toNet;
                    if (characterNet.IsCharacterReady() == false) {
                        list.Remove(key);
                        continue;
                    }
                }
                observerAddedCallback?.Invoke(toNet);
            }
            observedNetwork = list;
        }
    }
}