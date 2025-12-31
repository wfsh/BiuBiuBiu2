using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraAutoLockTarget : ComponentBase {
        private List<IGPO> gpoList = new List<IGPO>();
        private Transform lockTarget;
        private ClientGPO lookGPO;
        private CameraEntity cameraEntity;
        private Transform cameraTran;
        private ClientGPO localGPO;
        private float maxDistance = 100f;
        private float maxAngle = 60f;
        public float autoLookSpeed = 10.0f; // 控制旋转速度
        private RaycastHit[] raycastHit;
        private Camera camera;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Register<CM_Camera.StartAutoLookTarget>(StartAutoLookTargetCallBack);
            MsgRegister.Register<CM_Camera.LookNearestGPO>(LookNearestGPOCallBack);
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            MsgRegister.Register<CM_Camera.FindNearestGPOInFrontAngle>(FindNearestGPOInFrontAngleCallBack);
            MsgRegister.Register<CM_Camera.FindNearestGPOInFrontScreen>(FindNearestGPOInFrontScreenCallBack);
            MsgRegister.Register<CM_Camera.CheckAngleAnDistance>(CheckAngleAnDistanceCallBack);
            MsgRegister.Register<CM_Camera.LockTarget>(OnLockTargetOCallBack);
            MsgRegister.Register<CM_Camera.CheckObstacleForGPO>(OnCheckObstacleForGPOCallBack);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            mySystem.AddUpdate(Update);
            cameraEntity = (CameraEntity)iEntity;
            cameraTran = cameraEntity.UseCamera.transform;
            camera = cameraEntity.UseCamera;
        }

        protected override void OnStart() {
            base.OnStart();
            raycastHit = new RaycastHit[10];
            MsgRegister.Dispatcher(new CM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
            MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                CallBack = SetLocalGPO
            });
        }

        protected override void OnClear() {
            base.OnClear();
            gpoList.Clear();
            mySystem.RemoveUpdate(Update);
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            MsgRegister.Unregister<CM_Camera.LockTarget>(OnLockTargetOCallBack);
            MsgRegister.Unregister<CM_Camera.SetDelta>(SetDeltaCallBack);
            MsgRegister.Unregister<CM_Camera.LookNearestGPO>(LookNearestGPOCallBack);
            MsgRegister.Unregister<CM_Camera.StartAutoLookTarget>(StartAutoLookTargetCallBack);
            MsgRegister.Unregister<CM_Camera.FindNearestGPOInFrontAngle>(FindNearestGPOInFrontAngleCallBack);
            MsgRegister.Unregister<CM_Camera.FindNearestGPOInFrontScreen>(FindNearestGPOInFrontScreenCallBack);
            MsgRegister.Unregister<CM_Camera.CheckAngleAnDistance>(CheckAngleAnDistanceCallBack);
            MsgRegister.Unregister<CM_Camera.CheckObstacleForGPO>(OnCheckObstacleForGPOCallBack);
            camera = null;
            cameraEntity = null;
            cameraTran = null;
            this.localGPO = null;
        }

        public void Update(float deltaTime) {
            LockTarget(lockTarget, autoLookSpeed);
        }

        protected void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
            SetLocalGPO(ent.LocalGPO);
        }

        protected void SetLocalGPO(IGPO igpo) {
            this.localGPO = (ClientGPO)igpo;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            gpoList = list;
        }

        private void SetDeltaCallBack(CM_Camera.SetDelta ent) {
            if (Mathf.Abs(ent.Delta.x) + Mathf.Abs(ent.Delta.y) <= 6f) {
                return;
            }
            lookGPO = null;
            lockTarget = null;
            MsgRegister.Dispatcher(new CM_Camera.SetAutoLookGPO {
                GPO = null
            });
        }

        private void FindNearestGPOInFrontAngleCallBack(CM_Camera.FindNearestGPOInFrontAngle ent) {
            var lookGPO = (ClientGPO)FindNearestGPOInFront(ent.Angle, ent.Distance);
            ent.CallBack(lookGPO);
        }

        private void FindNearestGPOInFrontScreenCallBack(CM_Camera.FindNearestGPOInFrontScreen ent) {
            var lookGPO = (ClientGPO)FindNearestGPOInFrontForScreen(ent.MaxScreenDistance, ent.MinScreenDistance, ent.ScreenCheckDistance, ent.Distance);
            ent.CallBack(lookGPO);
        }

        private void LookNearestGPOCallBack(CM_Camera.LookNearestGPO ent) {
            var lookGPO = (ClientGPO)FindNearestGPOInFront(ent.LimitAngle, maxDistance);
            if (lookGPO == null) {
                return;
            }
            LockTarget(GetTargetTransform(lookGPO), ent.Speed);
        }

        private void StartAutoLookTargetCallBack(CM_Camera.StartAutoLookTarget ent) {
            lookGPO = (ClientGPO)FindNearestGPOInFront(maxAngle, maxDistance);
            if (lookGPO == null) {
                lockTarget = null;
                return;
            }
            lockTarget = GetTargetTransform(lookGPO);
            MsgRegister.Dispatcher(new CM_Camera.SetAutoLookGPO {
                GPO = lookGPO
            });
        }

        private Transform GetTargetTransform(IGPO igpo) {
            Transform lockTarget = null;
            if (igpo.GetGPOType() == GPOData.GPOType.Role) {
                lockTarget = igpo.GetBodyTran(GPOData.PartEnum.Head);
            } else {
                lockTarget = igpo.GetBodyTran(GPOData.PartEnum.Head);
                if (lockTarget == null) {
                    lockTarget = igpo.GetBodyTran(GPOData.PartEnum.Body);
                }
            }
            if (lockTarget == null) {
                lockTarget = igpo.GetBodyTran(GPOData.PartEnum.RootBody);
            }
            return lockTarget;
        }

        private void CheckAngleAnDistanceCallBack(CM_Camera.CheckAngleAnDistance ent) {
            ent.CallBack(CheckAngleAnDistance(ent.TargetGPO, ent.Angle, ent.Distance));
        }

        private void OnCheckObstacleForGPOCallBack(CM_Camera.CheckObstacleForGPO ent) {
            ent.CallBack(CheckObstacle(ent.startPoint, ent.TargetGPO));
        }

        private bool CheckAngleAnDistance(IGPO target, float maxAngle, float maxDistance) {
            var point = cameraTran.position;
            var directionToPet = target.GetPoint() - point;
            var distanceToPet = directionToPet.magnitude;
            if (distanceToPet <= maxDistance) {
                var angle = Vector3.Angle(cameraTran.forward, directionToPet);
                if (angle <= maxAngle) {
                    return true;
                }
            }
            return false;
        }

        private IGPO FindNearestGPOInFront(float maxAngle, float maxDistance) {
            IGPO nearestGpo = null;
            var nearestValue = Mathf.Infinity;
            var point = cameraTran.position;
            foreach (var gpo in gpoList) {
                if (localGPO.GetTeamID() == gpo.GetTeamID()) {
                    continue;
                }
                var directionToPet = gpo.GetPoint() - point;
                var distanceToPet = directionToPet.magnitude;
                if (distanceToPet <= maxDistance) {
                    // 计算当前对象正前方与宠物方向的夹角
                    var angle = Vector3.Angle(cameraTran.forward, directionToPet);
                    // 判断宠物是否在前方 90 度范围内
                    if (angle <= maxAngle) {
                        // 90度的范围，单边45度
                        var isHit = CheckObstacle(point, gpo);
                        if (isHit == false) {
                            var checkValue = angle + distanceToPet;
                            if (checkValue < nearestValue) {
                                nearestValue = checkValue;
                                nearestGpo = gpo;
                            }
                        }
                    }
                }
            }
            return nearestGpo;
        }

        /// <summary>
        ///  查找屏幕前方最近的 GPO
        /// </summary>
        private IGPO FindNearestGPOInFrontForScreen(float maxScreenRadius, float minScreenRadius, float checkScreenDistance, float maxDistance) {
            if (gpoList.Count <= 0 || camera == null || localGPO == null || localGPO.IsClear()) {
                return null;
            }
            IGPO nearestGpo = null;
            var nearestValue = Mathf.Infinity;
            var point = cameraTran.position;
            var screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            foreach (var gpo in gpoList) {
                if (gpo.IsClear() || localGPO.GetTeamID() == gpo.GetTeamID() || gpo.IsGodMode() || gpo.IsDead()) {
                    continue;
                }
                var transform = GetTargetTransform(gpo);
                var targetPosition = gpo.GetPoint();
                if (transform != null) {
                    targetPosition = transform.position;
                } 
                var directionToPet = targetPosition - point;
                var distanceToPet = directionToPet.magnitude;
                var radius = Mathf.Min(distanceToPet / checkScreenDistance, 1f);
                if (radius < 0f) {
                    radius = 0f;
                }
                var diffScreenRadius = (maxScreenRadius - minScreenRadius) * radius;
                var screenRadius = maxScreenRadius - diffScreenRadius;
                // 检查目标是否在可视距离内
                if (distanceToPet <= maxDistance) {
                    // 获取目标角色的位置
                    var screenPoint = camera.WorldToScreenPoint(targetPosition);
                    var screenDistance = Vector2.Distance(new Vector2(screenPoint.x, screenPoint.y), new Vector2(screenCenter.x, screenCenter.y));
                    if (screenPoint.z <= 0f) {
                        continue;
                    }
                    // 检查目标是否在屏幕前方（z > 0 表示在摄像机前方）
                    if (screenDistance <= screenRadius) {
                        // 检查是否有遮挡
                        var isHit = CheckObstacle(point, gpo);
                        if (isHit == false) {
                            // 使用屏幕距离 + 世界距离作为评估标准
                            var checkValue = screenDistance + distanceToPet;
                            if (checkValue < nearestValue) {
                                nearestValue = checkValue;
                                nearestGpo = gpo;
                                MsgRegister.Dispatcher(new CM_Camera.SetUIAutoLockDistance {
                                    Distance = screenRadius
                                });
                            }
                        }
                    }
                }
            }
            return nearestGpo;
        }

        // 检查和 GPO 之间是否有障碍物
        private bool CheckObstacle(Vector3 startPoint, IGPO target) {
            var targetTran = GetTargetTransform(target);
            if (targetTran == null) {
                return false;
            }
            var endPoint = targetTran.position;
            var forward = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,
                ~(LayerData.ServerLayerMask));
            var isHit = false;
            if (count > 0) {
                isHit = CheckObstacleRaycastHit(count);
            }
            return isHit;
        }

        public bool CheckObstacleRaycastHit(int count) {
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }
                return true;
            }
            return false;
        }

        private void OnLockTargetOCallBack(CM_Camera.LockTarget ent) {
            LockTarget(GetTargetTransform(ent.TargetGPO), ent.Speed);
        }

        private void LockTarget(Transform target, float speed) {
            if (target != null) {
                Vector3 entityPoint = cameraTran.position;
#if UNITY_EDITOR
                Debug.DrawLine(entityPoint, target.position, Color.green);
#endif
                var checkPoint = target.position;
                checkPoint.y = entityPoint.y;
                var distance = Vector3.Distance(entityPoint, checkPoint);
                if (distance < 1f) {
                    return;
                }
                // 计算目标方向和旋转
                var direction = (target.position - entityPoint).normalized;
                var targetRotation = Quaternion.LookRotation(direction);

                // 使用 Slerp 进行平滑旋转
                var eulerRota = Quaternion.Slerp(cameraTran.rotation, targetRotation, speed * Time.deltaTime);

                // 取得插值后的旋转角度并发送
                var eulerAngles = eulerRota.eulerAngles;
                var sendDelta = new Vector2(eulerAngles.x, eulerAngles.y);
                mySystem.Dispatcher(new CE_Camera.SetLockTargetDelta {
                    Delta = sendDelta,
                });
            }
        }
    }
}