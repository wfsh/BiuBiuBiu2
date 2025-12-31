using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOItemPack : ServerNetworkComponentBase {
        private int MaxPackGrid = 60; // 背包最大格子数量
        private int MaxQuickPackGrid = 3; // 快捷背包最大格子数量
        protected List<ItemData.OwnItemData> packItemList;
        protected List<ItemData.OwnItemData> quickPackItemList;
        private Dictionary<int, List<ItemData.OwnItemData>> packItemData;

        protected override void OnAwake() {
            mySystem.Register<SE_Item.Event_CanInItemPack>(OnCanInItemPackCallBack);
            mySystem.Register<SE_Item.Event_AddPickItem>(OnAddPickItemCallBack);
            mySystem.Register<SE_Item.Event_GetALLItemForType>(OnGetALLItemForTypeCallBack);
            mySystem.Register<SE_Item.Event_DownBullet>(OnDownBulletCallBack);
            mySystem.Register<SE_Item.Event_DropUnprotectedItems>(OnDropUnprotectedItemsCallBack);
            mySystem.Register<SE_Item.Event_GPOPickUpItem>(OnGPOPickUpItemCallBack);
            mySystem.Register<SE_Item.Event_OnPickItemForRange>(OnPickItemForRangeCallBack);
            mySystem.Register<SE_Item.Event_OnRemoveQuickPackItem>(OnRemoveQuickPackItemCallBack);
            mySystem.Register<SE_Item.Event_OnEquipItem>(OnEquipItemCallBack);
            mySystem.Register<SE_Item.Event_OnUseHoldBall>(OnUseHoldBallCallBack);
            mySystem.Register<SE_Item.Event_OnUseAbility>(OnUseAbilityCallBack);
            mySystem.Register<SE_Item.Event_GetPackItemList>(OnGetPackItemListCallBack);
            mySystem.Register<SE_Item.Event_GetQuickPackItemList>(OnGetQuickPackItemListCallBack);
        }
        
        protected void InitPack(int maxPackGrid, int maxQuickPackGrid) {
            MaxPackGrid = maxPackGrid;
            MaxQuickPackGrid = maxQuickPackGrid;
            packItemList = new List<ItemData.OwnItemData>(MaxPackGrid);
            quickPackItemList = new List<ItemData.OwnItemData>(MaxQuickPackGrid);
            packItemData = new Dictionary<int, List<ItemData.OwnItemData>>();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
        }

        protected override void OnClear() {
            packItemData.Clear();
            packItemList.Clear();
            packItemList = null;
            quickPackItemList.Clear();
            quickPackItemList = null;
            mySystem.Unregister<SE_Item.Event_DownBullet>(OnDownBulletCallBack);
            mySystem.Unregister<SE_Item.Event_CanInItemPack>(OnCanInItemPackCallBack);
            mySystem.Unregister<SE_Item.Event_GPOPickUpItem>(OnGPOPickUpItemCallBack);
            mySystem.Unregister<SE_Item.Event_AddPickItem>(OnAddPickItemCallBack);
            mySystem.Unregister<SE_Item.Event_GetALLItemForType>(OnGetALLItemForTypeCallBack);
            mySystem.Unregister<SE_Item.Event_DropUnprotectedItems>(OnDropUnprotectedItemsCallBack);
            mySystem.Unregister<SE_Item.Event_OnPickItemForRange>(OnPickItemForRangeCallBack);
            mySystem.Unregister<SE_Item.Event_OnRemoveQuickPackItem>(OnRemoveQuickPackItemCallBack);
            mySystem.Unregister<SE_Item.Event_OnEquipItem>(OnEquipItemCallBack);
            mySystem.Unregister<SE_Item.Event_OnUseHoldBall>(OnUseHoldBallCallBack);
            mySystem.Unregister<SE_Item.Event_OnUseAbility>(OnUseAbilityCallBack);
            mySystem.Unregister<SE_Item.Event_GetPackItemList>(OnGetPackItemListCallBack);
            mySystem.Unregister<SE_Item.Event_GetQuickPackItemList>(OnGetQuickPackItemListCallBack);
        }

        private void OnGetPackItemListCallBack(ISystemMsg body, SE_Item.Event_GetPackItemList ent) {
            ent.CallBack(packItemList);
        }

        private void OnGetQuickPackItemListCallBack(ISystemMsg body, SE_Item.Event_GetQuickPackItemList ent) {
            ent.CallBack(quickPackItemList);
        }
        
        private void OnDropUnprotectedItemsCallBack(ISystemMsg body, SE_Item.Event_DropUnprotectedItems ent) {
            DropUnprotectedItems(ent.IsDrop);
        }

        private void DropUnprotectedItems(bool isDrop) {
            var count = packItemList.Count - 1;
            for (int i = count; i >= 0; i--) {
                var item = packItemList[i];
                if (!item.IsProtect) {
                    RemoveItem(item);
                    RemoveQuickPackItem(item.AutoItemId);
                    MsgRegister.Dispatcher(new SM_Item.Event_DiscardItem {
                        AutoItemId = item.AutoItemId, PickGPOID = GpoID
                    });
                    if (isDrop) {
                        MsgRegister.Dispatcher(new SM_Item.Event_DropItem {
                            ItemId = item.ItemId, ItemNum = item.ItemNum, Point = iEntity.GetPoint()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 判断背包是否还有位置
        /// </summary>
        /// <param name="ent"></param>
        private void OnGetALLItemForTypeCallBack(ISystemMsg body, SE_Item.Event_GetALLItemForType ent) {
            List<ItemData.OwnItemData> typeList;
            if (packItemData.TryGetValue((int)ent.ItemType, out typeList)) {
                ent.CallBack(typeList);
            } else {
                ent.CallBack(null);
            }
        }

        /// <summary>
        /// 判断背包是否还有位置
        /// </summary>
        /// <param name="ent"></param>
        private void OnCanInItemPackCallBack(ISystemMsg body, SE_Item.Event_CanInItemPack ent) {
            ent.CallBack(packItemList.Count < MaxPackGrid);
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="ent"></param>
        private void OnAddPickItemCallBack(ISystemMsg body, SE_Item.Event_AddPickItem ent) {
            var modData = ItemData.GetData(ent.ItemId);
            var itemData = ItemData.GetData(ent.ItemId);
            var itemSign = itemData.Sign;
            if (ent.SkinItemId > 0) {
                var skinData = ItemData.GetData(ent.SkinItemId);
                itemSign = skinData.Sign;
            }
            MsgRegister.Dispatcher(new SM_Item.Event_AddOwnItem {
                ItemSign = itemSign,
                ItemNum = ent.ItemNum,
                OwnGPO = iGPO,
                IsProtect = ent.IsProtect,
                AddCallBack = data => {
                    if (ent.IsQuickUse && ItemData.CanPackUseType(modData.ItemTypeId)) {
                        EquipItem(data.AutoItemId);
                    }
                }
            });
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="ent"></param>
        private void OnOnRemoveItemForItemIdCallBack(ISystemMsg body, SE_Item.Event_AddPickItem ent) {
            var modData = ItemData.GetData(ent.ItemId);
            MsgRegister.Dispatcher(new SM_Item.Event_AddOwnItem {
                ItemSign = modData.Sign,
                ItemNum = ent.ItemNum,
                OwnGPO = iGPO,
                IsProtect = ent.IsProtect,
                AddCallBack = data => {
                    if (ent.IsQuickUse && ItemData.CanPackUseType(modData.ItemTypeId)) {
                        EquipItem(data.AutoItemId);
                    }
                }
            });
        }
        
        /// <summary>
        /// 获得物品回调
        /// </summary>
        /// <param name="ent"></param>
        private void OnGPOPickUpItemCallBack(ISystemMsg body, SE_Item.Event_GPOPickUpItem ent) {
            var pickItem = ent.Item;
            if (HasItem(pickItem.AutoItemId)) {
                Debug.LogError("无法存储相同 ID 的物品:" + pickItem.AutoItemId);
                return;
            }
            ItemInPack(pickItem);
        }

        /// <summary>
        /// 根据坐标拾取物品
        /// </summary>
        private void OnPickItemForRangeCallBack(ISystemMsg body, SE_Item.Event_OnPickItemForRange ent) {
            if (packItemList.Count >= MaxPackGrid) {
                Debug.LogError("背包物品数量达到上限，无法继续拾取");
                return;
            }
            MsgRegister.Dispatcher(new SM_Item.Event_PickUpItemForPoint {
                PickGpo = iGPO, Point = iEntity.GetPoint(), Range = 1.5f
            });
        }

        private void OnRemoveQuickPackItemCallBack(ISystemMsg body, SE_Item.Event_OnRemoveQuickPackItem ent) {
            RemoveQuickPackItem(ent.AutoItemId);
        }

        /// <summary>
        /// quickPackItemList 移除物品
        /// </summary>
        private void RemoveQuickPackItem(int autoItemId) {
            for (int i = quickPackItemList.Count - 1; i >= 0; i--) {
                var quickItem = quickPackItemList[i];
                if (quickItem.AutoItemId == autoItemId) {
                    quickPackItemList.RemoveAt(i);
                    mySystem.Dispatcher(new SE_Item.Event_RemoveQuickPackItem {
                        AutoItemId = quickItem.AutoItemId,
                    });
                }
            }
        }

        private bool HasQuickPackItem(int autoItemId) {
            for (int i = 0; i < quickPackItemList.Count; i++) {
                var quickItem = quickPackItemList[i];
                if (quickItem.AutoItemId == autoItemId) {
                    return true;
                }
            }
            return false;
        }

        private void OnEquipItemCallBack(ISystemMsg body, SE_Item.Event_OnEquipItem ent) {
            EquipItem(ent.AutoItemId);
        }

        /// <summary>
        /// 装备物品
        /// </summary>
        public void EquipItem(int autoItemId) {
            var item = GetItem(autoItemId);
            if (item == null) {
                return;
            }
            var modeData = ItemData.GetData(item.ItemId);
            if (ItemData.QuickUseItem(modeData.ItemTypeId)) {
                if (quickPackItemList.Count >= MaxQuickPackGrid || HasQuickPackItem(item.AutoItemId)) {
                    return;
                }
                // 物品装备到快捷栏
                quickPackItemList.Add(item);
                mySystem.Dispatcher(new SE_Item.Event_AddQuickPackItem {
                    AutoItemId = item.AutoItemId,
                });
            } else {
                var isUse = true;
                var isWeapon = false;
                if (modeData.ItemTypeId == ItemTypeSet.Id_WeaponSkin) {
                    var skinData = WeaponSkinSet.GetWeaponSkinBySkinItemId(item.ItemId);
                    var weaponData = ItemData.GetData(skinData.WeaponItemId);
                    isWeapon = weaponData.ItemTypeId == ItemTypeSet.Id_Weapon;
                } else if (modeData.ItemTypeId == ItemTypeSet.Id_Weapon) {
                    isWeapon = true;
                }
                if (isWeapon) {
                    // 判断武器背包是否可以存放
                    mySystem.Dispatcher(new SE_GPO.Event_CanInWeaponPack {
                        CallBack = isTrue => {
                            isUse = isTrue;
                        },
                    });
                }

                if (isUse) {
                    MsgRegister.Dispatcher(new SM_Item.Event_UseItem {
                        AutoItemId = autoItemId, UseGPO = iGPO, UseCallBack = UseItemSuccessCallBack,
                    });
                }
            }
        }

        private void OnUseHoldBallCallBack(ISystemMsg body, SE_Item.Event_OnUseHoldBall ent) {
            UseHoldBall(ent.AutoItemId, ent.Points);
        }

        /// <summary>
        /// 使用投掷物
        /// </summary>
        /// <param name="autoItemId"></param>
        /// <param name="points"></param>
        public void UseHoldBall(int autoItemId, Vector3[] points) {
            var item = GetItem(autoItemId);
            if (item == null) {
                Debug.LogError("没有该物品，无法投掷:" + autoItemId);
                return;
            }
            var modeData = ItemData.GetData(item.ItemId);
            if (modeData.ItemTypeId != ItemTypeSet.Id_HoldBall) {
                Debug.LogError("物品类型不是投掷类型:" + modeData.ItemTypeId);
                return;
            }
            MsgRegister.Dispatcher(new SM_Item.Event_UseItem {
                AutoItemId = autoItemId,
                UseGPO = iGPO,
                UseCallBack = itemData => {
                    UseHoldBallItemSuccessCallBack(itemData, points);
                },
            });
        }

        private void UseHoldBallItemSuccessCallBack(ItemData.OwnItemData item, Vector3[] points) {
            UpdateItemNum(item);
            var modeData = ItemData.GetData(item.ItemId);
            if (modeData.Id == ItemSet.Id_Grenade) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    OR_CallBack = null, 
                    FireGPO = iGPO, 
                    MData = AbilityM_Grenade.CreateForID(AbilityM_Grenade.ID_Grenade),
                    InData = new AbilityIn_Grenade {
                        In_Points = points,
                        In_WeaponItemId = item.ItemId,
                    },
                });
            }
        }

        private void OnUseAbilityCallBack(ISystemMsg body, SE_Item.Event_OnUseAbility ent) {
            UseAbility(ent.AutoItemId);
        }

        /// <summary>
        /// 使用直接释放类型物品
        /// </summary>
        /// <param name="autoItemId"></param>
        private void UseAbility(int autoItemId) {
            var item = GetItem(autoItemId);
            if (item == null) {
                Debug.LogError("没有该物品，无法拾取:" + autoItemId);
                return;
            }
            var modeData = ItemData.GetData(item.ItemId);
            if (modeData.ItemTypeId != ItemTypeSet.Id_UseAbility) {
                Debug.LogError("不是消耗类型物品不能走这边:" + modeData.Sign);
                return;
            }
            var canUse = CheckCanUseAbility(modeData.Id);
            if (canUse == false) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Item.Event_UseItem {
                AutoItemId = autoItemId, UseGPO = iGPO, UseCallBack = UseItemSuccessCallBack,
            });
        }

        private bool CheckCanUseAbility(int item) {
            var isTrue = true;
            if (item == ItemSet.Id_FirstAidKit) {
                mySystem.Dispatcher(new SE_GPO.Event_GetAttributeData {
                    CallBack = attr => {
                        isTrue = AbilityM_UpHPForFireRole.CheckCanUse(attr.nowHp, attr.maxHp);
                    }
                });
            }
            return isTrue;
        }

        private void UseItemSuccessCallBack(ItemData.OwnItemData item) {
            UpdateItemNum(item);
            var modeData = ItemData.GetData(item.ItemId);
            var itemType = modeData.ItemTypeId;
            if (itemType == ItemTypeSet.Id_WeaponSkin || itemType == ItemTypeSet.Id_Weapon) {
                var weaponItemId = item.ItemId;
                if (itemType == ItemTypeSet.Id_WeaponSkin) {
                    var skinData = WeaponSkinSet.GetWeaponSkinBySkinItemId(item.ItemId);
                    var weaponData = ItemData.GetData(skinData.WeaponItemId);
                    weaponItemId = weaponData.Id;
                    itemType = weaponData.ItemTypeId;
                }
                if (itemType == ItemTypeSet.Id_Weapon) {
                    mySystem.Dispatcher(new SE_GPO.AddWeaponPack {
                        AddWeaponId = weaponItemId,
                        AddWeaponSkinId = modeData.Id
                    });
                }
            } else if (modeData.ItemTypeId == ItemTypeSet.Id_UseAbility) {
                UseAbility(modeData);
            } else if (itemType == ItemTypeSet.Id_MeleeWeapon) {
                var weaponItemId = item.ItemId;
                mySystem.Dispatcher(new SE_GPO.AddWeaponPack {
                    AddWeaponId = weaponItemId,
                    AddWeaponSkinId = modeData.Id
                });
            }
        }

        private void UseAbility(Item modeData) {
            if (modeData.Id == ItemSet.Id_FirstAidKit) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_UpHPForFireRole.Create(),
                    InData = new AbilityIn_UpHPForFireRole {
                        In_UpHPValue = 1000
                    }
                });
            } else {
                Debug.LogError($"{modeData.Sign} 物品没有可以使用的能力");
            }
        }

        /// <summary>
        /// 使用子弹
        /// </summary>
        /// <param name="ent"></param>
        private void OnDownBulletCallBack(ISystemMsg body, SE_Item.Event_DownBullet ent) {
            List<ItemData.OwnItemData> typeList;
            if (packItemData.TryGetValue(ItemTypeSet.Id_Bullet, out typeList) == false) {
                return;
            }
            for (int i = 0; i < typeList.Count; i++) {
                var saveItem = typeList[i];
                var modeData = ItemData.GetData(saveItem.ItemId);
                if (modeData.Id == ent.UseBullet) {
                    if (saveItem.ItemNum > 0) {
                        var useBullNum = (ushort)Mathf.Min(ent.BulletNum, saveItem.ItemNum);
                        MsgRegister.Dispatcher(new SM_Item.Event_UseItem {
                            AutoItemId = saveItem.AutoItemId,
                            UseGPO = iGPO,
                            UseItemNum = useBullNum,
                            UseCallBack = item => {
                                ent.CallBack?.Invoke(useBullNum);
                                if (item.ItemNum > 0) {
                                    SetItemNum(item, item.ItemNum);
                                } else {
                                    RemoveItem(item.AutoItemId);
                                }
                            },
                        });
                        return;
                    }
                }
            }
            Debug.LogError($"没有 {ent.UseBullet} 子弹，无法使用");
        }

        private bool HasItem(int autoItemId) {
            for (int i = 0; i < packItemList.Count; i++) {
                var saveItem = packItemList[i];
                if (saveItem.AutoItemId == autoItemId) {
                    return true;
                }
            }
            return false;
        }

        private void ItemInPack(ItemData.OwnItemData pickItem) {
            var modData = ItemData.GetData(pickItem.ItemId);
            var lostNum = pickItem.ItemNum;
            for (int i = 0; i < packItemList.Count; i++) {
                var saveItem = packItemList[i];
                var packMaxNum = ItemData.MaxPackageNum(modData.ItemTypeId);
                if (saveItem.ItemId == pickItem.ItemId && saveItem.ItemNum < packMaxNum) {
                    var countItemNum = (ushort)(saveItem.ItemNum + lostNum);
                    if (countItemNum < packMaxNum) {
                        lostNum = 0;
                        SetItemNum(saveItem, countItemNum);
                        break;
                    } else {
                        lostNum = (ushort)(countItemNum - packMaxNum);
                        SetItemNum(saveItem, packMaxNum);
                    }
                }
            }
            if (lostNum > 0) {
                //   Debug.Log($"拾取物品: {pickItem.AutoItemId}  物品数量: {lostNum}");
                pickItem.ItemNum = lostNum;
                AddItem(pickItem);
                mySystem.Dispatcher(new SE_Item.Event_PickItemSuccess {
                    AutoItemId = pickItem.AutoItemId, ItemId = pickItem.ItemId, ItemNum = pickItem.ItemNum,
                });
                UpdateItem();
            } else {
                //    Debug.Log($"完全合并删除物品: {pickItem.AutoItemId}");
                MsgRegister.Dispatcher(new SM_Item.Event_DiscardItem {
                    AutoItemId = pickItem.AutoItemId, PickGPOID = GpoID
                });
            }
        }

        private void SetItemNum(ItemData.OwnItemData data, ushort newNum) {
            //  Debug.Log($"物品数量变化: {data.AutoItemId}  物品数量: {data.ItemNum} => {newNum}");
            data.ItemNum = newNum;
            mySystem.Dispatcher(new SE_Item.Event_ItemNumChange {
                AutoItemId = data.AutoItemId, ItemNum = data.ItemNum,
            });
            UpdateItem();
        }

        private void UpdateItem() {
            mySystem.Dispatcher(new SE_Item.Event_UpdateItems {
                ItemList = packItemList,
            });
        }

        private void RemoveItem(int autoItemId) {
            for (int i = 0; i < packItemList.Count; i++) {
                var item = packItemList[i];
                if (item.AutoItemId == autoItemId) {
                    RemoveItem(item);
                    RemoveQuickPackItem(autoItemId);
                    UpdateItem();
                    return;
                }
            }
        }

        private void AddItem(ItemData.OwnItemData pickItem) {
            var modData = ItemData.GetData(pickItem.ItemId);
            packItemList.Add(pickItem);
            List<ItemData.OwnItemData> typeList;
            if (packItemData.TryGetValue(modData.ItemTypeId, out typeList) == false) {
                typeList = new List<ItemData.OwnItemData>();
                packItemData.Add(modData.ItemTypeId, typeList);
            }
            typeList.Add(pickItem);
        }

        private void RemoveItem(ItemData.OwnItemData pickItem) {
            var modData = ItemData.GetData(pickItem.ItemId);
            packItemList.Remove(pickItem);
            List<ItemData.OwnItemData> typeList;
            if (packItemData.TryGetValue(modData.ItemTypeId, out typeList)) {
                typeList.Remove(pickItem);
            }
            mySystem.Dispatcher(new SE_Item.Event_RemoveItem {
                AutoItemId = pickItem.AutoItemId,
            });
        }

        private ItemData.OwnItemData GetItem(int autoItemId) {
            for (int i = 0; i < packItemList.Count; i++) {
                var item = packItemList[i];
                if (item.AutoItemId == autoItemId) {
                    return item;
                }
            }
            return null;
        }

        private void UpdateItemNum(ItemData.OwnItemData item) {
            if (item.ItemNum > 0) {
                SetItemNum(item, item.ItemNum);
            } else {
                RemoveItem(item.AutoItemId);
            }
        }
    }
}