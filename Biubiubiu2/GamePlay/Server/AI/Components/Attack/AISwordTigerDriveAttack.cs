using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AISwordTigerDriveAttack : ComponentBase {
        private Transform fireBox;
        private float attackReloadTime = 1.0f;
        private float countAttackReloadTime = 1.0f;
        private bool isDrive = false;

        protected override void OnAwake() {
            Register<SE_AI.Event_DriveState>(OnDriveIngCallBack);
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
            Unregister<SE_AI.Event_DriveState>(OnDriveIngCallBack);
            base.OnClear();
        }

        private void OnDriveIngCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            isDrive = ent.IsDrive;
        }

        private void OnUpdate(float delta) {
            if (isDrive == false) {
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
            var firePoint = this.fireBox.position;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO, 
                MData = AbilityM_BellowAttack.CreateForID(AbilityM_BellowAttack.ID_SwordTigerBellowAttack),
                InData = new AbilityIn_BellowAttack {
                    In_StartPoint = firePoint,
                },
            });
        }
    }
}