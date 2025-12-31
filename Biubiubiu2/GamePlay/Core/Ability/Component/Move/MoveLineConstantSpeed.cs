using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    // 直线匀速移动
    public class MoveLineConstantSpeed : ComponentBase{
        public struct InitData : SystemBase.IComponentInitData {
            public float Speed;
            public float MaxDistance;
        }
        private float speed;
        private float maxDistance;
        private float maxDistanceSq;
        private Vector3 forward = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        protected override void OnAwake() {
            base.OnAwake();
            if (initDataBase != null) {
                var initData = (InitData)initDataBase;
                SetSpeed(initData.Speed);
                SetMaxDistance(initData.MaxDistance);
            }
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            startPoint = iEntity.GetPoint();
            forward = iEntity.GetRota() * Vector3.forward;
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }

        public void SetSpeed(float speed) {
            this.speed = speed;
        }

        public void SetMaxDistance(float distance) {
            maxDistance = distance;
            maxDistanceSq = distance * distance;
        }
        
        private void OnUpdate(float delta) {
            var point = iEntity.GetPoint();
            var targetPoint = point + forward * speed * delta;
            if (maxDistance > 0f && maxDistanceSq <= (targetPoint - startPoint).sqrMagnitude) {
                targetPoint = startPoint + forward * maxDistance;
            }
            point = targetPoint;
            iEntity.SetPoint(point);
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, point, Color.red);
#endif
        }
    }
}
