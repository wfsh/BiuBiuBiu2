using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientItemWorld : ComponentBase {
        private List<ClientItemBox> itemList = new List<ClientItemBox>();

        protected override void OnAwake() {
            MsgRegister.Register<CM_Item.GetItemCountForPoint>(OnGetItemCountForPointCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Item.Rpc_AddDropItem.ID, OnRpcAddDropItemCallBack);
            AddProtoCallBack(Proto_Item.Rpc_DiscardItem.ID, OnRpcDiscardItemCallBack);
            AddProtoCallBack(Proto_Item.Rpc_PickUpItem.ID, OnRpcPickUpItemCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_AddDropItem.ID, OnTargetRpcAddDropItemItemCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveAllItems();
            MsgRegister.Unregister<CM_Item.GetItemCountForPoint>(OnGetItemCountForPointCallBack);
            RemoveProtoCallBack(Proto_Item.Rpc_AddDropItem.ID, OnRpcAddDropItemCallBack);
            RemoveProtoCallBack(Proto_Item.Rpc_DiscardItem.ID, OnRpcDiscardItemCallBack);
            RemoveProtoCallBack(Proto_Item.Rpc_PickUpItem.ID, OnRpcPickUpItemCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_AddDropItem.ID, OnTargetRpcAddDropItemItemCallBack);
        }

        private void OnRpcAddDropItemCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Item.Rpc_AddDropItem)protoDoc;
            AddItem(data.itemId, data.autoItemId, data.itemNum, data.point);
        }

        private void OnTargetRpcAddDropItemItemCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Item.TargetRpc_AddDropItem)protoDoc;
            AddItem(data.itemId, data.autoItemId, data.itemNum, data.point);
        }

        private void AddItem(int itemId, int autoItemId, ushort itemNum, Vector3 point) {
            var box = new ClientItemBox();
            box.Init(itemId, autoItemId, itemNum, point);
            itemList.Add(box);
        }

        private void OnRpcDiscardItemCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Item.Rpc_DiscardItem)protoDoc;
            RemoveItem(data.autoItemId);
        }

        private void OnRpcPickUpItemCallBack(INetwork iNetworkBase, IProto_Doc protoDoc) {
            var data = (Proto_Item.Rpc_PickUpItem)protoDoc;
            RemoveItem(data.autoItemId);
        }

        private void OnGetItemCountForPointCallBack(CM_Item.GetItemCountForPoint ent) {
            var count = GetPointRangeItemCount(ent.Point, ent.Range);
            ent.CallBack(count);
        }

        private void RemoveItem(int autoItemId) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == autoItemId) {
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

        private int GetPointRangeItemCount(Vector3 point, float range) {
            int count = 0;
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (Vector3.Distance(point, item.Point) < range) {
                    count++;
                }
            }
            return count;
        }
    }
}