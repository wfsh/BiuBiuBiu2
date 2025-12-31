using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunAttribute : ComponentBase {
        private WeaponData.GunData gunData;
        private int atk;
        private int magazineNum;
        private float reloadTime;
        private float intervalTime;
        private float fireDistance;
        private float fireOverHotTime;
        
        protected override void OnAwake() {
            var system = (C_Weapon_Base)mySystem;
            gunData = (WeaponData.GunData)system.GetData();
            mySystem.Register<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            mySystem.Register<CE_Weapon.Event_GetGunMoveSpeed>(OnGetGunMoveSpeedCallBack);
            mySystem.Register<CE_Weapon.GetFireDistance>(OnGetFireDistanceCallBack);
            mySystem.Register<CE_Weapon.GetReloadTime>(OnGetReloadTimeCallBack);
            mySystem.Register<CE_Weapon.GetFireOverHotTimer>(OnGetFireOverHotTimerCallBack);
        }
        
        protected override void OnStart() {
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Weapon.SetWeaponAttribute>(OnSetWeaponAttributeCallBack);
            mySystem.Unregister<CE_Weapon.Event_GetGunMoveSpeed>(OnGetGunMoveSpeedCallBack);
            mySystem.Unregister<CE_Weapon.GetFireDistance>(OnGetFireDistanceCallBack);
            mySystem.Unregister<CE_Weapon.GetReloadTime>(OnGetReloadTimeCallBack);
            mySystem.Unregister<CE_Weapon.GetFireOverHotTimer>(OnGetFireOverHotTimerCallBack);
        }

        private void OnSetWeaponAttributeCallBack(ISystemMsg body, CE_Weapon.SetWeaponAttribute ent) {
            atk = ent.ATK;
            magazineNum = ent.MagazineNum;
            reloadTime = ent.ReloadTime;
            intervalTime = ent.IntervalTime;
            fireDistance = ent.FireDistance;
            fireOverHotTime = ent.FireOverHotTime;
        }

        private void OnGetGunMoveSpeedCallBack(ISystemMsg body, CE_Weapon.Event_GetGunMoveSpeed ent) {
            ent.CallBack.Invoke(gunData.MoveSpeed * 0.1f);
        }

        private void OnGetFireDistanceCallBack(ISystemMsg body, CE_Weapon.GetFireDistance ent) {
            ent.CallBack.Invoke(fireDistance);
        }

        private void OnGetReloadTimeCallBack(ISystemMsg body, CE_Weapon.GetReloadTime ent) {
            ent.CallBack.Invoke(reloadTime);
        }

        private void OnGetFireOverHotTimerCallBack(ISystemMsg body, CE_Weapon.GetFireOverHotTimer ent) {
            ent.CallBack.Invoke(fireOverHotTime);
        }
    }
}