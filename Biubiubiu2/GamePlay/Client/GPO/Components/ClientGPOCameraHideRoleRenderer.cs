using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOCameraHideRoleRenderer : ComponentBase {
        private IGPO lookGPO;
        private Vector3 farLocalPoint = Vector3.zero;
        private Vector3 cameraLocalPoint = Vector3.zero;
        private bool isShow = true;
        private List<Renderer> renderers;
        protected override void OnAwake() {
            base.OnAwake();
            renderers = new List<Renderer>();
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            mySystem.Register<CE_Camera.EndCameraLocalPoint>(OnEndCameraLocalPointCallBack);
            mySystem.Register<CE_Camera.EndFarPoint>(OnEndFarPointCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            renderers = null;
            lookGPO = null;
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            mySystem.Unregister<CE_Camera.EndCameraLocalPoint>(OnEndCameraLocalPointCallBack);
            mySystem.Unregister<CE_Camera.EndFarPoint>(OnEndFarPointCallBack);
        }

        private void OnEndCameraLocalPointCallBack(ISystemMsg body, CE_Camera.EndCameraLocalPoint ent) {
            cameraLocalPoint = ent.LocalPoint;
            UpdateMaterialHideRole();
        }

        private void OnEndFarPointCallBack(ISystemMsg body, CE_Camera.EndFarPoint ent) {
            farLocalPoint = ent.FarPoint;
            UpdateMaterialHideRole();
        }
        
        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            lookGPO = ent.LookGPO;
            UpdateMaterialHideRole();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entityBase = iEntity as EntityBase;
            entityBase.GetComponentsInChildren(renderers);

            for (int index = renderers.Count -1; index >= 0; index--) {
                var renderer = renderers[index];
                if (entityBase != renderer.GetComponentInParent<EntityBase>()) {
                    renderers.RemoveAtSwapBack(index);
                }
            }

            UpdateMaterialHideRole();
        }

        private void UpdateMaterialHideRole() {
            if (isSetEntityObj == false || lookGPO == null || renderers == null) {
                return;
            }
            var isShow = true;
            if (cameraLocalPoint != Vector3.zero && farLocalPoint != Vector3.zero && lookGPO != null && lookGPO == iGPO) {
                var distance = Mathf.Abs(farLocalPoint.z + cameraLocalPoint.z);
                isShow = distance > 2f;
            }
            if (this.isShow != isShow) {
                foreach (var renderer in renderers) {
                    if (renderer == null) {
                        continue;
                    }
                    renderer.enabled = isShow;
                }
                this.isShow = isShow;
            }
        }
    }
}