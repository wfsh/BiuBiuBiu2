using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 击飞组件
    /// </summary>
    public class CharacterStrikeFly : ComponentBase {
        private Vector3 initialVelocity;
        private Vector3 currentVelocity;
        private float force = 5.0f; // 击飞力度
        private float gravity = 4f; // 重力
        private float Duration = 1.0f; // 击飞持续时间
        private float Angle = 30f * Mathf.Deg2Rad;
        private float currentTime = 0.0f;
        private bool isPlay = false;

        protected override void OnAwake() {
            this.mySystem.Register<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            CanceAction();
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_StrikeFly.ID, OnStrikeFlyGPOCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.TargetRpc_StrikeFly.ID, OnStrikeFlyGPOCallBack);
        }

        private void OnWirebugMoveStateBack(ISystemMsg body, CE_Character.Event_WirebugMoveState ent) {
            if (CharacterData.IsWirebugMove(ent.State)) {
                CanceAction();
            }
        }

        private void CanceAction() {
            isPlay = false;
            currentVelocity = Vector3.zero;
            mySystem.Dispatcher(new CE_Character.Event_StrikeFlyMovePoint {
                StrikeFlyMove = Vector3.zero
            });
        }

        public void OnStrikeFlyGPOCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Character.TargetRpc_StrikeFly)proto;
            currentTime = 0.0f;
            force = data.Force;
            Duration = data.Duration;
            var dir = data.Dir.normalized;
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
                CanceAction();
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
                mySystem.Dispatcher(new CE_Character.Event_StrikeFlyMovePoint {
                    StrikeFlyMove = displacement
                });
            }
        }
    }
}