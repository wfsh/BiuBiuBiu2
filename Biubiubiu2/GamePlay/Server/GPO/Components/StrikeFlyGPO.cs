using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 击飞组件
    /// </summary>
    public class StrikeFlyGPO : ComponentBase {
        private Vector3 initialVelocity;
        private Vector3 currentVelocity;
        private float force = 5.0f; // 击飞力度
        private float gravity = 4f; // 重力
        private float Duration = 1.0f; // 击飞持续时间
        private float Angle = 30f * Mathf.Deg2Rad;
        private float currentTime = 0.0f;
        private bool isPlay = false;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_StrikeFlyGPO>(OnStrikeFlyGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_StrikeFlyGPO>(OnStrikeFlyGPOCallBack);
            RemoveUpdate(OnUpdate);
        }

        public void OnStrikeFlyGPOCallBack(ISystemMsg body, SE_GPO.Event_StrikeFlyGPO ent) {
            currentTime = 0.0f;
            force = ent.Force;
            Duration = ent.Duration;
            var dir = ent.Dir.normalized;
            // 初始速度分量
            initialVelocity.x = dir.x * force;
            initialVelocity.z = dir.z * force;
            initialVelocity.y = force * Mathf.Sin(Angle);
            currentVelocity = initialVelocity;
            isPlay = true;
        }

        private void OnUpdate(float deltaTime) {
            if (isPlay == false) {
                return;
            }
            currentTime += Time.deltaTime;
            var displacement = Vector3.zero;
            if (currentTime > Duration) {
                isPlay = false;
                currentVelocity = Vector3.zero;
            } else {
                // 计算速度衰减
                var t = currentTime / Duration;
                var currentForce = Mathf.Lerp(force, 0, t);
                // 更新水平速度和垂直速度
                var nowForce = currentForce / force;
                var cosAngle = Mathf.Cos(Angle) * nowForce;
                currentVelocity.x = initialVelocity.x * cosAngle;
                currentVelocity.z = initialVelocity.z * cosAngle;
                currentVelocity.y = initialVelocity.y * Mathf.Sin(Angle) * nowForce - gravity * currentTime;
                // 计算位移
                displacement = currentVelocity * Time.deltaTime;
            }
            mySystem.Dispatcher(new SE_GPO.Event_KnockbackMovePoint {
                KnockbackMove = displacement
            });
        }
    }
}