using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class WirebugLineMove {
        public bool IsPlay { get; set; }
        private const float force = 40.0f; // 移动速度
        private const float gravity = 4f; // 重力
        private const float duration = 1.2f; // 持续时间
        private float Angle = 1f;
        private float currentTime = 0.0f;
        private Vector3 initialVelocity;
        private Vector3 currentVelocity;
        private Action endCallBack;
        private Action<Vector3, float> moveCallBack;

        public void OnClear() {
            IsPlay = false;
            currentVelocity = Vector3.zero;
            this.endCallBack = null;
            this.moveCallBack = null;
        }

        public void SetCallBack(Action endCallBack, Action<Vector3, float> moveCallBack) {
            this.endCallBack = endCallBack;
            this.moveCallBack = moveCallBack;
        }

        public void OnUpdate() {
            ExecuteSkill();
        }

        public void StartMove(Vector3 startPoint, Vector3 endPoint) {
            currentTime = 0.0f;
            var direction = endPoint - startPoint;
            var dir = (direction).normalized;
            var verticalDistance = endPoint.y - startPoint.y;
            var horizontalDistance = new Vector2(direction.x, direction.z).magnitude;
            // 计算出垂直方向的角度，不论目标点在角色的上方还是下方
            Angle = Mathf.Atan2(Mathf.Abs(verticalDistance), horizontalDistance) * Mathf.Rad2Deg;
            Angle = Mathf.Clamp(Angle, 1f, 45f); // 可以根据需求调整角度限制
            // 初始化速度
            initialVelocity.x = dir.x * force;
            initialVelocity.z = dir.z * force;
            initialVelocity.y = Mathf.Sin(Angle * Mathf.Deg2Rad) * Mathf.Sign(verticalDistance) * force;
            currentVelocity = initialVelocity;
            IsPlay = true;
        }

        void ExecuteSkill() {
            if (IsPlay == false) {
                return;
            }
            currentTime += Time.deltaTime;
            if (currentTime > duration) {
                CancelMove();
                return;
            }
            // 计算速度衰减
            var t = currentTime / duration;
            var currentForce = Mathf.Lerp(force, 0, t);
            // 更新水平速度和垂直速度
            var forceRatio = currentForce / force;
            if (forceRatio <= 0.2f) {
                CancelMove();
            } else {
                currentVelocity.x = initialVelocity.x * forceRatio;
                currentVelocity.z = initialVelocity.z * forceRatio;
                currentVelocity.y = initialVelocity.y * forceRatio - gravity * currentTime;
                // 计算位移
                var displacement = currentVelocity * Time.deltaTime;
                this.moveCallBack.Invoke(displacement, currentTime);
            }
        }

        public void CancelMove() {
            IsPlay = false;
            currentVelocity = Vector3.zero;
            endCallBack();
        }
    }
}