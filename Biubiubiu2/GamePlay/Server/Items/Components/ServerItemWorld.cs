using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerItemWorld : ComponentBase {
        private Dictionary<int, ItemData.OwnItemData> autoItemData = new Dictionary<int, ItemData.OwnItemData>();
        private List<ItemData.OwnItemData> itemList = new List<ItemData.OwnItemData>();
        private int itemAutoIndex = 0;
        private int itemGroupIndex = 0;

        protected override void OnAwake() {
            MsgRegister.Register<SM_Item.Event_DropItem>(OnDropItemCallBack);
            MsgRegister.Register<SM_Item.Event_DiscardItem>(OnDiscardItemCallBack);
            MsgRegister.Register<SM_Item.Event_PickUpItem>(OnPickUpItemCallBack);
            MsgRegister.Register<SM_Item.Event_PickUpItemForPoint>(OnPickUpItemForPointCallBack);
            MsgRegister.Register<SM_Item.Event_AddOwnItem>(OnAddOwnItemCallBack);
            MsgRegister.Register<SM_Item.Event_UseItem>(OnUseItemCallBack);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLogin);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            autoItemData.Clear();
            itemList.Clear();
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLogin);
            MsgRegister.Unregister<SM_Item.Event_DropItem>(OnDropItemCallBack);
            MsgRegister.Unregister<SM_Item.Event_DiscardItem>(OnDiscardItemCallBack);
            MsgRegister.Unregister<SM_Item.Event_PickUpItem>(OnPickUpItemCallBack);
            MsgRegister.Unregister<SM_Item.Event_PickUpItemForPoint>(OnPickUpItemForPointCallBack);
            MsgRegister.Unregister<SM_Item.Event_AddOwnItem>(OnAddOwnItemCallBack);
            MsgRegister.Unregister<SM_Item.Event_UseItem>(OnUseItemCallBack);
        }

        private void OnCharacterLogin(SM_Character.CharacterLogin ent) {
            if (ent.INetwork == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            for (int i = 0; i < itemList.Count; i++) {
                var ownData = itemList[i];
                if (ownData.OwnGPOID != 0) {
                    continue;
                }
                TargetRpc(ent.INetwork, new Proto_Item.TargetRpc_AddDropItem() {
                    itemId = ownData.ItemId,
                    itemNum = ownData.ItemNum,
                    autoItemId = ownData.AutoItemId,
                    point = ownData.Point,
                });
            }
        }

        // private void OnMonsterDropItemCallBack(SM_Item.Event_MonsterDropItem ent) {
        //     var monsterItemDrop = ItemData.GetMonsterItemDropData(ent.MonsterModId);
        //     if (monsterItemDrop == null) {
        //         return;
        //     }
        //     itemGroupIndex++;
        //     for (int i = 0; i < monsterItemDrop.DropList.Length; i++) {
        //         AddDropItem(monsterItemDrop.DropList[i], ent.Point, itemGroupIndex);
        //     }
        // }

        private void OnDropItemCallBack(SM_Item.Event_DropItem ent) {
            AddItem(ent.ItemId, ent.ItemNum, ent.Point, 0);
        }

        private void OnDiscardItemCallBack(SM_Item.Event_DiscardItem ent) {
            DiscardItem(ent.PickGPOID, ent.AutoItemId);
        }

        private void OnPickUpItemCallBack(SM_Item.Event_PickUpItem ent) {
            PickUpItem((ServerGPO)ent.PickGpo, ent.AutoItemId);
        }

        private void OnAddOwnItemCallBack(SM_Item.Event_AddOwnItem ent) {
            var modData = ItemData.GetData(ent.ItemSign);
            var ownData = CreateItem(modData.Id, ent.ItemNum);
            ownData.OwnGPOID = ent.OwnGPO.GetGpoID();
            ownData.IsProtect = ent.IsProtect;
            ent.OwnGPO.Dispatcher(new SE_Item.Event_GPOPickUpItem {
                Item = ownData,
            });
            ent.AddCallBack?.Invoke(ownData);
        }

        /// <summary>
        /// 从背包中使用物品
        /// </summary>
        /// <param name="ent"></param>
        private void OnUseItemCallBack(SM_Item.Event_UseItem ent) {
            var item = GetItem(ent.AutoItemId);
            if (item == null) {
                Debug.LogError("没有该物品:" + ent.AutoItemId);
                return;
            }
            if (item.OwnGPOID != ent.UseGPO.GetGpoID()) {
                Debug.LogError($"不是物品的所有者 {item.OwnGPOID} != {ent.UseGPO.GetGpoID()}");
                return;
            }
            var useItemNum = (ushort)Mathf.Max(1, ent.UseItemNum);
            UseItem(item, useItemNum);
            ent.UseCallBack(item);
        }

        private void UseItem(ItemData.OwnItemData item, ushort itemNum) {
            item.ItemNum -= itemNum;
            if (item.ItemNum <= 0) {
                RemoveItem(item);
            }
        }

        private void OnPickUpItemForPointCallBack(SM_Item.Event_PickUpItemForPoint ent) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.OwnGPOID != 0) {
                    continue;
                }
                var distance = Vector3.Distance(item.Point, ent.Point);
                if (distance < ent.Range) {
                    PickUpItem((ServerGPO)ent.PickGpo, item.AutoItemId);
                    return;
                }
            }
        }

        /// <summary>
        /// 生成掉落物品
        /// </summary>
        private void AddItem(int itemId, ushort itemNum, Vector3 point, int groupId) {
            var ownData = CreateItem(itemId, itemNum);
            ownData.Point = GetRandomPointInCircle(point, 0.3f);
            ownData.ItemGroupId = groupId;
            Rpc(new Proto_Item.Rpc_AddDropItem {
                itemId = ownData.ItemId,
                itemNum = ownData.ItemNum,
                autoItemId = ownData.AutoItemId,
                point = ownData.Point,
            });
        }

        private ItemData.OwnItemData CreateItem(int itemId, ushort itemNum) {
            var modData = ItemData.GetData(itemId);
            var packMaxNum = ItemData.MaxPackageNum(modData.ItemTypeId);
            if (itemNum > packMaxNum) {
                itemNum = packMaxNum;
                Debug.LogError($"新增物品 {modData.Name} 的数量不能超过背包上线 {itemNum} > {packMaxNum}");
            }
            var ownData = new ItemData.OwnItemData();
            ownData.AutoItemId = ++itemAutoIndex;
            ownData.ItemId = itemId;
            ownData.CreateId = GetCreateId(modData);
            ownData.OwnGPOID = 0;
            ownData.ItemNum = itemNum;
            autoItemData.Add(ownData.AutoItemId, ownData);
            itemList.Add(ownData);
            return ownData;
        }

        public Vector3 GetRandomPointInCircle(Vector3 center, float radius) {
            var angle = Random.value * 360f;
            var radians = Mathf.Deg2Rad * angle;
            var x = radius * Mathf.Cos(radians);
            var z = radius * Mathf.Sin(radians);
            var randomPoint = new Vector3(x + center.x, center.y, z + center.z);
            return randomPoint;
        }

        /// <summary>
        /// 生成全局唯一 ID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ulong GetCreateId(Item data) {
            var title = data.Sign + TimeUtil.GetMillisecond() + Random.Range(0, 100000);
            var pid = xxHashUtil.ComputeHash64(title);
            return pid;
        }

        private void DiscardItem(int itemAutoId) {
            DiscardItem(0, itemAutoId);
        }

        /// <summary>
        /// 丢弃物品
        /// </summary>
        /// <param name="pickGpoId"></param>
        /// <param name="itemAutoId"></param>
        private void DiscardItem(int pickGpoId, int itemAutoId) {
            ItemData.OwnItemData item;
            if (autoItemData.TryGetValue(itemAutoId, out item)) {
                if (item.OwnGPOID != 0 && item.OwnGPOID != pickGpoId) {
                    return;
                }
                RemoveItem(item);
                if (pickGpoId == 0) {
                    Rpc(new Proto_Item.Rpc_DiscardItem() {
                        autoItemId = item.AutoItemId,
                    });
                }
            }
        }

        private void RemoveItem(ItemData.OwnItemData item) {
            if (autoItemData.ContainsKey(item.AutoItemId)) {
                RemovePickItemList(item.AutoItemId);
                autoItemData.Remove(item.AutoItemId);
            }
        }

        private void RemovePickItemList(int autoItemId) {
            for (int i = 0; i < itemList.Count; i++) {
                var item = itemList[i];
                if (item.AutoItemId == autoItemId) {
                    itemList.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 拾取物品
        /// </summary>
        /// <param name="pickGpoId"></param>
        /// <param name="itemAutoId"></param>
        /// <returns></returns>
        private void PickUpItem(ServerGPO pickGpo, int itemAutoId) {
            ItemData.OwnItemData data;
            if (autoItemData.TryGetValue(itemAutoId, out data)) {
                if (data.OwnGPOID != 0) {
                    return;
                }
                data.OwnGPOID = pickGpo.GetGpoID();
                autoItemData[itemAutoId] = data;
                Rpc(new Proto_Item.Rpc_PickUpItem {
                    autoItemId = itemAutoId,
                });
                pickGpo.Dispatcher(new SE_Item.Event_GPOPickUpItem {
                    Item = data,
                });
            }
        }

        private ItemData.OwnItemData GetItem(int itemAutoId) {
            ItemData.OwnItemData data;
            if (autoItemData.TryGetValue(itemAutoId, out data)) {
                return data;
            }
            return null;
        }
    }
}