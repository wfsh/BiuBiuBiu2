using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterItemPack : ServerGPOItemPack {
        private const int MaxPackGrid = 60; // 背包最大格子数量
        private const int MaxQuickPackGrid = 3; // 快捷背包最大格子数量
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Item.Event_RemoveItem>(OnRemoveItemCallBack);
            mySystem.Register<SE_Item.Event_RemoveQuickPackItem>(OnRemoveQuickPackItemCallBack);
            mySystem.Register<SE_Item.Event_AddQuickPackItem>(OnAddQuickPackItemCallBack);
            mySystem.Register<SE_Item.Event_PickItemSuccess>(OnPickItemSuccessCallBack);
            mySystem.Register<SE_Item.Event_ItemNumChange>(OnItemNumChangeCallBack);
            InitPack(MaxPackGrid, MaxQuickPackGrid);
        }
        
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Item.Cmd_PickItem.ID, OnCmdPickItemCallBack);
            AddProtoCallBack(Proto_Item.Cmd_EquipItem.ID, OnCmdEquipItemCallBack);
            AddProtoCallBack(Proto_Item.Cmd_UseAbilityItem.ID, OnCmdUseAbilityItemCallBack);
            AddProtoCallBack(Proto_Item.Cmd_RemoveQuickPackItem.ID, OnCmdRemoveQuickPackItemCallBack);
            AddProtoCallBack(Proto_Item.Cmd_UseHoldBallItem.ID, OnUseHoldBallCallBack);
            RpcALLItems();
            RpcALLQuickItems();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Item.Event_RemoveItem>(OnRemoveItemCallBack);
            mySystem.Unregister<SE_Item.Event_RemoveQuickPackItem>(OnRemoveQuickPackItemCallBack);
            mySystem.Unregister<SE_Item.Event_AddQuickPackItem>(OnAddQuickPackItemCallBack);
            mySystem.Unregister<SE_Item.Event_PickItemSuccess>(OnPickItemSuccessCallBack);
            mySystem.Unregister<SE_Item.Event_ItemNumChange>(OnItemNumChangeCallBack);
            RemoveProtoCallBack(Proto_Item.Cmd_PickItem.ID, OnCmdPickItemCallBack);
            RemoveProtoCallBack(Proto_Item.Cmd_UseHoldBallItem.ID, OnUseHoldBallCallBack);
            RemoveProtoCallBack(Proto_Item.Cmd_RemoveQuickPackItem.ID, OnCmdRemoveQuickPackItemCallBack);
            RemoveProtoCallBack(Proto_Item.Cmd_UseAbilityItem.ID, OnCmdUseAbilityItemCallBack);
            RemoveProtoCallBack(Proto_Item.Cmd_EquipItem.ID, OnCmdEquipItemCallBack);
        }

        private void RpcALLItems() {
            for (int i = 0; i < packItemList.Count; i++) {
                var item = packItemList[i];
                TargetRpc(networkBase, new Proto_Item.TargetRpc_PickItem {
                    autoItemId = item.AutoItemId, itemId = item.ItemId, itemNum = item.ItemNum,
                });
            }
        }

        private void RpcALLQuickItems() { 
            for (int i = 0; i < quickPackItemList.Count; i++) {
                var item = quickPackItemList[i];
                TargetRpc(networkBase, new Proto_Item.TargetRpc_AddQuickPackItem {
                    autoItemId = item.AutoItemId,
                });
            }
        }        
        
        private void OnRemoveItemCallBack(ISystemMsg body, SE_Item.Event_RemoveItem ent) {
            TargetRpc(networkBase, new Proto_Item.TargetRpc_RemoveItem {
                autoItemId = ent.AutoItemId,
            });
        }      
        
        private void OnRemoveQuickPackItemCallBack(ISystemMsg body, SE_Item.Event_RemoveQuickPackItem ent) {
            TargetRpc(networkBase, new Proto_Item.TargetRpc_RemoveQuickPackItem {
                autoItemId = ent.AutoItemId,
            });
        }      
        
        private void OnAddQuickPackItemCallBack(ISystemMsg body, SE_Item.Event_AddQuickPackItem ent) {
            TargetRpc(networkBase, new Proto_Item.TargetRpc_AddQuickPackItem {
                autoItemId = ent.AutoItemId,
            });
        }      
        
        private void OnPickItemSuccessCallBack(ISystemMsg body, SE_Item.Event_PickItemSuccess ent) {
            TargetRpc(networkBase, new Proto_Item.TargetRpc_PickItem {
                autoItemId = ent.AutoItemId, itemId = ent.ItemId, itemNum = (ushort)ent.ItemNum,
            });
        }      
        
        private void OnItemNumChangeCallBack(ISystemMsg body, SE_Item.Event_ItemNumChange ent) {
            TargetRpc(networkBase, new Proto_Item.TargetRpc_ItemNumChange() {
                autoItemId = ent.AutoItemId, itemNum = (ushort)ent.ItemNum,
            });
        }
        
        private void OnCmdPickItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            mySystem.Dispatcher(new SE_Item.Event_OnPickItemForRange());
        }

        private void OnCmdRemoveQuickPackItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var data = (Proto_Item.Cmd_RemoveQuickPackItem)protoDoc;
            mySystem.Dispatcher(new SE_Item.Event_OnRemoveQuickPackItem {
                AutoItemId = data.autoItemId
            });
        }

        private void OnCmdEquipItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var data = (Proto_Item.Cmd_EquipItem)protoDoc;
            mySystem.Dispatcher(new SE_Item.Event_OnEquipItem {
                AutoItemId = data.autoItemId
            });
        }
        
        public void OnUseHoldBallCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Item.Cmd_UseHoldBallItem)cmdData;
            mySystem.Dispatcher(new SE_Item.Event_OnUseHoldBall {
                AutoItemId = data.autoItemId,
                Points = data.points
            });
        }

        private void OnCmdUseAbilityItemCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var data = (Proto_Item.Cmd_UseAbilityItem)protoDoc;
            mySystem.Dispatcher(new SE_Item.Event_OnUseAbility {
                AutoItemId = data.autoItemId,
            });
        }
    }
}