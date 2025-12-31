using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("随机跳跃")]
    [TaskCategory("AI")]
    public class ActionRandomJump : ActionComponent {
        [SerializeField]
        private float MinTime = 0.0f;

        [SerializeField]
        private float MaxTime = 0.0f;

        [SerializeField]
        private float JumpWeight = 0.0f;

        [SerializeField]
        private float AirJumpWeight = 0.0f;

        private float deltaTime = 0.0f;
        private float randomTime = 0.0f;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_GPO.Event_SetAIJumpActionRatioTime>(OnSetAIJumpActionRatioTime);
        }

        override protected void OnClear() {
            iGPO?.Unregister<SE_GPO.Event_SetAIJumpActionRatioTime>(OnSetAIJumpActionRatioTime);
        }

        public override void OnStart() {
            iGPO?.Dispatcher(new SE_GPO.Event_GetAIJumpActionRatioTime {
                CallBack = (f, f1) => {
                    SetJumpValue(f, f1);
                }
            });
            GetRandomTime();
        }

        private void OnSetAIJumpActionRatioTime(ISystemMsg body, SE_GPO.Event_SetAIJumpActionRatioTime ent) {
            SetJumpValue(ent.JumpMinIntervalTime, ent.JumpMaxIntervalTime);
        }

        private void SetJumpValue(float minValue, float maxValue) {
            MinTime = minValue;
            MaxTime = maxValue;
        }
        
        private void GetRandomTime() {
            if (MinTime >= MaxTime) {
                randomTime = MaxTime;
            } else {
                randomTime = Random.Range(MinTime, MaxTime);
            }
            deltaTime = Time.time;
        }

        public override TaskStatus OnUpdate() {
            if (iGPO == null) {
                return TaskStatus.Running;
            }
            if (Time.time - deltaTime > randomTime) {
                deltaTime = Time.time;
                GetRandomTime();
                PlayJump();
            }
            return TaskStatus.Running;
        }

        private void PlayJump() {
            var maxWeight = JumpWeight + AirJumpWeight;
            var randomWeight = Random.Range(0.0f, maxWeight);
            if (randomWeight < JumpWeight) {
                iGPO.Dispatcher(new SE_Behaviour.Event_OnJump());
            } else {
                iGPO.Dispatcher(new SE_Behaviour.Event_OnAirJump());
            }
        }
    }
}