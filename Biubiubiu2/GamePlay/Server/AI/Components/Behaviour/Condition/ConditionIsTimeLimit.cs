using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("延迟时间")]
    [TaskCategory("AI")]
    public class ConditionIsTimeLimit : ConditionComponent {
        [SerializeField]
        private float MinTime = 0.0f;
        [SerializeField]
        private float MaxTime = 0.0f;
        
        
        private float deltaTime = 0.0f;
        private float randomTime = 0.0f;

        override public void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
        }
        
        public override void OnStart() {
            GetRandomTime();
        }
        
        private void GetRandomTime() {
            if (MinTime >= MaxTime) {
                randomTime = MaxTime;
            } else {
                randomTime = Random.Range(MinTime, MaxTime);
            }
        }

        public override TaskStatus OnUpdate() {
            Debug.Log(Time.time - deltaTime);
            if (Time.time - deltaTime > randomTime) {
                deltaTime = Time.time;
                GetRandomTime();
                Debug.Log("Time Limit");
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
    }
}