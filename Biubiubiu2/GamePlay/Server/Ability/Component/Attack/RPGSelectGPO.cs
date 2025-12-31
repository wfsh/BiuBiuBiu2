using System;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class RPGSelectGPO : MovePointRaycastHit {
        public class InitData : MovePointRaycastHit.InitData {
            public AbilityM_Bullet AbilityMData;
        }
        
        private IGPO fireGPO;
        private int fireATK = 0;
        private float fireRange = 0;
        private AbilityM_Bullet useMData; 
        private AbilityIn_Bullet useInData; 

        protected override void OnAwake() {
            base.OnAwake();
            if (initDataBase != null) {
                var initData = (InitData)initDataBase;
                SetModData(initData.AbilityMData);
            }
        }
        protected override void OnStart() {
            base.OnStart();
            var abSystem = (S_Ability_Base)mySystem;
            useInData = (AbilityIn_Bullet)abSystem.InData;
            SetLayerMask(LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            fireGPO = abSystem.FireGPO;
            fireGPO.Dispatcher(new SE_GPO.Event_GetATK {
                CallBack = OnFireAtkCallBack
            });
            fireGPO.Dispatcher(new SE_GPO.Event_GetAttackRange {
                CallBack = OnFireAttackRangeCallBack
            });
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
        }
        
        public void SetModData(AbilityM_Bullet data) {
            this.useMData = data;
        }
        protected override void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            PlayHitAbility(hitRay.point);
        }

        private void PlayHitAbility(Vector3 hitPoint) {
            var hurtValue = Mathf.CeilToInt(useMData.M_Power * fireATK * 0.01f);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_Explosive.CreateForID(useMData.M_Explosive),
                InData = new AbilityIn_Explosive {
                    In_StartPoint = hitPoint,
                    In_Hurt = hurtValue,
                    In_WeaponId = useInData.In_WeaponItemId,
                    In_Range = fireRange,
                }
            });
        }
    }
}