using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraMove : ComponentBase {
        private float speed = 20f;
        private float cameraHDelta = 0f;
        private IGPO lookGPO;
        private Transform lookTarget;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            UpdateRegister.AddLateUpdate(Update);
        }

        protected override void OnClear() {
            base.OnClear();
            UpdateRegister.RemoveLateUpdate(Update);
            MsgRegister.Unregister<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            ClearLookGPO();
        }

        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            ClearLookGPO();
            lookGPO = ent.LookGPO;
            lookGPO.Register<Event_SystemBase.SetEntityObj>(OnSetEntityObj);
            lookTarget = lookGPO.GetBodyTran(GPOData.PartEnum.CameraLook);
            if (lookTarget != null) {
                iEntity.SetPoint(lookTarget.position);
            } else {
                iEntity.SetPoint(lookGPO.GetPoint());
            }
        }
        
        private void ClearLookGPO() {
            if (lookGPO == null) {
                return;
            }
            lookGPO.Unregister<Event_SystemBase.SetEntityObj>(OnSetEntityObj);
            lookGPO = null;
        }
        
        private void OnSetEntityObj(ISystemMsg body, Event_SystemBase.SetEntityObj ent) {
            lookTarget = lookGPO.GetBodyTran(GPOData.PartEnum.CameraLook);
            if (lookTarget == null) {
                Debug.LogError("CameraMove OnSetEntityObj lookTarget is null");
            }
        }

        public void Update(float deltaTime) {
            if (lookTarget == null) {
                return;
            }
            var targetPoint = lookTarget.position;
            var distance = Vector3.Distance(iEntity.GetPoint(), targetPoint);
            if (distance <= 0) {
                return;
            }
            var useSpeed = speed + cameraHDelta;
            if (distance <= 10f && cameraHDelta <= 10f) {
                var speedRatio = distance <= 1f ? 1f : distance;
                iEntity.SetPoint(Vector3.MoveTowards(iEntity.GetPoint(), targetPoint,speedRatio * useSpeed * Time.deltaTime));
            } else {
                iEntity.SetPoint(targetPoint);
            }
        }

        private void SetDeltaCallBack(CM_Camera.SetDelta ent) {
            cameraHDelta = Mathf.Abs(ent.Delta.x);
        }
    }
}