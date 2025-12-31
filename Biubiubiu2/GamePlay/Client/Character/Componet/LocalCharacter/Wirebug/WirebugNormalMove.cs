using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class WirebugNormalMove : CharacterLocalWirebug.AbsWirebugMove {
        private const float lineDistance = 12f; // 线长度
        private WirebugLineMove MyWirebugLineMove;
        private bool isPlay = false;

        override protected void OnStart() {
            CanceWirebug();
            AddMoveType();
            myComponent.SetWirebugType(CharacterData.WirebugType.Start);
            UpdateRegister.AddInvoke(Drop, 0.2f);
        }

        protected override void OnClear() {
            base.OnClear();
            MyWirebugLineMove?.OnClear();
            MyWirebugLineMove = null;
        }

        private void AddMoveType() {
            MyWirebugLineMove = new WirebugLineMove();
            MyWirebugLineMove.SetCallBack(CanceWirebug, GetLineMovePoint);
        }

        override protected void OnUpdate() {
            if (isPlay == false) {
                return;
            }
            MyWirebugLineMove?.OnUpdate();
        }

        override protected void OnCanceWirebug() {
            if (isPlay == false) {
                return;
            }
            isPlay = false;
            MyWirebugLineMove.CancelMove();
            SetWirebugType(CharacterData.WirebugType.None);
            myComponent.Dispatcher(new CE_Character.Event_WirebugMove {
                WirebugMove = Vector3.zero
            });
        }

        private void Drop() {
            SetWirebugType(CharacterData.WirebugType.Drop);
            var bodyRota = BodyTran.localEulerAngles;
            var normalizedYAngle = NormalizeAngle(bodyRota.y);
            if (normalizedYAngle > -20f && normalizedYAngle < 40f) {
                MsgRegister.Dispatcher(new CM_Camera.GetCameraCenterObjPoint {
                    CallBack = GetCameraTargetPoint, FarDistance = lineDistance
                });
            } else {
                var forwardPoint = myComponent.GetTargetPoint(LHandTran.position, BodyTran.forward, lineDistance);
                GetCameraTargetPoint(forwardPoint, false);
            }
        }

        private float NormalizeAngle(float angle) {
            if (angle > 180f) {
                angle -= 360f;
            }
            return angle;
        }

        private void GetCameraTargetPoint(Vector3 point, bool isHit) {
            isPlay = true;
            ClearWirebugLine();
            CreateWirebug(point, OnTargetObjMoveEndCallBack);
            myComponent.Dispatcher(new CE_Character.Event_WirebugMoveTargetPoint {
                MoveTarget = point
            });
        }

        private void OnTargetObjMoveEndCallBack(Vector3 point) {
            MyWirebugLineMove.StartMove(LHandTran.position, point);
        }

        private void GetLineMovePoint(Vector3 displacement, float currentTime) {
            myComponent.Dispatcher(new CE_Character.Event_WirebugMove {
                WirebugMove = displacement
            });
            if (MyWirebugLine) {
                if (MyWirebugLine.LineDistance() <= 0.5f || currentTime >= 0.3f) {
                    SetWirebugType(CharacterData.WirebugType.Glide);
                    ClearWirebugLine();
                    myComponent.Dispatcher(new CE_Character.Event_WirebugMoveTargetPoint {
                        MoveTarget = Vector3.zero
                    });
                }
            }
        }
    }
}