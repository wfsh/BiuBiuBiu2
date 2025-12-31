using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("MoveForMasterRange")]
    [TaskCategory("AI")]
    public class ActionMoveForMasterRange : ActionComponent {
        [SerializeField]
        private float maxDistance = 0f;
        [SerializeField]
        private float minDistance = 0f;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        [SerializeField]
        private float checkTime;
        private float countCheckTime;
        private IGPO masterGPO;

        override public void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            base.OnClear();
            this.masterGPO = null;
        }

        public override TaskStatus OnUpdate() {
            if (this.masterGPO == null) {
                iGPO.Dispatcher(new SE_AI.Event_GetMasterGPO {
                    CallBack = gpo => {
                        this.masterGPO = gpo;
                    }
                });
            }
            if (this.masterGPO == null) {
                return TaskStatus.Failure;
            }
            if (Time.realtimeSinceStartup - countCheckTime <= this.checkTime) {
                return TaskStatus.Running;
            }
            countCheckTime = Time.realtimeSinceStartup;
            var targetPoint = masterGPO.GetPoint();
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = CalculateRandomPoint(targetPoint),
                MoveType = moveType
            });
            return TaskStatus.Running;
        }

        // 获取范围内的随机点
        public Vector3 CalculateRandomPoint(Vector3 startPoint) {
            var angle = Random.Range(0f, 360f);
            var direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            var distance = Random.Range(minDistance, maxDistance);
            var randomDisplacement = direction * distance;
            var randomPoint = new Vector3(startPoint.x + randomDisplacement.x, startPoint.y,
                startPoint.z + randomDisplacement.y);
            return randomPoint;
        }
    }
}