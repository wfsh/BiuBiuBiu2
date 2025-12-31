using System;
using Sofunny.BiuBiuBiu2.Message;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class MovePointsLoop : ComponentBase {
        private float speed;
        private int index = 0;
        private bool isMoveEnd = false;
        private List<Vector3> points = null;
        private Vector3 nextPoint = Vector3.zero;
        private bool isIndexReverse = false;

        protected override void OnStart() {
            base.OnStart();
            mySystem.Register<SE_Ability.SetMovePointsLoop>(OnSetMovePointsLoopCallBack);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Ability.SetMovePointsLoop>(OnSetMovePointsLoopCallBack);
            base.OnClear();
        }
        
        private void OnSetMovePointsLoopCallBack(ISystemMsg body, SE_Ability.SetMovePointsLoop ent) {
            SetData(ent.Points, ent.MoveSpeed);
        }

        private void SetData(List<Vector3> points, float speed) {
            this.speed = speed;
            this.points = points;
            var firstPoint = this.points[0];
            iEntity.SetPoint(firstPoint);
            index++;
            GetNextPoint();
            AddUpdate(OnUpdate);
        }

        private void GetNextPoint() {
            nextPoint = this.points[index];
            index++;
            if (index >= this.points.Count) {
                isIndexReverse = true;
                index -= 2;
            }
        }

        private void GetPrevPoint() {
            nextPoint = this.points[index];
            index--;
            if (index < 0) {
                isIndexReverse = false;
                index += 2;
            }
        }
        
        private void OnUpdate(float delta) {
            if (isMoveEnd == true) {
                return;
            }
#if UNITY_EDITOR
            for (int i = 1; i < this.points.Count; i++) {
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
                if (isIndexReverse) {
                    GetNextPoint();
                } else {
                    GetPrevPoint();
                }
            }
        }
    }
}
