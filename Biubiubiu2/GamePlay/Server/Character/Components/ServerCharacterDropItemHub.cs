using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterDropItemHub : ServerCharacterComponent {
        private ServerGPO fireGPO;
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        private int maxItemNum = 0;
        private ServerCharacterSystem characterSystem;
        private List<int> dropItemList = new List<int>();

        protected override void OnAwake() {
            mySystem.Register<SE_Item.Event_OnDropItemData>(OnGPOHurtCallBack);
            mySystem.Register<SE_Item.Event_GetDropItemList>(OnGetDropItemListBack);
            characterSystem = (ServerCharacterSystem)mySystem;
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Item.Event_OnDropItemData>(OnGPOHurtCallBack);
            mySystem.Unregister<SE_Item.Event_GetDropItemList>(OnGetDropItemListBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            TargetRpc(networkBase, new Proto_Item.TargetRpc_GetMaxDropItemNum {
                maxItemNum = maxItemNum,
            });
        }

        private void OnGetDropItemListBack(ISystemMsg body, SE_Item.Event_GetDropItemList ent) {
            ent.CallBack(dropItemList);
        }

        private void OnGPOHurtCallBack(ISystemMsg body, SE_Item.Event_OnDropItemData ent) {
            maxItemNum++;
            dropItemList.Add(ent.ItemId);
            TargetRpc(networkBase, new Proto_Item.TargetRpc_DropItemData {
                itemId = ent.ItemId, maxItemNum = maxItemNum, point = ent.DropGPO.GetPoint(),
            });
        }
    }
}