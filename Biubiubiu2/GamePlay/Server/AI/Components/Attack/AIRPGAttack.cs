using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AIRPGAttack : ComponentBase {
        private IGPO target;
        private Transform fireBox;
        private float attackReloadTime = 4.0f;
        private bool skillType = false;
        private float countAttackReloadTime = 0.0f;

        protected override void OnAwake() {
            Register<SE_Behaviour.Event_AttackTargetGPO>(OnAttackGPOCallBack);
            Register<SE_AI.Event_SkillType>(OnSkillTypCallBack);
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
            Unregister<SE_AI.Event_SkillType>(OnSkillTypCallBack);
            base.OnClear();
        }

        private void OnUpdate(float delta) {
            if (iGPO.IsDead()) {
                return;
            }
            if (target == null || target.IsClear() || this.skillType == true) {
                return;
            }
            if (countAttackReloadTime > 0) {
                countAttackReloadTime -= delta;
                return;
            }
            Fire();
            countAttackReloadTime = attackReloadTime;
        }

        private void Fire() {
            var targetPoint = GetTargetPoint();
            var firePoint = this.fireBox.position;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_Bullet.CreateForID(AbilityM_Bullet.ID_BulletRPG),
                InData = new AbilityIn_Bullet {
                    In_StartPoint = firePoint,
                    In_TargetPoint = targetPoint,
                    In_Speed = 100,
                    In_MoveDistance = 200,
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

        public void SetFireBox(Transform fireBox) {
            this.fireBox = fireBox;
        }

        private void OnAttackGPOCallBack(ISystemMsg body, SE_Behaviour.Event_AttackTargetGPO ent) {
            this.target = ent.TargetGPO;
        }

        private void OnSkillTypCallBack(ISystemMsg body, SE_AI.Event_SkillType ent) {
            this.skillType = ent.IsSkillType;
        }
    }
}