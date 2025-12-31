using System;
using Mirror;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class WarReportNetworkBase : MonoBehaviour {
        protected bool isDestroy = false;
        public static int FrameInvokeCount = 0;
        public static float FrameInvokeTime = 0f;

        private struct SerializeProto {
            public int connId;
            public IProto_Doc proto;
        }

        public struct SendSaveProtoData {
            public Action<INetwork, IProto_Doc> Action;
            public List<SaveProtoStruct> ProtoList;
        }

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

        protected Dictionary<int, Dictionary<string, List<Body>>> connListeners =
            new Dictionary<int, Dictionary<string, List<Body>>>();

        private Dictionary<string, List<SaveProtoStruct>>
            saveListener = new Dictionary<string, List<SaveProtoStruct>>();

        private List<NewListenerData> newProtoListenerList = new List<NewListenerData>();

        private Dictionary<int, Dictionary<string, IProto_Doc>> unreliableData =
            new Dictionary<int, Dictionary<string, IProto_Doc>>();

        private float unreliableDelayTime = 0f;
        private EntityBase entity;
        private SystemBase mySystem;
        private StageData.GameWorldLayerType layer = StageData.GameWorldLayerType.None;
        private INetwork iNetwork;
        private INetworkSync iNetworkSync;
        private int connId = -1;
        private byte networkId = 0;


        public void Init(int connId, byte networkId) {
            this.connId = connId;
            this.networkId = networkId;
            iNetwork = GetComponent<INetwork>();
            iNetworkSync = GetComponent<INetworkSync>();
            if (iNetworkSync != null) {
                iNetworkSync.SetNetwork(iNetwork);
            }
        }

        public bool IsIgnoreSyncDistance() {
            return true;
        }

        void Start() {
            OnStart();
        }
        
        void OnDestroy() {
            OnClear();
            iNetwork = null;
            iNetworkSync = null;
            isDestroy = true;
            listeners.Clear();
            saveListener.Clear();
            connListeners.Clear();
            unreliableData.Clear();
        }

        void Update() {
            UpdateSaveProto();
            OnUpdate();
        }

        virtual protected void OnUpdate() {
        }

        virtual protected void OnStart() {
        }


        virtual protected void OnClear() {
        }

        public bool IsDestroy() {
            return isDestroy;
        }

        public void ConnectionToClientDisconnect() {
            if (isDestroy) {
                return;
            }
        }
        
        public byte GetNetworkId() {
            return networkId;
        }

        public int ConnId() {
            return this.connId;
        }

        public void Rpc(IRpc proto) {
        }

        public void Rpc(int connId, IRpc proto) {
        }

        public void TargetRpc(INetwork targeConn, ITargetRpc proto) {
        }
        
        public void TargetRpcList(List<INetworkCharacter> targeConnList, IProto_Doc proto) {
        }

        public void TargetRpc(INetwork targeConn, int connID, ITargetRpc proto) {
        }

        public void TargetRpc(INetwork targeConn, int connID, IRpc proto) {
        }

        public void TargetRpcList(List<INetworkCharacter> targeConnList, int connID, IProto_Doc proto) {
        }
        private void OnTargetRPCSerializeCallBack(int channel, int connId, byte[] bytes, INetwork targetConn) {
        }

        public void RPC(int channel, int connId, byte[] bytes) {
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

        public void TargetRPC(int channel, int connId, byte[] bytes, INetwork targetConn) {
            var target = (WarReportNetworkBase)targetConn;
            if (channel == NetworkData.Channels.Unreliable) {
                if (connId > 0) {
                    TargetRpcBytesForSyncIdUnreliable(bytes, connId);
                } else {
                    TargetRpcBytesUnreliable(bytes);
                }
            } else {
                if (connId > 0) {
                    TargetRpcBytesForSyncId(bytes, connId);
                } else {
                    TargetRpcBytes(bytes);
                }
            }
        }

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

        private void TargetRpcBytes(byte[] bytes) {
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

        private void TargetRpcBytesUnreliable(byte[] bytes) {
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

        private void TargetRpcBytesForSyncId(byte[] bytes, int connId) {
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

        private void TargetRpcBytesForSyncIdUnreliable(byte[] bytes, int connId) {
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
                ConnId = -1, ID = id
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
                ConnId = connId, ID = id
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
            List<SaveProtoStruct> saveList;
            if (saveListener.TryGetValue(data.GetID(), out saveList) == false) {
                saveList = new List<SaveProtoStruct>(10);
                saveListener.Add(data.GetID(), saveList);
            }
            if (saveList.Count >= 10) {
                saveList.RemoveAt(0);
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
            return this.isDestroy == false;
        }
        public bool HasObservers(int connectionId) {
            return true;
        }
        public void AddObserverNetwork(Action<INetwork> action) {
        }
        public void RemoveObserverNetwork(Action<INetwork> action) {
        }
    }
}