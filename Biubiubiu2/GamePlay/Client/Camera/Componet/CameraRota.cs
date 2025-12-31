using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraRota : ComponentBase {
        private CameraEntity cameraEntity;
        private Transform cameraTran;
        private Quaternion HRota = Quaternion.identity;
        private Quaternion VRota = Quaternion.identity;
        private float rotaSpeed = 0.5f;
        private IGPO lookGPO;
        private Transform driveTarget;
        private bool isDriveIng = false;
        private Vector3 targetPoint;            // 当前虚拟目标点
        private Vector3 velocity = Vector3.zero; // 用于平滑移动的速度
        public float targetMoveSpeed = 1.0f;    // 目标点平滑移动速度

        protected override void OnAwake() {
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            MsgRegister.Register<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Register<CM_Camera.GetCameraForward>(GetCameraForwardCallBack);
            MsgRegister.Register<CM_InputPlayer.CameraRatio>(GetCameraRatioCallBack);
            mySystem.Register<CE_Camera.SetLockTargetDelta>(OnSetLockTargetDelta);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            cameraEntity = (CameraEntity)iEntity;
            cameraTran = cameraEntity.UseCamera.transform;
            HRota = cameraEntity.HRota.localRotation;
            VRota = cameraEntity.VRota.localRotation;
            UpdateLookGPOState();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            MsgRegister.Unregister<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Unregister<CM_Camera.GetCameraForward>(GetCameraForwardCallBack);
            MsgRegister.Unregister<CM_InputPlayer.CameraRatio>(GetCameraRatioCallBack);
            mySystem.Unregister<CE_Camera.SetLockTargetDelta>(OnSetLockTargetDelta);
            cameraEntity = null;
            cameraTran = null;
        }
        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            ClearLookGPO();
            lookGPO = ent.LookGPO;
            lookGPO.Register<CE_GPO.ServerSetPoint>(OnServerSetPointCallBack);
            isDriveIng = ent.IsDrive;
            if (isDriveIng == false) {
                UpdateLookGPOState();
            }
        }

        private void UpdateLookGPOState() {
            if (lookGPO == null || cameraEntity == null) {
                return;
            }
            iEntity.SetRota(Quaternion.identity);
            HRota = Quaternion.Euler(0f, lookGPO.GetRota().eulerAngles.y, 0f);
            cameraEntity.HRota.localRotation = HRota;
        }
        
        private void ClearLookGPO() {
            if (lookGPO == null) {
                return;
            }
            lookGPO.Unregister<CE_GPO.ServerSetPoint>(OnServerSetPointCallBack);
            lookGPO = null;
        }

        private void OnServerSetPointCallBack(ISystemMsg body, CE_GPO.ServerSetPoint ent) {
            HRota = Quaternion.Euler(0f, lookGPO.GetRota().eulerAngles.y, 0f);
            if (cameraEntity != null) {
                cameraEntity.HRota.localRotation = HRota;
            }
        }

        private void UpdateRota() {
            DefaultAxis();
            if (cameraEntity != null) {
                cameraEntity.VRota.localRotation = VRota;
                cameraEntity.HRota.localRotation = HRota;
            }
            MsgRegister.Dispatcher(new CM_Camera.CameraHVRota {
                HRota = HRota, VRota = VRota
            });
        }

        private void GetCameraRatioCallBack(CM_InputPlayer.CameraRatio ent) {
            rotaSpeed = ent.speed;
        }

        private void GetCameraForwardCallBack(CM_Camera.GetCameraForward ent) {
            if (cameraEntity == null) {
                return;
            }
            ent.CallBack(cameraTran.forward);
        }

        private void SetDeltaCallBack(CM_Camera.SetDelta ent) {
            // if (isDriveIng) {
            //     return;
            // }
            HRota *= Quaternion.Euler(0f, ent.Delta.y * rotaSpeed, 0f);
            VRota *= Quaternion.Euler(ent.Delta.x * rotaSpeed, 0f, 0f);
            UpdateRota();
        }

        private void OnSetLockTargetDelta(ISystemMsg body, CE_Camera.SetLockTargetDelta ent) {
            // 计算目标水平和垂直旋转
            var targetHRotation = Quaternion.Euler(0, ent.Delta.y, 0);
            var targetVRotation = Quaternion.Euler(ent.Delta.x, 0, 0);
            // 使用插值平滑旋转
            HRota = targetHRotation;
            VRota = targetVRotation;
            UpdateRota();
        }

        private void DefaultAxis() {
            if (isDriveIng) {
                VRota = ClampVRotaAroundXAxis(VRota, -45f, 60f);
            } else {
                VRota = ClampVRotaAroundXAxis(VRota, -65f, 75f);
            }
        }

        Quaternion ClampVRotaAroundXAxis(Quaternion q, float min, float max) {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
            var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, min, max);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
            return q;
        }
        
        private void OnUpdate(float deltaTime) {
            // if (isDriveIng == false) {
            //     return;
            // }
            // var desiredTargetPoint = driveTarget.position;
            // if (desiredTargetPoint.y > iEntity.GetPoint().y) {
            //     desiredTargetPoint.y = iEntity.GetPoint().y;
            // }
            // Debug.DrawLine(desiredTargetPoint, iEntity.GetPoint(), Color.green);
            // // 使用阻尼算法（或插值算法）平滑移动目标点
            // targetPoint = Vector3.SmoothDamp(targetPoint, desiredTargetPoint, ref velocity, targetMoveSpeed * Time.deltaTime);
            // // // 将水平和垂直旋转应用到相应的组件
            // cameraEntity.LookAT(targetPoint);
            // var rota = cameraEntity.GetEulerAngles();
            // cameraEntity.VRota.localEulerAngles = new Vector3(rota.x, 0f, 0f);
            // cameraEntity.HRota.localEulerAngles = new Vector3(0f, rota.y, 0f);
            // HRota = cameraEntity.HRota.localRotation;
            // VRota = cameraEntity.VRota.localRotation;
            // cameraEntity.SetRota(Quaternion.identity);
        }
    }
}