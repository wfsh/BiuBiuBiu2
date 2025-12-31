using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AIFocusGunAttack : ComponentBase {
        private IGPO target;
        private Transform fireBox;

        private float attackLimit = 0.2f;
        private float countAttackLimit = 0.2f;
        private float attackTime = 1.0f;
        private float countAttackTime = 1.0f;
        private float attackReloadTime = 3.0f;
        private float countAttackReloadTime = 0.0f;

        protected override void OnAwake() {
            Register<SE_Behaviour.Event_AttackTargetGPO>(OnAttackGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (AIEntity)iEntity;
            fireBox = entity.AttackTran;
        }

        protected override void OnClear() {
            this.fireBox = null;
            RemoveUpdate(OnUpdate);
            Unregister<SE_Behaviour.Event_AttackTargetGPO>(OnAttackGPOCallBack);
            base.OnClear();
        }

        private void OnUpdate(float delta) {
            if (iGPO.IsDead()) {
                return;
            }
            if (target == null || target.IsClear()) {
                target = null;
                return;
            }
            if (countAttackReloadTime > 0) {
                countAttackReloadTime -= delta;
                return;
            }
            
            // 持续射击射击，结束后进入冷却
            if (countAttackTime > 0) {
                countAttackTime -= delta;
            } else {
                countAttackReloadTime = attackReloadTime;
                countAttackTime = attackTime;
                countAttackLimit = 0;
            }

            // 射击间隔
            if (countAttackLimit > 0) {
                countAttackLimit -= delta;
            } else {
                countAttackLimit = attackLimit;
                Fire();
            }
        }

        private void Fire() {
            iGPO.Dispatcher(new SE_Behaviour.Event_StopMove());
            var targetPoint = GetTargetPoint();
            targetPoint.y += 0.5f;
            var firePoint = this.fireBox.position;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_Bullet.CreateForID(AbilityM_Bullet.ID_BulletGun),
                InData = new AbilityIn_Bullet {
                    In_StartPoint = firePoint,
                    In_TargetPoint = targetPoint,
                    In_Speed = 300,
                    In_MoveDistance = 100,
                    In_BulletAttnMap = new List<WeaponData.BulletAttnMap>(),
                    In_WeaponItemId = 0
                }
            });
        }
        
        private Vector3 GetTargetPoint() {
            var targetBody = this.target.GetBodyTran(GPOData.PartEnum.Body);
            var point = this.target.GetPoint();
            if (targetBody == null) {
                point.y += 0.5f;
            } else {
                point = targetBody.position;
            }
            return point;
        }

        private void OnAttackGPOCallBack(ISystemMsg body, SE_Behaviour.Event_AttackTargetGPO ent) {
            this.target = ent.TargetGPO;
        }
    }
}