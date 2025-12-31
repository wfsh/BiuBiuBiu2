using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 击退组件
    /// </summary>
    public class CharacterKnockback : ComponentBase {
        public Vector3 dir; // 击退方向，
        public float speed; // 击退速度  
        public float duration; // 击退持续时间
        public float totalDistance; // 总击退距离
        private float endTime;
        private bool isPlay = false;
        private bool isWirebugMove = false;

        protected override void OnAwake() {
            this.mySystem.Register<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            CanceAction();
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_Knockback.ID, OnKnockbackGPOCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.TargetRpc_Knockback.ID, OnKnockbackGPOCallBack);
        }

        private void OnWirebugMoveStateBack(ISystemMsg body, CE_Character.Event_WirebugMoveState ent) {
            isWirebugMove = CharacterData.IsWirebugMove(ent.State);
            if (isWirebugMove) {
                CanceAction();
            }
        }

        public void OnKnockbackGPOCallBack(INetwork network, IProto_Doc proto) {
            if (isWirebugMove) {
                return;
            }
            var data = (Proto_Character.TargetRpc_Knockback)proto;
            dir = data.Dir.normalized;
            speed = data.Speed;
            duration = data.Duration;
            totalDistance = speed * duration;
            endTime = Time.time + duration;
            isPlay = true;
        }

        private void CanceAction() {
            isPlay = false;
            mySystem.Dispatcher(new CE_Character.Event_KnockbackMovePoint {
                KnockbackMove = Vector3.zero
            });
        }

        private void OnUpdate(float deltaTime) {
            if (isPlay == false) {
                return;
            }
            var currentTime = Time.time;
            var knockbackMove = Vector3.zero;
            if (currentTime >= endTime) {
                CanceAction();
            } else {
                var lerpT = (endTime - currentTime) / duration;
                var distance = Mathf.Lerp(0, totalDistance, lerpT);
                knockbackMove = dir * distance * Time.deltaTime;
                mySystem.Dispatcher(new CE_Character.Event_KnockbackMovePoint {
                    KnockbackMove = knockbackMove
                });
            }
        }
    }
}