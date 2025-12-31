using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageGunAttribute : ServerGunAttribute {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Weapon.Event_GetHitHeadMultiplier>(OnGetHitHeadMultiplierCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_GetHitHeadMultiplier>(OnGetHitHeadMultiplierCallBack);
        }

        private void OnGetHitHeadMultiplierCallBack(ISystemMsg body, SE_Weapon.Event_GetHitHeadMultiplier ent) {
            ent.CallBack.Invoke(GetHeadHurtRate());
        }

        private float GetHeadHurtRate() {
            if (gunData == null) {
                return 0;
            }
            return gunData.HitHeadMultiplier;
        }

        override protected int ATK() {
            if (gunData == null) {
                return 0;
            }
            return Mathf.FloorToInt(gunData.ATK * RandomAtk * 0.01f);
        }

        override protected float AttackRange() {
            if (gunData == null) {
                return 0;
            }
            return (gunData.AttackRange + RandomAttackRange) * 0.01f;
        }

        override protected int MagazineNum() {
            if (gunData == null) {
                return 0;
            }
            return gunData.MagazineNum + RandomMagazineNum;
        }

        override protected float FireDistance() {
            if (gunData == null) {
                return 0;
            }
            return gunData.FireDistance;
        }

        override protected float ReloadTime() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.ReloadTime + RandReloadTime;
        }

        override protected float IntervalTime() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.IntervalTime + RandIntervalTime;
        }

        override protected float FireOverHotTime() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.FireOverHotTime + RandFireOverHotTime;
        }

        override protected float FireRange() {
            if (gunData == null) {
                return 0f;
            }
            return gunData.FireRange - RandFireRange;
        }
    }
}