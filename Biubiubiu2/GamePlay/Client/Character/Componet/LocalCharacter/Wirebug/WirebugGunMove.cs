using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class WirebugGunMove : CharacterLocalWirebug.AbsWirebugMove {
        private const float lineDistance = 20f; // 线长度
        private bool isCircle = false;
        private WirebugLineMove MyWirebugLineMove;
        private WirebugCircleMove MyWirebugCircleMove;
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
            MyWirebugCircleMove?.OnClear();
            MyWirebugCircleMove = null;
        }

        private void AddMoveType() {
            MyWirebugLineMove = new WirebugLineMove();
            MyWirebugLineMove.SetCallBack(CanceWirebug, GetLineMovePoint);
            MyWirebugCircleMove = new WirebugCircleMove();
            MyWirebugCircleMove.SetCallBack(CanceWirebug, GetCircleMovePoint);
        }

        override protected void OnUpdate() {
            if (isPlay == false) {
                return;
            }
            if (isCircle) {
                MyWirebugCircleMove?.OnUpdate();
            } else {
                MyWirebugLineMove?.OnUpdate();
            }
        }

        override protected void OnCanceWirebug() {
            if (isPlay == false) {
                return;
            }
            isPlay = false;
            MyWirebugCircleMove.CancelMove();
            MyWirebugLineMove.CancelMove();
            ClearWirebugLine();
            SetWirebugType(CharacterData.WirebugType.None);
            myComponent.Dispatcher(new CE_Character.Event_WirebugMove {
                WirebugMove = Vector3.zero
            });
            myComponent.Dispatcher(new CE_Character.Event_WirebugMoveTargetPoint {
                MoveTarget = Vector3.zero
            });
        }

        private void Drop() {
            SetWirebugType(CharacterData.WirebugType.Drop);
            MsgRegister.Dispatcher(new CM_Camera.GetCameraCenterObjPoint {
                CallBack = GetCameraTargetPoint, FarDistance = lineDistance
            });
        }

        private void GetCameraTargetPoint(Vector3 point, bool isHit) {
            ClearWirebugLine();
            CreateWirebug(point, OnTargetObjMoveEndCallBack);
            myComponent.Dispatcher(new CE_Character.Event_WirebugMoveTargetPoint {
                MoveTarget = point
            });
        }

        private void OnTargetObjMoveEndCallBack(Vector3 point) {
            isPlay = true;
            if (Mathf.Abs(moveDir.x) < 0.4f) {
                AddWirebugLineMove(point);
            } else {
                AddWirebugCircleMove(point);
            }
        }

        private void AddWirebugLineMove(Vector3 point) {
            isCircle = false;
            MyWirebugLineMove.StartMove(LHandTran.position, point);
        }

        private void AddWirebugCircleMove(Vector3 point) {
            isCircle = true;
            MyWirebugCircleMove.StartMove(LHandTran, lineDistance, moveDir, point);
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

        private void GetCircleMovePoint(Vector3 displacement) {
            myComponent.Dispatcher(new CE_Character.Event_WirebugMove {
                WirebugMove = displacement
            });
        }
    }
}