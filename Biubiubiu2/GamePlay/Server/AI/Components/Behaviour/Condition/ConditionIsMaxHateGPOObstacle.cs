using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("检查和目标之间是否有障碍物（头）")]
    [TaskCategory("AI")]
    public class ConditionIsMaxHateGPOObstacle : ConditionComponent {
        private bool isMaxHateGPOObstacle = false;
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_isMaxHateGPOObstacle>(OnMaxHateGPOObstacleCallBack);
        }

        override protected void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_isMaxHateGPOObstacle>(OnMaxHateGPOObstacleCallBack);
        }

        private void OnMaxHateGPOObstacleCallBack(ISystemMsg body, SE_Behaviour.Event_isMaxHateGPOObstacle ent) {
            this.isMaxHateGPOObstacle = ent.IsObstacle;
        }

        public override TaskStatus OnUpdate() {
            if (isMaxHateGPOObstacle) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}