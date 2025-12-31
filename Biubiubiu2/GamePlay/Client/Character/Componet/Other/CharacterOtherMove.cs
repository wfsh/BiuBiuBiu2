using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherMove : ClientCharacterComponent {
        private Vector3 lastPosition = Vector3.zero;
        private Vector3 targetPosition = Vector3.zero;
        private float lerpTime = 0.0f;
        private float pointLerpDuration = 0.2f; 
        private bool isDrive = false;
        private float pointLerpTime = 0.0f;
        private bool isFirstPoint = true;
        private bool isSetPointSync = false;
        private int pointDistance = 20; // 位置差 大于 多少不使用插值直接移动
        private bool isNetLerpPositionEnable = false;
        
        protected float delayEnableLerpPositionTime = 5f;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            RemoveProtoCallBack(Proto_Character.Rpc_MoveDir.ID, OnRpcMoveDirCallBack);
            RemoveProtoCallBack(Proto_Character.Rpc_FallToGrounded.ID, OnRpcFallToGroundedCallBack);
            RemoveProtoCallBack(Proto_Character.Rpc_CameraVRota.ID, OnRpcCameraVRotaCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Rpc_MoveDir.ID, OnRpcMoveDirCallBack);
            AddProtoCallBack(Proto_Character.Rpc_FallToGrounded.ID, OnRpcFallToGroundedCallBack);
            AddProtoCallBack(Proto_Character.Rpc_CameraVRota.ID, OnRpcCameraVRotaCallBack);
            ResetEnableLerpPositionState();
        }

        private void ResetEnableLerpPositionState() {
            delayEnableLerpPositionTime = 5;
            isNetLerpPositionEnable = false;
        }

        private void OnUpdate(float deltaTime) {
            UpdateEnableLerpPosition(deltaTime);
            if (isDrive == true || characterNetwork == null || characterNetwork.IsDestroy()) {
                return;
            }
            SetPoint(characterNetwork.GetPoint());
            Move();
            iEntity.SetRota(characterNetwork.GetRota());
        }
        
        
        private void UpdateEnableLerpPosition(float deltaTime) {
            if (WarReportData.IsStartWarReport()) {
                return;
            }
            delayEnableLerpPositionTime -= deltaTime;
            if (delayEnableLerpPositionTime > -5 && delayEnableLerpPositionTime <= 0) {
                delayEnableLerpPositionTime = -10;
                characterNetwork.SetInterpolatePositionState(true);
                isNetLerpPositionEnable = true;
            }
        }

        private void SetPoint(Vector3 point) {
            if (isNetLerpPositionEnable) {
                iEntity.SetPoint(point);
            } else {
                var distance = Vector3.Distance(iEntity.GetPoint(), point);
                if (isFirstPoint || distance > pointDistance) {
                    isFirstPoint = false;
                    iEntity.SetPoint(point);
                } else {
                    if (distance > pointLerpDuration) {
                        pointLerpTime = 0f;
                        isSetPointSync = true;
                    }
                }
                lastPosition = iEntity.GetPoint();
                targetPosition = point;
            }
        }

        private void Move() {
            if (isSetPointSync == false) {
                return;
            }
            pointLerpTime += Time.deltaTime;
            var t = Mathf.Clamp01(pointLerpTime / pointLerpDuration);
            var lerpPos = Vector3.Lerp(lastPosition, targetPosition, t);
            iEntity.SetPoint(lerpPos);
            if (t >= 1.0f) {
                isSetPointSync = false;
                lastPosition = targetPosition;
            }
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            isDrive = ent.DriveGPO != null;
        }

        private void OnRpcMoveDirCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Character.Rpc_MoveDir)rpcData;
            this.mySystem.Dispatcher(new CE_GPO.Event_MoveDir {
                MoveH = data.moveX * 0.01f, MoveV = data.moveY * 0.01f,
            });
        }

        private void OnRpcFallToGroundedCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            this.mySystem.Dispatcher(new CE_Character.FallToGrounded());
        }

        private void OnRpcCameraVRotaCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Character.Rpc_CameraVRota)rpcData;
            var vRota = data.vRota * 0.1f;
            this.mySystem.Dispatcher(new CE_Character.CameraRotaV {
                vRota = vRota
            });
        }
    }
}