using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class WirebugCircleMove {
        public bool IsPlay { get; set; }
        private const float targetAngle = 90f; // 角度
        private const float duration = 1.5f; // 移动耗时
        private Vector3 startPosition; // 开始移动时的起始位置
        private Vector3 targetPosition; // C点的目标位置  
        private float currentTime = 0.0f;
        private float radius = 0f; // 半径
        private int dir = 1; // 朝向
        private Action endCallBack;
        private Action<Vector3> moveCallBack;
        private Transform lHandTran;

        public void OnUpdate() {
            ExecuteSkill();
        }

        public void OnClear() {
            IsPlay = false;
            this.endCallBack = null;
            this.moveCallBack = null;
        }

        public void SetCallBack(Action endCallBack, Action<Vector3> moveCallBack) {
            this.endCallBack = endCallBack;
            this.moveCallBack = moveCallBack;
        }

        public void StartMove(Transform lHandTran, float lineDistance, Vector3 moveDir, Vector3 point) {
            this.lHandTran = lHandTran;
            startPosition = lHandTran.position;
            point.y = startPosition.y;
            var directionAB = point - startPosition;
            var unitDirectionAB = directionAB.normalized;
            targetPosition = startPosition + lineDistance * unitDirectionAB;
            targetPosition.y = startPosition.y;
            radius = Vector3.Distance(targetPosition, startPosition);
            currentTime = 0.0f;
            IsPlay = true;
            dir = moveDir.x < 0 ? 1 : -1;
        }

        private Vector3 GetCirclePoint(Vector3 target, Vector3 origin, float angle, float ratio) {
            var direction = (target - origin).normalized;
            var rotation = Quaternion.Euler(0, angle, 0);
            var rotatedDirection = rotation * direction;
            var circlePoint = target + rotatedDirection * ratio;
            circlePoint.y = origin.y; // 保持水平面不变
            return circlePoint;
        }

        void ExecuteSkill() {
            if (!IsPlay) {
                return;
            }
            currentTime += Time.deltaTime;
            if (currentTime > duration) {
                CancelMove();
                return;
            }
            var nowPoint = this.lHandTran.position;
            // 计算插值比例  
            var t = currentTime / duration;
            // 插值角度（从 0 到 90 度）  
            var angle = Mathf.Lerp(0, targetAngle, t * (2f - t));
            targetPosition.y = startPosition.y;
            var newPoint = GetCirclePoint(targetPosition, startPosition, 180 + angle * dir, radius);
#if UNITY_EDITOR
            Debug.DrawLine(targetPosition, nowPoint, Color.green);
#endif
            this.moveCallBack(newPoint - nowPoint);
        }

        public void CancelMove() {
            IsPlay = false;
            endCallBack();
        }
    }
}