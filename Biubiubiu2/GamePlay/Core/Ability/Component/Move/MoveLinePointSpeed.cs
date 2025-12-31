using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    // 直线匀速移动
    public class MoveLinePointSpeed : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float Speed;
            public Vector3 TargetPoint;
            public Action CallBack;
        }
        private float speed;
        private Vector3 targetPoint = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        private Vector3 forward = Vector3.zero;
        private Action moveEndCallBack;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Ability.SetEntityStartPoint>(OnSetEntityStartPointCallBack);
            var initData = (InitData)initDataBase;
            SetData(initData.Speed, initData.TargetPoint, initData.CallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            startPoint = iEntity.GetPoint();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            mySystem.Unregister<CE_Ability.SetEntityStartPoint>(OnSetEntityStartPointCallBack);
            RemoveUpdate(OnUpdate);
            moveEndCallBack = null;
        }
        
        private void OnSetEntityStartPointCallBack(ISystemMsg body, CE_Ability.SetEntityStartPoint ent) {
            startPoint = ent.StartPoint;
            forward = iEntity.GetRota() * Vector3.forward;
        }

        public void SetData(float speed, Vector3 targetPoint, Action moveEndCallBack) {
            this.speed = speed;
            this.targetPoint = targetPoint;
            forward = iEntity.GetRota() * Vector3.forward;
            this.moveEndCallBack = moveEndCallBack;
        }
        
        private void OnUpdate(float delta) {
            if (isSetEntityObj == false) {
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
