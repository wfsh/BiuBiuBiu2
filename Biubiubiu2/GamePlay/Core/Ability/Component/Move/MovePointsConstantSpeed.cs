using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    // 直线匀速移动
    public class MovePointsConstantSpeed : ComponentBase {
        private float speed;
        private int index = 0;
        private bool isMoveEnd = false;
        private Vector3[] points;
        private Vector3 nextPoint = Vector3.zero;
        private Action onMoveEndCallBack;
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            this.onMoveEndCallBack = null;
            RemoveUpdate(OnUpdate);
        }

        public void SetData(Vector3[] points, float speed, Action moveEndCallBack) {
            this.onMoveEndCallBack = moveEndCallBack;
            this.speed = speed;
            this.points = points;
            var firstPoint = this.points[0];
            iEntity.SetPoint(firstPoint);
            index++;
            GetNextPoint();
        }

        public void MoveEnd() {
            if (isMoveEnd) {
                return;
            }
            isMoveEnd = true;
            this.onMoveEndCallBack.Invoke();
        }

        private void GetNextPoint() {
            if (index >= this.points.Length) {
                MoveEnd();
                return;
            }
            nextPoint = this.points[index];
            var entity = (EntityBase)iEntity;
            entity.LookAT(nextPoint);
            index++;
        }
        
        private void OnUpdate(float delta) {
            if (isMoveEnd == true) {
                return;
            }
#if UNITY_EDITOR
            for (int i = 1; i < this.points.Length; i++) {
                var prevPoint = this.points[i - 1];
                var nowPoint = this.points[i];
                if (index < i) {
                    Debug.DrawLine(prevPoint, nowPoint, Color.green);
                } else {
                    Debug.DrawLine(prevPoint, nowPoint, Color.red);
                }
            }
#endif
            var point = iEntity.GetPoint();
            var movePoint = Vector3.MoveTowards(point, nextPoint, speed * delta);
            iEntity.SetPoint(movePoint);
            var distance = Vector3.Distance(point, nextPoint);
            var limitDistance = Mathf.Max(0.1f, this.speed * 0.1f);
            if (distance < limitDistance) {
                GetNextPoint();
            } 
        }
    }
}
