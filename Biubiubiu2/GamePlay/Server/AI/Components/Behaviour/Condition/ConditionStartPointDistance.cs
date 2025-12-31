using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 判断和目标的距离
    /// </summary>
    [TaskName("判断和出生点的距离")]
    [TaskCategory("AI")]
    public class ConditionStartPointDistance : ConditionComponent {
        [SerializeField]
        private float checkDistance = 0.0f;

        private Vector3 attackPoint = Vector3.zero;
        private bool isCheckTarget = false;
        private bool isComeBack = false;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            iGPO.Register<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
        }

        public override void OnStart() {
            base.OnStart();
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            if (isComeBack == true) {
                isCheckTarget = false;
                return;
            }
            var isTrue = ent.TargetGPO != null;
            if (isCheckTarget != isTrue) {
                if (isTrue) {
                    attackPoint = iGPO.GetPoint();
                } else {
                    attackPoint = Vector3.zero;
                }
                isCheckTarget = isTrue;
            }
        }

        private void OnComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            isComeBack = ent.IsTrue;
        }

        public override TaskStatus OnUpdate() {
            if (iGPO == null || isCheckTarget == false || attackPoint == Vector3.zero) {
                return TaskStatus.Success;
            }
            var distance = Vector3.Distance(this.attackPoint, iEntity.GetPoint());
            if (distance < checkDistance) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}