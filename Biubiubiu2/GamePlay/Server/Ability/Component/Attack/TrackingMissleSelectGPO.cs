using System;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class TrackingMissleSelectGPO : MovePointRaycastHit {
        public class MissleSelectGPOInitData : InitData {
            public AbilityData.PlayAbility_TrackingMissle AbilityData;
        }
        private IGPO fireGPO;
        private int fireATK = 0;
        private float fireRange = 0;
        private AbilityData.PlayAbility_TrackingMissle _abilityData; 
        private float delayBombTime = -10;
        private Vector3 hitPoint = Vector3.zero;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (MissleSelectGPOInitData)initDataBase;
            SetModData(initData.AbilityData);
        }
        protected override void OnStart() {
            base.OnStart();
            var abSystem = (S_Ability_Base)mySystem;
            SetLayerMask(LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            fireGPO = abSystem.FireGPO;
            fireGPO.Dispatcher(new SE_GPO.Event_GetATK {
                CallBack = OnFireAtkCallBack
            });
            fireGPO.Dispatcher(new SE_GPO.Event_GetAttackRange {
                CallBack = OnFireAttackRangeCallBack
            });
            AddUpdate(OnUpdate);
        }

        public void OnUpdate(float deltaTime) {
            if (delayBombTime < -5) {
                return;
            }
            if (delayBombTime > 0) {
                delayBombTime -= deltaTime;
            } else {
                delayBombTime = -10;
                var hurtValue = Mathf.CeilToInt(_abilityData.M_Power * fireATK * 0.01f);
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_Explosive.CreateForID(_abilityData.M_HitAbility),
                    InData = new AbilityIn_Explosive {
                        In_StartPoint = hitPoint,
                        In_Hurt = hurtValue,
                        In_WeaponId = _abilityData.In_WeaponItemId,
                        In_Range = fireRange,
                    }
                });
            }
        }

        private void OnFireAtkCallBack(int atk) {
            fireATK = atk;
        }

        private void OnFireAttackRangeCallBack(float range) {
            fireRange = range;
        }

        protected override void OnClear() {
            base.OnClear();
            fireGPO = null;
            RemoveUpdate(OnUpdate);
        }
        
        public void SetModData(AbilityData.PlayAbility_TrackingMissle data) {
            this._abilityData = data;
        }
        protected override void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            PlayHitAbility(hitRay.point);
        }

        private void PlayHitAbility(Vector3 hitPoint) {
            this.hitPoint = hitPoint;
            delayBombTime = 0.15f;
        }
    }
}