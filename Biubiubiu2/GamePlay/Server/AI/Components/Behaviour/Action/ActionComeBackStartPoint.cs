using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("回到出生点")]
    [TaskCategory("AI")]
    public class ActionComeBackStartPoint : ActionComponent {
        [SerializeField]
        private float checkDistance = 2;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 startPoint;
        private IGPO target;
        private bool isComeBack = false;
        
        public override void OnAwake() {
            base.OnAwake();
        }

        public override void OnStart() {
            base.OnStart();
            startPoint = iEntity.GetPoint();
        }
        
        protected override void OnClear() {
            base.OnClear();
        }

        public override TaskStatus OnUpdate() {
            iGPO.Dispatcher(new SE_Behaviour.Event_GetNextPatrolPoint {
                CallBack = point => {
                    startPoint = point;
                }
            });
            if (isComeBack) {
                var distance = Vector3.Distance(iEntity.GetPoint(), startPoint);
#if UNITY_EDITOR
                Debug.DrawLine(iEntity.GetPoint(), startPoint, Color.cyan);
#endif
                if (distance < checkDistance) {
                    iGPO.Dispatcher(new SE_Behaviour.Event_ComeBack {
                        IsTrue = false,
                    });
                    isComeBack = false;
                }
            } else {
                isComeBack = true;
                iGPO.Dispatcher(new SE_Behaviour.Event_ComeBack {
                    IsTrue = true,
                });
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = startPoint,
                MoveType = this.moveType,
            });
            return TaskStatus.Running;
        }
    }
}