using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_Weapon_Base : SystemBase, IWeapon {
        protected Transform parent;
        public WeaponData.Data modeData {
            get;
            private set;
        }
        public int weaponId {
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
        public string weaponSign {
            get;
            private set;
        }

        private IGPO useGPO;

        public IGPO UseGPO() {
            return useGPO;
        }

        public int GetWeaponId() {
            return weaponId;
        }

        public string GetWeaponSign() {
            return weaponSign;
        }


        public int GetWeaponItemId() {
            return weaponItemId;
        }
        public int GetWeaponSkinItemId() {
            return weaponSkinItemId;
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            useGPO = null;
        }
        public void SetWeaponData(int weaponId, int weaponItemId, int weaponSkinItemId) {
            this.weaponId = weaponId;
            this.weaponItemId = weaponItemId;
            this.weaponSkinItemId = weaponSkinItemId;
            this.modeData = WeaponData.Get(weaponItemId);
            this.weaponSign = modeData.ItemSign;
        }

        public void SetUseGPO(IGPO useGPO) {
            this.useGPO = useGPO;
        }

        public WeaponData.Data GetData() {
            return modeData;
        }


        public void SetParent(Transform parent) {
            this.parent = parent;
            iEntity.SetParent(parent);
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
        public WeaponData.WeaponType GetWeaponType() {
            return modeData.WeaponType;
        }
    }
}