using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterWeight : ClientCharacterComponent {
        private float itemWeight;
        private float weaponWeight;
        private float aerocraftWeight;

        private float countWeight {
            get { return itemWeight + weaponWeight + aerocraftWeight; }
        }

        protected override void OnAwake() {
            mySystem.Register<CE_Character.Event_UpDateItem>(OnUpdateItemCallBack);
            mySystem.Register<CE_Character.UpdateWeaponList>(OnUpdateWeaponListCallBack);
            mySystem.Register<CE_Character.UsePackAerocraft>(OnUsePackAerocraftCallBack);
            mySystem.Register<CE_Character.Event_GetWeight>(OnGetWeightCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Character.Event_UpDateItem>(OnUpdateItemCallBack);
            mySystem.Unregister<CE_Character.UpdateWeaponList>(OnUpdateWeaponListCallBack);
            mySystem.Unregister<CE_Character.UsePackAerocraft>(OnUsePackAerocraftCallBack);
            mySystem.Unregister<CE_Character.Event_GetWeight>(OnGetWeightCallBack);
        }

        private void OnUpdateItemCallBack(ISystemMsg body, CE_Character.Event_UpDateItem ent) {
            itemWeight = 0;
            for (int i = 0; i < ent.PickItemList.Count; i++) {
                var item = ent.PickItemList[i];
                var modeData = ItemData.GetData(item.ItemId);
                itemWeight += item.ItemNum * ItemData.GetWeight(modeData.ItemTypeId);
                UpdateWeight();
            }
        }

        private void OnUpdateWeaponListCallBack(ISystemMsg body, CE_Character.UpdateWeaponList ent) {
            weaponWeight = 0;
            for (int i = 0; i < ent.Weapons.Count; i++) {
                var item = ent.Weapons[i];
                var modeData = ItemData.GetData(item.GetWeaponSign());
                weaponWeight += ItemData.GetWeight(modeData.ItemTypeId);
                UpdateWeight();
            }
        }

        private void OnUsePackAerocraftCallBack(ISystemMsg body, CE_Character.UsePackAerocraft ent) {
            aerocraftWeight = 0;
            if (ent.useAerocraft == "") {
                return;
            }
            var modeData = ItemData.GetData(ent.useAerocraft);
            aerocraftWeight += ItemData.GetWeight(modeData.ItemTypeId);
            UpdateWeight();
        }

        private void OnGetWeightCallBack(ISystemMsg body, CE_Character.Event_GetWeight ent) {
            ent.CallBack(countWeight, CharacterData.MaxPackWeight);
        }

        private void UpdateWeight() {
            mySystem.Dispatcher(new CE_Character.Event_UpdateWeight {
                Weight = countWeight, MaxWeight = CharacterData.MaxPackWeight,
            });
        }
    }
}