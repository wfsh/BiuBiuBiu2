using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGunAttribute : ServerWeaponAttribute {
        protected WeaponData.GunData gunData;
        protected int RandomMagazineNum = 0;
        protected int nowMagazineNum = 0;
        protected float RandFireRange = 0;
        protected float RandReloadTime = 0;
        protected float RandFireOverHotTime = 0;
        protected float RandomFireDistance = 0;
        protected float nowReloadTime = 0f;
        protected float nowFireOverHotTime = 0f;
        protected float nowFireRange = 0f;
        protected float nowFireDistance = 0;
        protected float RandAddEffectFireDistanceRate = 0;
        private float ability_ReloadRatio = 0;
        private float ability_ShootIntervalRatio = 0;
        private IGPO useGPO;
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Weapon.Event_GetAddEffectFireDistanceRate>(OnGetEffectFireDistanceCallBack);
            mySystem.Register<SE_Weapon.Event_GetMagazineNum>(OnGetMagazineNumCallBack);
            mySystem.Register<SE_Weapon.Event_GetGunAttribute>(OnGetGunAttributeCallBack);
            mySystem.Register<SE_Weapon.Event_GetGunMoveSpeed>(OnGetGunMoveSpeedCallBack);
            gunData = (WeaponData.GunData)weaponData;
        }
        
        protected override void OnStart() {
            UpdateGunAttribute();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_GetAddEffectFireDistanceRate>(OnGetEffectFireDistanceCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetMagazineNum>(OnGetMagazineNumCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetGunAttribute>(OnGetGunAttributeCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetGunMoveSpeed>(OnGetGunMoveSpeedCallBack);
        }

        override protected void OnSetRandomAttributeData(SE_Mode.PlayModeCharacterWeapon data) {
            base.OnSetRandomAttributeData(data);
            RandomMagazineNum = data.RandMagazineNum;
            RandFireRange = data.RandFireRange;
            RandReloadTime = data.RandReloadTime;
            RandAddEffectFireDistanceRate = data.RandAddEffectFireDistanceRate;
            RandFireOverHotTime = data.RandFireOverHotTime;
            RandomFireDistance = data.RandFireDistance;
            UpdateGunAttribute();
        }
        
        private void UpdateGunAttribute() {
            UpdateMagazineNum();
            UpdateReloadTime();
            UpdateIntervalTime();
            UpdateFireRange();
            UpdateFireDistance();
            UpdateFireOverHotTime();
        }
        
        override protected void OnUpdateEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_UpdateEffect env) {
            base.OnUpdateEffectCallBack(body, env);
            switch (env.Effect) {
                case AbilityEffectData.Effect.GpoReloadRate:
                    ability_ReloadRatio = env.Value;
                    UpdateReloadTime();
                    break;
                case AbilityEffectData.Effect.GpoShootIntervalRate:
                    ability_ShootIntervalRatio = env.Value;
                    UpdateIntervalTime();
                    break;
            }
        }

        private void UpdateMagazineNum() {
            if (nowMagazineNum == MagazineNum()) {
                return;
            }
            nowMagazineNum = MagazineNum();
            mySystem.Dispatcher(new SE_Weapon.Event_UpdateMagazineNum {
                MagazineNum = MagazineNum()
            });
        }

        private void UpdateReloadTime() {
            if (nowReloadTime == ReloadTime()) {
                return;
            }
            nowReloadTime = ReloadTime();
            mySystem.Dispatcher(new SE_Weapon.Event_UpdateReloadTime {
                ReloadTime = nowReloadTime
            });
        }

        private void UpdateFireOverHotTime() {
            if (nowFireOverHotTime == FireOverHotTime()) {
                return;
            }
            nowFireOverHotTime = FireOverHotTime();
            mySystem.Dispatcher(new SE_Weapon.Event_FireOverHotTime {
                FireOverHotTime = nowFireOverHotTime
            });
        }

        private void UpdateFireRange() {
            if (nowFireRange == FireRange()) {
                return;
            }
            nowFireRange = FireRange();
            mySystem.Dispatcher(new SE_Weapon.Event_UpdateFireRange {
                FireRange = nowFireRange
            });
        }

        private void UpdateFireDistance() {
            if (nowFireDistance == FireDistance()) {
                return;
            }
            nowFireDistance = FireDistance();
            mySystem.Dispatcher(new SE_Weapon.Event_UpdateFireDistance {
                FireDistance = nowFireDistance
            });
        }
        
        private void OnGetGunAttributeCallBack(ISystemMsg body, SE_Weapon.Event_GetGunAttribute ent) {
            ent.CallBack.Invoke(ATK(), MagazineNum(), IntervalTime(), FireRange(), ReloadTime(), FireDistance(), FireOverHotTime());
        }

        private void OnGetEffectFireDistanceCallBack(ISystemMsg body, SE_Weapon.Event_GetAddEffectFireDistanceRate ent) {
            ent.CallBack.Invoke(RandAddEffectFireDistanceRate);
        }
        
        private void OnGetMagazineNumCallBack(ISystemMsg body, SE_Weapon.Event_GetMagazineNum ent) {
            ent.CallBack.Invoke(MagazineNum());
        }

        private void OnGetGunMoveSpeedCallBack(ISystemMsg body, SE_Weapon.Event_GetGunMoveSpeed ent) {
            ent.CallBack.Invoke(gunData.MoveSpeed * 0.1f);
        }

        virtual protected int MagazineNum() {
            if (gunData == null) {
                return 0;
            }
            return gunData.MagazineNum + RandomMagazineNum;
        }

        virtual protected float FireDistance() {
            if (gunData == null) {
                return 0;
            }
            return gunData.FireDistance + RandomFireDistance;
        }

        virtual protected float ReloadTime() {
            if (gunData == null) {
                return 0f;
            }
            return ((gunData.ReloadTime + RandReloadTime) * (1 - ability_ReloadRatio)) * 0.001f;
        }

        override protected float IntervalTime() {
            if (gunData == null) {
                return 0f;
            }
            return (gunData.IntervalTime + RandIntervalTime) * (1 - ability_ShootIntervalRatio) * 0.001f;
        }

        virtual protected float FireOverHotTime() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.FireOverHotTime + RandFireOverHotTime;
        }

        virtual protected float FireRange() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.FireRange - RandFireRange;
        }
    }
}