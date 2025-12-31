using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("朝目自身和目标一定角度移动")]
    [TaskCategory("AI")]
    public class ActionMoveForSelfTargetAngle : ActionComponent {
        [SerializeField]
        private float checkTime;
        [SerializeField]
        private float minDistance = 0f;
        [SerializeField]
        private float maxDistance = 0f;
        [SerializeField]
        private float angle = 90;
        [SerializeField]
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private float countCheckTime;
        private IGPO target;
        
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        override protected void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            this.target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.target = ent.TargetGPO;
        }

        public override TaskStatus OnUpdate() { 
            if (Time.realtimeSinceStartup - countCheckTime <= this.checkTime) {
                return TaskStatus.Running;
            }
            countCheckTime = Time.realtimeSinceStartup;
            if (this.target == null || this.target.IsClear()) {
                this.target = null;
                return TaskStatus.Running;
            }
            iGPO.Dispatcher(new SE_Behaviour.Event_MovePoint {
                movePoint = CalculateRandomPoint(),
                MoveType = moveType
            });
            return TaskStatus.Running;
        }

        // 获取范围内的随机点
        public Vector3 CalculateRandomPoint() {
            // 获取 A 对象相对 B 对象的方向
            var directionToTarget = (iEntity.GetPoint() - this.target.GetPoint()).normalized;
            // 随机获取背后180度内的角度 (-90度到90度)
            var rangeAngle = Random.Range(-angle, angle);
            // 将目标朝向旋转相应角度
            var direction = Quaternion.Euler(0, rangeAngle, 0) * directionToTarget ;
            var distance = Random.Range(minDistance, maxDistance);
            var randomDisplacement = direction * distance;
            var randomPoint = new Vector3(iEntity.GetPoint().x + randomDisplacement.x, iEntity.GetPoint().y,
                iEntity.GetPoint().z + randomDisplacement.y);
            return randomPoint;
        }
    }
}