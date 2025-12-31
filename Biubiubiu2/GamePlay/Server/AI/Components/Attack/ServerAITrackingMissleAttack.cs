using System;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAITrackingMissleAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float IntervalTime;
        }
        private IGPO targetGPO;
        private Transform fireBox;
        private float attackReloadTime = 1.0f;
        private float countAttackReloadTime = 0.0f;
        private  bool isGetTarget = false;
        private AIEntity mAIEntity;
        private bool isFire = false;
        
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.IntervalTime);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            mAIEntity = (AIEntity)iEntity;
            fireBox = mAIEntity.AttackTran;
        }

        protected override void OnClear() {
            this.fireBox = null;
            RemoveUpdate(OnUpdate);
            base.OnClear();
        }

        public void SetData(float reloadTime) {
            this.attackReloadTime = reloadTime;
        }

        private void GetTarget() {
            if (isSetEntityObj == false || isGetTarget) {
                return;
            }

            targetGPO = null;//FindNearestGPOInFront();
            mySystem.Dispatcher(new SE_AI.Event_GetInsightTarget() {
                CallBack = target => {
                    targetGPO = target;
                }
            });
            if (targetGPO != null) {
                isGetTarget = true;
            } else {
                isFire = false;
                mySystem.Dispatcher(new SE_AI.Event_OnAIFire {
                    IsFire = false,
                    TargetGPO = null
                });
            }
        }
        
        private void OnUpdate(float delta) {
            if (countAttackReloadTime > 0) {
                countAttackReloadTime -= delta;
                return;
            }

            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            GetTarget();
            if (isGetTarget) {
                if (targetGPO.IsDead() == false) {
                    Fire();
                }
            }
            countAttackReloadTime = attackReloadTime;
        }

        private void Fire() {
            countAttackReloadTime = attackReloadTime;
            isGetTarget = false;
            isFire = true;
            mySystem.Dispatcher(new SE_AI.Event_OnAIFire {
                IsFire = true,
                TargetGPO = targetGPO
            });
            var targetPoint = GetTargetPoint();
            var firePoint = this.fireBox.position;
            var gunData = (WeaponData.GunData)WeaponData.Get(ItemSet.Id_Uav);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = iGPO,
                AbilityMData = new AbilityData.PlayAbility_TrackingMissle() {
                    ConfigId = AbilityConfig.UAVTrackingMissle,
                    In_StartPoint = firePoint,
                    In_TargetPoint = targetPoint,
                    In_TargetGPOId = targetGPO.GetGpoID(),
                    In_WeaponItemId = gunData.ItemId,
                }
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(AbilityM_PlayEffect.ID_UAVFire),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = firePoint,
                    In_StartRota = iGPO.GetRota(),
                }
            });
        }

        private Vector3 GetTargetPoint() {
            var targetBody = this.targetGPO.GetBodyTran(GPOData.PartEnum.Body);
            var point = this.targetGPO.GetPoint();
            if (targetBody == null) {
                point.y += 0.5f;
            } else {
                point = targetBody.position;
            }

            return point;
        }
    }
}