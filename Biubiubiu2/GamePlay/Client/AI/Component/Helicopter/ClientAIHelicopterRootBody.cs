using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIHelicopterRootBody : ComponentBase {
        private float targetPosY = 0f;
        private float targetRotaX = 0f;
        private Transform rootTran;
        private Vector3 moveDir = Vector3.zero;
        public float tiltAngle = 15f; // 最大倾斜角度
        public float tiltSpeed = 5f; // 倾斜恢复速度
        private bool isUpdateTargetPosY = false;
        private bool isUpdateTargetRotaX = false;
        private bool isMoveDir = false;
        private GPOM_Helicopter useMData;
        private C_AI_Base aiSystem;

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_MoveDir>(OnMoveCallBack);
            aiSystem = (C_AI_Base)mySystem;
            useMData = (GPOM_Helicopter)aiSystem.MData;
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new CE_GPO.Event_DeviceSkillCD {
                NowCD = 0f, MaxCD = 0f,
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_AI.Event_MoveDir>(OnMoveCallBack);
            RemoveProtoCallBack(Proto_AI.Rpc_SyncHelicopterRootPosY.ID, OnRpcSyncHelicopterRootPosY);
            RemoveProtoCallBack(Proto_AI.Rpc_SyncHelicopterRootRotaX.ID, OnRpcSyncHelicopterRootRotaX);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootTran = iEntity.GetBodyTran(GPOData.PartEnum.RootBody);
        }

        public void OnMoveCallBack(ISystemMsg body, CE_AI.Event_MoveDir ent) {
            moveDir.x = ent.MoveX;
            moveDir.z = ent.MoveZ;
            isMoveDir = true;
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_SyncHelicopterRootPosY.ID, OnRpcSyncHelicopterRootPosY);
            AddProtoCallBack(Proto_AI.Rpc_SyncHelicopterRootRotaX.ID, OnRpcSyncHelicopterRootRotaX);
        }

        private void OnRpcSyncHelicopterRootPosY(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_AI.Rpc_SyncHelicopterRootPosY)rpcData;
            targetPosY = data.py;
            isUpdateTargetPosY = true;
        }

        private void OnRpcSyncHelicopterRootRotaX(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_AI.Rpc_SyncHelicopterRootRotaX)rpcData;
            targetRotaX = data.rx;
            isUpdateTargetRotaX = true;
        }

        private void OnUpdate(float delta) {
            if (rootTran == null) {
                return;
            }
            if (isMoveDir) {
                UpdateHelicopterTilt(delta);
            }
            if (isUpdateTargetPosY) {
                UpdateTargetPosY();
            }
            if (isUpdateTargetRotaX) {
                UpdateTargetRotaX();
            }
        }

        private void UpdateTargetPosY() {
            var localPosition = rootTran.localPosition;
            if (Mathf.Abs(localPosition.y - targetPosY) > 0.001f) {
                localPosition.y = Mathf.Lerp(localPosition.y, targetPosY,
                    Time.deltaTime * useMData.HeightAdjustSpeed);
                rootTran.localPosition = localPosition;
            } else {
                isUpdateTargetPosY = false;
            }
        }

        private void UpdateTargetRotaX() {
            var localEulerAngles = rootTran.localEulerAngles;
            if (Mathf.Abs(localEulerAngles.x - targetRotaX) > 0.001f) {
                localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, targetRotaX, Time.deltaTime * 5f);
                rootTran.localEulerAngles = localEulerAngles;
            } else {
                isUpdateTargetRotaX = false;
            }
        }

        private void UpdateHelicopterTilt(float delta) {
            // 模拟倾斜角度
            var targetTilt = new Vector3(moveDir.z * tiltAngle, 0, -moveDir.x * tiltAngle);
            var targetRotation = Quaternion.Euler(targetTilt);
            rootTran.localRotation = Quaternion.Slerp(rootTran.localRotation, targetRotation, delta * tiltSpeed);
        }
    }
}