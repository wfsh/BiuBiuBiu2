using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 锁定 GPO 进行抛物线移动
    public class MoveParabola : ComponentBase {
        private float speed;
        private bool isMoveEnd = true;
        private Action onMoveEndCallBack;
        private IGPO target;
        private bool isLookAT = false;
        private float rotaSpeed;

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            this.onMoveEndCallBack = null;
            this.target = null;
            RemoveUpdate(OnUpdate);
            base.OnClear();
        }

        public void SetData(float speed, IGPO ability, Action moveEndCallBack) {
            this.onMoveEndCallBack = moveEndCallBack;
            this.speed = speed;
            this.rotaSpeed = 2 + speed * 0.1f;
            this.target = ability;
            this.isMoveEnd = false;
            this.isLookAT = false;
        }

        public void MoveEnd() {
            if (isMoveEnd) {
                return;
            }
            isMoveEnd = true;
            this.onMoveEndCallBack.Invoke();
        }

        private void OnUpdate(float delta) {
            if (isMoveEnd == true) {
                return;
            }
            Move(delta);
        }
        
        private void Move(float delta) {
            var targetPosition = target.GetPoint() + Vector3.up * 1;
            var direction = (targetPosition - iEntity.GetPoint()).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            var angle = Quaternion.Angle(iEntity.GetRota(), targetRotation);
            var distance = Vector3.Distance(iEntity.GetPoint(), targetPosition);
            if (angle <= 10f || distance <= 5f) {
                isLookAT = true;
            }
            if (isLookAT) {
                iEntity.LookAT(targetPosition);
            } else {
                iEntity.SetRota(Quaternion.Lerp(iEntity.GetRota(), targetRotation, this.rotaSpeed * delta));
                this.rotaSpeed += delta;
            }
            var movePoint = iEntity.GetPoint() + (iEntity.GetForward() * speed * delta);
#if UNITY_EDITOR
            Debug.DrawLine(iEntity.GetPoint(), target.GetPoint(), Color.green);
#endif
            iEntity.SetPoint(movePoint);
            if (distance < 1f) {
                MoveEnd();
            }
        }
    }
}