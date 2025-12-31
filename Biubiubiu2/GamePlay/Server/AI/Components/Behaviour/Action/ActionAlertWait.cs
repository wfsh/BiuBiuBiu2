using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    
    [TaskName("预警时间")]
    [TaskCategory("AI")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    public class ActionAlertWait : ActionComponent {
        private float duration;
        private float startTime;
        
        public override void OnAwake() {
            base.OnAwake();
            var monsterBehaviourData = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
            duration = monsterBehaviourData.AlertTime;
        }

        public override void OnStart() {
            startTime = Time.time;
        }

        public override TaskStatus OnUpdate() {
            if (Time.time - startTime >= duration) {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}