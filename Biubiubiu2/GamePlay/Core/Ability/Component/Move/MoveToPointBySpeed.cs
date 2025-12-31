using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class MoveToPointBySpeed : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float Speed;
            public Vector3 StartPoint;
            public Vector3 TargetPoint;
            public Action MoveEndCallBack;
        }
        private float speed;
        private Vector3 targetPoint = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        private Vector3 forward = Vector3.zero;
        private Action moveEndCallBack;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.Speed, initData.StartPoint, initData.TargetPoint, initData.MoveEndCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            moveEndCallBack = null;
        }

        public void SetData(float speed, Vector3 startPoint, Vector3 targetPoint, Action moveEndCallBack) {
            this.speed = speed;
            this.startPoint = startPoint;
            this.targetPoint = targetPoint;
            forward = (targetPoint - startPoint).normalized;
            this.moveEndCallBack = moveEndCallBack;
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (iEntity == null) {
                return;
            }

            var point = iEntity.GetPoint();
            point += forward * this.speed * delta;
            iEntity.SetPoint(point);
            var nowDistance = Vector3.Distance(startPoint, point);
            var distance = Vector3.Distance(startPoint, targetPoint);
            if (nowDistance >= distance) {
                iEntity.SetPoint(targetPoint);
                moveEndCallBack?.Invoke();
            }
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, point, Color.green);
            Debug.DrawLine(point, targetPoint, Color.magenta);
#endif
        }
    }
}