using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWeaponAttribute : ComponentBase {
        protected WeaponData.Data weaponData;
        protected int RandomAtk = 0;
        protected float RandomAttackRange = 0f;
        protected float RandomDiffusionReduction = 0;
        protected float RandIntervalTime = 0;
        protected float nowIntervalTime = 0f;
        private IGPO useGPO;
        
        protected override void OnAwake() {
            base.OnAwake();
            var system = (S_Weapon_Base)mySystem;
            mySystem.Register<SE_Weapon.Event_SetRandomAttributeData>(OnSetRandomAttributeDataCallBack);
            mySystem.Register<SE_Weapon.Event_GetATK>(OnGetATKCallBack);
            mySystem.Register<SE_Weapon.Event_GetAttackRange>(OnGetAttackRangeCallBack);
            mySystem.Register<SE_Weapon.Event_GetRandomATK>(OnGetRandomATKCallBack);
            mySystem.Register<SE_Weapon.Event_GetRandomDiffusionReduction>(OnGetRandomDiffusionReductionCallBack);
            weaponData = system.GetData();
            useGPO = system.UseGPO();
            useGPO?.Register<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Weapon.Event_SetRandomAttributeData>(OnSetRandomAttributeDataCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetATK>(OnGetATKCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetAttackRange>(OnGetAttackRangeCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetRandomATK>(OnGetRandomATKCallBack);
            mySystem.Unregister<SE_Weapon.Event_GetRandomDiffusionReduction>(OnGetRandomDiffusionReductionCallBack);
            useGPO?.Unregister<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
        }

        protected void OnSetRandomAttributeDataCallBack(ISystemMsg body, SE_Weapon.Event_SetRandomAttributeData ent) {
            RandomAtk = ent.Data.RandAttack;
            RandomAttackRange = ent.Data.RandAttackRange;
            RandomDiffusionReduction = ent.Data.RandDiffusionReduction;
            RandIntervalTime = ent.Data.RandIntervalTime;
            UpdateAttribute();
            OnSetRandomAttributeData(ent.Data);
        }
        
        private void UpdateAttribute() {
            UpdateIntervalTime();
        }

        virtual protected void OnSetRandomAttributeData(SE_Mode.PlayModeCharacterWeapon data) {
        }
        
        virtual protected void OnUpdateEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_UpdateEffect env) {
        }

        private void OnGetATKCallBack(ISystemMsg body, SE_Weapon.Event_GetATK ent) {
            ent.CallBack.Invoke(ATK());
        }

        private void OnGetAttackRangeCallBack(ISystemMsg body, SE_Weapon.Event_GetAttackRange ent) {
            ent.CallBack.Invoke(AttackRange());
        }
        
        private void OnGetRandomATKCallBack(ISystemMsg body, SE_Weapon.Event_GetRandomATK ent) {
            ent.CallBack.Invoke(RandomAtk);
        }

        private void OnGetRandomDiffusionReductionCallBack(ISystemMsg body, SE_Weapon.Event_GetRandomDiffusionReduction ent) {
            ent.CallBack.Invoke(RandomDiffusionReduction);
        }

        protected void UpdateIntervalTime() {
            if (nowIntervalTime == IntervalTime()) {
                return;
            }
            nowIntervalTime = IntervalTime();
            mySystem.Dispatcher(new SE_Weapon.Event_UpdateIntervalTime {
                IntervalTime = nowIntervalTime
            });
        }

        virtual protected float IntervalTime() {
            if (weaponData == null) {
                return 0f;
            }
            return weaponData.IntervalTime + RandIntervalTime;
        }
        
        virtual protected int ATK() {
            if (weaponData == null) {
                return 0;
            }
            return weaponData.ATK + RandomAtk;
        }   
        
        virtual protected float AttackRange() {
            if (weaponData == null) {
                return 0;
            }
            return weaponData.AttackRange + RandomAttackRange;
        }   
    }
}