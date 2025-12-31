using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWorldNetworkBehaviourList : ComponentBase {
        private List<IServerWorldNetwork> worldNetworkLists = new List<IServerWorldNetwork>();
        private Dictionary<int, IServerWorldNetwork> worldNetworkData = new Dictionary<int, IServerWorldNetwork>();

        protected override void OnAwake() {
            MsgRegister.Register<SM_Network.AddWorldNetworkBehaviour>(OnAddWorldNetworkCallBack);
            MsgRegister.Register<SM_Network.RemoveWorldNetworkBehaviour>(OnRemoveWorldNetworkCallBack);
            MsgRegister.Register<SM_Network.GetWorldNetworkBehaviour>(OnGetWorldNetworkCallBack);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Network.AddWorldNetworkBehaviour>(OnAddWorldNetworkCallBack);
            MsgRegister.Unregister<SM_Network.RemoveWorldNetworkBehaviour>(OnRemoveWorldNetworkCallBack);
            MsgRegister.Unregister<SM_Network.GetWorldNetworkBehaviour>(OnGetWorldNetworkCallBack);
        }

        private void OnAddWorldNetworkCallBack(SM_Network.AddWorldNetworkBehaviour ent) {
            var network = ent.network;
            if (worldNetworkData.ContainsKey(network.GetConnID())) {
                Debug.LogError("GpoID 为唯一 ID，不能重复");
                return;
            }
            worldNetworkData.Add(network.GetConnID(), network);
            worldNetworkLists.Add(network);
        }

        private void OnRemoveWorldNetworkCallBack(SM_Network.RemoveWorldNetworkBehaviour ent) {
            RemoveWorldNetwork(ent.ConnId);
        } 

        private void RemoveWorldNetwork(int connId) {
            for (int i = 0; i < worldNetworkLists.Count; i++) {
                var network = worldNetworkLists[i];
                if (network.GetConnID() == connId) {
                    worldNetworkLists.RemoveAt(i);
                    break;
                }
            }
            worldNetworkData.Remove(connId);
        }

        private void OnGetWorldNetworkCallBack(SM_Network.GetWorldNetworkBehaviour ent) {
            IServerWorldNetwork network;
            if (worldNetworkData.TryGetValue(ent.ConnId, out network)) {
                ent.CallBack.Invoke(network);
            } else {
                ent.CallBack.Invoke(null);
            }
        }
    }
}