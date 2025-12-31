using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("滑铲")]
    [TaskCategory("AI")]
    public class ActionSlideMove : ActionComponent {
        [SerializeField]
        private float MinTime = 0.0f;

        [SerializeField]
        private float MaxTime = 0.0f;

        private float deltaTime = 0.0f;
        private float randomTime = 0.0f;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_GPO.Event_SetAISlideMoveActionRatioTime>(OnSetAISlideMoveActionRatioTime);
        }

        override protected void OnClear() {
            iGPO?.Unregister<SE_GPO.Event_SetAISlideMoveActionRatioTime>(OnSetAISlideMoveActionRatioTime);
        }

        public override void OnStart() {
            iGPO.Dispatcher(new SE_GPO.Event_GetAISlideMoveActionRatioTime {
                CallBack = (f, f1) => {
                    SetSlideValue(f, f1);
                }
            });
            GetRandomTime();
        }

        private void OnSetAISlideMoveActionRatioTime(ISystemMsg body, SE_GPO.Event_SetAISlideMoveActionRatioTime ent) {
            SetSlideValue(ent.SlideMinIntervalTime, ent.SlideMaxIntervalTime);
        }
        
        private void SetSlideValue(float minValue, float maxValue) {
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
                return TaskStatus.Success;
            }
            if (Time.time - deltaTime > randomTime) {
                deltaTime = Time.time;
                GetRandomTime();
                iGPO.Dispatcher(new SE_Behaviour.Event_OnSlide());
            }
            return TaskStatus.Running;
        }
    }
}