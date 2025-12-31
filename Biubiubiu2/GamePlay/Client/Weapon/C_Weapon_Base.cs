﻿using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class C_Weapon_Base : SystemBase, IWeapon {
        protected Transform parent;

        public WeaponData.Data ModeData {
            get;
            private set;
        }
        public int weaponId {
            get;
            private set;
        }
        public string weaponSign {
            get;
            private set;
        }

        public string weaponSkinSign {
            get;
            private set;
        }
        public int weaponItemId {
            get;
            private set;
        }
        public int weaponSkinItemId {
            get;
            private set;
        }
        public ClientGPO useGPO;

        protected override void OnClear() {
            base.OnClear();
            this.parent = null;
        }

        public void SetWeaponData(int weaponId, int itemId, int skinItemId) {
            this.weaponId = weaponId;
            this.weaponItemId = itemId;
            this.weaponSkinItemId = skinItemId;
            ModeData = WeaponData.Get(itemId);
            this.weaponSign = ModeData.ItemSign;
            weaponSkinSign = skinItemId == 0 ? weaponSign : ItemData.GetData(skinItemId).ResSign;
        }

        public void SetUseGPO(IGPO useGPO) {
            this.useGPO = (ClientGPO)useGPO;
        }

        public void SetParent(Transform parent) {
            this.parent = parent;
            if (iEntity is EntityBase) {
                var entity = (EntityBase)iEntity;
                entity.SetParent(parent);
            }
        }

        protected void CreateEntity(string sign) {
            CreateEntityObj(string.Concat("Weapon/", sign), StageData.GameWorldLayerType.Item);
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            if (iEnter == null) {
                Debug.LogError("WeaponSystem 实体没有加载:" + this.weaponSign);
                return;
            }
            var entity = (EntityBase)iEntity;
            entity.SetParent(parent);
        }

        public WeaponData.Data GetData() {
            return ModeData;
        }

        public IGPO UseGPO() {
            return useGPO;
        }

        public int GetWeaponId() {
            return weaponId;
        }

        public int GetWeaponItemId() {
            return this.weaponItemId;
        }

        public int GetWeaponSkinItemId() {
            return weaponSkinItemId;
        }

        public string GetWeaponSign() {
            return weaponSign;
        }

        public WeaponData.WeaponType GetWeaponType() {
            return ModeData.WeaponType;
        }
    }
}
