using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    
    [TaskName("检测开火距离(模板)")]
    [TaskCategory("AI")]
    public class ConditionIsHateGPOInCatchFireRange : ConditionComponent {
        private IGPO target;
        private float fireRangeInTemplate;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
        }

        override protected void OnClear() {
            base.OnClear();
            iGPO.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            target = null;
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            target = ent.TargetGPO;
            if (iGPO != null && fireRangeInTemplate == 0) {
                var myBehaviourData = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
                fireRangeInTemplate = myBehaviourData.CatchFireRange;
            }
        }

        public override TaskStatus OnUpdate() {
            if (target == null || target.IsClear()|| iEntity.IsClear()) {
                return TaskStatus.Failure;
            }

            if (fireRangeInTemplate > 0) {
                var myPoint = iEntity.GetPoint();
                myPoint.y = 0;
                var targetPoint = target.GetPoint();
                targetPoint.y = 0;
                var checkDistance = Vector3.Distance(myPoint, targetPoint);
                if (checkDistance < fireRangeInTemplate) {
                    return TaskStatus.Success;
                } else {
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Failure;
        }
    }
}