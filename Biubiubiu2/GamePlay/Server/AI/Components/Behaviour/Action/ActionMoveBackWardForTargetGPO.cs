using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("朝着自身和目标相反的方向移动")]
    [TaskCategory("AI")]
    public class ActionMoveBackWardForTargetGPO : ActionComponent {
        [SerializeField]
        private float checkTime = 0f;
        [SerializeField]
        private float distance = 0f;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private IGPO target;
        private RaycastHit[] raycastHit;
        private float countCheckTime;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            raycastHit = new RaycastHit[10];
        }

        override protected void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            this.target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() {
            if (this.distance <= 0) {
                Debug.LogError("距离不能为 0");
                return TaskStatus.Failure;
            }
            if (this.target == null) {
                return TaskStatus.Running;
            }

            if (Time.realtimeSinceStartup - countCheckTime <= this.checkTime) {
                return TaskStatus.Running;
            }
            countCheckTime = Time.realtimeSinceStartup;

            Vector3 targetPoint = target.GetPoint();
            var behindPoint = CalculatePointBehind(targetPoint);
            behindPoint.y = targetPoint.y;

            float hitSqrtDis = (behindPoint - targetPoint).sqrMagnitude;
            if (hitSqrtDis <= 2.25f) {
                iGPO.Dispatcher(new SE_Behaviour.Event_StopMove());
            } else {
#if UNITY_EDITOR
                Debug.DrawLine(behindPoint, iEntity.GetPoint(), Color.red);
#endif
                iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                    movePoint = behindPoint,
                    MoveType = this.moveType
                });
            }
            return TaskStatus.Running;
        }

        // 这个方法计算以A为起点，面向B方向，在A背后distanceBehind米处的坐标点
        private Vector3 CalculatePointBehind(Vector3 pointB) {
            Vector3 origin = iEntity.GetPoint() + 0.5f * Vector3.up;
            pointB.y = origin.y;
            Vector3 direction = (pointB - origin).normalized;
            direction *= -1;
            Vector3 pointBehind = origin + direction * distance;

            int count = Physics.RaycastNonAlloc(origin, direction, raycastHit, distance, ~(LayerData.ClientLayerMask));
            if (count > 0) {
                bool hasPoint = CheckObstacleRaycastClosestHit(count, out Vector3 closestPoint);
                if (hasPoint) {
                    float hitDis = (closestPoint - origin).magnitude;
                    hitDis = hitDis - 1 > 1f ? hitDis - 1 : 0;
                    pointBehind = origin + direction * hitDis;
                }
            }

            return pointBehind;
        }

        private bool CheckObstacleRaycastClosestHit(int count, out Vector3 closestPoint) {
            bool hasPoint = false;
            float minDis = float.MaxValue;
            closestPoint = Vector3.zero;

            for (int i = 0; i < count; i++) {
                var hit = raycastHit[i];
                if (hit.collider == null || hit.collider.isTrigger) {
                    continue;
                }
                var gameObj = hit.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }

                if (hit.distance < minDis) {
                    closestPoint = hit.point;
                    hasPoint = true;
                }
            }

            return hasPoint;
        }
    }
}