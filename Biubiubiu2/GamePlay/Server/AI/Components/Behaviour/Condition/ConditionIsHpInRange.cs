using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("Hp 是否在指定百分比范围内（左闭右开）")]
    [TaskCategory("AI")]
    public class ConditionIsHpInRange : ConditionComponent {
        [SerializeField]
        public float minRange;
        [SerializeField]
        public float maxRange;

        public override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
        }

        public override void OnStart() {
        }

        public override TaskStatus OnUpdate() {
            float ratio = 0;
            iGPO.Dispatcher(new SE_GPO.Event_GetHPInfo() {
                CallBack = (nowHp, maxHp) => {
                    ratio = nowHp * 1f / maxHp;
                }
            });

            bool isInRange = ratio >= minRange && ratio < maxRange;
            return isInRange ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}