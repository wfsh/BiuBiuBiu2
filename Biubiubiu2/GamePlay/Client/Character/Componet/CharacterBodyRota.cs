using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterBodyRota : ComponentBase {
        private Transform bodyTransform;
        private Quaternion targetRota = Quaternion.identity;
        private bool isBodyChanged = false;
        private bool isLookForward = false;
        private bool isSlideMove = false;
        private bool isWirebugDrop = false;
        private bool isDrive = false;
        private float rotaSpeed = 10f;
        private CharacterData.WirebugType wirebugType = CharacterData.WirebugType.None;
        private Vector3 wirebugMoveTargetPoint = Vector3.zero;

        protected override void OnAwake() {
            this.mySystem.Register<CE_GPO.Event_MoveDir>(OnMoveCallBack);
            this.mySystem.Register<CE_Character.IsLookForward>(OnIsLookForwardCallBack);
            this.mySystem.Register<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Register<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateCallBack);
            this.mySystem.Register<CE_Character.Event_WirebugMoveTargetPoint>(OnWirebugMoveTargetPointCallBack);
            this.mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
        }
        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var entity = (CharacterEntity)iEntity;
            bodyTransform = entity.cBodyTran;
            CheckLookDirection();
            AddUpdate(OnUpdate);
        }
       
        protected override void OnClear() {
            base.OnClear();
            this.mySystem.Unregister<CE_GPO.Event_MoveDir>(OnMoveCallBack);
            this.mySystem.Unregister<CE_Character.IsLookForward>(OnIsLookForwardCallBack);
            this.mySystem.Unregister<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateCallBack);
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveTargetPoint>(OnWirebugMoveTargetPointCallBack);
            this.mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void OnMoveCallBack(ISystemMsg body, CE_GPO.Event_MoveDir entData) {
            if (Mathf.Abs(entData.MoveH) < 0.01f && Mathf.Abs(entData.MoveV) < 0.01f) {
                if (isLookForward) {
                    targetRota = Quaternion.identity;
                }
                return;
            }
            // 计算旋转角度
            var targetRotation = Mathf.Atan2(entData.MoveH, entData.MoveV) * Mathf.Rad2Deg;
            targetRota = Quaternion.Euler(0, targetRotation, 0);
            isBodyChanged = true;
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO entData) {
            isDrive = entData.DriveGPO != null;
            CheckLookDirection();
        }

        private void OnIsLookForwardCallBack(ISystemMsg body, CE_Character.IsLookForward entData) {
            isLookForward = entData.IsTrue;
            CheckLookDirection();
        }

        private void OnSlideMoveCallBack(ISystemMsg body, CE_Character.Event_SlideMove entData) {
            if (isSlideMove != entData.IsSlide) {
                isSlideMove = entData.IsSlide;
            }
        }
        
        private void OnWirebugMoveStateCallBack(ISystemMsg body, CE_Character.Event_WirebugMoveState entData) {
            wirebugType = entData.State;
            isWirebugDrop = wirebugType == CharacterData.WirebugType.Drop || wirebugType == CharacterData.WirebugType.Start;
            if (isWirebugDrop == false) {
                wirebugMoveTargetPoint = Vector3.zero;
            }
            CheckLookDirection();
        }

        private void OnWirebugMoveTargetPointCallBack(ISystemMsg body, CE_Character.Event_WirebugMoveTargetPoint entData) {
            wirebugMoveTargetPoint = entData.MoveTarget;
            CheckLookDirection();
        }
        
        private void CheckLookDirection() {
            if (bodyTransform == null) {
                return;
            }
            if (isLookForward || isDrive) {
                bodyTransform.localRotation = Quaternion.identity;
            }  else {
                CheckChangeMoveRota();
            }
        }

        private void OnUpdate(float deltaTime) {
            UpdateWirebugLook();
            UpateDefaultLook();
        }

        private void UpdateWirebugLook() {
            if (isWirebugDrop == false || isDrive ||wirebugMoveTargetPoint == Vector3.zero) {
                return;
            }
            wirebugMoveTargetPoint.y = bodyTransform.position.y;
            bodyTransform.rotation = Quaternion.LookRotation(wirebugMoveTargetPoint - bodyTransform.position);
        }

        private void UpateDefaultLook() {
            if (isSlideMove == false) {
                if (isLookForward == true || isDrive || isWirebugDrop || isBodyChanged == false) {
                    if (isLookForward) {
                        var forwrdAngle = Quaternion.Angle(bodyTransform.localRotation, Quaternion.identity);
                        if (forwrdAngle > 0.5f) {
                            bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, Quaternion.identity, rotaSpeed * Time.deltaTime);
                        }
                    }
                    return;
                }
            }
            // 将角色的本地旋转设置为目标旋转
            var nowRota = Quaternion.Lerp(bodyTransform.localRotation, targetRota, rotaSpeed * Time.deltaTime);
            bodyTransform.localRotation = nowRota;
            var angle = Quaternion.Angle(nowRota, targetRota);
            if (angle < 0.5f) {
                isBodyChanged = false;
                bodyTransform.localRotation = targetRota;
            } 
        }

        private void CheckChangeMoveRota() {
            var angle = Quaternion.Angle(bodyTransform.localRotation, targetRota);
            if (angle > 0.5f) {
                isBodyChanged = true;
            }
        }
    }
}