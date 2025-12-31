using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class RandomAreaMove : ComponentBase {
        private Vector3 targetPoint = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        private float area = 100.0f;
        private float speed = 0.0f;
        private float addSpeed = 0.0f;
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }
        
        void OnUpdate(float delta) {
            var distance = Vector3.Distance(iEntity.GetPoint(), targetPoint);
            if (distance <= 1) {
                GetTarget();
            }
#if UNITY_EDITOR
            Debug.DrawLine(iEntity.GetPoint(), targetPoint, Color.green);
#endif
            iEntity.SetPoint(Vector3.MoveTowards(iEntity.GetPoint(), targetPoint, (this.speed + this.addSpeed) * delta));
        }

        public void SetArea(Vector3 startPoint, float area, float moveSpeed) {
            this.area = area;
            this.speed = moveSpeed;
            this.startPoint = startPoint;
            GetTarget();
            iEntity.SetPoint(targetPoint);
            GetTarget();
        }

        public void AddSpeed(float speed) {
            this.addSpeed = speed;
        }

        private void GetTarget() {
            var tempArge = this.area * 0.99f; // 保证在 < this.arge 的范围内进行随机
            var randX = Random.Range(-tempArge, tempArge);
            var randZ = Random.Range(-tempArge, tempArge);
            targetPoint = new Vector3(startPoint.x + randX, startPoint.y, startPoint.z + randZ);
        }
    }
}
