using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWorldNetworkBehaviourList : ComponentBase {
        private List<IClientWorldNetwork> worldNetworkLists = new List<IClientWorldNetwork>();
        private Dictionary<int, IClientWorldNetwork> worldNetworkData = new Dictionary<int, IClientWorldNetwork>();
        private Dictionary<int, bool> connAddData = new Dictionary<int, bool>();

        protected override void OnAwake() {
            MsgRegister.Register<CM_Network.AddWorldNetworkBehaviour>(OnAddWorldNetworkCallBack);
            MsgRegister.Register<CM_Network.RemoveWorldNetworkBehaviour>(OnRemoveWorldNetworkCallBack);
            MsgRegister.Register<CM_Network.GetWorldNetworkBehaviour>(OnGetWorldNetworkCallBack);
        }

        protected override void OnClear() {
            this.RemoveProtoCallBack(Proto_Network.TargetRpc_RemoveWorldNetwork.ID, TargetRpcRemoveWorldNetworkCallBack);
            this.RemoveProtoCallBack(Proto_Network.TargetRpc_SyncSpawnData.ID, TargetRpcSyncSpawnDataCallBack);
            MsgRegister.Unregister<CM_Network.AddWorldNetworkBehaviour>(OnAddWorldNetworkCallBack);
            MsgRegister.Unregister<CM_Network.RemoveWorldNetworkBehaviour>(OnRemoveWorldNetworkCallBack);
            MsgRegister.Unregister<CM_Network.GetWorldNetworkBehaviour>(OnGetWorldNetworkCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Network.TargetRpc_SyncSpawnData.ID, TargetRpcSyncSpawnDataCallBack);
            AddProtoCallBack(Proto_Network.TargetRpc_RemoveWorldNetwork.ID, TargetRpcRemoveWorldNetworkCallBack);
        }

        private void TargetRpcSyncSpawnDataCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Network.TargetRpc_SyncSpawnData)data;
            var type = (NetworkData.SpawnConnType)rpcData.connType;
            connAddData[rpcData.connID] = true;
            MsgRegister.Dispatcher(new M_Network.RPCSyncUnSerialize {
                Datas = rpcData.spawnData,
                ConnID = rpcData.connID,
                SpawnType = type,
                CallBack = OnRPCSyncUnSerializeCallBack,
            });
        }

        private void OnRPCSyncUnSerializeCallBack(int syncId, NetworkData.SpawnConnType syncType,
            IProto_Doc spawnData) {
            // 给对应的 system 附加 ClientWorldNetworkBehaviour Component
            // 附加成功后，ClientWorldNetworkBehaviour 会调用 CM_Network.AddWorldNetworkBehaviour
            MsgRegister.Dispatcher(new CM_Network.SpawnWorldNetwork {
                ProtoDoc = spawnData, ConnID = syncId, ConnType = syncType,
            });
        }

        private void TargetRpcRemoveWorldNetworkCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Network.TargetRpc_RemoveWorldNetwork)data;
            RemoveWorldNetwork(rpcData.syncId);
        }

        private void OnAddWorldNetworkCallBack(CM_Network.AddWorldNetworkBehaviour ent) {
            if (ent.IWorldNetwork == null) {
                Debug.LogError("添加 WorldNetwork 失败，IWorldNetwork 不能为空");
                return;
            }
            var network = ent.IWorldNetwork;
            if (network.GetConnID() <= 0) {
#if UNITY_EDITOR
                Debug.LogError("添加 WorldNetwork 失败，ConnID 不能小于0, 请检查服务端是否有添加 ServerNetworkSync 的组件");
#endif
                return;
            }
            var isAdd = connAddData[network.GetConnID()];
            if (isAdd == false) {
                network.SetIsOnline(false);
            } else {
                if (worldNetworkData.ContainsKey(network.GetConnID())) {
                    Debug.LogError("connId 为唯一 ID，不能重复:" + network.GetConnID());
                    return;
                }
                network.SetIsOnline(true);
                worldNetworkData.Add(network.GetConnID(), network);
                worldNetworkLists.Add(network);
            }
        }

        private void OnRemoveWorldNetworkCallBack(CM_Network.RemoveWorldNetworkBehaviour ent) {
            RemoveWorldNetwork(ent.ConnID);
        }

        private void RemoveWorldNetwork(int connId) {
            connAddData[connId] = false;
            for (int i = 0; i < worldNetworkLists.Count; i++) {
                var network = worldNetworkLists[i];
                if (network.GetConnID() == connId) {
                    network.SetIsOnline(false);
                    worldNetworkLists.RemoveAt(i);
                    break;
                }
            }
            worldNetworkData.Remove(connId);
        }

        private void OnGetWorldNetworkCallBack(CM_Network.GetWorldNetworkBehaviour ent) {
            IClientWorldNetwork network;
            if (worldNetworkData.TryGetValue(ent.ConnID, out network)) {
                ent.CallBack.Invoke(network, ent.ProtoDoc);
            } else {
                ent.CallBack.Invoke(null, ent.ProtoDoc);
            }
        }
    }
}