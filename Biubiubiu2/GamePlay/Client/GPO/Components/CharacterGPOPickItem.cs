using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterGPOPickItem : ComponentBase {
        protected List<ItemData.PickItemData> itemList = new List<ItemData.PickItemData>();
        protected List<ItemData.PickItemData> quickItemList = new List<ItemData.PickItemData>();

        protected override void OnAwake() {
            mySystem.Register<CE_Character.Event_GetItems>(OnGetItemsCallBack);
            mySystem.Register<CE_Character.Event_GetQuickItems>(OnGetQuickItemsCallBack);
            mySystem.Register<CE_Game.Event_GetSaveItemData>(OnGetSaveItemDataCallBack);
            mySystem.Register<CE_Character.Event_GetItemDataForItemId>(OnGetItemDataForItemIdCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Item.TargetRpc_PickItem.ID, OnTargetRpcPickItemCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_ItemNumChange.ID, OnTargetRpcItemNumChangeCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_RemoveItem.ID, OnTargetRpcRemoveItemCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_AddQuickPackItem.ID, OnTargetRpcAddQuickPackItemCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_RemoveQuickPackItem.ID, OnTargetRpcRemoveQuickPackItemCallBack);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
        }

        protected override void OnClear() {
            base.OnClear();
            itemList.Clear();
            mySystem.Unregister<CE_Character.Event_GetItems>(OnGetItemsCallBack);
            mySystem.Unregister<CE_Character.Event_GetQuickItems>(OnGetQuickItemsCallBack);
            mySystem.Unregister<CE_Character.Event_GetItemDataForItemId>(OnGetItemDataForItemIdCallBack);
            mySystem.Unregister<CE_Game.Event_GetSaveItemData>(OnGetSaveItemDataCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_PickItem.ID, OnTargetRpcPickItemCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_ItemNumChange.ID, OnTargetRpcItemNumChangeCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_RemoveItem.ID, OnTargetRpcRemoveItemCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_AddQuickPackItem.ID, OnTargetRpcAddQuickPackItemCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_RemoveQuickPackItem.ID, OnTargetRpcRemoveQuickPackItemCallBack);
        }

        private void OnGetItemsCallBack(ISystemMsg body, CE_Character.Event_GetItems ent) {
            ent.CallBack(itemList);
        }

        private void OnGetItemDataForItemIdCallBack(ISystemMsg body, CE_Character.Event_GetItemDataForItemId ent) {
            for (int i = 0; i < itemList.Count; i++) {
                var itemData = itemList[i];
                if (itemData.ItemId == ent.ItemId) {
                    ent.CallBack(itemData);
                    return;
                }
            }
        }

        private void OnGetQuickItemsCallBack(ISystemMsg body, CE_Character.Event_GetQuickItems ent) {
            ent.CallBack(quickItemList);
        }

        private void OnGetSaveItemDataCallBack(ISystemMsg body, CE_Game.Event_GetSaveItemData ent) {
            var list = new List<CE_Game.SaveItemData>();
            for (int i = 0; i < itemList.Count; i++) {
                var data = new CE_Game.SaveItemData();
                var item = itemList[i];
                data.ItemId = item.ItemId;
                data.ItemNum = item.ItemNum;
                data.IsQuickUse = HasQuickItem(item.AutoItemId);
                list.Add(data);
            }
            ent.CallBack(list);
        }

        private void OnTargetRpcAddQuickPackItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var proto = (Proto_Item.TargetRpc_AddQuickPackItem)protoDoc;
            var item = GetItem(proto.autoItemId);
            if (item == null) {
                return;
            }
            if (quickItemList.Count >= 3) {
                return;
            }
            quickItemList.Add(item);
            UpdateQuickItem();
        }

        private void OnTargetRpcRemoveQuickPackItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var proto = (Proto_Item.TargetRpc_RemoveQuickPackItem)protoDoc;
            for (int i = 0; i < quickItemList.Count; i++) {
                var item = quickItemList[i];
                if (item.AutoItemId == proto.autoItemId) {
                    quickItemList.RemoveAt(i);
                    UpdateQuickItem();
                    return;
                }
            }
        }

        private void CheckQuickItemNum(int autoItemId) {
            for (int i = 0; i < quickItemList.Count; i++) {
                var item = quickItemList[i];
                if (item.AutoItemId == autoItemId) {
                    UpdateQuickItem();
                    return;
                }
            }
        }

        private void UpdateQuickItem() {
            mySystem.Dispatcher(new CE_Character.Event_UpDateQuickItem {
                PickItemList = quickItemList,
            });
        }

        private void OnTargetRpcPickItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var proto = (Proto_Item.TargetRpc_PickItem)protoDoc;
            if (HasItem(proto.autoItemId)) {
                return;
            }
            var data = new ItemData.PickItemData();
            data.AutoItemId = proto.autoItemId;
            data.ItemId = proto.itemId;
            data.ItemNum = proto.itemNum;
            itemList.Add(data);
            UpdateItemList();
        }

        private void OnTargetRpcItemNumChangeCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var proto = (Proto_Item.TargetRpc_ItemNumChange)protoDoc;
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == proto.autoItemId) {
                    item.ItemNum = proto.itemNum;
                    UpdateItemList();
                    CheckQuickItemNum(proto.autoItemId);
                    return;
                }
            }
        }

        private void OnTargetRpcRemoveItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var proto = (Proto_Item.TargetRpc_RemoveItem)protoDoc;
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == proto.autoItemId) {
                    itemList.RemoveAt(i);
                    UpdateItemList();
                    return;
                }
            }
        }

        private void UpdateItemList() {
            mySystem.Dispatcher(new CE_Character.Event_UpDateItem() {
                PickItemList = itemList
            });
        }

        protected bool HasQuickItem(int autoItemId) {
            for (int i = 0; i < quickItemList.Count; i++) {
                var item = quickItemList[i];
                if (item.AutoItemId == autoItemId) {
                    return true;
                }
            }
            return false;
        }

        protected bool HasItem(int autoItemId) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == autoItemId) {
                    return true;
                }
            }
            return false;
        }

        private ItemData.PickItemData GetItem(int autoItemId) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == autoItemId) {
                    return item;
                }
            }
            return null;
        }
    }
}