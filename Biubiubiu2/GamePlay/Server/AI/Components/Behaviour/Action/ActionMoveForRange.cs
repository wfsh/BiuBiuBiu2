using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("起点范围内移动")]
    [TaskCategory("AI")]
    public class ActionMoveForRange : ActionComponent {
        [SerializeField]
        private float range;
        [SerializeField]
        private float checkTime;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        [SerializeField]
        private float standByRatio = 0.7f;
        private float countCheckTime;
        private Vector3 startPoint;
        private bool isMovePoint = false;
        private bool isPrevStandBy = false;
        private float rangeCheckTime = 0.0f;
        
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            startPoint = iEntity.GetPoint();
        }

        public override void OnStart() {
            base.OnStart();
            rangeCheckTime = 0f;
        }

        override protected void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
        }

        public void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            this.isMovePoint = ent.IsTrue;
        }

        public override TaskStatus OnUpdate() { 
            if (Time.realtimeSinceStartup - countCheckTime <= rangeCheckTime) {
                return TaskStatus.Running;
            }
            countCheckTime = Time.realtimeSinceStartup;
            rangeCheckTime = Random.Range(0f, this.checkTime);
            if (isPrevStandBy == false) {
                var ratio = Random.Range(0f, 1f);
                if (ratio <= standByRatio) {
                    isPrevStandBy = true;
                    return TaskStatus.Running;
                }
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_GetNextPatrolPoint {
                CallBack = point => {
                    startPoint = point;
                }
            });
            // var targetPoint = startPoint;
            // targetPoint.x = startPoint.x + Random.Range(-range, range);
            // targetPoint.z = startPoint.z + Random.Range(-range, range);
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = startPoint,
                MoveType = moveType
            });
            return TaskStatus.Running;
        }
    }
}