using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeDropItem : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.Event_DropItem>(OnDeadDropItemCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Mode.Event_DropItem>(OnDeadDropItemCallBack);
        }
        
        public void OnDeadDropItemCallBack(SM_Mode.Event_DropItem ent) {
            var attackGPO = ent.AttackGPO;
            if (attackGPO == null) return;
            var killCount = 0;
            MsgRegister.Dispatcher(new SM_Mode.GetKillCount {
                GpoId = attackGPO.GetGpoID(),
                CallBack = count => {
                    killCount = count;
                }
            });
            var warLevelData = WarData.GetWarLevel(killCount);
            var warLevel = warLevelData.Level;
            var warLevelRatio = Random.Range(0, 100);
            if (warLevelRatio > warLevelData.WeaponDropRate) {
                return;
            }

            // 物品品质随机
            var qualityList = WarQualityDropRateSet.GetWarQualityDropRatesByWarLevel(warLevel);
            var isDropQuality = GetRandomDropQuality(qualityList, out var qualityData);
            if (isDropQuality == false) {
                Debug.LogError("获取掉落品质失败 战场等级：" + warLevel);
                return;
            }

            // 物品随机
            var weaponList = WarWeaponDropRateSet.GetWarWeaponDropRatesByWarLevelQuality(warLevel, qualityData.Quality);
            var isDropItem = GetRandomDropItem(weaponList, out var itemData);
            if (isDropItem == false) {
                Debug.LogError("获取掉落物品失败 战场等级：" + warLevel + " 品质：" + qualityData.Quality);
                return;
            }
            attackGPO.Dispatcher(new SE_Item.Event_OnDropItemData {
                ItemId = itemData.ItemId, DropGPO = ent.DropGpo,
            });
        }

        public bool GetRandomDropQuality(List<WarQualityDropRate> dropRates, out WarQualityDropRate data) {
            if (dropRates == null || dropRates.Count == 0) {
                data = new WarQualityDropRate();
                return false;
            }
            // 计算权重总和
            var totalWeight = 0;
            foreach (var item in dropRates) {
                totalWeight += item.DropRate;
            }
            if (totalWeight == 0) {
                data = new WarQualityDropRate();
                foreach (var item in dropRates) {
                    Debug.LogError("品质 ID：" + item.Id + "权重是 0");
                }
                return false;
            }
            var randomValue = Random.Range(0, totalWeight);
            var cumulativeWeight = 0;
            foreach (var item in dropRates) {
                cumulativeWeight += item.DropRate;
                if (randomValue < cumulativeWeight) {
                    data = item;
                    return true;
                }
            }
            data = new WarQualityDropRate();
            Debug.LogError("获取掉落品质失败 总权重：" + totalWeight + " 随机值：" + randomValue);
            return false;
        }

        public bool GetRandomDropItem(List<WarWeaponDropRate> dropRates, out WarWeaponDropRate data) {
            if (dropRates == null || dropRates.Count == 0) {
                data = new WarWeaponDropRate();
                return false;
            }
            // 计算权重总和
            var totalWeight = 0;
            foreach (var item in dropRates) {
                totalWeight += item.Rate;
            }
            if (totalWeight == 0) {
                data = new WarWeaponDropRate();
                foreach (var item in dropRates) {
                    Debug.LogError("物品ID：" + item.ItemId + "权重是 0");
                }
                return false;
            }
            var randomValue = Random.Range(0, totalWeight);
            // 遍历物品列表，查找掉落物品
            var cumulativeWeight = 0;
            foreach (var item in dropRates) {
                cumulativeWeight += item.Rate;
                if (randomValue < cumulativeWeight) {
                    data = item;
                    return true;
                }
            }
            data = new WarWeaponDropRate();
            Debug.LogError("获取掉落物品失败 总权重：" + totalWeight + " 随机值：" + randomValue);
            return false;
        }
    }
}