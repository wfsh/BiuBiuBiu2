using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 击退组件
    /// </summary>
    public class KnockbackGPO : ComponentBase {
        public Vector3 dir; // 击退方向，
        public float speed; // 击退速度  
        public float duration; // 击退持续时间
        public float totalDistance; // 总击退距离
        private float endTime;
        private bool isPlay = false;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_KnockbackGPO>(OnKnockbackGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_KnockbackGPO>(OnKnockbackGPOCallBack);
            RemoveUpdate(OnUpdate);
        }

        public void OnKnockbackGPOCallBack(ISystemMsg body, SE_GPO.Event_KnockbackGPO ent) {
            dir = ent.Dir.normalized;
            speed = ent.Speed;
            duration = ent.Duration;
            totalDistance = speed * duration;
            endTime = Time.time + duration;
            isPlay = true;
        }

        private void OnUpdate(float deltaTime) {
            if (isPlay == false) {
                return;
            }
            var currentTime = Time.time;
            var knockbackMove = Vector3.zero;
            if (currentTime >= endTime) {
                isPlay = false;
            } else {
                var lerpT = (endTime - currentTime) / duration;
                var distance = Mathf.Lerp(0, totalDistance, lerpT);
                knockbackMove = dir * distance * Time.deltaTime;
            }
            mySystem.Dispatcher(new SE_GPO.Event_KnockbackMovePoint {
                KnockbackMove = knockbackMove
            });
        }
    }
}