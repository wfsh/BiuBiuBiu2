using System;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSwordTigerAnim : ComponentBase {
        private const float runThresholdAngle = 20f; // 转向多少角度内开始跑步
        private int playAnim = AnimConfig_SwordTiger.Anim_idle;
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 moveDirection;
        private float moveAngle;
        private Vector3 cameraForward = Vector3.zero;
        private bool isEnabledDriveMoveIng = false;

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Register<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
            mySystem.Unregister<CE_AI.Event_MoveDir>(SetMoveDirCallBack);
        }

        private void OnUpdate(float delta) {
            if (isEnabledDriveMoveIng == false) {
                return;
            }
            GetCameraForward();
            UpdateDir();
        }
        private void OnEnabledDriveMoveCallBack(ISystemMsg body, CE_AI.Event_EnabledDriveMove ent) {
            isEnabledDriveMoveIng = ent.IsTrue;
            RemoveUpdate(OnUpdate);
            if (isEnabledDriveMoveIng) {
                AddUpdate(OnUpdate);
            }
        }

        private void SetMoveDirCallBack(ISystemMsg body, CE_AI.Event_MoveDir ent) {
            if (isEnabledDriveMoveIng == false) {
                return;
            }
            moveDir.x = ent.MoveX;
            moveDir.z = ent.MoveZ;
            if (moveDir.z < 0) {
                moveType = AIData.MoveType.BackMove;
            } else {
                moveType = Mathf.Abs(moveDir.z) + Mathf.Abs(moveDir.x) > 0.6f ? AIData.MoveType.Run : AIData.MoveType.Walk;
            }
            moveDirection = new Vector3(moveDir.x, 0, moveDir.z).normalized;
            moveAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        }

        private void GetCameraForward() {
            MsgRegister.Dispatcher(new CM_Camera.GetCameraForward {
                CallBack = (forward) => {
                    cameraForward = forward;
                }
            });
        }

        private void UpdateDir() {
            moveAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, moveAngle, 0) * cameraForwardRotation();
            var currentForward = iEntity.GetForward();
            var targetForward = targetRotation * Vector3.forward;
            var angleDiff = Vector3.SignedAngle(currentForward, targetForward, Vector3.up);
            if (moveDirection.magnitude > 0.1f) {
                if (angleDiff > 15f) {  // 右转
                    if (moveType == AIData.MoveType.BackMove) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_back_right);
                    } else {
                        PlayAnim(moveType == AIData.MoveType.Run ? AnimConfig_SwordTiger.Anim_run_right : AnimConfig_SwordTiger.Anim_walk_right);
                    }
                } else if (angleDiff < -15f) {  // 左转
                    if (moveType == AIData.MoveType.BackMove) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_back_left);
                    } else {
                        PlayAnim(moveType == AIData.MoveType.Run ? AnimConfig_SwordTiger.Anim_run_left : AnimConfig_SwordTiger.Anim_walk_left);
                    }
                } else {
                    if (moveType == AIData.MoveType.BackMove) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_back_forward);
                    } else {
                        PlayAnim(moveType == AIData.MoveType.Run ? AnimConfig_SwordTiger.Anim_run_forward : AnimConfig_SwordTiger.Anim_walk_forward);
                    }
                }
            } else {
                // 不在移动，播放 Idle 动画
                PlayAnim(AnimConfig_SwordTiger.Anim_idle);
            }
        }

        private Quaternion cameraForwardRotation() {
            return Quaternion.LookRotation(cameraForward, Vector3.up);
        }

        private void PlayAnim(int anim) {
            mySystem.Dispatcher(new CE_AI.Event_SetPlayClipId {
                ClipId = anim
            });
        }
    }
}