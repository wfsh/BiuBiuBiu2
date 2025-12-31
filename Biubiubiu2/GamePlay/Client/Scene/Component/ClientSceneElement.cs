using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSceneElement : ComponentBase {
        private List<ClientElementBox> itemList = new List<ClientElementBox>();

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Scene.Rpc_AddSceneElement.ID, OnRpcAddNaturalResourceCallBack);
            AddProtoCallBack(Proto_Scene.TargetRpc_AddSceneElement.ID,
                OnTargetRpcAddNaturalResourceCallBack);
            AddProtoCallBack(Proto_Scene.Rpc_RemoveSceneElement.ID, OnRpcRemoveNaturalResourceCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveAllItems();
            RemoveProtoCallBack(Proto_Scene.Rpc_AddSceneElement.ID, OnRpcAddNaturalResourceCallBack);
            RemoveProtoCallBack(Proto_Scene.TargetRpc_AddSceneElement.ID,
                OnTargetRpcAddNaturalResourceCallBack);
            RemoveProtoCallBack(Proto_Scene.Rpc_RemoveSceneElement.ID, OnRpcRemoveNaturalResourceCallBack);
            // MsgRegister.Unregister<CM_NaturalResource.GetResourceCountForCreateId>(OnGetResourceCountForCreateIdCallBack);
        }

        private void OnRpcAddNaturalResourceCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Scene.Rpc_AddSceneElement)protoDoc;
            AddItem(data.element, data.createID, data.point);
        }

        private void OnTargetRpcAddNaturalResourceCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Scene.TargetRpc_AddSceneElement)protoDoc;
            AddItem(data.element, data.createID, data.point);
        }

        private void AddItem(ushort resourceId, int createId, Vector3 point) {
            var box = new ClientElementBox();
            box.Init(resourceId, createId, point);
            itemList.Add(box);
        }

        private void OnRpcRemoveNaturalResourceCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Scene.Rpc_RemoveSceneElement)protoDoc;
            RemoveItem(data.createID);
        }

        private void RemoveItem(int createID) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.CreateId == createID) {
                    item.Clear();
                    itemList.RemoveAt(i);
                    return;
                }
            }
        }

        private void RemoveAllItems() {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                item.Clear();
            }
            itemList.Clear();
        }
    }
}