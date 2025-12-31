using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class GetCameraCenterGPO : ComponentBase {
        private CameraEntity cameraEntity;
        private RaycastHit[] raycastHit;
        private Transform cameraTran;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Camera.GetCameraCenterObjPoint>(OnGetCameraCenterObjPointCallBack);
            MsgRegister.Register<CM_Camera.GetScreenCenterGPO>(OnGetScreenCenterGPOCallBack);
            MsgRegister.Register<CM_Camera.GetScreenCenterBarrier>(OnGetScreenCenterBarrier);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            cameraEntity = (CameraEntity)iEntity;
            raycastHit = new RaycastHit[10];
            cameraTran = cameraEntity.UseCamera.transform;
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_Camera.GetCameraCenterObjPoint>(OnGetCameraCenterObjPointCallBack);
            MsgRegister.Unregister<CM_Camera.GetScreenCenterGPO>(OnGetScreenCenterGPOCallBack);
            MsgRegister.Unregister<CM_Camera.GetScreenCenterBarrier>(OnGetScreenCenterBarrier);
            raycastHit = null;
            cameraEntity = null;
            cameraTran = null;
        }

        private void OnGetCameraCenterObjPointCallBack(CM_Camera.GetCameraCenterObjPoint ent) {
            if (ent.CallBack == null) {
                return;
            }
            var (point, isHit) = GetCenterPoint(ent.CheckForwardPoint, ent.FarDistance ,ent.IgnoreTeamId);
            ent.CallBack.Invoke(point, isHit);
        }

        /// <summary>
        /// 获取屏幕中心对象坐标，最远 200 米
        ///
        /// checkForwardPoint: 检测的前方点
        /// </summary>
        /// <returns></returns>
        private (Vector3, bool) GetCenterPoint(Vector3 checkForwardPoint, float farDistance, int ignoreTeamId) {
            if (farDistance == 0) {
                Debug.LogError("检测距离不能为 0");
                return (Vector3.zero, false);
            }
            var startPoint = cameraTran.position;
            var forward = cameraTran.forward;
            var endPoint = startPoint + forward * farDistance;
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, farDistance, ~(LayerData.ServerLayerMask));
            var minDistance = -1f;
            var isHit = false;
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null) {
                    continue;
                }
                var checkPoint = ray.point;
                // 额外检查，防止目标点在角色背后
                if (checkForwardPoint != Vector3.zero) {
                    var fireToTargetDir = (checkPoint - checkForwardPoint).normalized;
                    if (Vector3.Dot(fireToTargetDir, forward) < 0) {
                        continue;
                    }
                }
                
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    if (hitType.MyEntity != null) {
                        if (hitType.MyEntity.GetTeamID() == ignoreTeamId) {
                            continue;
                        }
                    }
                }
                var distance = Vector3.Distance(startPoint, checkPoint);
                if (minDistance < 0f || distance < minDistance) {
                    minDistance = distance;
                    endPoint = checkPoint;
                    isHit = true;
                }
            }
            return (endPoint, isHit);
        }

        private void OnGetScreenCenterGPOCallBack(CM_Camera.GetScreenCenterGPO ent) {
            if (ent.CallBack == null) {
                return;
            }
            var gpo = GetCenterGPO(ent.FarDistance, ent.IgnoreGpoID);
            ent.CallBack.Invoke(gpo);
        }

        /// <summary>
        /// 获取屏幕中心 GPO
        /// </summary>
        /// <returns></returns>
        private IGPO GetCenterGPO(float farDistance, int ignoreGpoID) {
            if (cameraTran == null || cameraEntity == null) {
                return null;
            }
            if (farDistance == 0) {
                Debug.LogError("检测距离不能为 0");
                return null;
            }
            var startPoint = cameraTran.position;
            var forward = cameraTran.rotation * Vector3.forward;
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, farDistance,LayerData.ClientLayerMask);
            var distance = farDistance;
            HitType checkHitType = null;
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }
                    if (hitType.MyEntity == null) {
                        continue;
                    }
                    if (hitType.MyEntity.IsClear()) {
                        continue;
                    }
                    if (hitType.MyEntity.GetGPOID() == ignoreGpoID) {
                        continue;
                    }
                }
                var checkDistance = Vector3.Distance(ray.point, startPoint);
                if (distance > checkDistance) {
                    checkHitType = hitType;
                    distance = checkDistance;
                }
            }
            if (checkHitType != null && checkHitType.MyEntity != null && checkHitType.MyEntity.GetGPO() != null) {
                return checkHitType.MyEntity.GetGPO();
            }
            return null;
        }

        /// <summary>
        /// 获取屏幕中心的障碍物
        /// 往相机方向打射线，如果未命中，则在最远点往底下在打射线
        /// </summary>
        private void OnGetScreenCenterBarrier(CM_Camera.GetScreenCenterBarrier ent) {
            if (ent.CallBack == null) {
                return;
            }
            (var point, bool isHit) = GetScreenCenterBarrier(ent.FarDistance, ent.CheckForwardPoint);
            ent.CallBack.Invoke(point, isHit);
        }

        private (Vector3, bool) GetScreenCenterBarrier(float farDistance, Vector3 checkForwardPoint) {
            if (farDistance == 0) {
                Debug.LogError("检测距离不能为 0");
                return (Vector3.zero, false);
            }
            var isHit = false;
            var startPoint = cameraTran.position;
            var forward = cameraTran.forward;
            var endPoint = startPoint + forward * farDistance;
            if (PhysicsUtil.CheckBlocked(startPoint, endPoint, raycast => IgnoreFunc(raycast, checkForwardPoint, forward), ~LayerData.ServerLayerMask, out var hit)) {
                isHit = true;
                endPoint = hit;
            }
            if (!isHit && PhysicsUtil.CheckBlocked(endPoint, endPoint + Vector3.down * 200, raycast => IgnoreFunc(raycast, checkForwardPoint, forward), ~LayerData.ServerLayerMask, out hit)) {
                isHit = true;
                endPoint = hit;
            }
            return (endPoint, isHit);
        }

        private static bool IgnoreFunc(RaycastHit hit, Vector3 checkForwardPoint, Vector3 forward) {
            var hitType = hit.collider.gameObject.GetComponent<HitType>();
            if (hitType != null) {
                // 忽略空气墙与角色
                if (hitType.Layer == GPOData.LayerEnum.Ignore || hitType.MyEntity != null) {
                    return true;
                }
            }
            if (checkForwardPoint != Vector3.zero) {
                var fireToTargetDir = (hit.point - checkForwardPoint).normalized;
                if (Vector3.Dot(fireToTargetDir, forward) < 0) {
                    return true;
                }
            }
            return false;
        }
    }
}