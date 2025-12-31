using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityMissileMove : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public Vector3[] Points;
            public float Speed;
            public Action CallBack;
        }
        private float speed;
        private int index = 0;
        private bool isMoveEnd = false;
        private Vector3[] points;
        private Vector3 nextPoint = Vector3.zero;
        private Action OnMoveEnd;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.Points, initData.Speed);
            SetMoveEnd(initData.CallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            GetNextPoint();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }

        public void SetData(Vector3[] points, float speed) {
            this.speed = speed;
            this.points = points;
            var firstPoint = this.points[0];
            iEntity.SetPoint(firstPoint);
            index++;
        }

        private void GetNextPoint() {
            if (index >= this.points.Length) {
                MoveEnd();
                return;
            }
            nextPoint = this.points[index];
            iEntity.LookAT(nextPoint);
            index++;
        }

        private void OnUpdate(float delta) {
            if (isMoveEnd == true) {
                return;
            }
            for (int i = 1; i < this.points.Length; i++) {
                var prevPoint = this.points[i - 1];
                var nowPoint = this.points[i];
#if UNITY_EDITOR
                if (index < i) {
                    Debug.DrawLine(prevPoint, nowPoint, Color.green);
                } else {
                    Debug.DrawLine(prevPoint, nowPoint, Color.red);
                }
#endif
            }
            var point = iEntity.GetPoint();
            var movePoint = Vector3.MoveTowards(point, nextPoint, speed * delta);
            iEntity.SetPoint(movePoint);
            var distance = Vector3.Distance(point, nextPoint);
            var limitDistance = Mathf.Max(0.1f, this.speed * 0.1f);
            if (distance < limitDistance) {
                GetNextPoint();
            }
        }
        
        public void SetMoveEnd(Action callback) {
            OnMoveEnd = callback;
        }

        public void MoveEnd() {
            if (isMoveEnd) {
                return;
            }
            isMoveEnd = true;
            // 确保位置到最后
            if (points.Length > 0) {
                iEntity.SetPoint(points[^1]);
            }
            mySystem.Dispatcher(new CE_Ability.MissileMoveEnd());
            OnMoveEnd?.Invoke();
        }
    }
}