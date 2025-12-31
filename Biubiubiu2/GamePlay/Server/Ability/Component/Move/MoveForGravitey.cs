using UnityEngine;

using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class MoveForGravitey : ComponentBase {
        private float gravitey = 1f; // 影响下落速度和跳跃高度 默认 1 倍重力
        private RaycastHit[] raycastHit;
        private Vector3 movePoint;
        private float groundDis = -1f;
        private Vector3 groundPoint = Vector3.zero;

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            raycastHit = new RaycastHit[10];
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            base.OnClear();
        }

        private void OnUpdate(float delta) {
            CheckGroundedData();
            if (isGround() == false) {
                movePoint = iEntity.GetPoint();
                FallMove(delta, 15f);
                iEntity.SetPoint(movePoint);
            } else {
                iEntity.SetPoint(groundPoint);
            }
        }

        private void FallMove(float deltaTime, float weight) {
            movePoint.y -= gravitey * 15 * deltaTime;
            var maxDownGravitey = -weight * gravitey;
            if (groundDis >= 0f) {
                var speedRatio = groundDis / 5f;
                maxDownGravitey *= speedRatio;
            }
            if (movePoint.y <= maxDownGravitey) {
                movePoint.y = maxDownGravitey;
            }
        }

        private void CheckGroundedData() {
            var point = this.iEntity.GetPoint() + Vector3.up * 0.3f;
            var count = Physics.RaycastNonAlloc(point, Vector3.down, raycastHit, 1.0f);
            GetGound(count, raycastHit);
        }

        public void GetGound(int count, RaycastHit[] list) {
            groundDis = -1f;
            groundPoint = Vector3.zero;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var dis = ray.distance;
                if (groundDis < 0f || groundDis >= dis) {
                    groundDis = dis;
                    groundPoint = ray.point;
                }
            }
        }

        private bool isGround() {
            return groundDis >= 0 && groundDis <= 1.0f;
        }
    }
}