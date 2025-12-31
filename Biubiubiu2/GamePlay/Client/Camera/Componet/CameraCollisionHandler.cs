using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraCollisionHandler : ComponentBase {
        private const float SPHERE_RADIUS = 0.3f;

        private Transform cameraTransform;
        private IGPO lookGPO;
        private Transform lookTransform;
        private Transform farTransform;
        private RaycastHit[] raycastHit;
        private float minDistance = 0.5f; // 最小距离，避免摄像机太靠近角色
        private float smoothSpeed = 10f; // 平滑调整速度
        private bool isCameraMove = false;
        public float defaultNearClip = 0.3f; // Near Clipping Plane 默认值
        public float minNearClip = 0.05f; // Near Clipping Plane 最小值
        private Camera useCamera;
        private bool isLoginGameScene = false;
        private Vector3 nowLocalPoint = Vector3.zero;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            MsgRegister.Register<M_Game.LoginGameScene>(OnLoginGameSceneCompleted);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var cameraEntity = (CameraEntity)iEntity;
            cameraTransform = cameraEntity.UseCamera.transform;
            useCamera = cameraEntity.UseCamera;
            farTransform = cameraEntity.Far;
            raycastHit = new RaycastHit[10];
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Game.LoginGameScene>(OnLoginGameSceneCompleted);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            RemoveUpdate(OnUpdate);
            ClearLookGpo();
            useCamera = null;
            cameraTransform = null;
            lookTransform = null;
            farTransform = null;
            raycastHit = null;
        }

        private void OnLoginGameSceneCompleted(M_Game.LoginGameScene ent) {
            isLoginGameScene = true;
            AddUpdate(OnUpdate);
        }

        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            ClearLookGpo();
            lookGPO = ent.LookGPO;
            lookTransform = null;
            UpdateLookTran();
            if (lookTransform == null) {
                lookGPO.Register<Event_SystemBase.SetEntityObj>(OnSetLookGpoEntityObjCallBack);
            }
        }

        private void OnSetLookGpoEntityObjCallBack(ISystemMsg body, Event_SystemBase.SetEntityObj ent) {
            lookGPO.Unregister<Event_SystemBase.SetEntityObj>(OnSetLookGpoEntityObjCallBack);
            UpdateLookTran();
        }

        private void ClearLookGpo() {
            if (lookGPO == null) {
                return;
            }
            lookGPO.Unregister<Event_SystemBase.SetEntityObj>(OnSetLookGpoEntityObjCallBack);
            lookGPO = null;
        }

        private void UpdateLookTran() {
            if (lookTransform != null) {
                return;
            }
            lookTransform = lookGPO.GetBodyTran(GPOData.PartEnum.Head);
            if (lookTransform == null) {
                lookTransform = lookGPO.GetBodyTran(GPOData.PartEnum.Body);
            }
            if (lookTransform == null) {
                lookTransform = lookGPO.GetBodyTran(GPOData.PartEnum.RootBody);
            }
        }

        private void OnUpdate(float delta) {
            if (isLoginGameScene == false || lookTransform == null || farTransform == null) {
                return;
            }
            UpdateCameraLocalPoint();
            var localPoint = cameraTransform.localPosition;
            if ((nowLocalPoint - localPoint).sqrMagnitude > 0.1f) {
                nowLocalPoint = localPoint;
                lookGPO?.Dispatcher(new CE_Camera.EndCameraLocalPoint {
                    LocalPoint = cameraTransform.localPosition
                });
                
            }
        }

        private void UpdateCameraLocalPoint() {
            if (lookGPO.IsClear()) {
                return;
            }
            var lookPoint = lookGPO.GetPoint();
            lookPoint.y = lookTransform.position.y;
            var targetPoint = farTransform.position;
            var forward = (targetPoint - lookPoint).normalized;
            var distance = Vector3.Distance(targetPoint, lookPoint);
            var cameraDistance = Vector3.Distance(cameraTransform.position, lookPoint);
            if (cameraDistance > distance) {
                isCameraMove = false;
                cameraTransform.localPosition = Vector3.zero;
            } else {
                var count = Physics.SphereCastNonAlloc(lookPoint, SPHERE_RADIUS, forward, raycastHit, distance, ~LayerData.ServerLayerMask);
                var cameraPoint = Vector3.zero;
                if (count > 0) {
                    var hitDistance = GetHitPoint(count, raycastHit, distance);
                    if (hitDistance < distance) {
                        AdjustNearClipPlane(hitDistance, distance);
                        var currentDistance = Mathf.Clamp(hitDistance - SPHERE_RADIUS, minDistance, distance);
                        var point = lookPoint + forward * currentDistance;
#if UNITY_EDITOR
                        Debug.DrawLine(lookPoint, point, Color.red);
#endif
                        cameraTransform.position = point;
                        isCameraMove = true;
                        return;
                    }
                    cameraPoint = Vector3.zero;
                }
                if (isCameraMove) {
                    AdjustNearClipPlane(distance, distance);
                    cameraTransform.localPosition =
                        Vector3.Lerp(cameraTransform.localPosition, cameraPoint, Time.deltaTime * 5);
                    var resetDistance = Vector3.Distance(cameraTransform.localPosition, cameraPoint);
                    if (resetDistance < 0.01f) {
                        isCameraMove = false;
                        cameraTransform.localPosition = Vector3.zero;
                    }
                }
            }
        }

        void AdjustNearClipPlane(float currentDistance, float defaultDistance) {
            if (currentDistance < defaultDistance) {
                useCamera.nearClipPlane = minNearClip;
                useCamera.nearClipPlane = minNearClip;
            } else {
                useCamera.nearClipPlane = defaultNearClip;
            }
        }

        public float GetHitPoint(int count, RaycastHit[] list, float maxDistance) {
            var distance = maxDistance;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    if (hitType.MyEntity != null) {
                        continue;
                    }
                    if (hitType.MyEntity.GetGPO() != null) {
                        continue;
                    }
                }
                if (distance > ray.distance) {
                    distance = ray.distance;
                }
            }
            return distance;
        }
    }
}